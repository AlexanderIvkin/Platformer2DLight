using System;
using UnityEngine;

public class Vision : MonoBehaviour
{
    public event Action<Transform> Viewed;

    [SerializeField] private float _viewDistance;

    private void Update()
    {
        MakeView();
    }

    private void MakeView()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right, _viewDistance);

        if (hit.collider != null)
        {
            Viewed?.Invoke(hit.transform);
        }
    }
}
