using UnityEngine;

public interface IDamageable
{
    void TakeDamage(int damageAmount, float stun = 0);
}
