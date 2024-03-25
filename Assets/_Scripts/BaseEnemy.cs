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

    protected override void Awake()
    {
        base.Awake();

        agent = GetComponent<NavMeshAgent>();
        agent.updateUpAxis = false;
    }

    public void Damage(int _damage)
    {
        health -= _damage;
        
        if (health <= 0)
            Death();
    }

    public void Death()
    {
        Destroy(gameObject);
    }

    public void TakeElementEffect(Elements _element)
    {
        switch (_element)
        {
            case Elements.none:
                break;
            case Elements.fire:

                break;
            case Elements.lightning:

                break;
            case Elements.slime:

                break;
        }
    }
}