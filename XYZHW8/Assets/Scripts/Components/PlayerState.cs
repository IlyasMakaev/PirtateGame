using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Scripts.Components
{
    public class PlayerState : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private float _jumpForce;
        [SerializeField] private float _damageJumpForce;
        [SerializeField] private LayerCheck _groundCheck;
        [SerializeField] private float _interactionRadius;
        [SerializeField] private ParticleSystem _hitParicles;
        
        [SerializeField] private LayerMask _interactionLayer;

        [SerializeField] private SpawnComponent _footStepParticles;
        [SerializeField] private SpawnComponent _jumpDust;
        [SerializeField] private SpawnComponent _fallDust;


        private Rigidbody2D _playerRigid;
        private Vector2 _direction;
        private Animator _animator;
        private bool _isGrounded;
        private int _coins = 0;
        private float _dropCheck = -14f;
        private bool _allowDoubleJump;
        private Collider2D[] _interactionResult = new Collider2D[1];

        private static readonly int isGorundKey = Animator.StringToHash("isGrounded");
        private static readonly int isRunning = Animator.StringToHash("isRunning");
        private static readonly int verticalVelocity = Animator.StringToHash("verticalVelocity");
        private static readonly int Hit = Animator.StringToHash("hit");
        private void Awake()
        {
            _playerRigid = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
            
        }
        public void SetDirection(Vector2 direction)
        {
            _direction = direction;
        }
        private void Update()
        {
            _isGrounded = IsGrounded();
        }

        private void FixedUpdate()
        {
            var xVelocity = _direction.x * _speed;
            var yVelocity = CalculateYVelocity();
            _playerRigid.velocity = new Vector2(xVelocity, yVelocity);


            _animator.SetBool(isGorundKey, _isGrounded);
            _animator.SetBool(isRunning, _direction.x != 0);
            _animator.SetFloat(verticalVelocity, _playerRigid.velocity.y);

            UpdateSpriteDirection();
        }


        private float CalculateYVelocity()
        {
            var yVelocity = _playerRigid.velocity.y;
            var isJumpPressed = _direction.y > 0;

            if (_isGrounded) _allowDoubleJump = true;
            if (isJumpPressed)
            {
                yVelocity = CalculateJumpVelocity(yVelocity);


            }
            else if (_playerRigid.velocity.y > 0)
            {
                yVelocity *= 0.5f;
            }

            return yVelocity;
        }

        private float CalculateJumpVelocity(float yVelocity)
        {
            var isFalling = _playerRigid.velocity.y <= 0.001f;
            if (!isFalling) return yVelocity;

            if (_isGrounded & isFalling)
            {
                yVelocity += _jumpForce;
            }
            else if (_allowDoubleJump)
            {
                yVelocity = _jumpForce;
                _allowDoubleJump = false;
            }

            return yVelocity;
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
                transform.localScale = Vector3.one;
                

            }
            else if (_direction.x < 0)
            {
                transform.localScale = new Vector3(-1, 1, 1);
                
            }
        }

        public void SaySomething()
        {
            Debug.Log("something");
        }


        public void TakeDamage()
        {
            _animator.SetTrigger(Hit);
            _playerRigid.velocity = new Vector2(_playerRigid.velocity.x, _damageJumpForce);


           if(_coins > 0)
            {
                SpawnCoins();
            }
           
        }

        public void AddCoins(int coins)
        {
            _coins += coins;
            Debug.Log($"Coins Added {_coins}");
        }

        private void SpawnCoins()
        {
            var numCoinsToDispose = Mathf.Min(_coins, 5);
            _coins -= numCoinsToDispose;

            var burst = _hitParicles.emission.GetBurst(0);
            burst.count = numCoinsToDispose;
            _hitParicles.emission.SetBurst(0, burst);
            _hitParicles.gameObject.SetActive(true);
            _hitParicles.Play();
        }

        public void Interact()
        {
            var size = Physics2D.OverlapCircleNonAlloc(
                transform.position,
                _interactionRadius,
                _interactionResult,
                _interactionLayer);

            for (int i = 0; i < size; i++)
            {
                var interactble = _interactionResult[i].GetComponent<InteractbleComponent>();
                if (interactble != null) interactble.Interact();
            }
        }


        public void SpawnFootDust()
        {
            _footStepParticles.Spawn();
        }

        public void DropDust()
        {
            if(_playerRigid.velocity.y < _dropCheck && _isGrounded)
            {
                _fallDust.Spawn();
            }
        }

        public void SpawnJumpDust()
        {
            _jumpDust.Spawn();
        }


    }
}