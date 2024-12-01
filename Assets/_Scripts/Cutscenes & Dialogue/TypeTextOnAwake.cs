using System.Collections;
using TMPro;
using UnityEngine;

public class TypeTextOnAwake : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;
    private float typingSpeed = .055f; 
    private float typingSpeedMultiplier = 1;

    private void OnEnable()
    {
        StartCoroutine(DisplayLine(dialogueText.text));
    }

    private IEnumerator DisplayLine(string line)
    {
        dialogueText.text = line;
        dialogueText.maxVisibleCharacters = 0;

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
                    AudioManager.instance.PlayOneShot(FMODEvents.instance.voice_typing, transform.position);
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
                        typingSpeedMultiplier = 1f;
                        break;
                }
            }

            if (letter == '>')
            {
                ignoringText = false;
            }
        }
        
    }
}
