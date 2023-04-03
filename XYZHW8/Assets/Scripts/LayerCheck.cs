using UnityEngine;

public class LayerCheck : MonoBehaviour
{
    [SerializeField] LayerMask _layerCheck;
    private Collider2D _collider2D;

    public bool IsGrounded;

    public void Awake()
    {
        _collider2D = GetComponent<Collider2D>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        IsGrounded = _collider2D.IsTouchingLayers(_layerCheck);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        IsGrounded = _collider2D.IsTouchingLayers(_layerCheck);
    }
}
