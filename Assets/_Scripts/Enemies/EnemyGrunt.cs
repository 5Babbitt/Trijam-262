using UnityEngine;

/// <summary>
/// EnemyGrunt
/// </summary>
public class EnemyGrunt : MoveableEnemy
{
    // Added State machine about 

    [Header("Grunt Settings")]
    public float attackRadius;
    public Transform attackPoint;
    public LayerMask attackLayers;

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

        if (CanAttack()) Attack();
    }

    protected override void WanderState()
    {
        // Randomly walk around
        // If player enters area of sight switch to chase state
    }

    protected override void ChaseState()
    {
        // Keep track of and chase player
        // If player is in attack range, switch to attack state
    }

    protected override void AttackState()
    {
        // Attack the player
        // If player exits attack range, switch to chase state
    }

    protected override void Attack()
    {
        GetDamageableObject().Damage(attackDamage);
        base.Attack();
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
