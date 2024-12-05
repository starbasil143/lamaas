using UnityEngine;
using UnityEngine.Playables;

public class Scroll : MonoBehaviour
{
    private PlayerCasting _playerCasting;
    public TileData tileData;
    public int index;
    [SerializeField] private bool objectTransmutation;
    public string objectName;
    public TextAsset dialogueAsset;
    public PlayableDirector cutscene;
    void Start()
    {
        _playerCasting = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<PlayerCasting>();

        if (objectTransmutation)
        {
            if (_playerCasting.KnowsObjectSpell(objectName))
            {
                Destroy(gameObject);
            }
        }
        else if (_playerCasting.KnowsSpell(tileData, index))
        {
            Destroy(gameObject);
        }
       
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
        
        if (cutscene != null)
        {
            GetComponent<SpriteRenderer>().enabled = false;
            cutscene.gameObject.SetActive(true);
            cutscene.Play();
        }   
        else
        {
            Destroy(gameObject);
        }
    }
}
