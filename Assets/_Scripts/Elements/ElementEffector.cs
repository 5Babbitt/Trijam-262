using UnityEngine;

/// <summary>
/// ElementEffector
/// </summary>
public class ElementEffector : MonoBehaviour
{
    Rigidbody2D rb;
    
    public Elements element;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var effectedObject = collision.gameObject.GetComponent<IElementEffectable>();

        if (effectedObject != null)
        {
            effectedObject.TakeElementEffect(element);
        }
    }
}

public enum Elements
{ 
    none,
    fire,
    lightning,
    slime
}

