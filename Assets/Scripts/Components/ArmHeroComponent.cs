using System;
using System.Collections.Generic;
using UnityEngine;


namespace Scripts.Components
{
    class ArmHeroComponent : MonoBehaviour
    {
        
        public void ArmHero(GameObject go)
        {
            var hero = go.GetComponent<PlayerState>();
            if(hero != null)
            {
                hero.ArmHero();
            }
        }
    }
}
