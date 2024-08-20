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

    private Vision _vision;
    private Rigidbody2D _rigidBody;
    private Animator _animator;
    private bool _isFree;

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
        _isFree = true;
    }

    private void OnEnable()
    {
        _vision.Viewed += PrepareToFight;
    }

    private void OnDisable()
    {
        _vision.Viewed -= PrepareToFight;

    }

    private void Start()
    {
        StartCoroutine(Behaviour());
    }

    private IEnumerator Behaviour()
    {
        var wait = new WaitForSeconds(ReturnTimeOfAction);

        while (IsAlive)
        {
            if (IsGrounded)
            {
                if (_isFree)
                {
                    float maxChance = 1f;
                    float searchChance = 0.7f;
                    float eatChance = 0.3f;
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

        var waitSeconds = new WaitForSeconds(GetRandomPositiveNumber(_maxTimeToAction));

        while (_isFree)
        {
            _animator.SetTrigger(IDLETrigger);

            yield return waitSeconds;

            _isFree = false;
        }

        _isFree = true;

        yield break;
    }

    private void PrepareToFight(Transform target)
    {
        if (target.gameObject.TryGetComponent<Player>(out _))
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
        var waitSeconds = new WaitForSeconds(GetRandomPositiveNumber(_maxTimeToAction));

        while (_isFree)
        {
            _animator.SetTrigger(EatTrigger);

            yield return waitSeconds;

            _isFree = false;
        }

        _isFree = true;

        yield break;
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

        while (_isFree)
        {
            do
            {
                _animator.SetTrigger(FlyTrigger);

                yield return waitEndFrame;

            } while (!IsGrounded);

            _animator.SetTrigger(IDLETrigger);

            yield return waitSeconds;

            _isFree = false;
        }

        _isFree = true;

        yield break;
    }
}
