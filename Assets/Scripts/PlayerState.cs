using Scripts.Components;
using Scripts.Utils;
using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;
using Scripts.Model;

namespace Scripts
{
    public class PlayerState : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private float _jumpForce;
        [SerializeField] private float _damageJumpForce;
        [SerializeField] private LayerCheck _groundCheck;
        [SerializeField] private float _interactionRadius;
        [SerializeField] private ParticleSystem _hitParicles;
        [SerializeField] private AnimatorController _armed;
        [SerializeField] private AnimatorController _disarmed;

        [SerializeField] private LayerMask _interactionLayer;
        [SerializeField] private LayerMask _groundLayer;
        [SerializeField] private CheckCircleOverlap _attackRange;

        [SerializeField] private SpawnComponent _swordEffect;
        [SerializeField] private SpawnComponent _footStepParticles;
        [SerializeField] private SpawnComponent _jumpDust;
        [SerializeField] private SpawnComponent _fallDust;

        [SerializeField] private int _damage;


        private Rigidbody2D _playerRigid;
        private Vector2 _direction;
        private Animator _animator;
        private bool _isGrounded;
        private bool _allowDoubleJump;
        private float _dropCheck = 14f;
        private Collider2D[] _interactionResult = new Collider2D[1];
        private Vector3 _groundCheckPositionDelta;

        private static readonly int isGorundKey = Animator.StringToHash("isGrounded");
        private static readonly int isRunning = Animator.StringToHash("isRunning");
        private static readonly int verticalVelocity = Animator.StringToHash("verticalVelocity");
        private static readonly int Hit = Animator.StringToHash("hit");
        private static readonly int AttackKey = Animator.StringToHash("attack");
        
        private GameSession _gameSession;




        private void Awake()
        {
            _playerRigid = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
            _groundCheckPositionDelta = new Vector3(0f, -0.3f, 0f);
        }

      
        private void Start()
        {
            _gameSession= FindObjectOfType<GameSession>();
            var health = GetComponent<HealthComponent>();
            health.SetHealth(_gameSession.Data.Hp);
            UpdateHeroWeapon();
        }
        public void OnHealthChange(int currentHealth)
        {
            _gameSession.Data.Hp = currentHealth;
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

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {

            Handles.color = IsGrounded() ? HandlesUtils.TransperentGreen : HandlesUtils.TransperentRed;
            Handles.DrawSolidDisc(transform.position + _groundCheckPositionDelta, Vector3.forward, 0.3f);


        }
#endif


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


           if(_gameSession.Data.Coins > 0)
            {
                SpawnCoins();
            }
           
        }

        public void AddCoins(int coins)
        {
            _gameSession.Data.Coins += coins;
            Debug.Log($"Coins Added {_gameSession.Data.Coins}");
        }

        private void SpawnCoins()
        {
            var numCoinsToDispose = Mathf.Min(_gameSession.Data.Coins, 5);
            _gameSession.Data.Coins -= numCoinsToDispose;

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

        private void OnCollisionEnter2D(Collision2D collision)
        {

            if(collision.gameObject.isInLayer(_groundLayer))
            {
                var contact = collision.contacts[0];
                if(contact.relativeVelocity.y >= _dropCheck)
                {
                    _fallDust.Spawn();
                }
            }
                
        }

        public void SpawnFootDust()
        {
            _footStepParticles.Spawn();
        }

        public void SpawnJumpDust()
        {
            _jumpDust.Spawn();
        }

        public void Attack()
        {
            if (!_gameSession.Data.IsArmed) return;
            _animator.SetTrigger(AttackKey);
            
        }

        public void OnAttack()
        {
            var gos = _attackRange.GetObjectsInRange();
            foreach (var go in gos)
            {
                var hp = go.GetComponent<HealthComponent>();
                if (hp != null && go.CompareTag("Barrel"))
                {
                    hp.ApplyDamage(_damage);
                }
            }
        }

        public void OnSwordParticles()
        {
            _swordEffect.Spawn();
        }

        public void ArmHero()
        {
            _gameSession.Data.IsArmed = true;
            UpdateHeroWeapon();
            _animator.runtimeAnimatorController = _armed;
        }

        private void UpdateHeroWeapon()
        {

            _animator.runtimeAnimatorController = _gameSession.Data.IsArmed ? _armed : _disarmed;
            
        }

    }
}