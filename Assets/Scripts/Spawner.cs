using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Coin _coin;
    [SerializeField] private float _delay;
    [SerializeField] private int _maxCount;

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
        while (Physics2D.OverlapCircle(freePlaceCoordinate, _coin.gameObject.transform.localScale.y, 3));

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
