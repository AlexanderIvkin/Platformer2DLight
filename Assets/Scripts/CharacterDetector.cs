using UnityEngine;

public class CharacterDetector : MonoBehaviour
{
    public bool IsDetected { get; private set; }
    public Character Target { get; private set; }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<Character>(out Character Target))
        {
            IsDetected = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<Character>(out Character Target))
        {
            IsDetected = false;
        }
    }
}
