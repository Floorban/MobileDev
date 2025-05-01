using UnityEngine;

public class Rifle : GunController
{
    public override int AmmoCostPerShot => 1;
    public override FireMode fireMode => FireMode.Auto;

    public override void Initialize() {
        canShoot = true;
    }
    public override void OnTouchBegin(Vector2 screenPos) {
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
        aimDir = (worldPos - (Vector2)transform.position).normalized;
    }
    public override void OnTouchDrag(Vector2 screenPos) {
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
        aimDir = (worldPos - (Vector2)transform.position).normalized;

        if (Time.time - lastShotTime >= cooldown && TryFire(AmmoCostPerShot, true)) {
            ShootProjectile(aimDir);
            player.ApplyRecoil(inputAimDIr * aimDir, recoilForce);
            lastShotTime = Time.time;
        }
    }
    public override void OnTouchEnd() {
        //disable ui
    }
    public override void ShootProjectile(Vector2 direction) {
        GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);
        var brb = bullet.GetComponent<Rigidbody2D>();
        brb.constraints = RigidbodyConstraints2D.FreezeRotation;
        brb.linearVelocity = direction.normalized * bulletSpeed;
        Destroy(bullet, bulletLifetime);
    }
}
