using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(InputScaner))]

public class Player : Character
{
    private readonly int JumpTrigger = Animator.StringToHash("Jump");
    private readonly int DigTrigger = Animator.StringToHash("Dig");
    private readonly string WalkAnimatorParameter = "Speed";

    [SerializeField] private TextMeshProUGUI _walletView;
    [SerializeField] private float _speed;
    [SerializeField] private float _jumpForce;
    [SerializeField] private bool _isGrounded;

    private Rigidbody2D _rigidbody2D;
    private InputScaner _inputScaner;
    private Wallet _wallet = new Wallet();

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
        _inputScaner = GetComponent<InputScaner>();
        _wallet.SetTextMeshProUGUI(_walletView);
    }

    private void OnEnable()
    {
        _inputScaner.Jumped += Jump;
        _inputScaner.Moved += Move;
        _inputScaner.Digging += Dig;
    }

    private void OnDisable()
    {
        _inputScaner.Jumped -= Jump;
        _inputScaner.Moved -= Move;
        _inputScaner.Digging -= Dig;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<Coin>(out _))
        {
            _wallet.Add();
        }

        
    }

    private void Update()
    {
        _wallet.Show();
    }

    private void Move(float direction)
    {
        Flip(direction);

        transform.Translate(_speed * direction * Time.deltaTime * Vector2.right);

        if (_isGrounded)
        {
            Animator.SetFloat(WalkAnimatorParameter, Mathf.Abs(direction));
        }
    }

    private void Jump()
    {
        if (_isGrounded)
        {
            Animator.SetTrigger(JumpTrigger);
            _rigidbody2D.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
        }
    }

    private void Dig()
    {
        Animator.SetTrigger(DigTrigger);
    }
}
