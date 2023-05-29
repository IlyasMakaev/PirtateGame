using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Scripts.Utils
{
    public static class GameObjectExtensions
    {
       public static bool IsInLayer(this GameObject go, LayerMask layer)
        {
            return layer == (layer | 1 << go.layer);
        }

        public static TinterfaceType GetInterface<TinterfaceType>(this GameObject go)
        {
            var components = go.GetComponents<Component>();
            foreach (var component in components)
            {
                if (component is TinterfaceType type)
                {
                    return type;
                }
            }

            return default;
        }
    }
}


