using UnityEngine;

public class Shotgun : GunController
{
    [Header("Shotgun Settings")]
    public int pelletCount = 6;
    public float spreadAngle = 20f;

    public override void Initialize(Rigidbody2D rb) {
        Setup(rb);
    }

    public override void OnTouchBegin(Vector2 screenPos) {
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
        aimDir = (worldPos - (Vector2)transform.position).normalized;
    }

    public override void OnTouchDrag(Vector2 screenPos) {
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
        aimDir = (worldPos - (Vector2)transform.position).normalized;
    }

    public override void OnTouchEnd(Vector2 screenPos) {
        // handle cooldown in GunBase
/*        if (Time.time - lastShotTime < cooldown || currentAmmo <= 0)
            return;*/

        Fire(aimDir);
    }
    public override void Fire(Vector2 baseDirection) {
        if (!TryConsumeAmmo())
            return;

        float baseAngle = Mathf.Atan2(baseDirection.y, baseDirection.x) * Mathf.Rad2Deg;

        for (int i = 0; i < pelletCount; i++) {
            float offset = Random.Range(-spreadAngle / 2f, spreadAngle / 2f);
            float finalAngle = baseAngle + offset;
            Vector2 shotDir = new Vector2(Mathf.Cos(finalAngle * Mathf.Deg2Rad), Mathf.Sin(finalAngle * Mathf.Deg2Rad));

            GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);
            bullet.GetComponent<Rigidbody2D>().linearVelocity = shotDir * bulletSpeed;
        }

        ApplyRecoil(baseDirection);
    }

    // how can I keep the ammo calculation in base class while having a fire methods in abtract class and use them both at the same time?
    /*    void FireShotgun(Vector2 baseDirection) {
            if (!canShoot || currentAmmo <= 0)
                return;

            float baseAngle = Mathf.Atan2(baseDirection.y, baseDirection.x) * Mathf.Rad2Deg;

            for (int i = 0; i < pelletCount; i++) {
                float offset = Random.Range(-spreadAngle / 2f, spreadAngle / 2f);
                float finalAngle = baseAngle + offset;
                Vector2 shotDir = new Vector2(Mathf.Cos(finalAngle * Mathf.Deg2Rad), Mathf.Sin(finalAngle * Mathf.Deg2Rad));

                GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);
                bullet.GetComponent<Rigidbody2D>().linearVelocity = shotDir.normalized * bulletSpeed;
            }

            playerRb.AddForce(-baseDirection.normalized * recoilForce, ForceMode2D.Impulse);
            currentAmmo--;
            canShoot = false;
            Invoke(nameof(ResetCooldown), cooldown);
        }*/
}
