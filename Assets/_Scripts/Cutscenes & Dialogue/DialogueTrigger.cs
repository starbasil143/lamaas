using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public TextAsset dialogueAsset;

    public void TriggerDialogue()
    {
        DialogueManager.instance.EnterDialogue(dialogueAsset);
    }
}
