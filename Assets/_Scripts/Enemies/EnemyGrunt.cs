using UnityEngine;
using UnityEngine.EventSystems;

public class EnemyGrunt : BaseEnemy
{

    [Header("Grunt Settings")]
    public float speed;
    public Transform attackPoint;
    public float attackRadius;
    public float attackCooldown;

    private float timeSinceAttacked;

    private void Start()
    {
        agent.speed = speed;
    }

    protected override void Update()
    {
        agent.SetDestination(player.position);

        base.Update(); 
        
        RotateInMoveDirection();

        timeSinceAttacked += Time.deltaTime;
    }

    protected override void Attack(IDamageable damageable)
    {
        damageable.Damage(damage);
        timeSinceAttacked = 0;
    }

    void RotateInMoveDirection()
    {
        if (agent.velocity == Vector3.zero)
            return;
   
        Vector2 moveDirection = new Vector2(agent.velocity.x, agent.velocity.y);

        if (moveDirection == Vector2.zero)
            return;
        
        float lookAngle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg - 90;
        transform.rotation = Quaternion.AngleAxis(lookAngle, Vector3.forward);
    }
}
