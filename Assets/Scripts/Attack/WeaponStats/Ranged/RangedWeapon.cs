using UnityEngine;

[CreateAssetMenu(menuName = "Attacks/SwingMeleeStats")]
public class RangedWeapon : AttackStats
{
    public GameObject projectilePrefab;
    public override void Activate(WeaponManager p)
    {
        throw new System.NotImplementedException();
    }

    public void Fire(Transform target, Vector2 dir)
    {
        GameObject proj = Instantiate(projectilePrefab, target.position, Quaternion.identity);
        Rigidbody2D rb = proj.GetComponent<Rigidbody2D>();
        rb.linearVelocity = dir * speed;
        Destroy(proj, lifeTime);
    }
}
