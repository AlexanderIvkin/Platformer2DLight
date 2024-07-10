using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    }

    private IEnumerator Spawn()
    {
        var wait = new WaitForSeconds(_rate);

        while (_coins.Count < _maxCount)
        {


            yield return wait;
        }

    }

    private void Create()
    {

    }


    private Vector2 ReturnFreePlacePosition()
    {
        RaycastHit2D hit;

        Ray ray = _camera.ScreenPointToRay(CalculateBackGroundSize());

        return new Vector2();
    }

    private Vector3 CalculateBackGroundSize()
    {
        float boundX = 0.5f * _frame.transform.localScale.x;
        float boundY = 0.5f * _frame.transform.localScale.y;

        return new Vector3(Random.Range(-boundX, boundX) ,Random.Range(-boundY, boundY));
    }
}
