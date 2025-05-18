using DG.Tweening;
using UnityEngine;

public class SwingMeleeWeapon : MonoBehaviour
{
    public float swingAngle = 90f;         // Total swing arc
    public float swingDuration = 0.3f;     // Time for one swing
    public float windupAngle = 20f;         // How far to rotate back during wind-up
    public float windupDuration = 0.3f;
    public float cooldown = 1.5f;
    public int damage = 10;
    public float detectionRadius = 2f;
    public LayerMask enemyLayer;
    public Vector3 centerOffset;

    private bool isSwinging = false;
    private float cooldownTimer = 0f;
    private Quaternion originalRot;

    void Update()
    {
        cooldownTimer -= Time.deltaTime;

        if (!isSwinging && cooldownTimer <= 0f && EnemyInRange())
        {
            DoSwing();
        }
    }

    private bool EnemyInRange()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position + centerOffset, detectionRadius, enemyLayer);
        return hits.Length > 0;
    }

    private void DoSwing()
    {
        isSwinging = true;

        float startAngle = -swingAngle / 2;
        float endAngle = swingAngle / 2;
        float prepAngle = -windupAngle;

        Sequence swingSequence = DOTween.Sequence();

        // Reset rotation first
        transform.localRotation = Quaternion.Euler(0, 0, 0);

        swingSequence
            .Append(transform.DOLocalRotate(new Vector3(0, 0, prepAngle), windupDuration)) // Wind-up back
            .Append(transform.DOLocalRotate(new Vector3(0, 0, endAngle), swingDuration).SetEase(Ease.OutCubic)) // Fast swing
            .AppendCallback(() => DealDamage()) // Damage on hit
            .Append(transform.DOLocalRotate(Vector3.zero, 0.15f)) // Return to center
            .OnComplete(() =>
            {
                cooldownTimer = cooldown;
                isSwinging = false;
            });
    }

    private void DealDamage()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position + centerOffset, detectionRadius, enemyLayer);
        foreach (var hit in hits)
        {
            var health = hit.GetComponent<IDamageable>();
            if (health != null)
                health.TakeDamage(damage);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + centerOffset, detectionRadius);
    }
}
