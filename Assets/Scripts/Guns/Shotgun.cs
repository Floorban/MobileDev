using UnityEngine;

public class Shotgun : GunController
{
    [Header("Shotgun Settings")]
    public int pelletCount = 6;
    public float spreadAngle = 20f;

    public override void Initialize() {
        canShoot = true;
        Setup();
    }
    public override void OnTouchBegin(Vector2 screenPos) {
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
        aimDir = inputAimDIr * (worldPos - (Vector2)transform.position).normalized;
    }
    public override void OnTouchDrag(Vector2 screenPos) {
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
        aimDir = inputAimDIr * (worldPos - (Vector2)transform.position).normalized;
    }
    public override void OnTouchEnd(Vector2 screenPos) {
        Fire(aimDir, pelletCount);
    }
    public override void Fire(Vector2 baseDirection, int consumedAmmo) {
        if(!TryFire(consumedAmmo)) return;

        float baseAngle = Mathf.Atan2(baseDirection.y, baseDirection.x) * Mathf.Rad2Deg;
        for (int i = 0; i < pelletCount; i++) {
            float offset = Random.Range(-spreadAngle / 2f, spreadAngle / 2f);
            float finalAngle = baseAngle + offset;
            Vector2 shotDir = new Vector2(Mathf.Cos(finalAngle * Mathf.Deg2Rad), Mathf.Sin(finalAngle * Mathf.Deg2Rad));

            GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);
            bullet.GetComponent<Rigidbody2D>().linearVelocity = shotDir * bulletSpeed;
            Destroy(bullet, 1f);
        }
    }
}
