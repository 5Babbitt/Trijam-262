using UnityEngine;

/// <summary>
/// EnemyTurret
/// </summary>
public class EnemyTurret : BaseEnemy
{
    [Header("Turret Settings")]
    public float attackCooldown;
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
        base.Update();

        timeSinceAttacked += Time.deltaTime;
    }

    private bool CanAttack()
    {
        bool playerInRange = Physics2D.OverlapCircle(attackPoint.position, attackRadius, attackLayers);
        bool timeToAttack = (timeSinceAttacked > attackCooldown);

        return playerInRange && timeToAttack;
    }
}
