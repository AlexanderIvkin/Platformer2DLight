using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Frame _frame;
    [SerializeField] private Coin _coin;
    [SerializeField] private float _rate;
    [SerializeField] private int _maxCount;

    private List<Coin> _coins = new List<Coin>();


    private void Start()
    {
        StartCoroutine(Spawn());
    }

    private IEnumerator Spawn()
    {
        Debug.Log("Спавню");

        var wait = new WaitForSeconds(_rate);

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
            freePlaceCoordinate = ReturnRandomPosition();
        }
        while (Physics2D.OverlapCircle(freePlaceCoordinate, 0.5f, 3));

        return freePlaceCoordinate;
    }

    private Vector2 ReturnRandomPosition()
    {
        float scaleFactor = 0.5f;

        float boundX = scaleFactor * _frame.transform.localScale.x;
        float boundY = scaleFactor * _frame.transform.localScale.y;

        return new Vector2(Random.Range(-boundX, boundX), Random.Range(-boundY, boundY));
    }
}
