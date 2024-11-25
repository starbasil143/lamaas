using System.Collections;
using TMPro;
using UnityEngine;

public class TypeTextOnAwake : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;
    private float typingSpeed = .055f; 


    private void OnEnable()
    {
        StartCoroutine(DisplayLine(dialogueText.text));
    }

    private IEnumerator DisplayLine(string line)
    {
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
                yield return new WaitForSeconds(typingSpeed);
                dialogueText.maxVisibleCharacters++;
                if (letter != ' ')
                {
                    //VoiceSource.PlayOneShot(currentVoice);
                }
            }

            if (letter == '>')
            {
                ignoringText = false;
            }
        }
    }
}
