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
    public Vector2 moveDirection;
    public Vector2 movement;
    public Vector2 movementForce;
    public float moveSpeed;

    //Dash
    public float dashSpeed;
    public bool dashInAimDirection;

    //Drag
    public float stopDrag;
    public float moveDrag;

    public AnimationCurve forceModifier;

    //Aiming
    [Header("Aim Settings")]
    public Vector2 mousePos;
    public float lookAngle;
    public Transform target;
    public Vector2 targetDirection;
    public float targetDistance = 7f;
    public Camera cam;

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
    [Header("Events")]
    public GameEvent playerPositionEvent;
    
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
        // Movement
        movementForce = rb.mass * movement * forceModifier.Evaluate(Vector2.Dot(rb.velocity.normalized, moveDirection.normalized));

        rb.AddForce(movementForce);

        // Aim
        Vector2 _mousePos = cam.ScreenToWorldPoint(mousePos);

        targetDirection = (_mousePos - rb.position).normalized;
        target.position = Vector2.Distance(rb.position, _mousePos) < targetDistance ? _mousePos : rb.position + (targetDirection * targetDistance);

        timeSinceDashed += Time.deltaTime;
        timeSinceFired += Time.deltaTime;

        base.Update();
    }

    private void FixedUpdate()
    {
        // Movment
        rb.AddForce(movementForce * rb.drag);

        rb.drag = (moveDirection == Vector2.zero) ? stopDrag : moveDrag;
        
        float lookAngle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg - 90;
        rb.MoveRotation(lookAngle);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Wall" || collision.collider.tag == "Entity")
        {
            Bounce(collision, lastVelocity.magnitude);
        }
    }

    // Input Methods
    void OnMove(InputValue inputValue) 
    {
        moveDirection = inputValue.Get<Vector2>();

        movement = moveDirection * moveSpeed;
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

        var _dashDirection = (dashInAimDirection) ? targetDirection : moveDirection;

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

