using UnityEngine;
using UnityEngine.InputSystem;


namespace Scripts.Components
{
    public class PlayerBehavior : MonoBehaviour
    {
        [SerializeField] private PlayerState _playerState;
        
        public void OnMovement(InputAction.CallbackContext context)
        {
            var direction = context.ReadValue<Vector2>();
            _playerState.SetDirection(direction);
        }

     

        public void OnSaySomething(InputAction.CallbackContext context)
        {
            if (context.canceled)
            {
                _playerState.SaySomething();
            }


        }
        public void OnInteract(InputAction.CallbackContext context)
        {
            if (context.canceled)
            {
                _playerState.Interact();
            }
        }

        public void Invoke(InputAction.CallbackContext context)
        {
            if(context.canceled)
            {
                _playerState.Attack();
            }
        }
    }
}