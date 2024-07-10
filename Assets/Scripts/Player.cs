using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(InputScaner))]

public class Player : MonoBehaviour
{
    private readonly int JumpTrigger = Animator.StringToHash("Jump");
    private readonly int UseTrigger = Animator.StringToHash("Attack");
    private readonly int DigTrigger = Animator.StringToHash("Dig");
    private readonly int SitTrigger = Animator.StringToHash("Sit");
    private readonly string WalkAnimatorParameter = "Speed";

    [SerializeField] private TextMeshProUGUI _walletView;
    [SerializeField] private float _speed;
    [SerializeField] private float _jumpForce;
    [SerializeField] private bool _isGrounded;

    private Rigidbody2D _rigidbody2D;
    private Animator _animator;
    private InputScaner _inputScaner;
    private Wallet _wallet = new Wallet();

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _inputScaner = GetComponent<InputScaner>();
        _wallet.SetTextMeshProUGUI(_walletView);

    }

    private void OnEnable()
    {
        _inputScaner.Jumped += Jump;
        _inputScaner.Moved += Move;
        _inputScaner.Attacked += Use;
        _inputScaner.Digging += Dig;
        _inputScaner.Sitting += Sit;
    }

    private void OnDisable()
    {
        _inputScaner.Jumped -= Jump;
        _inputScaner.Moved -= Move;
        _inputScaner.Attacked -= Use;
        _inputScaner.Digging -= Dig;
        _inputScaner.Sitting -= Sit;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<Coin>(out _))
        {
            _wallet.Add();
        }

        if (collision.gameObject.TryGetComponent<Ground>(out _))
        {
            _isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<Ground>(out _))
        {
            _isGrounded = false;
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
            _animator.SetFloat(WalkAnimatorParameter, Mathf.Abs(direction));
        }
    }

    private void Flip(float direction)
    {
        float scaleFactor = -1f;
        bool rightOriented = direction <= 0;

        transform.localScale = new Vector3(Mathf.Sign(scaleFactor * direction), transform.localScale.y, transform.localScale.z);

        if (transform.localScale.x == 0) { 
        
        }
    }

    private void Jump()
    {
        if (_isGrounded)
        {
            _animator.SetTrigger(JumpTrigger);
            _rigidbody2D.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
        }
    }

    private void Use()
    {
        _animator.SetTrigger(UseTrigger);
    }

    private void Dig()
    {
        _animator.SetTrigger(DigTrigger);
    }

    private void Sit()
    {
        Debug.Log("Сидим");
        _animator.SetTrigger(SitTrigger);
    }
}
