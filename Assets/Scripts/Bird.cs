using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(PlayerScanner))]

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

    private PlayerScanner _vision;
    private Rigidbody2D _rigidBody;
    private Animator _animator;

    private float ReturnTimeOfAction => GetRandomValue(0f, _maxTimeToAction);

    protected override void Awake()
    {
        base.Awake();

        Init();
    }

    private void Init()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _vision = GetComponent<PlayerScanner>();
    }

    private void OnEnable()
    {
        _vision.Viewed += GoToAttackDistance;
    }

    private void OnDisable()
    {
        _vision.Viewed -= GoToAttackDistance;

    }

    private void Start()
    {
        StartCoroutine(Behaviour());
    }

    private IEnumerator Behaviour()
    {
        var wait = new WaitForEndOfFrame();

        while (IsAlive)
        {
            Debug.Log("Стою");
            if (IsGrounded)
            {
                Debug.Log("На земле");

                if (IsFree)
                {
                    Debug.Log("Свободен");
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
        Debug.Log("IDLE");
        var waitSeconds = new WaitForSeconds(GetRandomPositiveNumber(_maxTimeToAction));
        IsFree = false;

        while (!IsFree)
        {
            _animator.SetTrigger(IDLETrigger);

            yield return waitSeconds;
            IsFree = true;

        }


        yield break;
    }

    private void GoToAttackDistance(Player target)
    {
        float currentDistance = transform.position.x - target.transform.position.x;

        do
        {
            Vector2 attackPosition = new Vector2(target.transform.position.x - _attackDistance, target.transform.position.y);

            transform.position = Vector2.MoveTowards(transform.position, attackPosition, _speed * Time.deltaTime);
            _rigidBody.AddForce(attackPosition + Vector2.up * GetRandomValue(_minJumpForce, _maxJumpForce) * Time.deltaTime);
            _animator.SetTrigger(FlyTrigger);

        } while (currentDistance >= _attackDistance);
    }

    private IEnumerator Eat()
    {
        Debug.Log("Ща кушоц");
        var waitSeconds = new WaitForSeconds(GetRandomPositiveNumber(_maxTimeToAction));
        IsFree = false;

        while (!IsFree)
        {
            Debug.Log("ЖРООООТЬ");
            _animator.SetTrigger(EatTrigger);

            yield return waitSeconds;

            IsFree = true;
        }


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
        Debug.Log("Ща полечу");

        var waitSeconds = new WaitForSeconds(GetRandomPositiveNumber(_maxTimeToAction));
        var waitEndFrame = new WaitForEndOfFrame();

        Vector2 direction = GetRandomDirection();

        Flip(direction.x);

        _rigidBody.AddForce(direction * GetRandomValue(_minJumpForce, _maxJumpForce), ForceMode2D.Impulse);
        IsFree = false;

        while (!IsFree)
        {
            do
            {
                _animator.SetTrigger(FlyTrigger);
                Debug.Log("ЛЕЧУУУУУ");

            } while (!IsGrounded);

            _animator.SetTrigger(IDLETrigger);

            yield return waitSeconds;

            IsFree = true;
        }


        yield break;
    }
}
