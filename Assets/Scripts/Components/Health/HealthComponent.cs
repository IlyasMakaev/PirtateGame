using System;
using UnityEngine;
using UnityEngine.Events;

namespace Scripts.Components
{
    public class HealthComponent : MonoBehaviour
    {
        [SerializeField] private int _health;
        [SerializeField] public UnityEvent _onDie;
        [SerializeField] private UnityEvent _onTakeDamage;
        
        [SerializeField] private HealthChangeEvent _onChange;
        public int Health => _health;
        
        public void ApplyDamage(int damageValue)
        {
            _health -= damageValue;
            _onChange?.Invoke(_health);
            _onTakeDamage?.Invoke();
            if(_health <= 0)
            {
                _onDie?.Invoke();
            }
            
        }

        public void ApplyHeal(int healValue)
        {
            _health += healValue;
            _onChange?.Invoke(_health);
            
        }

        public void SetHealth(int health)
        {
            _health = health;
        }

#if UNITY_EDITOR
        [ContextMenu("Update Health")]
        private void UpdateHealth()
        {
            _onChange?.Invoke(_health);
        }
#endif
        private void OnDestroy()
        {
            _onDie.RemoveAllListeners();
        }


        [Serializable]
        public class HealthChangeEvent : UnityEvent<int>
        {

        }
    }
  
}
