using UnityEngine;
using UnityEngine.InputSystem;
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

    //Movment
    [Header("Move Settings")]
    public Vector2 moveInput;
    public Vector2 moveVector;
    public float maxSpeed;
    public float acceleration;
    public float deceleration;


    //Dash
    [Header("Dash Settings")]
    public float dashSpeed;
    public bool dashInAimDirection;

    //Aiming
    [Header("Aim Settings")]
    public Transform target;
    public Camera cam;
    public Vector2 mousePos;
    public Vector2 targetDirection;
    public float targetDistance = 7f;
    public float lookAngle;

    //Firing
    [Header("Fire Settings")]
    public Transform fireTransform;
    public GameObject projectile;
    public float projectileSpeed;
    public int projectileDamage;
    public int projectileHealth;

    //Timers
    [Header("Timers")]
    public float dashCooldown;
    public float fireCooldown;
    float timeSinceDashed;
    float timeSinceFired;

    //Events

    
    protected override void Awake()
    {
        base.Awake();
        
        cam = Camera.main;
    } 
    
    void Start()
    {
        health = maxHealth;

        timeSinceDashed = dashCooldown;
        timeSinceFired = fireCooldown;
    }

    protected override void Update()
    {
        CalculateMovement();
        CalculateAim();

        // Timers
        timeSinceDashed += Time.deltaTime;
        timeSinceFired += Time.deltaTime;

        base.Update();
    }

    private void FixedUpdate()
    {
        // Movment
        rb.AddForce(moveVector);
        
        // Aiming
        rb.MoveRotation(lookAngle);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Wall" || collision.collider.tag == "Entity")
        {
            Bounce(collision, lastVelocity.magnitude);
        }
    }

    void CalculateMovement()
    {
        Vector2 targetSpeed = moveInput.normalized * maxSpeed;
        float accelerationRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : deceleration;
        Vector2 speedDifference = targetSpeed - rb.velocity.sqrMagnitude;
        Vector2 movement = speedDifference * accelerationRate;

        Debug.Log($"Target Speed:       {targetSpeed}");
        Debug.Log($"Acceleration Rate:  {accelerationRate}");
        Debug.Log($"Speed Difference:   {speedDifference}");
        Debug.Log($"Movement:           {movement}");

        moveVector = movement * moveInput;
    }

    void CalculateAim()
    {
        Vector2 _mousePos = cam.ScreenToWorldPoint(mousePos);

        targetDirection = (_mousePos - rb.position).normalized;
        target.position = Vector2.Distance(rb.position, _mousePos) < targetDistance ? _mousePos : rb.position + (targetDirection * targetDistance);
        lookAngle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg - 90;
    }

    // Input Methods
    void OnMove(InputValue inputValue) 
    {
        moveInput = inputValue.Get<Vector2>();
    }

    void OnFire(InputValue inputValue)
    {
        if (timeSinceFired < fireCooldown) return;
        
        Projectile thisProjectile = Instantiate(projectile, fireTransform.position, Quaternion.identity).GetComponent<Projectile>();
        thisProjectile.SetProjectileStats(targetDirection.normalized, projectileSpeed, projectileDamage, projectileHealth);

        timeSinceFired = 0;
    }

    void OnAim(InputValue inputValue)
    {
        mousePos = inputValue.Get<Vector2>();
    }

    void OnDash(InputValue inputValue)
    {
        if (timeSinceDashed < dashCooldown) return;

        var _dashDirection = (dashInAimDirection) ? targetDirection : moveInput;

        rb.AddForce(rb.mass * dashSpeed * _dashDirection, ForceMode2D.Impulse);
        timeSinceDashed = 0;
    }

    public void Damage(int _damage)
    {
        health -= _damage;
        Debug.Log("Took Damage");

        if (health < 0)
            Death();
    }

    public void Death()
    {
        // Game Over
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

