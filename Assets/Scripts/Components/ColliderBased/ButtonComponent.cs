using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonComponent : MonoBehaviour
{
    [SerializeField]private Transform _defaultPosition;
   


    public void ChangePosition()
    {
        _defaultPosition.Translate(new Vector2(0,-2));
    }

    
}
