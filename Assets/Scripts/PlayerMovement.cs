using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 5f;
    
    private Vector2 _movement;
    private Rigidbody2D _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        _movement.Set(InputManager.Movement.x, InputManager.Movement.y);
        _rigidbody.linearVelocity = _movement * _moveSpeed;
    }
}
