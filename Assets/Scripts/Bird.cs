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

    [SerializeField] private float _minJumpForce;
    [SerializeField] private float _maxJumpForce;
    [SerializeField] private float _speed;

    [SerializeField] private float _maxTimeToAction;
    [SerializeField] private float _attackDistance;
    [SerializeField] private bool _isGrounded;

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

        while (IsAlive)
        {
            if (IsGrounded)
            {
                float maxChance = 1f;
                float searchChance = 0.6f;
                float eatChance = 0.2f;
                float currentChance = GetRandomPositiveNumber(maxChance);

                if (currentChance > searchChance)
                {
                    StartCoroutine(Search());
                }
                else if (currentChance > eatChance)
                {
                    StartCoroutine(Eat());
                }
                else
                {
                    StartCoroutine(Idle());
                }
            }

            yield return wait;
        }
    }

    private Vector2 GetRandomDirection()
    {
        Vector2 direction = new Vector2(GetRandomValue(-1, 1), GetRandomPositiveNumber(1));

        return direction;
    }

    private IEnumerator Idle()
    {
        var wait = new WaitForSeconds(GetRandomValue(0, _maxTimeToAction));

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

                Vector2 attackPosition = new Vector2(target.position.x - _attackDistance, target.position.y);

                transform.position = Vector2.MoveTowards(transform.position, attackPosition, _speed * Time.deltaTime);

                _animator.SetTrigger(FlyTrigger);

            } while (currentDistance >= _attackDistance);
        }
    }

    private IEnumerator Eat()
    {
        var wait = new WaitForSeconds(GetRandomPositiveNumber(_maxTimeToAction));

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

    private IEnumerator Search()
    {
        var waitSeconds = new WaitForSeconds(GetRandomPositiveNumber(_maxTimeToAction));
        var waitEndFrame = new WaitForEndOfFrame();

        Vector2 direction = GetRandomDirection();

        Flip(direction.x);

        _rigidBody.AddForce(direction * GetRandomValue(_minJumpForce, _maxJumpForce), ForceMode2D.Impulse);

        do
        {
            _animator.SetTrigger(FlyTrigger);

            yield return waitEndFrame;

        } while (!_isGrounded);

        _animator.SetTrigger(IDLETrigger);

        yield return waitSeconds;

        StopCoroutine(Search());
    }
}
