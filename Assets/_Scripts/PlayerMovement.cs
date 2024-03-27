using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : BounceableBehaviour
{
    //Movment
    [Header("Move Settings")]
    public Vector2 moveInput;
    public Vector2 moveVector;
    public float maxSpeed;
    public float acceleration;
    public float deceleration;
    private float accelerationRate;

    //Dash
    [Header("Dash Settings")]
    public float dashSpeed;
    public int maxConsecutiveDashes = 3;
    private int dashCount;
    private bool isDashing;

    //Timers
    [Header("Timers")]
    public float dashTime;
    public float dashCooldown;

    private Coroutine dashRefillCoroutine;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        dashRefillCoroutine = StartCoroutine(DashRefill());
        //dashCount = maxConsecutiveDashes;
    }

    protected override void Update()
    {
        CalculateMovement();

        // Timers
        base.Update();
    }

    private void FixedUpdate()
    {
        // Movment
        rb.AddForce(rb.mass * moveVector);
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
        float _accelerationRate = (targetSpeed.sqrMagnitude > 0.01f) ? acceleration : deceleration;
        accelerationRate = (!isDashing) ? _accelerationRate : 0;
        Vector2 speedDifference = targetSpeed - rb.velocity;
        Vector2 movement = speedDifference * accelerationRate;

        moveVector = movement;
    }

    void Dash()
    {
        StartCoroutine(DashCoroutine());
    }

    IEnumerator DashCoroutine()
    {
        isDashing = true;
        dashCount--;
        StopCoroutine(dashRefillCoroutine);

        rb.AddForce(-rb.velocity, ForceMode2D.Impulse);

        rb.AddForce(rb.mass * dashSpeed * moveInput.normalized, ForceMode2D.Impulse);

        Debug.Log(dashCount);

        yield return new WaitForSeconds(dashTime);

        isDashing = false;
        dashRefillCoroutine = StartCoroutine(DashRefill());
    }

    IEnumerator DashRefill()
    {
        yield return new WaitForSeconds( dashCooldown );

        dashCount = maxConsecutiveDashes;
    }

    bool CanDash()
    {
        return !isDashing && dashCount > 0;
    }

    // Input Methods
    void OnMove(InputValue inputValue)
    {
        moveInput = inputValue.Get<Vector2>();
    }

    void OnDash(InputValue inputValue)
    {
        if (CanDash()) Dash();
    }
}
