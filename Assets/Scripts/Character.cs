using System.Collections;
using System.Collections.Generic;
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
    protected bool IsAlive;
    protected Animator Animator;


    protected virtual void Awake()
    {
        Init();
    }

    private void OnEnable()
    {
        _health.Dead += ToDie;
    }

    private void OnDisable()
    {
        _health.Dead -= ToDie;
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        GroundCheck(collision);

        TargetCheck(collision);
    }

    private void GroundCheck(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<Ground>(out _))
        {
            IsGrounded = true;
        }
    }

    private void TargetCheck(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<Character>(out Character target))
        {
            StartCoroutine(Attack(target));
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
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
        Debug.Log("Начинаю атаку");

        float speedFactor = 1 / _attackPerSecond;

        var wait = new WaitForSeconds(speedFactor);

        IsFree = false;

        while (target != null && target.IsAlive)
        {
            Debug.Log("АТАКУЮ");
            _health.TryRemove(target._damage);

            Animator.SetTrigger(AttackTrigger);

            yield return wait;

            IsFree = true;
        }

        yield break;
    }

    private void Init()
    {
        _health = new Health(_maxHealth);
        IsAlive = true;
        IsFree = true;
        IsGrounded = false;
        Animator = GetComponent<Animator>();
    }

    protected virtual void ToDie()
    {
        StopAllCoroutines();
        Debug.Log("Сдох");

        Animator.SetTrigger(DeathTrigger);

        IsAlive = false;
    }

    protected void Flip(float direction)
    {
        transform.localScale = new Vector3(Mathf.Sign(_scaleFactor * direction), transform.localScale.y, transform.localScale.z);
    }
}
