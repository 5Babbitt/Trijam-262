using UnityEngine;

/// <summary>
/// EnemyGrunt
/// </summary>
public class EnemyGrunt : MoveableEnemy
{
    // Added State machine about 

    [Header("Grunt Settings")]
    public float attackRadius;

    protected override void Start()
    {
        base.Start();

        agent.speed = speed;
        agent.stoppingDistance = stopDistance;
        SetWanderState();
    }

    protected override void Update()
    {
        agent.SetDestination(target);

        base.Update(); 

        timeSinceAttacked += Time.deltaTime;
    }

    protected override void WanderState()
    {
        // Randomly walk around

        // If player enters area of sight switch to chase state

    }

    protected override void AttackState()
    {
        // Keep track of and chase player
        target = player.position;
        // Attack the player
        if (CanAttack()) Attack();

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

    public override void SetAttackState()
    {
        base.SetAttackState();
        agent.stoppingDistance = stopDistance;
    }

    public override void SetWanderState()
    {
        base.SetWanderState();
        agent.stoppingDistance = 0;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(attackPoint.position, attackRadius);

        Gizmos.color = Color.white;

        Gizmos.DrawWireSphere(transform.position, stopDistance);

    }
}
