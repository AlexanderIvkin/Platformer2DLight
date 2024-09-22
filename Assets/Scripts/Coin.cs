using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private float _pickUpTime = 0.5f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<Player>(out _))
        {
            Destroy(this.gameObject, _pickUpTime);
        }
    }
}
