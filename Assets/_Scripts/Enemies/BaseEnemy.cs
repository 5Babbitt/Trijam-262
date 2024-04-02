using FiveBabbittGames;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// BaseEnemy
/// </summary>
public class BaseEnemy : BounceableBehaviour, IDamageable, IElementEffectable
{
    protected NavMeshAgent agent;

    [Header("Base Settings")]
    public Transform player;
    public int health;
    public int damage;

    public GameEvent deathEvent;

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

    protected virtual void Attack(IDamageable damageable)
    {
        damageable.Damage(damage);
    }

    protected virtual void TakeDamage(int _damage)
    {
        health -= _damage;

        if (health <= 0)
            Death();
    }

    public void Damage(int _damage)
    {
        TakeDamage(_damage);
    }

    public void Death()
    {
        deathEvent.Raise(this, null);
        Destroy(gameObject);
    }

    public void TakeElementEffect(Elements _element)
    {
        switch (_element)
        {
            case Elements.fire:

                break;
            case Elements.lightning:

                break;
            case Elements.slime:

                break;
        }
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
