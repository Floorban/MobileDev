using UnityEngine;
using System.Collections;

[RequireComponent(typeof(HealthManager))]
[RequireComponent(typeof(Collider2D))]
public class EnemyBase : MonoBehaviour, IDamageable
{
    public EnemyStats stats;
    protected HealthSystem health;
    protected bool canBeDamaged = false;
    protected bool canAct = true;
    
    
    public void TakeDamage(int damageAmount, float stun) {
        if (!canBeDamaged) return;

        health.Damage(damageAmount);
        StartCoroutine(Invincible(stats.invincibleTime));
        StartCoroutine(StunRecovery(stun));
    }

    private void Awake() {
        health = GetComponent<HealthManager>().healthSystem;
    }

    private IEnumerator Invincible(float duration) {
        if (canBeDamaged) {
            canBeDamaged = false;
            float timestep = 0;
            while (timestep < duration) {
                timestep += Time.deltaTime;
            }
            yield return new WaitForSeconds(duration);
            canBeDamaged = true;
        }
    }
    private IEnumerator StunRecovery(float duration) {
        canAct = false;
        yield return new WaitForSeconds(duration);
        canAct = true;
    }
}
