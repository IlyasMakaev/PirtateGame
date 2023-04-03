﻿using System;
using UnityEngine;
using UnityEngine.Events;

namespace Scripts.Components
{
    public class DamageComponent : MonoBehaviour
    {
        [SerializeField] private int _damage;
        [SerializeField] private int _heal;

       

        public void Apply(GameObject target)
        {
            var healthComponent = target.GetComponent<HealthComponent>();
            if (healthComponent != null && gameObject.CompareTag("Potion"))
            {
                
                healthComponent.ApplyHeal(_heal);
                
               
                Debug.Log($"Current health is {healthComponent.Health}");
            }else if(healthComponent != null)
            {
                healthComponent.ApplyDamage(_damage);
                Debug.Log($"Current health is {healthComponent.Health}");
            }

        }


    }


}

