using UnityEngine;
using Scripts.Utils;
using Scripts.Components;
using System;

namespace Scripts.Creatures.Mobs
{
    class ShootingTrapAI : MonoBehaviour
    {
        [SerializeField] private LayerCheck _vision;

        [Header("Melee")]
        [SerializeField] private Cooldown _meleeCooldown;
        [SerializeField] private CheckCircleOverlap _meleeAttack;
        [SerializeField] private LayerCheck _meleeCanAttack;
        [SerializeField] private bool _cantDoMelee;

        [Header("Range")]
        [SerializeField] private SpawnComponent _rangeAttack;
        [SerializeField] private Cooldown _rangeCooldown;

        public static readonly int Melee = Animator.StringToHash("melee");
        public static readonly int Range = Animator.StringToHash("range");

        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }
        private void Update()
        {
            if(_vision.isTouchingLayer)
            {
                if(!_cantDoMelee)
                {
                    if (_meleeCanAttack.isTouchingLayer)
                    {
                        if (_meleeCooldown.IsReady)
                        {
                            MeleeAttack();
                            return;
                        }
                    }
                }
              

                if(_rangeCooldown.IsReady)
                {
                    RangeAttack();
                }
            }
        }

        private void RangeAttack()
        {
            _rangeCooldown.Reset();
            _animator.SetTrigger(Range);
        }

        private void MeleeAttack()
        {
            _meleeCooldown.Reset();
            _animator.SetTrigger(Melee);
        }

        public void OnMeleeAttack()
        {
            _meleeAttack.Check();
        }

        public void OnRangeAttack()
        {
            _rangeAttack.Spawn();
        }
    }
}
