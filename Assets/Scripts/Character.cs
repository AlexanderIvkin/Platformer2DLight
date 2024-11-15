using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(GroundDetector))]

public abstract class Character : MonoBehaviour
{
    private readonly int DeathTrigger = Animator.StringToHash("Death");
    private readonly int AttackTrigger = Animator.StringToHash("Attack");

    [SerializeField] private int _maxHealth;
    [SerializeField] private int _damage;
    [SerializeField] private int _attackPerSecond;

    private Health _health;

    protected bool ReadyToAction;
    protected Animator Animator;
    protected AnimationShower AnimationShower;
    protected GroundDetector GroundDetector;

    protected bool IsAlive => _health.CurrentValue > 0;

    protected virtual void Awake()
    {
        Init();
    }

    protected virtual void OnEnable()
    {
        _health.Dead += ToDie;
    }

    protected virtual void OnDisable()
    {
        _health.Dead -= ToDie;
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        CheckCollisionCharacter(collision);
    }

    private void CheckCollisionCharacter(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<Character>(out Character target))
        {
            StartCoroutine(Attack(target));
        }
    }

    protected void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<Character>(out Character target))
        {
            StopCoroutine(Attack(target));
        }
    }

    protected IEnumerator Attack(Character target)
    {
        float speedFactor = 1 / _attackPerSecond;

        var wait = new WaitForSeconds(speedFactor);

        ReadyToAction = false;

        while (target != null && target.ReadyToAction)
        {
            _health.TryRemove(target._damage);

            Animator.SetTrigger(AttackTrigger);

            yield return wait;
        }

        ReadyToAction = true;

        yield break;
    }

    protected virtual void Init()
    {
        ReadyToAction = true;
        Animator = GetComponent<Animator>();
        GroundDetector = GetComponent<GroundDetector>();
        _health = new Health(_maxHealth);
        AnimationShower = new AnimationShower(Animator);
    }

    protected void ToDie()
    {
        StopAllCoroutines();

        AnimationShower.Show(DeathTrigger);

        ReadyToAction = false;
    }
}
