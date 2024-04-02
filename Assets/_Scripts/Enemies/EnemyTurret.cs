using UnityEngine;

/// <summary>
/// EnemyTurret
/// </summary>
public class EnemyTurret : BaseEnemy
{
    [Header("Turret Settings")]
    public float attackCooldown;
    public float projectileSpeed;
    public int projectileDamage;
    public int projectileHealth;
    public GameObject projectile;
    public Transform attackPoint;
    public LayerMask attackLayers;

    private float timeSinceAttacked;

    protected override void Awake()
    {
        base.Awake();
    }

    void Start()
    {
    }

    protected override void Update()
    {
        agent.SetDestination(player.transform.position);
        
        base.Update();

        if (CanAttack()) Fire();

        timeSinceAttacked += Time.deltaTime;
    }

    private void Fire()
    {
        Vector2 targetDirection = player.transform.position - attackPoint.position;

        Projectile thisProjectile = Instantiate(projectile, attackPoint.position, Quaternion.identity).GetComponent<Projectile>();
        thisProjectile.SetProjectileStats(targetDirection.normalized, projectileSpeed, projectileDamage, projectileHealth);

        timeSinceAttacked = 0;
    }

    private bool CanAttack()
    {
        // Raycast to player and if anything in line of sight return false
        bool playerInSight = Physics2D.Raycast(attackPoint.position, (player.position - attackPoint.position).normalized, attackLayers);
        bool timeToAttack = (timeSinceAttacked > attackCooldown);

        return playerInSight && timeToAttack;
    }
}
