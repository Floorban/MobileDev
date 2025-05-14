using UnityEngine;

public class GunBase : MonoBehaviour {

    protected Player player;

    [Header("Setup")]
    public int clipSize = 6;
    public float cooldown = 0.3f;
    public float reloadDuration = 1f;
    public float bulletSpeed = 20f;
    public float recoilForce = 5f;
    [SerializeField] protected float bulletLifetime = 1f;
    public int baseDamage;
    [SerializeField] protected GameObject bulletPrefab;

    [Header("Stats")]
    protected int currentAmmo;
    public bool canShoot = true;
    protected bool isReloading = false;

    public virtual void Awake() {
        InitComponents();
        if (player) Setup(player);
    }

    private void InitComponents()
    {
        player = GetComponentInParent<Player>();
    }

    private void Setup(Player p) {
        canShoot = true;
        currentAmmo = clipSize;
        player = p;
    }

    public bool TryFire(int consumedAmmo) {
        if (currentAmmo <= 0 || !canShoot)
            return false;

        currentAmmo -= consumedAmmo;
        canShoot = false;

        if (currentAmmo <= 0)
            Reload();
        else
            Invoke(nameof(ResetCooldown), cooldown);

        return true;
    }

    protected void ResetCooldown() {
        canShoot = true;
    }

    public void Reload() {
        if (isReloading) return;
        isReloading = true;
        Invoke(nameof(FinishReload), reloadDuration);
    }

    protected void FinishReload() {
        currentAmmo = clipSize;
        isReloading = false;
        canShoot = true;
    }
}
