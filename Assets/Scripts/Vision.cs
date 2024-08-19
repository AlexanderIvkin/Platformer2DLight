using System;
using UnityEngine;

public class Vision : MonoBehaviour
{
    public event Action<Transform> Viewed;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null )
        {
            Viewed?.Invoke(collision.transform);
        }
    }
}
