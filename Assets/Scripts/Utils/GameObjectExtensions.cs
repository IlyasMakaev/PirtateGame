using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Scripts.Utils
{
    public static class GameObjectExtensions
    {
       public static bool isInLayer(this GameObject go, LayerMask layer)
        {
            return layer == (layer | 1 << go.layer);
        }
    }
}


