using UnityEngine;
using Scripts.Creatures;
using Scripts.Utils;
using Scripts.Components;
using System.Collections;

namespace Scripts.Creatures.Mobs
{
    class PinkStarEnemy : MobAI
    {

        
        private static readonly int isSpining = Animator.StringToHash("isSpining");
        private static readonly int isRunning = Animator.StringToHash("isRunning");
        private static readonly int AttackKey = Animator.StringToHash("attack");
        private float _startDelay = 1f;
       
       

        

        public override void OnHeroInVision(GameObject go)
        {
            if(_vision.isTouchingLayer)
            {
                _animator.SetTrigger(AttackKey);

                StartCoroutine(StartDelay());
                _animator.SetTrigger(isSpining);

            }

            

            base.OnHeroInVision(go);
            
            
           
            
        }

        protected override IEnumerator GoToHero()
        {
            if(_vision.isTouchingLayer == false)
            {
                _animator.ResetTrigger(isSpining);
                StopCoroutine(StartDelay());
                _animator.SetTrigger(isRunning);

                _creature.Speed = 2f;

            }

            return base.GoToHero();
            
           
        }



        protected override Vector2 GetDirectonToTarget()
        {

            var direction = _target.transform.position - transform.position;
            direction.y = 0;
            return direction;
        }

        private IEnumerator StartDelay()
        {
           
            _creature.Speed = 0f;
            yield return new WaitForSeconds(_startDelay);
            _creature.Speed = 8f;
        }



    }
}
