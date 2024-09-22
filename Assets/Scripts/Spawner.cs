using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Coin _coin;
    [SerializeField] private float _delay;
    [SerializeField] private float _coinDiametr;
    [SerializeField] private int _maxCount;
    [SerializeField] private int _searchingLayerNumber;

    private List<Coin> _coins = new List<Coin>();

    private void Start()
    {
        StartCoroutine(Spawn());
    }

    private IEnumerator Spawn()
    {
        var wait = new WaitForSeconds(_delay);

        while (_coins.Count < _maxCount)
        {
            _coins.Add(Instantiate(_coin, GetFreePlace(), Quaternion.identity));

            yield return wait;
        }
    }

    private Vector2 GetFreePlace()
    {
        Vector2 freePlaceCoordinate;

        do
        {
            freePlaceCoordinate = ReturnRandomPositionOnGround();
        }
        while (Physics2D.OverlapCircle(freePlaceCoordinate, _coinDiametr, _searchingLayerNumber));

        return freePlaceCoordinate;
    }

    private Vector2 ReturnRandomPositionOnGround()
    {
        float boundX = 40f;
        float downBoundY = 6.0f;
        float upBoundY = 80.0f;

        return new Vector2(Random.Range(-boundX, boundX), Random.Range(downBoundY, upBoundY));
    }
}
