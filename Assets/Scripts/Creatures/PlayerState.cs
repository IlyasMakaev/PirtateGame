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
        [SerializeField] private CheckCircleOverlap _interactionCheck;
        [SerializeField] private LayerMask _interactionLayer;

        [SerializeField] private ParticleSystem _hitParicles;

        [SerializeField] private AnimatorController _armed;
        [SerializeField] private AnimatorController _disarmed;
        private bool _allowDoubleJump;
        private static readonly int ThrowKey = Animator.StringToHash("Throw");



        private GameSession _gameSession;

        protected override void Awake()
        {
            base.Awake();
           
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
            if (IsGrounded ) //isOnWall
            {
                _allowDoubleJump = true;
            }   

            return base.CalculateYVelocity();
        }

        protected override float CalculateJumpVelocity(float yVelocity)
        {
           
     
            if (!IsGrounded && _allowDoubleJump)
            {
                _allowDoubleJump = false;
                return _jumpForce;
                
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
            _interactionCheck.Check();
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
            Animator.runtimeAnimatorController = _armed;
        }

        private void UpdateHeroWeapon()
        {

            Animator.runtimeAnimatorController = _gameSession.Data.IsArmed ? _armed : _disarmed;
            
        }

        public void OnDoThrow()
        {
            _particles.Spawn("Throw");
        }

        public void Throw()
        {
            Animator.SetTrigger(ThrowKey);
        }

    }
}