using UnityEngine;

public class AttackComponent : MonoBehaviour
{
    public int damageAmount;
    public float stunTime;
    //public ParticleSystem hitParc;
    private void OnTriggerEnter(Collider other) {
        IDamageable damageable = other.GetComponent<IDamageable>();
        if (damageable != null) {
            damageable.TakeDamage(damageAmount, stunTime);
        }
        // TO DO: get the contact point and spawn particle
        Destroy(gameObject);
    }
}
