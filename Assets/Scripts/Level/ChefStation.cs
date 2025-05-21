using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(RangeVisualizer))]
public class ChefStation : MonoBehaviour
{
    private CircleCollider2D col;
    public AttackBehavior attack;
    private WeaponManager player;
    private RangeVisualizer rangeVisual;
    public float triggerRange = 3f;
    private bool nearby;
    private float cooldownTimer = 0f;
    public bool CanActivate => attack && nearby && attack.EnemyInRange();

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
                attack.Activate(player);
                //cooldownTimer = attack.cooldown;
                cooldownTimer = 0f;
            }
        }
        else
        {
            attack.Deactivate(player);
            cooldownTimer = 0f;
        }
    }
    public void UpdateCD()
    {
        if (CanActivate && attack.hasActivated)
        {
            if (cooldownTimer > 0)
            {
                cooldownTimer -= Time.deltaTime;
            }
            else
            {
                cooldownTimer = attack.cooldown;
                attack.Activate(player);
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
        onEnter?.Invoke();
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == player.gameObject)
            PlayerInRange = false;
        onExit?.Invoke();
    }
}
