using UnityEngine;

/// <summary>
/// EnemyTurret
/// </summary>
public class EnemyTurret : StationaryEnemy
{
    [Header("Turret Settings")]
    public float projectileSpeed;
   
    public int projectileHealth;
    public GameObject projectile;

    bool playerInSight;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();

        timeSinceAttacked = 0;
    }

    protected override void Update()
    {
        base.Update();

        timeSinceAttacked += Time.deltaTime;
    }

    protected override void WanderState()
    {
        // Randomly look around
        searchTime += Time.deltaTime;
        
        if (searchTime > timePerSearch) 
        {
            target = GetRandomTarget();
            searchTime = 0;
        }

        // If player enters area of sight switch to chase state

    }

    protected override void AttackState()
    {
        // Keep track of player position
        target = player.transform.position;
        // Attack the player
        if (CanAttack()) Attack();
    }

    protected override void Attack()
    {
        Vector2 targetDirection = (target - (Vector2)attackPoint.position).normalized;

        Projectile thisProjectile = Instantiate(projectile, attackPoint.position, Quaternion.identity).GetComponent<Projectile>();
        thisProjectile.SetProjectileStats(targetDirection, projectileSpeed, attackDamage, projectileHealth);

        timeSinceAttacked = 0;
    }

    private bool CanAttack()
    {
        // Raycast to player and if anything in line of sight return false
        RaycastHit2D hit = Physics2D.Raycast(attackPoint.position, (player.position - attackPoint.position).normalized);

        playerInSight = (hit.collider.tag == player.tag);
        bool timeToAttack = (timeSinceAttacked > attackCooldown);

        return playerInSight && timeToAttack;
    }


    private void OnDrawGizmos()
    {
        if (playerInSight) { Gizmos.color = Color.green; }
        else { Gizmos.color = Color.red; }
        
        Gizmos.DrawLine(attackPoint.position, target);
    }
}
