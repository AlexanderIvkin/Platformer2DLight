using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Ground _ground;
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

        yield return null;
    }

    private Vector2 GetFreePlace()
    {
        Vector2 freePlaceCoordinate;

        do
        {
            freePlaceCoordinate = ReturnRandomPositionOnGround();
        }
        while (Physics2D.OverlapCircle(freePlaceCoordinate, 0.5f, 3));

        return freePlaceCoordinate;
    }

    private Vector2 ReturnRandomPositionOnGround()
    {
        float scaleFactor = 0.5f;

        float boundX = scaleFactor * _ground.transform.localScale.x;
        float boundY = scaleFactor * _ground.transform.localScale.y;

        return new Vector2(Random.Range(-boundX, boundX), Random.Range(-boundY, boundY));
    }
}
