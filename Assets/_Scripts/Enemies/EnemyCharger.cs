using System.Collections;
using UnityEngine;

/// <summary>
/// EnemyCharger
/// </summary>
public class EnemyCharger : MoveableEnemy
{
    [Header("Charger Settings")]
    [SerializeField] private float blastRadius;
    [SerializeField] private float blastFuseTime;
    [SerializeField] private Color blastGizmosColor;
    [SerializeField] private GameObject blast;

    protected override void Start()
    {
        base.Start();

        agent.speed = speed;
        agent.stoppingDistance = stopDistance;
        SetWanderState();
    }

    protected override void Update()
    {
        agent.SetDestination(target);

        base.Update();
    }

    protected override void WanderState()
    {
        
        
        base.WanderState();
    }

    protected override void AttackState()
    {
        if (Vector2.Distance(transform.position, player.transform.position) < stopDistance)
        {
            target = transform.position;
            agent.stoppingDistance = 0;

            StartCoroutine(BlastFuse());
        }
        else
        {
            target = player.position;
        }
        
        base.AttackState();
    }

    IEnumerator BlastFuse()
    {
        yield return new WaitForSeconds(blastFuseTime);
        Debug.Log("Blast");
        Blast();
    }

    void Blast()
    {
        GameObject blastObject = Instantiate(blast, transform.position, Quaternion.identity);
        
        Collider2D[] damageableObjects = Physics2D.OverlapCircleAll(transform.position, blastRadius);
        Debug.Log(damageableObjects.Length);

        foreach (Collider2D col in damageableObjects)
        {
            IDamageable damageable = col.gameObject.GetComponent<IDamageable>();
            if (damageable == null) continue;

            Debug.Log(damageable != null);

            damageable.Damage(attackDamage);
            Damage(attackDamage);
        }
    }

    protected override void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();

        Gizmos.color = blastGizmosColor;
        Gizmos.DrawWireSphere(transform.position, blastRadius);
    }
}
