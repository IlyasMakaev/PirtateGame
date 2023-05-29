/*using System;
using System.Collections;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class RandomSpawner : MonoBehaviour
{
    [Header("Spawn Bound: ")] [SerializeField]
    private float _sectorAngle = 60;

    [SerializeField] private float _sectorLocation;
    [Header("Spawn params: ")] [Space] [SerializeField]
    private GameObject _particle;

    [SerializeField] private float _waiTime = 0.1f;
    [SerializeField] private float _speed = 0.6f;
    [SerializeField] private float _itemPerBurst = 2;
    [SerializeField] private float _numParticles = 200;

    public void Start()
    {
        Restart();

    }

    private Coroutine _routine;


    [ContextMenu( "Restart")]
    private void Restart()
    {
        throw new NotImplementedException();
    }


    private IEnumerator StartGame()
    {
        for(var i = 0; i < _numParticles; i++)
        {
            for(var j = 0; j <_itemPerBurst; i++)
            {
                Spawn();
            }

            yield return new WaitForSeconds(_waiTime);
        }
    }

    [ContextMenu("Spawn one")]
    private void Spawn()
    {
        var instance = Instantiate(_particle, transform.position, Quaternion.identity);
        var rigibody = instance.GetComponent<Rigidbody2D>();

        var randomAngle = Random.Range(0, _sectorAngle);
        var forceVector = AngleToVectorInSector(randomAngle);
        rigibody.AddForce(forceVector * _speed, ForceMode2D.Impulse);
    }

    private object AngleToVectorInSector(float randomAngle)
    {
        throw new NotImplementedException();
    }
}
*/