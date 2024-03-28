using UnityEngine;
using Cinemachine;
using System;
using FiveBabbittGames;

/// <summary>
/// Player
/// </summary>
public class Player : BounceableBehaviour, IDamageable, IElementEffectable
{
    public int maxHealth;
    private int health;

    //Events
    public GameEvent deathEvent;

    protected override void Awake()
    {
        base.Awake();

        health = maxHealth;
    }

    protected override void Update()
    {
        
        
        base.Update();
    }

    public void Damage(int _damage)
    {
        health -= _damage;
        Debug.Log($"Health = {health}");

        if (health < 0)
            Death();
    }

    public void Death()
    {
        // Game Over
        deathEvent.Raise(this, null);
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

