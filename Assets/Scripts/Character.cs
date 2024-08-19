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
    [SerializeField] private Health _health;
    [SerializeField] private float _scaleFactor;



    public bool IsGrounded { get; private set; }
    protected bool IsWorks;
    protected bool IsAlive;
    protected Animator Animator;


    private void Awake()
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<Ground>(out _))
        {
            IsGrounded = true;
        }

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

        while (target != null && target.IsAlive)
        {
            Debug.Log("¿“¿ ”ﬁ");
            _health.TryRemove(target._damage);

            Animator.SetTrigger(AttackTrigger);

            yield return wait;
        }

    }

    private void Init()
    {
        _health = new Health(_maxHealth);
        IsAlive = true;
        IsWorks = false;
        IsGrounded = false;
        Animator = GetComponent<Animator>();
    }

    private void ToDie()
    {
        Animator.SetTrigger(DeathTrigger);
        IsAlive = false;
    }

    protected void Flip(float direction)
    {
        transform.localScale = new Vector3(Mathf.Sign(_scaleFactor * direction), transform.localScale.y, transform.localScale.z);
    }
}
