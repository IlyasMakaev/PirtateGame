using Scripts.Model;
using Scripts.Model.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Scripts.Components.Collectbles
{
    class CollectorComponent : MonoBehaviour, ICanAddInventory
    {
        [SerializeField] private List<InventoryItemData> _items = new List<InventoryItemData>();
        public void AddInInventory(string id, int value)
        {
            _items.Add(new InventoryItemData(id) {Value = value });
        }

        public void DropInventory()
        {
            var session = FindObjectOfType<GameSession>();
            foreach (var inventoryItemData in _items)
            {
                session.Data.Inventory.Add(inventoryItemData.Id, inventoryItemData.Value);
            }
            _items.Clear();
        }
    }
}
