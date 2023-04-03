using System;
using UnityEngine;
using UnityEngine.Events;

namespace Scripts.Components
{
    public class HealthComponent : MonoBehaviour
    {
        [SerializeField] private int _health;
        [SerializeField] private UnityEvent _onTakeDamage;
        [SerializeField] private UnityEvent _onDie;
        public int Health => _health;
        
        public void ApplyDamage(int damageValue)
        {
            _health -= damageValue;
            _onTakeDamage?.Invoke();
            if(_health == 0)
            {
                _onDie?.Invoke();
            }
        }

        public void ApplyHeal(int healValue)
        {
            _health += healValue;
            
        }
    }

}
