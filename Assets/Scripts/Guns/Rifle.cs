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
    private void FixedUpdate() {
        currentHeat = Mathf.Max(currentHeat, 0f);
        if (currentHeat > 0f)
            currentHeat -= heatCooldownRate * Time.fixedDeltaTime;
        if (isOverheated && currentHeat <= maxHeat * heatCooldown)
            isOverheated = false;
    }
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