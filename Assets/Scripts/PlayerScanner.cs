using System;
using UnityEngine;

public class PlayerScanner : MonoBehaviour
{
    public event Action<Player> Viewed;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null && collision.gameObject.TryGetComponent<Player>(out Player target))
        {
            Viewed?.Invoke(target);
        }
    }
}
