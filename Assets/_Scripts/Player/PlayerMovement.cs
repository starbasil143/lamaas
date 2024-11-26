using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _dashSpeed = 50f;
    [SerializeField] private float _dashLength = .3f;
    [SerializeField] private float _dashCooldown;

    private float currentDashCooldown = 0f;
    private bool isDashing;
    //private bool canDash = true;
    private Vector2 lastDirection;

    
    private const string _horizontal = "Horizontal";
    private const string _vertical = "Vertical";
    private const string _lastHorizontal = "LastHorizontal";
    private const string _lastVertical = "LastVertical";
    
    private Vector2 movement;
    private Rigidbody2D _rigidbody;
    private Animator _animator;
    private GameObject PlayerParent;
    private Player _player;
    [SerializeField] private GameObject PlayerGFX;


    private void Awake()
    {
        PlayerParent = transform.parent.gameObject;
        _player = PlayerParent.GetComponentInChildren<Player>();
        _rigidbody = PlayerParent.GetComponent<Rigidbody2D>();
        _animator = PlayerGFX.GetComponent<Animator>();
    }

    private void Update()
    {
        if (!_player.isPaused)
        {
            // Movement
            if (InputManager.Dash && currentDashCooldown <= 0) // Dashing
            {
                isDashing = true;
                currentDashCooldown = _dashCooldown;
                movement = lastDirection;
            }
        }

        _animator.SetFloat(_horizontal, movement.x);
        _animator.SetFloat(_vertical, movement.y);
        if (movement != Vector2.zero)
        {
            lastDirection = movement;
            _animator.SetFloat(_lastHorizontal, movement.x);
            _animator.SetFloat(_lastVertical, movement.y);
        }

    }
    private void FixedUpdate()
    {
        if (!_player.isPaused)
        {
            // Movement

            if (!isDashing) // Walking
            {
                movement.Set(InputManager.Movement.x, InputManager.Movement.y);
                _rigidbody.linearVelocity = movement * _moveSpeed;
            }
            
            if (isDashing)
            {
                _rigidbody.linearVelocity = movement * Mathf.Lerp(_dashSpeed, _moveSpeed, (_dashCooldown - currentDashCooldown)/_dashLength);

                if (_dashCooldown - currentDashCooldown >= _dashLength)
                {
                    isDashing = false;
                }
            }
            if (currentDashCooldown > 0)
            {
                currentDashCooldown -= Time.deltaTime;
                if (currentDashCooldown <= 0)
                {
                    isDashing = false;
                }
            }

            if (movement != Vector2.zero)
            {
                lastDirection = movement;
            }
        }
    }


    public void Pause()
    {
        _rigidbody.linearVelocity = Vector2.zero;
        movement = Vector2.zero;
        _player.isPaused = true;
    }

    public void Unpause()
    {
        _player.isPaused = false;
    }

    public void WalkBackwards()
    {
        StartCoroutine(WalkBackwardsCoroutine());
    }

    private IEnumerator WalkBackwardsCoroutine()
    {
        isDashing = false;
        Vector2 directionToReturn = lastDirection * -1;
        movement = directionToReturn * _moveSpeed;
        _rigidbody.linearVelocity = movement;
        yield return new WaitForSeconds(.4f);
        _rigidbody.linearVelocity = Vector2.zero;
    }
    
    /*
    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        _rigidbody.linearVelocity = new Vector2();
        float i = 0;
        float dashCompletion = 0;
        while (i < _dashLength)
        {
            i += Time.deltaTime;
            dashCompletion  = i/_dashLength;
            _rigidbody.linearVelocity = Lerp();

        }
    }*/


}

