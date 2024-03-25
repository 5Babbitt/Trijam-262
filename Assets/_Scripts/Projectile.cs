using UnityEngine;

public class Projectile : BounceableBehaviour, IDamageable, IElementEffectable
{
    public Vector2 direction;
    public float speed;
    public int damage;
    public int health;
    public float timeToDeath = 7f;

    private float timeTillDeath;

    public Elements element;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Wall")
        {
            Damage(1);
            Bounce(collision, speed);
        }

        var damageableObject = collision.collider.gameObject?.GetComponent<IDamageable>();
        var elementEffectableObject = collision.collider.gameObject?.GetComponent<IElementEffectable>();

        if (damageableObject != null)
        {
            damageableObject.Damage(damage);

            Death();
        }
    }

    private void Start()
    {
        timeTillDeath = timeToDeath;
        rb.velocity = direction * speed;
    }

    protected override void Update()
    {
        if (timeTillDeath <= 0) Death();

        timeTillDeath -= Time.deltaTime;

        base.Update();
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

    public void SetProjectileStats(Vector2 _direction, float _speed, int _damage, int _health)
    {
        direction = _direction;
        speed = _speed;
        damage = _damage;
        health = _health;
    }

    public void TakeElementEffect(Elements _element)
    {
        element = _element;
    }
}
