using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Ink.Runtime;
using TMPro;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.EventSystems;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance { get; private set; }

    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private TextMeshProUGUI dialogueName;
    [SerializeField] private TextMeshProUGUI dialogueText;

    [SerializeField] private GameObject[] choices;
    private int selectedChoiceIndex;
    private TextMeshProUGUI[] choicesText;

    public Color textUnselectedColor;
    public Color textSelectedColor;


    private Story currentStory;
    public bool dialogueIsPlaying { get; private set; }

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
                if (currentStory.currentChoices.Count == 0)
                {
                    ContinueStory();
                }
                else
                {
                    MakeChoice(selectedChoiceIndex);
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

    public void EnterDialogue(TextAsset inkJSON)
    {
        currentStory = new Story(inkJSON.text);
        dialogueIsPlaying = true;
        InputManager.SwitchToDialogueControls();
        dialogueBox.SetActive(true);

        ContinueStory();
    }

    private IEnumerator ExitDialogue()
    {
        dialogueIsPlaying = false;
        dialogueBox.SetActive(false);
        yield return new WaitForSeconds(.1f);
        InputManager.SwitchToPlayerControls();
        dialogueText.text = "";
    }

    private void ContinueStory()
    {
        if (currentStory.canContinue)
        {
            dialogueText.text = currentStory.Continue();
            ShowChoices();
        }
        else
        {
            StartCoroutine(ExitDialogue());
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

    public void MakeChoice(int choiceIndex)
    {
        currentStory.ChooseChoiceIndex(choiceIndex);
        ContinueStory();
    }
}
