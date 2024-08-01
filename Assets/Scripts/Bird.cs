using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Vision))]

public class Bird : MonoBehaviour
{
    private readonly int EatTrigger = Animator.StringToHash("Eat");
    private readonly int FlyTrigger = Animator.StringToHash("Fly");
    private readonly int IDLETrigger = Animator.StringToHash("IDLE");
    private readonly int DeathTrigger = Animator.StringToHash("Death");
    private readonly int AttackTrigger = Animator.StringToHash("Attack");

    [SerializeField] private float _jumpForce;

    [SerializeField] private float _timeToAction;
    [SerializeField] private bool _isGrounded;
    [SerializeField] private bool _isAlive;
    [SerializeField] private bool _isFree;
    [SerializeField] private bool _isSafely;

    private Vision _vision;
    private Rigidbody2D _rigidBody;
    private Animator _animator;

    private float ReturnTimeOfAction => GetRandomValue(1, 2);

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _vision = GetComponent<Vision>();

        Init();
    }

    private void Init()
    {
        _isAlive = true;
        _isFree = true;
        _isSafely = true;
    }

    private void OnEnable()
    {
        _vision.Viewed += Runaway;
    }

    private void OnDisable()
    {
        _vision.Viewed -= Runaway;

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<Ground>(out _))
        {
            _isGrounded = true;
        }

        if (collision.gameObject.TryGetComponent<Player>(out _))
        {
            StartCoroutine(Death());
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<Ground>(out _))
        {
            _isGrounded = false;
        }
    }

    private void Start()
    {
        StartCoroutine(Behaviour());
    }

    private IEnumerator Behaviour()
    {
        var wait = new WaitForSeconds(ReturnTimeOfAction);

        while (_isAlive)
        {
            if (_isFree)
            {
                if (_isSafely)
                {
                    if (_isGrounded)
                    {
                        StartCoroutine(Eat());
                    }
                }
            }
            else
            {
                StartCoroutine(Idle(ReturnTimeOfAction));
            }

            yield return wait;
        }
    }

    private Vector2 GetRandomDirection()
    {
        Vector2 direction = new Vector2(GetRandomValue(-1, 1), GetRandomValue(0, 1));

        return direction;
    }

    private IEnumerator Idle(float time)
    {
        Debug.Log("»ƒÀ≈");

        var wait = new WaitForSeconds(time);

        _isFree = true;

        _animator.SetTrigger(IDLETrigger);

        yield return wait;
    }

    private void Runaway(Transform target)
    {
        Debug.Log("–¿Õ›¬›…");

        if (target.gameObject.TryGetComponent<Player>(out _))
        {
            Debug.Log("√ÓÚÓ‚ËÏ ‡Ú‡ÍÛ");

            float attackDistance = 0.2f;

            _isFree = false;
            _isSafely = true;

            Vector2 direction = target.transform.position - transform.position;

            transform.LookAt(target);
            _rigidBody.AddForce(direction.normalized * _jumpForce, ForceMode2D.Impulse);
            _animator.SetTrigger(FlyTrigger);

            if (transform.position.x > target.position.x - attackDistance)
            {
                StartCoroutine(Fight(target));
            }
        }
    }

    private IEnumerator Fight(Transform target)
    {
        Debug.Log("ƒ–¿ ¿");

        _animator.SetTrigger(AttackTrigger);

        yield return null;
    }

    private IEnumerator Eat()
    {
        var wait = new WaitForSeconds(ReturnTimeOfAction);

        Debug.Log("∆–ŒŒŒ“‹ “””””");

        _isFree = false;

        _animator.SetTrigger(EatTrigger);

        yield return wait;

        StopCoroutine(Eat());
    }

    private float GetRandomValue(float min, float max)
    {
        return Random.Range(min, max);
    }

    private IEnumerator Fly()
    {
        Debug.Log("œÓÎ∏Ú");
        var wait = new WaitForSeconds(ReturnTimeOfAction);

        _isFree = false;

        _animator.SetTrigger(FlyTrigger);

        yield return null;
    }

    private IEnumerator Death()
    {
        Debug.Log("—ÏÂÚ¸");

        _isAlive = false;

        _animator.SetBool("IsAlive", _isAlive);

        StopAllCoroutines();

        yield return null;
    }
}
