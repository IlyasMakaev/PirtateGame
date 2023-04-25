using System.Collections;
using UnityEngine;
using Scripts;

namespace Scripts.Creatures
{
    class PlatformPatrol : Patrol
    {
        [SerializeField] private LayerCheck _groundCheck;
        [SerializeField] private Creature _creature;
        [SerializeField] private int _direction;
        public override IEnumerator DoPatrol()
        {
           while(enabled)
            {
                if(_groundCheck.isTouchingLayer)
                {
                    _creature.SetDirection(new Vector2(_direction, 0));

                }
                else
                {
                    _direction = -_direction;
                    _creature.SetDirection(new Vector2(_direction, 0));
                }
                yield return null;
            }
        }
    }
}
