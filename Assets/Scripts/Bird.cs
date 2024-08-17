using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Vision))]

public class Bird : Character
{
    private readonly int EatTrigger = Animator.StringToHash("Eat");
    private readonly int FlyTrigger = Animator.StringToHash("Fly");
    private readonly int IDLETrigger = Animator.StringToHash("IDLE");

    [SerializeField] private float _jumpForce;
    [SerializeField] private float _speed;

    [SerializeField] private float _maxTimeToAction;
    [SerializeField] private float _attackDistance;
    [SerializeField] private bool _isGrounded;
    [SerializeField] private bool _isAlive;
    [SerializeField] private bool _isFree;
    [SerializeField] private bool _isSafely;

    private Vision _vision;
    private Rigidbody2D _rigidBody;
    private Animator _animator;

    private float ReturnTimeOfAction => GetRandomValue(0f, _maxTimeToAction);

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _vision = GetComponent<Vision>();

        _isAlive = true;
        _isFree = true;
        _isSafely = true;
    }

    private void OnEnable()
    {
        _vision.Viewed += PrepareToFight;
    }

    private void OnDisable()
    {
        _vision.Viewed -= PrepareToFight;

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
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

    private void Start()
    {
        StartCoroutine(Behaviour());
    }

    private enum Coroutines
    {
        Idle,
        Fly,
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
                StartCoroutine(Idle());
            }

            yield return wait;
        }
    }

    private Vector2 GetRandomDirection()
    {
        Vector2 direction = new Vector2(GetRandomValue(-1, 1), GetRandomValue(0, 1));

        return direction;
    }

    private IEnumerator Idle()
    {
        Debug.Log("»ƒÀ≈");

        var wait = new WaitForSeconds(GetRandomValue(0, _maxTimeToAction));

        _isFree = true;

        _animator.SetTrigger(IDLETrigger);

        yield return wait;
    }

    private void PrepareToFight(Transform target)
    {
        if (target.TryGetComponent<Player>(out _))
        {
            float currentDistance = transform.position.x - target.position.x;

            do
            {
                _isFree = false;

                Vector2 attackPosition = new Vector2(target.position.x - _attackDistance, target.position.y);

                transform.position = Vector2.MoveTowards(transform.position, attackPosition, _speed * Time.deltaTime);

                _animator.SetTrigger(FlyTrigger);

            } while (currentDistance >= _attackDistance);
        }
    }

    private IEnumerator Eat()
    {
        var wait = new WaitForSeconds(GetRandomPositiveNumber(_maxTimeToAction));

        Debug.Log("∆–ŒŒŒ“‹ “””””");

        _isFree = false;

        _animator.SetTrigger(EatTrigger);

        yield return wait;

        StopCoroutine(Eat());
    }

    private float GetRandomPositiveNumber(float max)
    {
        return Random.Range(0, max);
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
