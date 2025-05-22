using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(RangeVisualizer))]
public class ChefStation : MonoBehaviour
{
    private CircleCollider2D col;
    public AttackStats attack;
    public GameObject activeAttack;
    private WeaponManager player;
    private RangeVisualizer rangeVisual;
    public float triggerRange = 3f;
    private bool nearby;
    private float cooldownTimer = 0f;
    public bool CanActivate => attack && nearby;

    private bool PlayerInRange
    {
        get => nearby;
        set
        {
            nearby = value;
            rangeVisual.UpdateRange(value);
            HandleBehaviour(value);
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
        player = FindFirstObjectByType<WeaponManager>();
        rangeVisual = GetComponent<RangeVisualizer>();
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
                cooldownTimer = attack.cooldown;
                attack.Activate(player);
                activeAttack = attack.spawnedWeapons[attack.spawnedWeapons.Count - 1];
            }
        }
        else
        {
            //cooldownTimer = 0f;
            //attack.Deactivate(player);

            if (activeAttack)
            {
                player.RemoveWeapon(activeAttack);
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
}
