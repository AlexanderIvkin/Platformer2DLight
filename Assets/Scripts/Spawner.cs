using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private Frame _frame;
    [SerializeField] private Coin _coin;
    [SerializeField] private float _rate;
    [SerializeField] private int _maxCount;

    private List<Coin> _coins;


    private void Awake()
    {
        StartCoroutine(Spawn());
    }

    private IEnumerator Spawn()
    {
        var wait = new WaitForSeconds(_rate);

        while (_coins.Count < _maxCount)
        {
            if (TryGetFreePlace(out Vector2 spawnPosition))
            {
                Instantiate(_coin, spawnPosition, Quaternion.identity);
            }

            yield return wait;
        }
    }

    private bool TryGetFreePlace(out Vector2 freePlaceCoordinate)
    {
        bool isFinding = false;
        freePlaceCoordinate = Vector2.zero;

        RaycastHit hit;

        Ray ray = _camera.ScreenPointToRay(CalculateBackGroundSize());

        if (Physics.Raycast(ray, out hit))
        {
            Transform objectHit = hit.transform;

            if (objectHit.TryGetComponent<BackGround>(out _))
            {
                isFinding = true;
                freePlaceCoordinate = hit.point;
            }
        }

        return isFinding;
    }

    private Vector3 CalculateBackGroundSize()
    {
        float boundX = 0.5f * _frame.transform.localScale.x;
        float boundY = 0.5f * _frame.transform.localScale.y;

        return new Vector3(Random.Range(-boundX, boundX), Random.Range(-boundY, boundY));
    }
}
