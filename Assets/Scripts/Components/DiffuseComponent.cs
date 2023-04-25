using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiffuseComponent : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _platformsTile1;
    [SerializeField] private SpriteRenderer _platformsTile2;
    [SerializeField] private SpriteRenderer _platformsTile3;
    [SerializeField] private float _disapearSpeed;

    private Color _startPosition;
    [SerializeField]private float _endPosition = 0;

    private IEnumerator DisappearCourutine(SpriteRenderer obj, float targetState, float speed)
    {
        float startColor = obj.color.a;
        float timeToDisapear = 0f;

        while(startColor != targetState)
        {
            startColor -= speed;
            timeToDisapear += Time.deltaTime;
            yield return null;
        }

            
    }


    private void Start()
    {
        _endPosition = 0f;   
    }

    private void Update()
    {
        StartCoroutine(DisappearCourutine(_platformsTile1, _endPosition, _disapearSpeed));
    }


}
