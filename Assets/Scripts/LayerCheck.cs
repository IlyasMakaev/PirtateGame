using UnityEngine;


namespace Scripts
{
    public class LayerCheck : MonoBehaviour
    {
        [SerializeField] LayerMask _layer;
        [SerializeField] private bool _isTouchingLayer;

        private Collider2D _collider2D;

        public bool isTouchingLayer => _isTouchingLayer;

        public void Awake()
        {
            _collider2D = GetComponent<Collider2D>();
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            _isTouchingLayer = _collider2D.IsTouchingLayers(_layer);
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            _isTouchingLayer = _collider2D.IsTouchingLayers(_layer);
        }

    }
}
