using UnityEngine;

public class HintTrigger : MonoBehaviour
{
    public string message;

    public void DisplayHint()
    {
        HintManager.instance.DisplayHint(message);
    }
}
