using FiveBabbittGames;
using UnityEngine;

/// <summary>
/// BaseEnemy
/// </summary>
public class BaseEnemy : BounceableBehaviour, IDamageable, IElementEffectable
{
    [Header("Base Settings")]
    public Transform player;
    public int health;

    [Header("State Settings")]
    public EnemyStates enemyState;
    public Vector2 target;
    public float searchRadius;
    public float timePerSearch;
    protected float searchTime;

    [Header("Base Attack Settings")]
    public Transform attackPoint;
    public LayerMask attackLayers;
    public float attackCooldown;
    public int attackDamage;
    protected float timeSinceAttacked;

    [Header("Events")]
    public GameEvent deathEvent;

    protected override void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        base.Awake();
    }

    protected override void Update()
    {
        //State Machine
        switch (enemyState)
        {
            case EnemyStates.Wander:
                WanderState();
                break;
            case EnemyStates.Attack:
                AttackState();
                break;
        }

        base.Update();

        timeSinceAttacked += Time.deltaTime;
    }

    protected virtual void WanderState()
    {

    }

    protected virtual void AttackState()
    {
        
    }

    protected virtual void Attack()
    {
        timeSinceAttacked = 0;
    }

    protected virtual Vector2 GetRandomTarget()
    {
        Vector2 randomPosition = (Vector2)transform.position + (Random.insideUnitCircle * searchRadius);
        
        return randomPosition;
    }

    protected virtual void TakeDamage(int _damage)
    {
        health -= _damage;

        if (health <= 0)
            Death();
    }

    public void Damage(int _damage)
    {
        TakeDamage(_damage);
    }

    public void Death()
    {
        deathEvent.Raise(this, null);
        Destroy(gameObject);
    }

    public virtual void TakeElementEffect(Elements _element)
    {
        switch (_element)
        {
            case Elements.none:
                break;
            case Elements.fire:

                break;
            case Elements.lightning:

                break;
            case Elements.slime:

                break;
            case Elements.wind:

                break;
        }
    }

    [ContextMenu("Switch To Attack State")]
    public virtual void SetAttackState()
    {
        enemyState = EnemyStates.Attack;
    }

    [ContextMenu("Switch To Wander State")]
    public virtual void SetWanderState()
    {
        enemyState = EnemyStates.Wander;
    }

}

public enum EnemyStates
{ 
    Wander,
    Attack
}

