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

    [SerializeField] private TextMeshProUGUI _textMeshPro;
    [SerializeField] private float _speed;
    [SerializeField] private float _jumpForce;

    private Rigidbody2D _rigidbody2D;
    
    private InputScaner _inputScaner;
    private Wallet _wallet;
    private WalletView _walletView;

    protected override void Awake()
    {
        base.Awake();

        Init();

        _rigidbody2D = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
        _inputScaner = GetComponent<InputScaner>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        _inputScaner.Jumped += Jump;
        _inputScaner.Moved += Move;
        _inputScaner.Digging += Dig;
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        _inputScaner.Jumped -= Jump;
        _inputScaner.Moved -= Move;
        _inputScaner.Digging -= Dig;
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);

        CoinPickUp(collision);
    }

    private void Update()
    {
        _walletView.Show();
    }
    
    protected override void Init()
    {
        base.Init();

        _wallet = new Wallet();
        _walletView = new WalletView(_wallet, _textMeshPro);
    }

    protected override void ToDie()
    {
        base.ToDie();

        _rigidbody2D.velocity = Vector2.zero;
        _rigidbody2D.mass = 500;
    }

    private void Move(float direction)
    {
        if (ReadyToAction)
        {
            Flip(direction);
            transform.Translate(_speed * direction * Time.deltaTime * Vector2.right);

            if (IsGrounded)
            {
                AnimationShower.Show(WalkAnimatorParameter, direction);
            }
        }
    }

    private void Jump()
    {
        if (IsGrounded)
        {
            AnimationShower.Show(JumpTrigger);
            _rigidbody2D.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
        }
    }

    private void Dig()
    {
        AnimationShower.Show(DigTrigger);
    }

    private void CoinPickUp(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<Coin>(out Coin coin))
        {
            coin.gameObject.SetActive(false);

            _wallet.Add();
        }
    }
}
