using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : BounceableBehaviour
{
    private Camera cam;

    //Aiming
    [Header("Aim Settings")]
    public Transform target;
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
    public float fireCooldown;
    float timeSinceFired;

    protected override void Awake()
    {
        base.Awake();

        cam = Camera.main;
    }

    void Start()
    {
        timeSinceFired = fireCooldown;
    }

    protected override void Update()
    {
        CalculateAim();

        // Timers
        timeSinceFired += Time.deltaTime;

        base.Update();
    }

    private void FixedUpdate()
    {
        // Aiming
        rb.MoveRotation(lookAngle);
    }

    void CalculateAim()
    {
        Vector2 _mousePos = cam.ScreenToWorldPoint(mousePos);

        targetDirection = (_mousePos - rb.position).normalized;
        target.position = Vector2.Distance(rb.position, _mousePos) < targetDistance ? _mousePos : rb.position + (targetDirection * targetDistance);
        lookAngle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg - 90;
    }

    // Input Methods
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
}
