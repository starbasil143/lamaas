using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Ink.Runtime;
using TMPro;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Playables;
using UnityEngine.SearchService;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance { get; private set; }

    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private TextMeshProUGUI dialogueName;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private GameObject continueIcon;

    [SerializeField] private GameObject[] choices;
    [SerializeField] private float typingSpeed = 0.02f;
    private int selectedChoiceIndex;
    private TextMeshProUGUI[] choicesText;
    private Coroutine displayLineCoroutine;

    public TimelineManager _timelineManager;

    public Color textUnselectedColor;
    public Color textSelectedColor;

    [Header("Voice Sounds")]
    public AudioClip defaultVoice;
    public AudioClip typingVoice;

    private AudioClip currentVoice;
    public AudioSource VoiceSource;


    private Story currentStory;
    public bool dialogueIsPlaying { get; private set; }
    private bool canContinueToNextLine;
    private bool inCutscene;
    private float typingSpeedMultiplier = 1f;

    private const string SPEAKER_TAG = "speaker";
    private const string VOICE_TAG = "voice";

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
        }
        instance = this;
    }

    private void Start()
    {
        dialogueIsPlaying = false;
        dialogueBox.SetActive(false);

        choicesText = new TextMeshProUGUI[choices.Length];
        int i = 0;
        foreach (GameObject choice in choices)
        {
            choicesText[i] = choice.GetComponentInChildren<TextMeshProUGUI>();
            i++;
        }
    }

    private void Update()
    {
        if (dialogueIsPlaying)
        {
            if (InputManager.Advance)
            {
                if (canContinueToNextLine)
                {
                    if (currentStory.currentChoices.Count == 0)
                    {
                        ContinueStory();
                    }
                    else
                    {
                        MakeChoice(selectedChoiceIndex);
                    }
                }
                else
                {
                    StopCoroutine(displayLineCoroutine);
                    dialogueText.maxVisibleCharacters = 1430;
                    EndTyping();
                }
            }

            if (InputManager.Right)
            {
                if (currentStory.currentChoices.Count > 0)
                {
                    choicesText[selectedChoiceIndex].color = textUnselectedColor;
                    if (selectedChoiceIndex + 1 >= currentStory.currentChoices.Count)
                    {
                        selectedChoiceIndex = 0;
                    }
                    else
                    {
                        selectedChoiceIndex++;
                    }
                    choicesText[selectedChoiceIndex].color = textSelectedColor;
                }
            }

            if (InputManager.Left)
            {
                if (currentStory.currentChoices.Count > 0)
                {
                    choicesText[selectedChoiceIndex].color = textUnselectedColor;
                    if (selectedChoiceIndex == 0)
                    {
                        selectedChoiceIndex = currentStory.currentChoices.Count - 1;
                    }
                    else
                    {
                        selectedChoiceIndex--;
                    }
                    choicesText[selectedChoiceIndex].color = textSelectedColor;
                }
            }
        }
    }

    public void EnterDialogue(TextAsset inkJSON, bool dialogueTriggeredByCutscene = false)
    {
        currentStory = new Story(inkJSON.text);
        dialogueIsPlaying = true;
        InputManager.SwitchToDialogueControls();
        dialogueBox.SetActive(true);
        inCutscene = dialogueTriggeredByCutscene;

        if (inCutscene)
        {
            _timelineManager = GameObject.FindGameObjectWithTag("TimelineManager").GetComponent<TimelineManager>();
        }


        dialogueName.text = "???";
        currentVoice = defaultVoice;

        ContinueStory();
    }

    private void ContinueStory()
    {
        if (currentStory.canContinue)
        {
            if (displayLineCoroutine != null)
            {
                StopCoroutine(displayLineCoroutine);
            }
            displayLineCoroutine = StartCoroutine(DisplayLine(currentStory.Continue()));
            HandleTags(currentStory.currentTags);
        }
        else
        {
            StartCoroutine(ExitDialogue());
        }
    }

    private IEnumerator ExitDialogue()
    {
        dialogueIsPlaying = false;
        dialogueBox.SetActive(false);
        yield return new WaitForSeconds(.1f);
        InputManager.SwitchToPlayerControls();
        dialogueText.text = "";
        
        if (inCutscene)
        {
            _timelineManager.ResumeTimeline();
        }
    }

    private IEnumerator DisplayLine(string line)
    {
        dialogueText.text = line;
        dialogueText.maxVisibleCharacters = 0;

        StartTyping();

        bool ignoringText = false;
        foreach (char letter in line)
        {
            if (letter == '<')
            {
                ignoringText = true;
            }

            
            if (!ignoringText)
            {


                yield return new WaitForSeconds(typingSpeed * typingSpeedMultiplier);
                dialogueText.maxVisibleCharacters++;
                if (letter != ' ')
                {
                    VoiceSource.PlayOneShot(currentVoice);
                }

                switch (letter)
                {
                    case '?':
                    case '!':
                    case '.':
                        typingSpeedMultiplier = 5;
                        break;

                    case ',':
                        typingSpeedMultiplier = 3;
                        break;

                    default:
                        typingSpeedMultiplier = 1;
                        break;
                }
            }

            if (letter == '>')
            {
                ignoringText = false;
            }
        }
        
        EndTyping();
        
    }

    private void StartTyping()
    {
        canContinueToNextLine = false;
        continueIcon.SetActive(false);
        HideChoices();
    }
    private void EndTyping()
    {
        ShowChoices();
        continueIcon.SetActive(true);
        canContinueToNextLine = true;
    }

    private void HandleTags(List<string> currentTags)
    {
        foreach (string tag in currentTags)
        {
            string[] splitTag = tag.Split(":");
            if (splitTag.Length == 2)
            {
                string tagKey = splitTag[0].Trim();
                string tagValue = splitTag[1].Trim();

                switch (tagKey)
                {
                    case SPEAKER_TAG:
                        dialogueName.text = tagValue;
                        break;
                    case VOICE_TAG:
                        switch (tagValue)
                        {
                            case "default":
                                currentVoice = defaultVoice;
                                break;
                            case "typing":
                                currentVoice = typingVoice;
                                break;
                        }
                        break;
                }
            }
            else
            {
                Debug.LogWarning("the tag is broken");
            }
        }
    }

    private void ShowChoices()
    {
        List<Choice> currentChoices = currentStory.currentChoices;

        int i = 0;

        foreach (Choice choice in currentChoices)
        {
            choices[i].gameObject.SetActive(true);
            choicesText[i].text = choice.text;
            choicesText[i].color = textUnselectedColor;
            i++;
        }

        for (int j = i; j < choices.Length; j++)
        {
            choices[j].gameObject.SetActive(false);
        }

        selectedChoiceIndex = 0;
        choicesText[0].color = textSelectedColor;
        
    }

    private void HideChoices()
    {
        foreach (GameObject choice in choices)
        {
            choice.SetActive(false);
        }
    }

    public void MakeChoice(int choiceIndex)
    {
        if (canContinueToNextLine)
        {
            currentStory.ChooseChoiceIndex(choiceIndex);
            ContinueStory();
        }
            
    }
    
}
