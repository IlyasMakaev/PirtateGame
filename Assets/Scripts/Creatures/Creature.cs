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
        [SerializeField] private bool _invertScale;
        [SerializeField] protected float _speed;
        [SerializeField] protected float _jumpForce;
        [SerializeField] private float _damageVelocity;
        [SerializeField] private int _damage;
        [SerializeField] private float _dropCheck = 10f;

        [Header("Checkers")]
        [SerializeField] protected LayerMask _groundLayer;
        [SerializeField] private LayerCheck _groundCheck;
        [SerializeField] private CheckCircleOverlap _attackRange;
        [SerializeField] protected SpawnListComponent _particles;

        protected Rigidbody2D Rigidbody;
        protected Vector2 DIrection;
        protected Animator Animator;
        protected bool IsGrounded;
        private bool _isJumping;

        public static readonly int IsDeadKey = Animator.StringToHash("isDead");
        private static readonly int isGorundKey = Animator.StringToHash("isGrounded");
        private static readonly int isRunning = Animator.StringToHash("isRunning");
        private static readonly int verticalVelocity = Animator.StringToHash("verticalVelocity");
        private static readonly int Hit = Animator.StringToHash("hit");
        private static readonly int AttackKey = Animator.StringToHash("attack");

        protected virtual void Awake()
        {
            Rigidbody = GetComponent<Rigidbody2D>();
            Animator = GetComponent<Animator>();
          
        }

        protected virtual void Update()
        {
            IsGrounded = _groundCheck.isTouchingLayer;
            
        }

        public void SetDirection(Vector2 direction)
        {
            DIrection = direction;
        }
        private void FixedUpdate()
        {
            var xVelocity = DIrection.x * _speed;
            var yVelocity = CalculateYVelocity();
            Rigidbody.velocity = new Vector2(xVelocity, yVelocity);



            Animator.SetBool(isGorundKey, IsGrounded);
            Animator.SetBool(isRunning, DIrection.x != 0);
            Animator.SetFloat(verticalVelocity, Rigidbody.velocity.y);

            UpdateSpriteDirection();
        }


        protected virtual float CalculateJumpVelocity(float yVelocity)
        {


            if (IsGrounded)
            {
                yVelocity += _jumpForce;
                
            }

            return yVelocity;
        }

        protected virtual float CalculateYVelocity()
        {
            var yVelocity = Rigidbody.velocity.y;
            var isJumpPressed = DIrection.y > 0;

            if (IsGrounded)
            {
                _isJumping = false;
            }

            if (isJumpPressed)
            {
                _isJumping= true;
                var isFalling = Rigidbody.velocity.y <= 0.001f;
                yVelocity = isFalling ? CalculateJumpVelocity(yVelocity) : yVelocity;


            }
            else if (Rigidbody.velocity.y > 0 && _isJumping)
            {
                yVelocity *= 0.5f;
            }

            return yVelocity;
        }


        private void UpdateSpriteDirection()
        {

            var multiplier = _invertScale ? -1 : 1;
            if (DIrection.x > 0)
            {
                transform.localScale = new Vector3(multiplier, 1, 1);


            }
            else if (DIrection.x < 0)
            {
                transform.localScale = new Vector3(-1 * multiplier, 1, 1);

            }
        }

        public virtual void TakeDamage()
        {
            Animator.SetTrigger(Hit);
            Rigidbody.velocity = new Vector2(Rigidbody.velocity.x, _damageVelocity);



        }

        public virtual void Attack()
        {
           
            Animator.SetTrigger(AttackKey);

        }

        public void OnAttackRange()
        {
            _attackRange.Check();
        }


        public void OnCollisionEnter2D(Collision2D collision)
        {

            if (collision.gameObject.IsInLayer(_groundLayer))
            {
                var contact = collision.contacts[0];
                if (contact.relativeVelocity.y >= _dropCheck)
                {
                    _particles.Spawn("FallDust");
                }
            }

        }

    }
}
