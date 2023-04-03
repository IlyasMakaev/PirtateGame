using UnityEngine;
using UnityEngine.InputSystem;


namespace Scripts
{
    public class PlayerBehavior : MonoBehaviour
    {
        [SerializeField] private PlayerState _playerState;
        private int _coins = 0;
        public void OnMovement(InputAction.CallbackContext context)
        {
            var direction = context.ReadValue<Vector2>();
            _playerState.SetDirection(direction);
        }

        public void AddCoins(int coins)
        {
            _coins += coins;
            Debug.Log($"Coins Added {_coins}");
        }

        public void OnSaySomething(InputAction.CallbackContext context)
        {
            if (context.canceled)
            {
                _playerState.SaySomething();
            }


        }
    }
}