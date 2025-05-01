using UnityEngine;

public class GunBase : MonoBehaviour {
    [Header("Gun Stats")]
    public bool reverseAimDir;
    [HideInInspector] public int  inputAimDIr = 1;
    public bool autoReload = true;
    public int clipSize = 6;
    public float cooldown = 0.3f;
    public float bulletSpeed = 20f;
    public float recoilForce = 5f;
    public int baseDamage;

    [Header("References")]
    [SerializeField] protected GameObject bulletPrefab;
    public Transform shootPoint;

    protected int currentAmmo;
    public bool canShoot = true;
    protected float lastShotTime;
    protected bool isReloading = false;
    protected Vector2 aimDir;
    private void Awake() {
        currentAmmo = clipSize;
        if (reverseAimDir)
            inputAimDIr = -1;
        else
            inputAimDIr = 1;
    }
    public virtual void Setup() {
        if (reverseAimDir)
            inputAimDIr = -1;
        else
            inputAimDIr = 1;

        lastShotTime = -1;
        Debug.Log(name + " in hand");
    }

    /*    
     *    // handle different fire logic with abstract class now
     *    protected void Fire(Vector2 direction) {
            if (!canShoot || currentAmmo <= 0)
                return;

            GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);
            bullet.GetComponent<Rigidbody2D>().linearVelocity = direction.normalized * bulletSpeed;

            playerRb.AddForce(-direction.normalized * recoilForce, ForceMode2D.Impulse);
            currentAmmo--;
            canShoot = false;
            Invoke(nameof(ResetCooldown), cooldown);
        }*/
    public bool TryFire(int consumedAmmo) {
        // try consume ammo and check cooldown first
        if (currentAmmo <= 0 || !canShoot || Time.time - lastShotTime < cooldown)
            return false;

        // - used amount of ammo
        currentAmmo -= consumedAmmo;
        Debug.Log(currentAmmo + " left");
        // activate cooldown
        lastShotTime = Time.time;
        canShoot = false;
        Invoke(nameof(ResetCooldown), cooldown);

        if (currentAmmo <= 0 && autoReload) Reload();
        return true;
    }
    protected void ResetCooldown() {
        canShoot = true;
    }
    public void Reload() {
        if (isReloading) return;
        isReloading = true;
        Debug.Log("reloading");
        // add anim and sfx here
        Invoke(nameof(FinishReload), 1f);
    }
    protected void FinishReload() {
        currentAmmo = clipSize;
        isReloading = false;
        Debug.Log("reloaded");
    }
}
