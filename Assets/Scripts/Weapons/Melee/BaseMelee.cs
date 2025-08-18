using UnityEngine;

public class BaseMelee : Melee
{
    [SerializeField] private Transform hitPoint; // where the swing is centered (usually a child on the player's hand)
    [SerializeField] private bool useLocalHitPoint = true; // if true, hitPoint position uses hitPoint, else uses transform.position

    protected override void Attack()
    {
        if (meleeData == null) return;

        Vector2 origin = (hitPoint != null && useLocalHitPoint) ? (Vector2)hitPoint.position : (Vector2)transform.position;
        float radius = meleeData.HitRadius;
        LayerMask mask = meleeData.HitMask;

        // 2D check — change to Physics.OverlapSphere for 3D projects
        Collider2D[] hits = Physics2D.OverlapCircleAll(origin, radius, mask);

        foreach (var col in hits)
        {
            // skip self
            if (col.transform == transform) continue;

            // angle check (so you only hit things in front)
            Vector2 toTarget = ((Vector2)col.transform.position - origin).normalized;

            // In top-down setups you probably use transform.right as forward for aiming (same as your bullets)
            Vector2 forward = (Vector2)transform.right;
            float angle = Vector2.Angle(forward, toTarget);

            if (angle > meleeData.HitAngle * 0.5f) continue;

            // Apply damage if target has Health
            var health = col.GetComponent<Health>();
            if (health != null)
            {
                health.TakeDamage(weaponData.Damage);
            }

            // Optional knockback for Rigidbody2D
            if (meleeData.KnockbackForce > 0f)
            {
                var rb = col.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    Vector2 knockDir = (col.transform.position - transform.position).normalized;
                    rb.AddForce(knockDir * meleeData.KnockbackForce, ForceMode2D.Impulse);
                }
            }
        }

        // Debug output (remove in production)
        Debug.Log($"{name} performed melee attack (damage: {weaponData.Damage}, radius: {radius})");
    }

    private void OnDrawGizmosSelected()
    {
        if (meleeData == null && hitPoint == null) return;
        Vector3 pos = (hitPoint != null && useLocalHitPoint) ? hitPoint.position : transform.position;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(pos, meleeData != null ? meleeData.HitRadius : 1f);
    }
}
