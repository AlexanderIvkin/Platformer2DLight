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
    [SerializeField]private Health _health;

    private bool _isFight;

    protected bool IsGrounded;

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
        if (collision.gameObject.TryGetComponent<Ground>(out _))
        {
            IsGrounded = true;
        }

        if (collision.gameObject.TryGetComponent<Character>(out Character target))
        {
            StartCoroutine(Attack(target));
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        


    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (_isFight)
        {
            if (collision.gameObject.TryGetComponent<Character>(out Character target))
            {
                StopCoroutine(Attack(target));

                _isFight = false;
            }
        }
    }

    protected IEnumerator Attack(Character target)
    {
        Debug.Log("¿“¿ ”ﬁ");

        _isFight = true;

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

    protected void Flip(float direction)
    {
        float scaleFactor = -1f;

        transform.localScale = new Vector3(Mathf.Sign(scaleFactor * direction), transform.localScale.y, transform.localScale.z);
    }
}
