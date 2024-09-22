using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]

public abstract class Character : MonoBehaviour
{
    private readonly int DeathTrigger = Animator.StringToHash("Death");
    private readonly int AttackTrigger = Animator.StringToHash("Attack");

    [SerializeField] private int _maxHealth;
    [SerializeField] private int _damage;
    [SerializeField] private int _attackPerSecond;
    [SerializeField] private float _scaleFactor;

    private Health _health;

    protected bool IsGrounded;
    protected bool IsFree;
    protected bool ReadyToAction;
    protected Animator Animator;


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
        GroundCollisionCheck(collision);

        ChatacterCollisionCheck(collision);
    }

    private void GroundCollisionCheck(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<Ground>(out _))
        {
            IsGrounded = true;
        }
    }

    private void ChatacterCollisionCheck(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<Character>(out Character target))
        {
            StartCoroutine(Attack(target));
        }
    }

    protected void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<Ground>(out _))
        {
            IsGrounded = false;
        }

        if (collision.gameObject.TryGetComponent<Character>(out Character target))
        {
            StopCoroutine(Attack(target));
        }
    }

    protected IEnumerator Attack(Character target)
    {
        float speedFactor = 1 / _attackPerSecond;

        var wait = new WaitForSeconds(speedFactor);

        IsFree = false;

        while (target != null && target.ReadyToAction)
        {
            _health.TryRemove(target._damage);

            Animator.SetTrigger(AttackTrigger);

            yield return wait;
        }

        IsFree = true;

        yield break;
    }

    private void Init()
    {
        _health = new Health(_maxHealth);
        ReadyToAction = true;
        IsFree = true;
        IsGrounded = false;
        Animator = GetComponent<Animator>();
    }

    protected virtual void ToDie()
    {
        StopAllCoroutines();

        Animator.SetTrigger(DeathTrigger);

        ReadyToAction = false;
    }

    protected void Flip(float direction)
    {
        transform.localScale = new Vector3(Mathf.Sign(_scaleFactor * direction), transform.localScale.y, transform.localScale.z);
    }
}
