using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]

public class Bird : Character
{
    private readonly int EatTrigger = Animator.StringToHash("Eat");
    private readonly int FlyTrigger = Animator.StringToHash("Fly");
    private readonly int IDLETrigger = Animator.StringToHash("IDLE");

    [SerializeField] private float _minJumpForce;
    [SerializeField] private float _maxJumpForce;
    [SerializeField] private float _speed;
    [SerializeField] private float _maxTimeToAction;

    private float _attackDistance;
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
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null && collision.gameObject.TryGetComponent<Player>(out Player target))
        {
            GoToAttackDistance(target);
        }
    }

    private void Start()
    {
        StartCoroutine(Behaviour());
    }

    private IEnumerator Behaviour()
    {
        var wait = new WaitForEndOfFrame();

        while (ReadyToAction)
        {
            if (IsGrounded)
            {
                if (IsFree)
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

        _rigidBody.AddForce((target.transform.position - transform.position).normalized + Vector3.up * GetRandomValue(_minJumpForce, _maxJumpForce));

        do
        {
            Vector2 attackPosition = new Vector2(target.transform.position.x - _attackDistance, target.transform.position.y);

            transform.position = Vector2.MoveTowards(transform.position, attackPosition, _speed * Time.deltaTime);

            do
            {
                _animator.SetTrigger(FlyTrigger);
            } while (!IsGrounded);

            _animator.SetTrigger(IDLETrigger);

        } while (currentDistance > _attackDistance);
    }

    private IEnumerator Eat()
    {
        var waitSeconds = new WaitForSeconds(GetRandomPositiveNumber(_maxTimeToAction));

        IsFree = false;

        while (!IsFree)
        {
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
        var waitSeconds = new WaitForSeconds(GetRandomPositiveNumber(_maxTimeToAction));

        Vector2 direction = GetRandomDirection();

        Flip(direction.x);

        _rigidBody.AddForce(direction * GetRandomValue(_minJumpForce, _maxJumpForce), ForceMode2D.Impulse);
        IsFree = false;

        while (!IsFree)
        {
            do
            {
                _animator.SetTrigger(FlyTrigger);
            } while (!IsGrounded);

            _animator.SetTrigger(IDLETrigger);

            yield return waitSeconds;

            IsFree = true;
        }

        yield break;
    }
}
