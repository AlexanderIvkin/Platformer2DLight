using System;
using UnityEngine;

public class Vision : MonoBehaviour
{
    public event Action<Transform> Viewed;

    [SerializeField] private float _viewDistance;
    [SerializeField] private Transform _eyePosition;

    private void Update()
    {
        MakeView();
    }

    private void MakeView()
    {
        RaycastHit2D hit = Physics2D.Raycast(_eyePosition.position , Vector2.right, _viewDistance);


        if (hit.collider != null)
        {
            Viewed?.Invoke(hit.transform);
            Debug.Log("Событие вызвалось");
        }
    }
}
