using UnityEngine;
using System.Collections.Generic;
using Scripts.Utils;
using System.Linq;
using Scripts.Components;

namespace Scripts.Creatures.Mobs
{
    class TotemTower : MonoBehaviour
    {
        [SerializeField] private List<TotemTrapAI> _traps;
        [SerializeField] private Cooldown _cooldown;

        private int _currentTrap;

        private void Start()
        {
            foreach (var totemTrapAI in _traps)
            {
                totemTrapAI.enabled = false;
                var hp = totemTrapAI.GetComponent<HealthComponent>();
                hp._onDie.AddListener(() => OnTrapDead(totemTrapAI));
            }
        }

        private void OnTrapDead(TotemTrapAI totemTrapAI)
        {
            var index = _traps.IndexOf(totemTrapAI);
            _traps.Remove(totemTrapAI);
            if(index < _currentTrap)
            {
                _currentTrap--;
            }

        }

        private void Update()
        {
            if(_traps.Count == 0)
            {
                enabled = false;
                Destroy(gameObject, 1f);
            }
            var hasAnyTarget = _traps.Any(x => x._vision.isTouchingLayer);
            if(hasAnyTarget)
            {
                if(_cooldown.IsReady)
                {
                    _traps[_currentTrap].Shoot();
                    _cooldown.Reset();
                    _currentTrap = (int) Mathf.Repeat(_currentTrap + 1, _traps.Count);

                }
            }
        }

    }
}
