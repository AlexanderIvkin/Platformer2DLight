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

    private Character targetCharacter;

    private Health _health;

    protected Animator Animator;

    public bool IsAlive { get; private set; }

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
        StartCoroutine(Attack(collision));
    }

    protected IEnumerator Attack(Character target)
    {
        float speedFactor = 1 / _attackPerSecond;

        var wait = new WaitForSeconds(speedFactor);

        while (target != null && target.IsAlive)
        {
            _health.TryRemove(target._damage);

            Animator.SetTrigger(AttackTrigger);

            yield return wait;
        }

        yield break;
    }

    private void Init()
    {
        _health = new Health(_maxHealth);
        IsAlive = true;
        Animator = GetComponent<Animator>();
    }

    private void ToDie()
    {
        Animator.SetTrigger(DeathTrigger);
        IsAlive = false;

    }
}
