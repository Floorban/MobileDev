using UnityEngine;

public class Shotgun : GunController
{
    [Header("Shotgun Settings")]
    public int pelletCount = 6;
    public float spreadAngle = 20f;
    public float speedRandom = 5f;
    public override int AmmoCostPerShot => pelletCount;
    public override void Perform() {
        if (!TryFire(AmmoCostPerShot))
            return;

        Vector2 dir = new Vector2(Random.Range(0, 1), Random.Range(0, 1));
        ShootProjectile(dir);
        player.ApplyRecoil(dir, recoilForce);
    }
    public override void ShootProjectile(Vector2 baseDirection) {
        float baseAngle = Mathf.Atan2(baseDirection.y, baseDirection.x) * Mathf.Rad2Deg;
        for (int i = 0; i < pelletCount; i++) {
            float angleOffset = Random.Range(-spreadAngle / 2f, spreadAngle / 2f);
            float finalAngle = baseAngle + angleOffset;
            Vector2 shotDir = new Vector2(
                    Mathf.Cos(finalAngle * Mathf.Deg2Rad), 
                    Mathf.Sin(finalAngle * Mathf.Deg2Rad)
            ).normalized;

            float speed = Random.Range(bulletSpeed - speedRandom, bulletSpeed + speedRandom);
            GameObject bullet = Instantiate(bulletPrefab,transform.position, Quaternion.identity);
            AttackComponent bac = bullet.AddComponent<AttackComponent>();
            bac.damageAmount = baseDamage;
            Rigidbody2D brb = bullet.GetComponent<Rigidbody2D>();
            brb.constraints = RigidbodyConstraints2D.FreezeRotation;
            brb.linearVelocity = shotDir * speed;
            Destroy(bullet, bulletLifetime);
        }
    }
}
