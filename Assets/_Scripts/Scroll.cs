using UnityEngine;

public class Scroll : MonoBehaviour
{
    private PlayerCasting _playerCasting;
    public TileData tileData;
    public int index;
    [SerializeField] private bool objectTransmutation;
    public string objectName;
    public TextAsset dialogueAsset;
    void Start()
    {
        _playerCasting = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<PlayerCasting>();
    }

    public void LearnSpell()
    {
        if (objectTransmutation)
        {
            _playerCasting.LearnObjectSpell(objectName);
        }
        else
        {
            _playerCasting.LearnSpell(tileData, index);
        }

        if (dialogueAsset != null)
        {
            DialogueManager.instance.EnterDialogue(dialogueAsset, false, true);
        }
        Destroy(gameObject);
    }
}
