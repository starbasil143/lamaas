using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TransButton : MonoBehaviour
{
    public int expCost;
    [SerializeField] private GameObject button;
    [SerializeField] private GameObject image;
    [SerializeField] private GameObject lockedGFX;
    private TextMeshProUGUI priceText;
    [SerializeField] private TextMeshProUGUI expText;
    public TileData t_tileData;
    public int t_index;

    private PlayerCasting _playerCasting;
    private Player _player;

    private bool unlocked;
    private Color originalButtonColor;
    private Color originalImageColor;


    private void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Player>();
        _playerCasting = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<PlayerCasting>();
        
        priceText = transform.Find("Button").Find("LockedGFX").Find("Price").gameObject.GetComponent<TextMeshProUGUI>();
        originalButtonColor = button.GetComponent<Image>().color;
        originalImageColor = image.GetComponent<Image>().color;
        priceText.text = expCost.ToString();
    }

    private void OnEnable()
    {
        unlocked = _playerCasting.KnowsSpell(t_tileData, t_index);

        if (!unlocked)
        {
            DisplayLocked();
        }
        else
        {
            DisplayUnlocked();
        }
    }


    private void DisplayLocked()
    {
        button.GetComponent<Image>().color = new Color(originalButtonColor.r/2,originalButtonColor.g/2,originalButtonColor.b/2);
        image.GetComponent<Image>().color = new Color(originalImageColor.r/2,originalImageColor.g/2,originalImageColor.b/2);
        lockedGFX.SetActive(true);
    }

    private void DisplayUnlocked()
    {
        button.GetComponent<Image>().color = originalButtonColor;
        image.GetComponent<Image>().color = originalImageColor;
        lockedGFX.SetActive(false);
    }


    public void HandleClick()
    {
        if (!unlocked)
        {
            if (_player.GetExpAmount() >= expCost)
            {
                AudioManager.instance.PlayOneShot(FMODEvents.instance.sfx_startgame, transform.position);
                _player.AddExp(-expCost);
                expText.text = _player.GetExpAmount().ToString();
                _playerCasting.LearnSpell(t_tileData, t_index);
                DisplayUnlocked();
                unlocked = true;
            }
        }
    }




}
