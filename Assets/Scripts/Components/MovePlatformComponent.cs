using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlatformComponent : MonoBehaviour
{
    [SerializeField] private GameObject _platformPathStart;
    [SerializeField] private GameObject _platformPathEnd;
    [SerializeField] private int _speed;

    private Vector2 _startPosition;
    private Vector2 _endPosition;
   
    private void Start()
    {
        _startPosition = _platformPathStart.transform.position;
        _endPosition = _platformPathEnd.transform.position;
        StartCoroutine(Vector2LerpCoroutine(gameObject, _endPosition, _speed));
    }

    private void Update()
    {
        if(transform.position.x == _endPosition.x)
        {
            StartCoroutine(Vector2LerpCoroutine(gameObject, _startPosition, _speed));
        }
        if(transform.position.x == _startPosition.x)
        {
            StartCoroutine(Vector2LerpCoroutine(gameObject, _endPosition, _speed));
        }
    }
    IEnumerator Vector2LerpCoroutine(GameObject obj, Vector2 target, float speed)
    {
        Vector2 startPosition = obj.transform.position;
        float time = 0f;

        while (obj.transform.position.x != target.x)
        {
            obj.transform.position = Vector2.Lerp(startPosition, target, (time / Vector2.Distance(startPosition, target)) * speed);
            time += Time.deltaTime;
            yield return null;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        collision.gameObject.transform.SetParent(gameObject.transform, true);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        collision.gameObject.transform.parent = null;
    }

}
