using System;
using System.Collections;
using UnityEngine;
using Scripts.Components;

namespace Scripts.Creatures
{
    class MobAI : MonoBehaviour
    {
        [SerializeField] protected LayerCheck _vision;
        [SerializeField] private LayerCheck _canAttack;


        [SerializeField] private float _alarmDelay = 0.5f;
        [SerializeField] private float _attackCooldown = 2f;
        [SerializeField] private float _missHeroCooldown = 0.5f;

        private Coroutine _current;
        protected GameObject _target;

        
        private SpawnListComponent _particles;
        protected Creature _creature;
        protected Animator _animator;
        private bool _isDead;
        private Patrol _patrol;
        



        private void Awake()
        {
            _particles = GetComponent<SpawnListComponent>();
            _creature = GetComponent<Creature>();
            _animator = GetComponent<Animator>();
            _patrol = GetComponent<Patrol>();

        }

        private void Start()
        {
            StartState(_patrol.DoPatrol());
        }

        public virtual void OnHeroInVision(GameObject go)
        {
            if (_isDead) return;
            _target = go;
            
            StartState(AgrotoHero());
        }

        private IEnumerator AgrotoHero()
        {
            LookAtHero();
            _particles.Spawn("Exclamation");
            yield return new WaitForSeconds(_alarmDelay);
            StartState(GoToHero());
        }



        private void LookAtHero()
        {
            var direction = GetDirectonToTarget();
            _creature.UpdateSpriteDirection(direction);
        }

        protected virtual IEnumerator GoToHero()
        {
            while(_vision.isTouchingLayer)
            {

                if(_canAttack.isTouchingLayer)
                {
                    StartState(Attack());
                }
                else
                {
                    SetDirectionToTarget();
                }

                
                yield return null;
            }

            _creature.SetDirection(Vector2.zero);
            _particles.Spawn("Miss");
           
            yield return new WaitForSeconds(_missHeroCooldown);

            

            StartState(_patrol.DoPatrol());
        }

        private  IEnumerator Attack()
        {
            while(_canAttack.isTouchingLayer)
            {
                _creature.Attack();
                yield return new WaitForSeconds(_attackCooldown);
            }

            StartState(GoToHero());
        }

        private void SetDirectionToTarget()
        {
            var direction = GetDirectonToTarget();
            _creature.SetDirection(direction);
        }

        protected virtual Vector2 GetDirectonToTarget()
        {
            var direction = _target.transform.position - transform.position;
            direction.y = 0;
            return direction.normalized;
        }


        private void StartState(IEnumerator coroutine)
        {
            _creature.SetDirection(Vector2.zero);

            if (_current != null)
                StopCoroutine(_current);
            _current = StartCoroutine(coroutine);

        }

        public virtual void OnDie()
        {
           
            _isDead = true;
            _animator.SetBool(Creature.IsDeadKey, true);
            _creature.SetDirection(Vector2.zero);
            if (_current != null)
            {
                StopCoroutine(_current);
            }
        }
    }
}
