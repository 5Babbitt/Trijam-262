using System.Collections;
using FiveBabbittGames;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// BaseEnemy
/// </summary>
public class BaseEnemy : BounceableBehaviour, IDamageable, IElementEffectable
{
    [Header("Base Settings")]
    [SerializeField] protected Transform player;
    [SerializeField] protected int health;

    [Header("State Settings")]
    [SerializeField] protected EnemyStates enemyState;
    [SerializeField] protected Vector2 target;
    [SerializeField] protected float timePerSearch;
    protected float searchTime;

    [Header("Base Attack Settings")]
    [SerializeField] protected Transform attackPoint;
    [SerializeField] protected LayerMask attackLayers;
    [SerializeField] protected float attackCooldown;
    [SerializeField] protected int attackDamage;
    protected float timeSinceAttacked;

    [Header("FOV Settings")]
    [SerializeField] protected float viewRadius;
    [SerializeField, Range(0, 360)] protected float viewAngle;
    [SerializeField] protected LayerMask targetMask;
    [SerializeField] protected LayerMask obstacleMask;
    protected bool playerInView;
    protected bool playerDetected;

    [Header("Events")]
    public GameEvent deathEvent;
    public GameEvent playerDetectedEvent;

    protected override void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        base.Awake();
    }

    protected virtual void Start()
    {
        StartCoroutine(FOVCheck(0.2f));
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
        Vector2 randomPosition = (Vector2)transform.position + (Random.insideUnitCircle * viewRadius);
        
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

        if (enemyState != EnemyStates.Attack) return;

        PlayerDetected();
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

    protected virtual void PlayerDetected()
    {
        if (enemyState == EnemyStates.Attack) return;
        
        playerDetected = true;
        playerDetectedEvent.Raise();
        SetAttackState();
    }

    public void OnPlayerDetected(Component component, object data)
    {
        if (enemyState == EnemyStates.Attack) return;
        SetAttackState();
    }

    // FOV Methods
    IEnumerator FOVCheck(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            FOV();
        }
    }

    void FOV()
    {
        Collider2D[] targetsInRange = Physics2D.OverlapCircleAll(transform.position, viewRadius, targetMask);

        if (targetsInRange.Length <= 0)
            return;

        Transform viewTarget = targetsInRange[0].transform;
        Vector2 directionToTarget = (viewTarget.position - transform.position).normalized;

        if (Vector2.Angle(transform.up, directionToTarget) < viewAngle / 2)
        {
            float distanceToTarget = Vector2.Distance(transform.position, viewTarget.position);
            
            if (!Physics2D.Raycast(transform.position, directionToTarget, distanceToTarget, obstacleMask))
            {
                playerInView = true;
                PlayerDetected();
            }
            else
                playerInView = false;
        }
        else
            playerInView = false;
    }

    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;

        Gizmos.DrawLine(transform.position, transform.position + (transform.up) * 2);
    }

    protected virtual void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        
        UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.forward, viewRadius);

        Vector3 leftAngle = DirectionFromAngle(-transform.eulerAngles.z, -viewAngle / 2);
        Vector3 rightAngle = DirectionFromAngle(-transform.eulerAngles.z, viewAngle / 2);

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + leftAngle * viewRadius);
        Gizmos.DrawLine(transform.position, transform.position + rightAngle * viewRadius);

        if (playerInView)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, player.transform.position);
        }
    }

    Vector2 DirectionFromAngle(float eulerY, float angle)
    {
        angle += eulerY;

        return new Vector2(Mathf.Sin(angle * Mathf.Deg2Rad), Mathf.Cos(angle * Mathf.Deg2Rad));
    }
}

public enum EnemyStates
{ 
    Wander,
    Attack
}