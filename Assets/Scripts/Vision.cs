using System;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class Vision : MonoBehaviour
{
    public event Action<Transform> Viewed;

    [SerializeField] private float _viewDistance;
    [SerializeField] private Transform _eyePosition;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null )
        {
            Viewed?.Invoke(collision.transform);
        }
    }
}
