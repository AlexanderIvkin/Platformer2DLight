using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(RotatorToDirection))]


public class Bird : Character
{
    private readonly int EatTrigger = Animator.StringToHash("Eat");
    private readonly int FlyTrigger = Animator.StringToHash("Fly");
    private readonly int IdleTrigger = Animator.StringToHash("IDLE");

    [SerializeField] private float _minJumpForce;
    [SerializeField] private float _maxJumpForce;
    [SerializeField] private float _speed;
    [SerializeField] private float _maxTimeToAction;

    private float _attackDistance;
    private Rigidbody2D _rigidBody;
    private RotatorToDirection _rotatorToDirection;

    private float ReturnTimeOfAction => GetRandomValue(0f, _maxTimeToAction);

    protected override void Awake()
    {
        base.Awake();

        Init();
    }

    protected override void Init()
    {
        base.Init();

        _rotatorToDirection = GetComponent<RotatorToDirection>();
        _rigidBody = GetComponent<Rigidbody2D>();
        _attackDistance = 6f;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null && collision.TryGetComponent<Player>(out Player target))
        {
            GoToAttackDistance(target);
        }
    }

    private void Start()
    {
        StartCoroutine(Behaviouring());
    }

    private IEnumerator Behaviouring()
    {
        var wait = new WaitForEndOfFrame();

        while (IsAlive)
        {
            DoAction();

            yield return wait;
        }
    }

    private void DoAction()
    {
        if (GroundDetector.IsGrounded)
        {
            if (ReadyToAction)
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
    }

    private Vector2 GetRandomDirection()
    {
        Vector2 direction = new Vector2(GetRandomValue(-1, 1), GetRandomPositiveNumber(1));

        return direction;
    }

    private IEnumerator Idle()
    {
        var waitSeconds = new WaitForSeconds(GetRandomPositiveNumber(_maxTimeToAction));

        ReadyToAction = false;

        while (!ReadyToAction)
        {
            AnimationShower.Show(IdleTrigger);

            yield return waitSeconds;
            ReadyToAction = true;
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
                AnimationShower.Show(FlyTrigger);
            } while (!GroundDetector.IsGrounded);

            AnimationShower.Show(IdleTrigger);

        } while (currentDistance > _attackDistance);
    }

    private IEnumerator Eat()
    {
        var waitSeconds = new WaitForSeconds(GetRandomPositiveNumber(_maxTimeToAction));

        ReadyToAction = false;

        while (!ReadyToAction)
        {
            AnimationShower.Show(EatTrigger);

            yield return waitSeconds;

            ReadyToAction = true;
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

        _rotatorToDirection.Rotate(direction.x);

        _rigidBody.AddForce(direction * GetRandomValue(_minJumpForce, _maxJumpForce), ForceMode2D.Impulse);
        ReadyToAction = false;

        while (!ReadyToAction)
        {
            do
            {
                AnimationShower.Show(FlyTrigger);
            } while (!GroundDetector.IsGrounded);

            AnimationShower.Show(IdleTrigger);

            yield return waitSeconds;

            ReadyToAction = true;
        }

        yield break;
    }
}
