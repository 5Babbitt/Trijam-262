using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// EnemyGrunt
/// </summary>
public class EnemyGrunt : BaseEnemy
{
    // Added State machine about 

    [Header("Grunt Settings")]
    public float speed;
    public float stopDistance;
    public float attackRadius;
    public float attackCooldown;
    public Transform attackPoint;
    public LayerMask attackLayers;

    private float timeSinceAttacked;

    private void Start()
    {
        agent.speed = speed;
        agent.stoppingDistance = stopDistance;
    }

    protected override void Update()
    {
        agent.SetDestination(player.position);

        base.Update(); 

        timeSinceAttacked += Time.deltaTime;

        if (CanAttack()) Attack(GetDamageableObject());
    }

    protected override void Attack(IDamageable damageable)
    {
        damageable.Damage(damage);
        timeSinceAttacked = 0;
    }

    private bool CanAttack()
    {
        bool playerInRange = Physics2D.OverlapCircle(attackPoint.position, attackRadius, attackLayers);
        bool timeToAttack = (timeSinceAttacked > attackCooldown);

        return playerInRange && timeToAttack;
    }

    private IDamageable GetDamageableObject()
    {
        return Physics2D.OverlapCircle(attackPoint.position, attackRadius, attackLayers).GetComponent<IDamageable>();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(attackPoint.position, attackRadius);

        Gizmos.color = Color.white;

        Gizmos.DrawWireSphere(transform.position, stopDistance);

    }
}
