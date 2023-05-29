using System;
using UnityEngine;

namespace Scripts.Components.Movement
{
    class CircularMovement : MonoBehaviour
    {
        [SerializeField] private float _radius = 1f;
        [SerializeField] private float _speed = 1f;
        private Rigidbody2D[] _boddies;
        private Vector2[] _positions;
        private float _time;

        private void Awake()
        {
            UpdateContent();
        }

        private void UpdateContent()
        {
           
            _boddies = GetComponentsInChildren<Rigidbody2D>();
            _positions = new Vector2[_boddies.Length];

        }

      

        private void Update()
        {
            var isAllDead = true;
            CalculatePositions();
            for (var i = 0; i < _boddies.Length; i++)
            {   if(_boddies[i])
                {
                    _boddies[i].MovePosition(_positions[i]);
                    isAllDead = false;
                }
                
            }
            if(isAllDead)
            {
                enabled = false;
                Destroy(gameObject, 1f);
            }
            _time += Time.deltaTime;
        }
        private void CalculatePositions()
        {
            var step = 2 * Mathf.PI / _boddies.Length;

            Vector2 containerPosition = transform.position;
            for (var i = 0; i < _boddies.Length; i++)
            {
                var angle = step * i;
                var pos = new Vector2(
                    Mathf.Cos(angle + _time * _speed) * _radius,
                    Mathf.Sin(angle + _time * _speed) * _radius
                    );
                _positions[i] = containerPosition + pos;
            }
        }
#if UNITY_EDITOR
        private void OnValidate()
        {
            UpdateContent();
            CalculatePositions();
            for (var i = 0; i < _boddies.Length; i++)
            {
                _boddies[i].transform.position = _positions[i];
            }
        }

        private void OnDrawGizmosSelected()
        {
            UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.forward, _radius);
        }

#endif

        

        
    }
}
