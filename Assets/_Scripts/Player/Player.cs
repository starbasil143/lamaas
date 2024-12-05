using System;
using System.Collections;
using System.Collections.Generic;
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
    public List<GameObject> disableOnMainMenu;
    
    public GameObject _exclamationIcon;

    public GameObject HealthBarFillImage;
    private NonEntrySceneChange _sceneChanger;

    public PlayerCasting _playerCasting;
    public GameObject _deathCanvas;
    public TextAsset deathMessage;

    public GameObject _pauseManager;

    private Coroutine slowTimeCoroutine;

    private int exp = 0;




    private void Awake()
    {
        if (SceneManager.GetActiveScene().name == "MainMenuScene")
        {
            isPaused = true;
        }
        _camera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        PlayerParent = transform.parent.gameObject;
        _rigidbody = PlayerParent.GetComponent<Rigidbody2D>();
        _animator = PlayerParent.GetComponentInChildren<Animator>();
    }

    public void LoadGameNow()
    {
        Revive();
        gameObject.GetComponent<PlayerSaveDataManager>().LoadGame();
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

        if (InputManager.ToggleMenu && (!isPaused ||  _pauseManager.GetComponent<PauseMenuController>().isPaused))
        {
            if (!isPaused)
            {
                _pauseManager.GetComponent<PauseMenuController>().TogglePauseMenu();
            }
            else if (_pauseManager.GetComponent<PauseMenuController>().isPaused)
            {
                _pauseManager.GetComponent<PauseMenuController>().BackOut();
            }
        }

        if (immunityTimer < immunityTime)
        {
            immunityTimer += Time.deltaTime;
        }
    }

    public void Revive()
    {
        healthAmount = 9;
        Heal(1);
        isPaused = false;
        _deathCanvas.SetActive(false);
    }

    public void CreateSaveData_Player()
    {
        PlayerPrefs.SetInt("exp", exp);
    }

    public void AddExp(int amount)
    {
        if (exp + amount <= 9999 && exp + amount >= 0)
        {
            exp += amount;
        }
    }
    public int GetExpAmount()
    {
        return exp;
    }

    public void LoadSaveData_Player()
    {
        exp = PlayerPrefs.GetInt("exp");
    }
    public void FullDisablePlayer()
    {
        foreach (GameObject obj in disableOnMainMenu)
        {
            obj.SetActive(false);
        }
    }

    public void FullEnablePlayer()
    {
        foreach (GameObject obj in disableOnMainMenu)
        {
            obj.SetActive(true);
        }
    }


    private void OnEnable()
    {
        PlayerSaveDataManager.onSaveData += CreateSaveData_Player;
        PlayerSaveDataManager.onLoadData += LoadSaveData_Player;
    }

    private void OnDisable()
    {
        PlayerSaveDataManager.onSaveData -= CreateSaveData_Player;
        PlayerSaveDataManager.onLoadData -= LoadSaveData_Player;
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

    public void CancelGrapple()
    {
        if (GameObject.FindWithTag("Grapple") != null)
        {
            GameObject.FindWithTag("Grapple").GetComponent<Vine>().CancelGrapple();
        }
    }

    public IEnumerator SlowTimeCoroutine(float duration, float multiplier)
    {
        Time.timeScale = multiplier;
        yield return new WaitForSecondsRealtime(duration);
        Time.timeScale = 1f;
    }

    
}
