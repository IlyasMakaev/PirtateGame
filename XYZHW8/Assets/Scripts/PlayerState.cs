using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _jumpForce;
    [SerializeField] private LayerCheck _groundCheck;

    private Rigidbody2D _playerRigid;
    private Vector2 _direction;
    private Animator _animator;
    private SpriteRenderer _sprite;

    private static readonly int isGorundKey = Animator.StringToHash("isGrounded");
    private static readonly int isRunning = Animator.StringToHash("isRunning");
    private static readonly int verticalVelocity = Animator.StringToHash("verticalVelocity");
    private void Awake()
    {
        _playerRigid = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _sprite = GetComponent<SpriteRenderer>();
    }
    public void SetDirection(Vector2 direction)
    {
        _direction = direction;
    }

    private void FixedUpdate()
    {
        _playerRigid.velocity = new Vector2(_direction.x * _speed, _playerRigid.velocity.y);

        var isJumping = _direction.y > 0;
        var isGrounded = IsGrounded();

        if(isJumping)
        {
            if (isGrounded & _playerRigid.velocity.y <= 0)
            {
                _playerRigid.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
            }

        }
        else if (_playerRigid.velocity.y > 0)
        {
            _playerRigid.velocity = new Vector2(_playerRigid.velocity.x, _playerRigid.velocity.y * 0.5f);
        }

        _animator.SetBool(isGorundKey, isGrounded);
        _animator.SetBool(isRunning, _direction.x != 0);
        _animator.SetFloat(verticalVelocity, _playerRigid.velocity.y);

        UpdateSpriteDirection();
    }

    private bool IsGrounded()
    {
        return _groundCheck.IsGrounded;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = IsGrounded() ? Color.green : Color.red;
        Gizmos.DrawSphere(transform.position, 0.3f);
        

    }

    private void UpdateSpriteDirection()
    {

        if (_direction.x > 0)
        {

            _sprite.flipX = false;

        }
        else if (_direction.x < 0)
        {
            _sprite.flipX = true;
        }
    }

    public void SaySomething()
    {
        Debug.Log("something");
    }

   
}
