using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Scripts.Utils
{
    class CheckCircleOverlap : MonoBehaviour
    {
        [SerializeField] private float _radius = 1f;
        [SerializeField] private LayerMask _mask;
        [SerializeField] private string[] _tags;
        [SerializeField] private OnOverLapEvent _onOverLap;
        private readonly Collider2D[] _interactionResult = new Collider2D[10];

        private void OnDrawGizmosSelected()
        {
            Handles.color = HandlesUtils.TransperentRed;
            Handles.DrawSolidDisc(transform.position, Vector3.forward, _radius); 
        }

        internal void Check()
        {
            var size = Physics2D.OverlapCircleNonAlloc(
                transform.position,
                _radius,
                _interactionResult,
                _mask);

            var overlaps = new List<GameObject>();
            for (var i = 0; i < size; i++)
            {
                var overlapResult = _interactionResult[i];
                var isInTags = _tags.Any(tag => overlapResult.CompareTag(tag));
                if(isInTags)
                { 
                    _onOverLap?.Invoke(overlapResult.gameObject);
                }
            }
        }
    }

    [Serializable]
    public class OnOverLapEvent : UnityEvent<GameObject>
    {

    }
}
