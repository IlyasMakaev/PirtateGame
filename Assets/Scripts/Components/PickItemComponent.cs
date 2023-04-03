using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Scripts.Components
{
    public class PickItemComponent : MonoBehaviour
    {
        [SerializeField]private int _coins;

        private PlayerState _playerState;
        
        private void Start()
        {
            _playerState = FindObjectOfType<PlayerState>();

            
        }

        public void Add()
        {
            _playerState.AddCoins(_coins);
        }



    }

}

