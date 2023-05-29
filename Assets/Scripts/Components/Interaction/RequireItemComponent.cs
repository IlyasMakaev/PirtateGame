using Scripts.Model;
using Scripts.Model.Data;
using Scripts.Model.Definitions;
using UnityEngine;
using UnityEngine.Events;

namespace Scripts.Components.Interaction
{
    class RequireItemComponent : MonoBehaviour
    {
        [SerializeField] private InventoryItemData[] _required;
    
        [SerializeField] private bool _removeAfterUse;

        [SerializeField] private UnityEvent _onSuccess;
        [SerializeField] private UnityEvent _onFail;


        public void Check()
        {
            var session = FindObjectOfType<GameSession>();
            var areAllRequirementsGet = true;
            foreach (var item in _required)
            {
                var numItems = session.Data.Inventory.Count(item.Id);
                if (numItems < item.Value)
                    areAllRequirementsGet = false;
            }
            
            if(areAllRequirementsGet)
            {
                if (_removeAfterUse)
                {
                    foreach (var item in _required)
                        session.Data.Inventory.Remove(item.Id, item.Value); 
                }

                _onSuccess?.Invoke();

            }
            else
            {
                _onFail?.Invoke();
            }
        }

    }
}
