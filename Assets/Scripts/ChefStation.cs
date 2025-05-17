using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(RangeVisualizer))]
public class ChefStation : MonoBehaviour
{
    private CircleCollider2D col;
    public ProjectileBehaviour projectile;
    private GameObject player;
    private RangeVisualizer rangeVisual;
    public float triggerRange = 3f;
    private bool nearby;
    private float cooldownTimer = 0f;
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
        player = FindFirstObjectByType<Player>().gameObject;
        rangeVisual = GetComponent<RangeVisualizer>();
    }
    public void TryActivate(Transform player)
    {
        if (PlayerInRange)
        {
/*            projectile.Activate(transform.position, player);
            cooldownTimer = projectile.cooldown;*/
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
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == player)
            PlayerInRange = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == player)
            PlayerInRange = true;
    }
}
