using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(RangeVisualizer))]
public class ChefStation : MonoBehaviour
{
    [SerializeField] Character sprite;
    private CircleCollider2D col;
    public Transform lookAt;
    public WeaponStats attack;
    public GameObject activeAttack;
    [HideInInspector] public Player player;
    private WeaponManager playerWeapon;
    private RangeVisualizer rangeVisual;
    public float triggerRange = 3f;
    private bool nearby;
    private float cooldownTimer = 0f;
    public float maxCooldown;
    public bool CanActivate => attack && nearby;

    private bool PlayerInRange
    {
        get => nearby;
        set
        {
            nearby = value;
            rangeVisual.UpdateRange(value);
            HandleBehaviour(value);
            sprite.Active = value;
        }
    }

    private void Awake()
    {
        InitComponents();
        SetRange(triggerRange);

        if (attack)
        {
            attack.spawnedWeapons.Clear();
        }
    }

    private void InitComponents()
    {
        col = GetComponent<CircleCollider2D>();
        col.isTrigger = true;
        rangeVisual = GetComponent<RangeVisualizer>();
        player = FindFirstObjectByType<Player>();
        if (player != null )
            playerWeapon = player.GetComponent<WeaponManager>();
    }
    public void SetRange(float newRange)
    {
        triggerRange = newRange;
        col.radius = newRange;
        rangeVisual.SetRadius(newRange);
    }
    public void HandleBehaviour(bool active)
    {
        if (!attack) return;

        if (active)
        {
            if (!attack.hasActivated)
            {
                cooldownTimer = attack.cooldown - maxCooldown;
                attack.Activate(playerWeapon);
                activeAttack = attack.spawnedWeapons[attack.spawnedWeapons.Count - 1];
            }
        }
        else
        {
            //cooldownTimer = 0f;
            //attack.Deactivate(player);

            if (activeAttack)
            {
                playerWeapon.RemoveWeapon(activeAttack);
                attack.spawnedWeapons.Remove(activeAttack);
                activeAttack = null;
            }
        }
    }
    public void UpdateCD()
    {
        if (CanActivate)
        {
            if (cooldownTimer > 0)
            {
                cooldownTimer -= Time.deltaTime;
            }
            else
            {
                foreach (var weapon in attack.spawnedWeapons)
                {
                    AttackBehavior behavior = weapon.GetComponent<AttackBehavior>();
                    if (behavior && !behavior.isPerforming)
                        behavior.canPerform = true;
                }
                cooldownTimer = attack.cooldown;
            }
        }
    }
    void Update()
    {
        UpdateCD();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == player.gameObject)
            PlayerInRange = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == player.gameObject)
            PlayerInRange = false;
    }

    public void UpgradeHP()
    {
        Debug.Log("works");
    }
    public void UpgradeCD(float changedAmount)
    {
        maxCooldown += changedAmount;
    }
    public void UpgradeRange(float changedAmount)
    {
        triggerRange += changedAmount;
    }
}
