using UnityEngine;

namespace Scripts
{
    public class LayerCheck : MonoBehaviour
    {
        [SerializeField] protected LayerMask _layer;
        [SerializeField] protected bool _isTouchingLayer;
        public bool isTouchingLayer => _isTouchingLayer;
    }
}
