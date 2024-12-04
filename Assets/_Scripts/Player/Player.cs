using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    
    [SerializeField] private Camera _camera;
    [SerializeField] private Transform _player;
    private Animator _animator;
    private Rigidbody2D _rigidbody;
    public float maxHealthAmount = 10f;
    public float healthAmount = 10f;
    public bool isPaused;
    public int currentSlot = 0;
    public float immunityTime = 1f;
    private float immunityTimer = 0f;
    private GameObject PlayerParent;
    
    public GameObject _exclamationIcon;

    public GameObject HealthBarFillImage;
    private NonEntrySceneChange _sceneChanger;

    public PlayerCasting _playerCasting;
    public GameObject _deathCanvas;
    public TextAsset deathMessage;

    private Coroutine slowTimeCoroutine;

    public float _exp = 0;




    private void Awake()
    {
        Debug.Log($"Player has {_exp} Experience Points");
        _camera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        PlayerParent = transform.parent.gameObject;
        _rigidbody = PlayerParent.GetComponent<Rigidbody2D>();
        _animator = PlayerParent.GetComponentInChildren<Animator>();
    }


    private void Update()
    {
        if (InputManager.Slot1) // R - Regular Attack
        {
            currentSlot = 1;
        }
        if (InputManager.Slot2) // F - Secondary Attack
        {
            currentSlot = 2;
        }
        if (InputManager.Slot3) // T - Special 
        {
            currentSlot = 3;
        }
        if (InputManager.Slot4) // G - Object Transmutation
        {
            currentSlot = 4;
        }
        if (InputManager.NoSlot) // does nothing
        {
            currentSlot = 0;
        }

        if (InputManager.ToggleMenu)
        {
            
        }

        if (immunityTimer < immunityTime)
        {
            immunityTimer += Time.deltaTime;
        }
    }





    public void Damage(float damage, bool silent = false)
    {
        Debug.Log(healthAmount);
        healthAmount -= damage;
        HealthBarFillImage.GetComponent<Image>().fillAmount = .97f - .1f*(10-healthAmount)*0.953f; // Visually update health bar
        if (!silent)
        {
            //_animator.Play("Damage", -1, 0f);
            //DamageSoundSource.Play();
        }
        if(healthAmount <= 0)
        {
            Die();
        }
    }

    public void Heal(float health)
    {
        
        if (healthAmount + health <= maxHealthAmount)
        {
            healthAmount += health;
        }
        else
        {
            healthAmount = maxHealthAmount;
        }
        HealthBarFillImage.GetComponent<Image>().fillAmount = .97f - .1f*(10-healthAmount)*0.953f;
        //HealSoundSource.Play();
    }

    public void ReceiveHarm(HarmfulObjectScript harmSource)
    {
        if (harmSource.GetComponent<HarmfulObjectScript>().canDamagePlayer && immunityTimer >= immunityTime)
        {
            Damage(harmSource.GetComponent<HarmfulObjectScript>().damageAmount);
            immunityTimer = 0f;
            _animator.Play("Harm");
            AudioManager.instance.PlayOneShot(FMODEvents.instance.playerDamage, transform.position);
        }
    }


    private void Die()
    {
        isPaused = true;
        _deathCanvas.SetActive(true);
        DialogueManager.instance.EnterDialogue(deathMessage);
    }

    public void NotifyOn()
    {
        _exclamationIcon.SetActive(true);
    }
    public void NotifyOff()
    {
        _exclamationIcon.SetActive(false);
    }

    public void FreezeForSauce(float duration, float multiplier = 0f)
    {
        if (slowTimeCoroutine != null)
        {
            StopCoroutine(slowTimeCoroutine);
        }
        else
        {
            slowTimeCoroutine = StartCoroutine(SlowTimeCoroutine(duration, multiplier));
        }
    }

    public IEnumerator SlowTimeCoroutine(float duration, float multiplier)
    {
        Time.timeScale = multiplier;
        yield return new WaitForSecondsRealtime(duration);
        Time.timeScale = 1f;
    }

    
}
