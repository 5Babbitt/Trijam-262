using UnityEngine;

public class BounceableBehaviour : MonoBehaviour
{
    protected Rigidbody2D rb;
    protected Vector2 lastVelocity;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    protected virtual void Update()
    { 
        lastVelocity = rb.velocity;
    }
    
    protected virtual void Bounce(Collision2D collision, float _speed)
    {
        Vector2 newDirection = Vector2.Reflect(lastVelocity.normalized, collision.contacts[0].normal);

        rb.AddForce(_speed * rb.mass * newDirection, ForceMode2D.Impulse);
    }
}
