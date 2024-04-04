using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// MoveableEnemy
/// </summary>
public class MoveableEnemy : BaseEnemy
{
    [Header("Movement Settings")]
    protected NavMeshAgent agent;
    public float speed;
    public float stopDistance;

    protected override void Awake()
    {
        base.Awake(); 
        
        agent = GetComponent<NavMeshAgent>();
        agent.updateUpAxis = false;
        agent.updateRotation = false;
    }

    protected override void Update()
    {
        base.Update();

        RotateInMoveDirection();
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
