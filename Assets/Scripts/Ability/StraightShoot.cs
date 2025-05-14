using UnityEngine;

public class StraightShoot : Ability
{
    public GameObject projectilePrefab;
    public Vector2 shootDir;

    public int baseDamage;
    public float projectileSpeed;
    private float projectileLifetime;
    private void Awake()
    {
        projectileLifetime = 2;
    }
    private void Update()
    {
        Shoot(performPoint);
    }
    private void Shoot(Transform shootPoint)
    {
        if (!canPerform || shootDir == Vector2.zero) return;

        GameObject bullet = Instantiate(projectilePrefab, shootPoint.transform.position, shootPoint.transform.rotation);
        AttackComponent bac = bullet.AddComponent<AttackComponent>();
        bac.damageAmount = baseDamage;
        Rigidbody2D brb = bullet.GetComponent<Rigidbody2D>();
        brb.constraints = RigidbodyConstraints2D.FreezeRotation;
        brb.linearVelocity = shootDir.normalized * projectileSpeed;
        Destroy(bullet, projectileLifetime);

        StartCooldown();
    }
}
