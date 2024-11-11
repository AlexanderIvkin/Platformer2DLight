using TMPro;
using UnityEngine;

[RequireComponent(typeof(Mover))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(InputReader))]

public class Player : Character
{
    private readonly int JumpTrigger = Animator.StringToHash("Jump");
    private readonly int DigTrigger = Animator.StringToHash("Dig");
    private readonly string WalkAnimatorParameter = "Speed";

    [SerializeField] private TextMeshProUGUI _textMeshPro;
    [SerializeField] private float _speed;
    [SerializeField] private float _jumpForce;

    private Mover _mover;
    private InputReader _inputReader;
    private Wallet _wallet;
    private WalletView _walletView;

    protected override void Awake()
    {
        base.Awake();

        Init();
        _mover = GetComponent<Mover>();
        Animator = GetComponent<Animator>();
        _inputReader = GetComponent<InputReader>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);

        CoinPickUp(collision);
    }

    private void Update()
    {
        _walletView.Show(_wallet.Count);
    }

    private void FixedUpdate()
    {
        Move(_inputReader.Direction);
        Jump();
        Dig();
    }

    protected override void Init()
    {
        base.Init();

        _wallet = new Wallet();
        _walletView = new WalletView(_textMeshPro);
    }

    private void Move(float direction)
    {
        if (ReadyToAction)
        {
            _mover.Move(direction);

            if (GroundDetector.IsGrounded)
            {
                AnimationShower.Show(WalkAnimatorParameter, direction);
            }
        }
    }

    private void Jump()
    {
        if (_inputReader.GetIsJump() && GroundDetector.IsGrounded && ReadyToAction)
        {
            AnimationShower.Show(JumpTrigger);

            _mover.Jump();
        }
    }

    private void Dig()
    {
        if (GroundDetector.IsGrounded && _inputReader.GetIsDig())
        {
            AnimationShower.Show(DigTrigger);
        }
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
