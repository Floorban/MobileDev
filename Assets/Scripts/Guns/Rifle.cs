using UnityEngine;

public class Rifle : GunController
{
    [Header("Overheat Settings")]
    public float heatPerShot = 10f;
    public float maxHeat = 100f;
    public float heatCooldownRate = 20f;
    public float heatCooldown = 0.2f;
    [SerializeField] private float currentHeat = 0f;
    [SerializeField] private bool isOverheated = false;
    public override int AmmoCostPerShot => 1;
    public override FireMode fireMode => FireMode.Auto;

    public override void Initialize() {
        canShoot = true;
    }
    private void FixedUpdate() {
        currentHeat = Mathf.Max(currentHeat, 0f);
        if (currentHeat > 0f)
            currentHeat -= heatCooldownRate * Time.fixedDeltaTime;
        if (isOverheated && currentHeat <= maxHeat * heatCooldown)
            isOverheated = false;
    }
    public override void OnTouchBegin(Vector2 screenPos) {
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
        aimDir = (worldPos - (Vector2)transform.position).normalized;
    }
    public override void OnTouchDrag(Vector2 screenPos) {
        if (isOverheated) return;
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
        aimDir = (worldPos - (Vector2)transform.position).normalized;

        if (Time.time - lastShotTime >= cooldown && TryFire(AmmoCostPerShot, true)) {
            ShootProjectile(inputAimDIr * aimDir);
            currentHeat += heatPerShot;
            if (currentHeat >= maxHeat) {
                isOverheated = true;
            }
        }
        if (canShoot)
            player.ApplyRecoil(inputAimDIr * aimDir, recoilForce);
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