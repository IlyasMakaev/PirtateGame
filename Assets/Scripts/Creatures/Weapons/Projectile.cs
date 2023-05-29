using UnityEngine;

namespace Scripts.Creatures.Weapons
{
    public class Projectile : BaseProjectile
    {
       
        protected override void Start()
        {

            base.Start();
            var force = new Vector2(Direction * _speed, 0);
            Rigidbody.AddForce(force, ForceMode2D.Impulse);

        }


        /*private void FixedUpdate()
        {
            var position = _rigidbody.position;
            position.x += _direction *_speed;
            _rigidbody.MovePosition(position);
        }*/
    }
}
