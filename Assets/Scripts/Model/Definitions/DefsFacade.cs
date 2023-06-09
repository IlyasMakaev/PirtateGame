﻿using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scripts.Model.Definitions
{
    [CreateAssetMenu(menuName = "Defs/Defsfacade", fileName = "Defsfacade")]
   public class DefsFacade : ScriptableObject
    {
        [SerializeField] private InventoryItemsDef _items;

        public InventoryItemsDef Items => _items;

        private static DefsFacade _instance;
        public static DefsFacade I => _instance == null ? LoadDefs() : _instance;

        private static DefsFacade LoadDefs()
        {
            return _instance = Resources.Load<DefsFacade>("DefsFacade");
        }
    }
}
