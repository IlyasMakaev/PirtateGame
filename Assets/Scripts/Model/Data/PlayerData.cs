using Scripts.Model.Data;
using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Scripts.Model
{
    [Serializable]
    internal class PlayerData
    {
        [SerializeField] private InventoryData _inventory;

        public InventoryData Inventory => _inventory;

        public int Hp;
        
        
    }
}
