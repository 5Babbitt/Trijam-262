using UnityEngine;

public class EnemyGrunt : BaseEnemy
{

    [Header("Grunt Settings")]
    public float speed;
    public Transform attackPoint;
    public float attackRadius;
    public float attackCooldown;

    private float timeSinceAttacked;

    protected override void Update()
    {
        agent.SetDestination(player.position);
        agent.updateUpAxis = false;
        agent.updateRotation = false;

        base.Update();

        timeSinceAttacked += Time.deltaTime;
    }

    private void Start()
    {
        agent.speed = speed;
    }

    protected override void Attack(IDamageable damageable)
    {
        damageable.Damage(damage);
        timeSinceAttacked = 0;
    }

    void RotateInMoveDirection()
    {

    }
}
