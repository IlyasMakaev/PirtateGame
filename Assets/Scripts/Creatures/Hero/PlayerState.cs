using Scripts.Components;
using Scripts.Utils;
using Scripts.Model;
using Scripts.Model.Data;
using System.Collections;
using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;


namespace Scripts.Creatures
{
    public class PlayerState : Creature, ICanAddInventory 
    {
        [SerializeField] private CheckCircleOverlap _interactionCheck;
        [SerializeField] private LayerMask _interactionLayer;
        
        [SerializeField] private int _swordThrow;
        [SerializeField] private ParticleSystem _hitParicles;

        [Header("Super Throw")]
        [SerializeField] private Cooldown _superThrowCooldown;
        [SerializeField] private int _superThrowParticles;
        [SerializeField] private float _superThrowDelay;

        [SerializeField] private Cooldown _throwCooldown;
        [SerializeField] private AnimatorController _armed;
        [SerializeField] private AnimatorController _disarmed;



        

        private static readonly int ThrowKey = Animator.StringToHash("Throw");

      
        private GameSession _gameSession;
        private DamageComponent _damageComponent;
     

        private int CoinCount => _gameSession.Data.Inventory.Count("Coin");
        private int SwordCount => _gameSession.Data.Inventory.Count("Sword");
        private int PotionsCount => _gameSession.Data.Inventory.Count("HealPotions");

        private bool _superThrow;
        private bool _allowDoubleJump;
        private bool _isOnWall;

        protected override void Awake() 
        {
            base.Awake();
           
        }


        private void Start()
        {
            _gameSession = FindObjectOfType<GameSession>();
            var health = GetComponent<HealthComponent>();
            
            _gameSession.Data.Inventory.OnChanged += OnInventoryChanged;
            _gameSession.Data.Inventory.OnChanged += AnotherHandler;
            health.SetHealth(_gameSession.Data.Hp);
            UpdateHeroWeapon();
        }

        private void OnDestroy()
        {
            _gameSession.Data.Inventory.OnChanged -= OnInventoryChanged;
            _gameSession.Data.Inventory.OnChanged -= AnotherHandler;
        }

        private void AnotherHandler(string id, int value)
        {
            Debug.Log($"Inventory changed{id}: {value}");
        }
        private void OnInventoryChanged(string id, int value)
        {
            if (id == "Sword")
                UpdateHeroWeapon();
        }

        public void OnHealhChange(int currentHealth)
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
                DoJumpVFX(); 
                return _jumpForce;
                
            }

            return base.CalculateJumpVelocity(yVelocity);
        }

        public override void TakeDamage()
        {
            base.TakeDamage();
           if(CoinCount > 0)
            {
                SpawnCoins();
            }
           
        }

        public void AddInInventory(string id, int value)
        {
            _gameSession.Data.Inventory.Add(id, value);
        }

      

    

    

        private void SpawnCoins()
        {
            
            var numCoinsToDispose = Mathf.Min(CoinCount, 5);
            _gameSession.Data.Inventory.Remove("Coin", numCoinsToDispose);
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
            if (SwordCount <= 0) return;
            
            base.Attack();
            
        }

        

        public void OnSwordParticles()
        {
            _particles.Spawn("SwordEffect");
        }


        private void UpdateHeroWeapon()
        {

            Animator.runtimeAnimatorController = SwordCount > 0 ? _armed : _disarmed;

            
        }

        public void OnDoThrow()
        {
            if(_superThrow)
            {
                var numThrows = Mathf.Min(_superThrowParticles, SwordCount - 1);
                StartCoroutine(DoSuperThrow(numThrows));
            }
            else
            {
                ThrowAndRemoveFromInventory();
            }

            _superThrow = false;
            
        }

        private IEnumerator DoSuperThrow(int numThrows)
        {
            for(int i = 0; i < numThrows; i++)
            {
                ThrowAndRemoveFromInventory();
                yield return new WaitForSeconds(_superThrowDelay);
            }
        }

        private void ThrowAndRemoveFromInventory()
        {
            Sounds.Play("Range");
            _particles.Spawn("Throw");
            _gameSession.Data.Inventory.Remove("Sword", 1);

        }


        public void StartThrowing()
        {
            _superThrowCooldown.Reset();
        }

        public void PerformThrowing()
        {
           

            if (_throwCooldown.IsReady && SwordCount > 1)
            {
              
                
                _throwCooldown.Reset();
                
                
                
            }
            else return;
            
            if (_superThrowCooldown.IsReady) _superThrow = true;
            Animator.SetTrigger(ThrowKey);
            _throwCooldown.Reset();
            
        }

        public void Heal()
        {
            _damageComponent = FindObjectOfType<DamageComponent>();
            if (PotionsCount <= 0) return;
            else
            {
                _damageComponent.Apply(gameObject);
                _gameSession.Data.Inventory.Remove("HealPotions", 1);
            }
            
           

        }
    }
}