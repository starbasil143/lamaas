using TMPro;
using UnityEngine;

public class QuestDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI questName;
    [SerializeField] private TextMeshProUGUI questDescription;
    private PlayerProgress _progress;

    private void Awake()
    {
        _progress = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<PlayerProgress>();
    }

    private void OnEnable()
    {
        if (_progress.progressFlags["openingScene"])
        {
            questName.text = "Shipping Delay";
            questDescription.text = "Mallory, the smith's supplier, is late on a shipment. Lithas wants you to investigate. Find Mallory in the house with the <color=yellow>yellow</color> roof.";
        }
        if (_progress.progressFlags["malloryVisit"])
        {
            questName.text = "";
            questDescription.text = "";
        }
        if (_progress.progressFlags["forestVisit1"])
        {
            questName.text = "Deer Lord...";
            questDescription.text = "";
        }
        if (_progress.progressFlags["deerDefeated"])
        {
            questName.text = "Deer Lord... (Completed)";
            questDescription.text = "";
        }
        if (_progress.progressFlags["kidCutscene"])
        {
            questName.text = "Forest Investigation";
            questDescription.text = "Lithas is hurt. Perhaps something in the forest can help you.";
        }
    }
}