using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(RangeVisualizer))]
public class ChefStation : MonoBehaviour
{
    private CircleCollider2D col;
    public ProjectileBehaviour projectile;
    private Player player;
    private RangeVisualizer rangeVisual;
    public float triggerRange = 3f;
    private bool nearby;
    private float cooldownTimer = 0f;
    public bool CanActivate => projectile && nearby && cooldownTimer <= 0f;

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
        UpdateRange(triggerRange);
    }

    private void InitComponents()
    {
        col = GetComponent<CircleCollider2D>();
        col.isTrigger = true;
        player = FindFirstObjectByType<Player>();
        rangeVisual = GetComponent<RangeVisualizer>();
    }
    public void TryActivate(Player p)
    {
        if (CanActivate)
        {
            projectile.Activate(p.transform);
            cooldownTimer = projectile.cooldown;
        }
    }
    public void UpdateRange(float newRange)
    {
        triggerRange = newRange;
        col.radius = newRange;
        rangeVisual.SetRadius(newRange);
    }
    public void HandleBehaviour(bool active)
    {
        if (active)
        {
            //do cooldown ui
        }
        else
        {
            projectile.Deactivate();
        }
    }
    void Update()
    {
        if (cooldownTimer > 0) cooldownTimer -= Time.deltaTime;
        else cooldownTimer = 0;

        TryActivate(player);
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
