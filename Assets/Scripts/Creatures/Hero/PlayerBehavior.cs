using UnityEngine;
using Scripts.Creatures;
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

        public void OnInteract(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _playerState.Interact();
            }
        }

        public void Invoke(InputAction.CallbackContext context)
        {
            if(context.performed)
            {
                
                _playerState.Attack();
            }
        }

        public void OnThrow(InputAction.CallbackContext context)
        {
            if(context.started)
            {
                _playerState.StartThrowing();
            }
            if(context.canceled)
            {
                _playerState.PerformThrowing();
            }

        }

        public void OnUsePotions(InputAction.CallbackContext context)
        {
            if(context.performed)
            {
                _playerState.Heal();
            }
        }
    }
}