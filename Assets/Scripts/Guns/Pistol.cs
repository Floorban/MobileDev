  using UnityEngine;

public class Pistol : GunController
{
    public override int AmmoCostPerShot => 1;
    public override void Perform()
    {
        if (!TryFire(AmmoCostPerShot))
            return;

        Vector2 dir = new Vector2(Random.Range(0, 1), Random.Range(0, 1));
        ShootProjectile(dir);
        player.ApplyRecoil(dir, recoilForce);
    }

    public override void ShootProjectile(Vector2 direction) {
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        AttackComponent bac = bullet.AddComponent<AttackComponent>();
        bac.damageAmount = baseDamage;
        Rigidbody2D brb = bullet.GetComponent<Rigidbody2D>();
        brb.constraints = RigidbodyConstraints2D.FreezeRotation;
        brb.linearVelocity = direction.normalized * bulletSpeed;
        Destroy(bullet, bulletLifetime);
    }
}
