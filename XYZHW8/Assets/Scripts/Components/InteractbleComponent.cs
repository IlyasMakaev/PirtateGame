using UnityEngine;
using UnityEngine.Events;


namespace Scripts.Components
{
    public class InteractbleComponent : MonoBehaviour
    {
        [SerializeField] private UnityEvent _action;

        public void Interact()
        {
            _action?.Invoke();
        }
    }

}
