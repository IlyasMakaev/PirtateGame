using Scripts.Components;
using Scripts.Utils;
using Scripts.Model;
using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;


namespace Scripts.Creatures
{
    public class PlayerState : Creature
    {

        [SerializeField] private float _interactionRadius;
        [SerializeField] private LayerMask _interactionLayer;

        [SerializeField] private ParticleSystem _hitParicles;

        [SerializeField] private AnimatorController _armed;
        [SerializeField] private AnimatorController _disarmed;

        private float _dropCheck = 14f;
        private bool _allowDoubleJump;

        private Collider2D[] _interactionResult = new Collider2D[1];
        private Vector3 _groundCheckPositionDelta;
        private GameSession _gameSession;







        protected override void Awake()
        {
            base.Awake();
            _groundCheckPositionDelta = new Vector3(0f, -0.3f, 0f);
        }


        private void Start()
        {
            _gameSession = FindObjectOfType<GameSession>();
            var health = GetComponent<HealthComponent>();
            health.SetHealth(_gameSession.Data.Hp);
            UpdateHeroWeapon();
        }
        public void OnHealthChange(int currentHealth)
        {
            _gameSession.Data.Hp = currentHealth;
        }



        protected override void Update()
        {
            base.Update();

        }

        protected override float CalculateYVelocity()
        {
            var yVelocity = _rigidbody.velocity.y;
            var isJumpPressed = _direction.y > 0;

            if (_isGrounded ) //isOnWall
            {
                _allowDoubleJump = true;
            }

            if(!isJumpPressed)//isOnwall
            {
                return 0f;
            }


            return base.CalculateYVelocity();
        }

        protected override float CalculateJumpVelocity(float yVelocity)
        {
           
     
            if (!_isGrounded && _allowDoubleJump)
            {
                _particles.Spawn("Jump");
                _allowDoubleJump = false;
                yVelocity = _jumpForce;
                
            }

            return base.CalculateJumpVelocity(yVelocity);
        }

        public override void TakeDamage()
        {
            base.TakeDamage();


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
                    _particles.Spawn("FallDust");
                }
            }
                
        }

        public override void Attack()
        {
            if (!_gameSession.Data.IsArmed) return;
            base.Attack();
            
        }

        

        public void OnSwordParticles()
        {
            _particles.Spawn("SwordEffect");
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