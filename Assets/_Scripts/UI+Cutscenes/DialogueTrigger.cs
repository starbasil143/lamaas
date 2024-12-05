using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public List<TextAsset> dialogueAssets;
    private int index = 0;

    public void TriggerDialogue()
    {
        DialogueManager.instance.EnterDialogue(dialogueAssets[index]);
        if (index < dialogueAssets.Count - 1)
        {
            index++;
        }
    }
}
