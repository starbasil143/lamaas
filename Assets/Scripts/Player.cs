using System.Linq;
using UnityEngine;
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

    public GameObject HealthBarFillImage;

    public PlayerCasting _playerCasting;



    private void Awake()
    {
        _camera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }


    private void Update()
    {
        if (InputManager.Slot1) // R - Broad Attack
        {
            currentSlot = 1;
        }
        if (InputManager.Slot2) // F - Specific Attack
        {
            currentSlot = 2;
        }
        if (InputManager.Slot3) // T - Special 1
        {
            currentSlot = 3;
        }
        if (InputManager.Slot4) // G - Special 2
        {
            currentSlot = 4;
        }
        if (InputManager.NoSlot)
        {
            currentSlot = 0;
        }

    }





    public void Damage(float damage, bool silent = false)
    {
        Debug.Log(healthAmount);
        healthAmount -= damage;
        HealthBarFillImage.GetComponent<Image>().fillAmount = .97f - .1f*(10-healthAmount)*0.953f;
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

    private void Heal(float health)
    {
        if (healthAmount + health <= maxHealthAmount)
        {
            healthAmount += health;
        }
        else
        {
            healthAmount = maxHealthAmount;
        }
        
        //healthBar.fillAmount = healthAmount / maxHealthAmount;
        //HealSoundSource.Play();
    }

    private void Die()
    {
        isPaused = true;
        gameObject.SetActive(false);
        //_deathCanvas.SetActive(true);
    }
}
