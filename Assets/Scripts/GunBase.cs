using UnityEngine;

public class GunBase : MonoBehaviour {
    [Header("Gun Stats")]
    public int clipSize = 6;
    public float cooldown = 0.3f;
    public float bulletSpeed = 20f;
    public float recoilForce = 5f;

    [Header("References")]
    public GameObject bulletPrefab;
    public Transform shootPoint;

    protected int currentAmmo;
    protected bool canShoot = true;
    protected Rigidbody2D playerRb;
    protected float lastShotTime;
    public bool autoReload = true;
    protected bool isReloading = false;
    protected Vector2 aimDir;

    public virtual void Setup(Rigidbody2D rb) {
        playerRb = rb;
        currentAmmo = clipSize;
        lastShotTime = -1;
        Debug.Log(name + " in hand");
    }

    /*    protected void FireBullet(Vector2 direction) {
            if (!canShoot || currentAmmo <= 0)
                return;

            GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);
            bullet.GetComponent<Rigidbody2D>().linearVelocity = direction.normalized * bulletSpeed;

            playerRb.AddForce(-direction.normalized * recoilForce, ForceMode2D.Impulse);
            currentAmmo--;
            canShoot = false;
            Invoke(nameof(ResetCooldown), cooldown);
        }*/
    protected bool TryConsumeAmmo() {
        if (!canShoot || currentAmmo <= 0 || Time.time - lastShotTime < cooldown)
            return false;
        currentAmmo--;
        lastShotTime = Time.time;
        canShoot = false;
        Invoke(nameof(ResetCooldown), cooldown);
        // if (autoReload) Reload();
        return true;
    }
    protected void ApplyRecoil(Vector2 direction) {
        playerRb.AddForce(-direction.normalized * recoilForce, ForceMode2D.Impulse);
    }
    protected void ResetCooldown() => canShoot = true;

    protected void Reload() {
        if (isReloading)
            return;

        isReloading = true;
        Invoke(nameof(FinishReload), 1f);
    }
    protected void FinishReload() {
        currentAmmo = clipSize;
        isReloading = false;
    }

    public void HandleReloadInput() {
        if (autoReload) return;
        // shake the phone to reload
#if UNITY_IOS || UNITY_ANDROID
        // For mobile, check for shake input to reload
        if (Input.acceleration.magnitude > 1.5f)  // Trigger shake when magnitude of the acceleration is high
        {
            Reload();
        }
#elif UNITY_EDITOR || UNITY_STANDALONE
        // For desktop, check if the "R" key is pressed to reload
        if (Input.GetKeyDown(KeyCode.R)) {
            Reload();
        }
#endif
    }
}
