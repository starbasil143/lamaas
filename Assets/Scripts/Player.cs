using System.Linq;
using UnityEngine;

public class Player : MonoBehaviour
{
    
    [SerializeField] private Camera _camera;
    [SerializeField] private Transform _player;
    private MapManager _mapManager;
    private Animator _animator;
    private Rigidbody2D _rigidbody;
    public float maxHealthAmount = 100f;
    public float healthAmount = 100f;
    public bool isPaused;



    private void Awake()
    {
        _camera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _mapManager = GetComponent<MapManager>();
    }


    private void Update()
    {
        if (InputManager.Cast)
        {
            
            TileData currentTileData = _mapManager.GetCurrentTile();
            
            string[] tileMaterials = currentTileData.materialTypes;

            Debug.Log(string.Join(", ", tileMaterials));
        }
    }





    private void Damage(float damage, bool silent = false)
    {
        healthAmount -= damage;
        //healthBar.fillAmount = healthAmount / maxHealthAmount;
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
