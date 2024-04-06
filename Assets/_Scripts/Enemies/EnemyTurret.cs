using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// EnemyTurret
/// </summary>
public class EnemyTurret : BaseEnemy
{
    [Header("Turret Settings")]
    public float projectileSpeed;
   
    public int projectileHealth;
    public GameObject projectile;
    public Transform attackPoint;
    public LayerMask attackLayers;

    public Vector2 target;

    bool playerInSight;

    protected override void Awake()
    {
        base.Awake();
    }

    void Start()
    {
        timeSinceAttacked = 0;
    }

    protected override void Update()
    {
        base.Update();

        LookAtTarget();

        if (CanAttack()) Attack();

        timeSinceAttacked += Time.deltaTime;
    }

    protected override void WanderState()
    {
        // Randomly look around
        searchTime += Time.deltaTime;
        
        if (searchTime > timePerSearch) 
        {
            Vector2 randomPosition = Random.insideUnitCircle * searchRadius;
            target = randomPosition;
        }

        // If player enters area of sight switch to chase state

    }

    protected override void ChaseState()
    {
        // Keep track of player position
        target = player.transform.position;

        // If player is in line of sight switch to attack state
    }

    protected override void AttackState()
    {
        // Attack the player
        // If player exits line of sight switch to chase state
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

    void LookAtTarget()
    {
        Vector2 targetDirection = target - (Vector2)attackPoint.position;

        float lookAngle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg - 90;
        transform.rotation = Quaternion.AngleAxis(lookAngle, Vector3.forward);
    }

    private void OnDrawGizmos()
    {
        if (playerInSight) { Gizmos.color = Color.green; }
        else { Gizmos.color = Color.red; }
        
        Gizmos.DrawLine(attackPoint.position, target);
    }
}
