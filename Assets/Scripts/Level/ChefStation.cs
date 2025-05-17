using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(RangeVisualizer))]
public class ChefStation : MonoBehaviour
{
    private CircleCollider2D col;
    public ProjectileBehaviour projectile;
    private GameObject playerObj;
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
        playerObj = player.gameObject;
        rangeVisual = GetComponent<RangeVisualizer>();
    }
    public void TryActivate(Player p)
    {
        if (CanActivate)
        {
            projectile.Activate(p.transform.position, p.moveDir);
            cooldownTimer = projectile.cooldown;
        }
    }
    public void UpdateRange(float newRange)
    {
        triggerRange = newRange;
        col.radius = newRange;
        rangeVisual.SetRadius(newRange);
    }
    void Update()
    {
        if (cooldownTimer > 0) cooldownTimer -= Time.deltaTime;
        else cooldownTimer = 0;

        TryActivate(player);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == playerObj)
            PlayerInRange = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == playerObj)
            PlayerInRange = false;
    }
}
