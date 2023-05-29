﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Scripts.Model.Definitions.Editor
{
    [CustomPropertyDrawer(typeof(InventoryIdAttribute))]
    class InventoryIdAttributeDrawer : PropertyDrawer
    {
        

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var defs = DefsFacade.I.Items.ItemsforEditor;
            var ids = new List<string>();
            foreach (var itemDef in defs)
            {
                ids.Add(itemDef.Id);
            }

            var index = Math.Max(ids.IndexOf(property.stringValue), 0);

            index = EditorGUI.Popup(position, property.displayName, index, ids.ToArray());
            property.stringValue = ids[index];
        }
    }
}
