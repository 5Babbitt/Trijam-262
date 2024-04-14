using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// MoveableEnemy
/// </summary>
public class MoveableEnemy : BaseEnemy
{
    protected NavMeshAgent agent;

    [Header("Movement Settings")]
    [SerializeField] protected float speed;
    [SerializeField] protected float stopDistance;

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

    protected override Vector2 GetRandomTarget()
    {
        var path = new NavMeshPath();
        Vector2 _target = base.GetRandomTarget();
        bool pathValid = false;

        while (!pathValid)
        {
            _target = base.GetRandomTarget();

            if (agent.CalculatePath(_target, path))
                pathValid = true;
        }

        return _target;
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

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.color = Color.white;
        
        UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.forward, stopDistance);
    }
}
