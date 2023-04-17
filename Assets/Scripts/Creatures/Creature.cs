using Scripts.Utils;
using UnityEngine;
using Scripts.Components;
using Scripts;
using UnityEngine.PlayerLoop;
using Scripts.Model;


namespace Scripts.Creatures
{
    public class Creature : MonoBehaviour
    {
        [Header("Params")]
        [SerializeField] protected float _speed;
        [SerializeField] protected float _jumpForce;
        [SerializeField] private float _damageVelocity;
        [SerializeField] private int _damage;

        [Header("Checkers")]
        [SerializeField] protected LayerMask _groundLayer;
        [SerializeField] private LayerCheck _groundCheck;
        [SerializeField] private CheckCircleOverlap _attackRange;
        [SerializeField] protected SpawnListComponent _particles;

        protected Rigidbody2D _rigidbody;
        protected Vector2 _direction;
        protected Animator _animator;
        protected bool _isGrounded;
        private bool _isJumping;

        private static readonly int isGorundKey = Animator.StringToHash("isGrounded");
        private static readonly int isRunning = Animator.StringToHash("isRunning");
        private static readonly int verticalVelocity = Animator.StringToHash("verticalVelocity");
        private static readonly int Hit = Animator.StringToHash("hit");
        private static readonly int AttackKey = Animator.StringToHash("attack");

        protected virtual void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
        }

        protected virtual void Update()
        {
            _isGrounded = _groundCheck.isTouchingLayer;
        }

        public void SetDirection(Vector2 direction)
        {
            _direction = direction;
        }
        private void FixedUpdate()
        {
            var xVelocity = _direction.x * _speed;
            var yVelocity = CalculateYVelocity();
            _rigidbody.velocity = new Vector2(xVelocity, yVelocity);



            _animator.SetBool(isGorundKey, _isGrounded);
            _animator.SetBool(isRunning, _direction.x != 0);
            _animator.SetFloat(verticalVelocity, _rigidbody.velocity.y);

            UpdateSpriteDirection();
        }

        protected virtual float CalculateYVelocity()
        {
            var yVelocity = _rigidbody.velocity.y;
            var isJumpPressed = _direction.y > 0;

            if (_isGrounded)
            {
               
                _isJumping = false;
            }

            if (isJumpPressed)
            {
                _isJumping= true;

                var isFalling = _rigidbody.velocity.y <= 0.001f;
                yVelocity = isFalling ? CalculateJumpVelocity(yVelocity) : yVelocity;


            }
            else if (_rigidbody.velocity.y > 0 && _isJumping)
            {
                yVelocity *= 0.5f;
            }

            return yVelocity;
        }

        protected virtual float CalculateJumpVelocity(float yVelocity)
        {
            

            if (_isGrounded)
            {
                yVelocity += _jumpForce;
                _particles.Spawn("Jump");
            }

            return yVelocity;
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

        public virtual void TakeDamage()
        {
            _animator.SetTrigger(Hit);
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _damageVelocity);



        }

        public virtual void Attack()
        {
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

    }
}
