using UnityEngine;

/// <summary>
/// StationaryEnemy
/// </summary>
public class StationaryEnemy : BaseEnemy
{
    [Header("Rotation Settings")]
    public float rotationSpeed = 2.5f;

    protected override void Update()
    {
        LookAtTarget();
        
        base.Update();
    }

    protected virtual void LookAtTarget()
    {
        Vector2 targetDirection = target - (Vector2)transform.position;
        //Vector2 targetDirection = target - (Vector2)attackPoint.position;

        float currentAngle = rb.rotation;
        float targetAngle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg - 90;
        float lookAngle = Mathf.MoveTowardsAngle(currentAngle, targetAngle, rotationSpeed * Time.deltaTime);

        transform.rotation = Quaternion.AngleAxis(lookAngle, Vector3.forward);
    }
}
