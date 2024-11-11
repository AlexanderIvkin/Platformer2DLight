using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(RotatorToDirection))]

public class Mover : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _jumpForce;

    private RotatorToDirection _rotatorToDirection;
    private Rigidbody2D _rigidbody2D;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _rotatorToDirection = GetComponent<RotatorToDirection>();
    }

    public void Move(float direction)
    {
        _rotatorToDirection.Rotate(direction);

        _rigidbody2D.AddForce(_speed * direction * Vector2.right, ForceMode2D.Force);
    }

    public void Jump()
    {
        _rigidbody2D.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
    }
}
