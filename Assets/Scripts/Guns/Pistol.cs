using UnityEngine;

public class Pistol : GunController
{
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
    public override void Fire(Vector2 direction) {
        if (!TryConsumeAmmo())
            return;

        GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);
        bullet.GetComponent<Rigidbody2D>().linearVelocity = direction.normalized * bulletSpeed;

        //Invoke(nameof(ResetCooldown), cooldown);

        ApplyRecoil(direction);
    }
    private void Update() {
        HandleReloadInput();
    }
}
