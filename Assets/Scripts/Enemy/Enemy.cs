using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.Events;

[RequireComponent(typeof(HealthManager))]
[RequireComponent(typeof(Collider2D))]
public class Enemy : MonoBehaviour, IDamageable
{
    public EnemyStats stats;
    protected HealthSystem health;
    public bool canBeDamaged = true;
    protected bool canAct = false;
    [SerializeField] private GameObject sprite;
    private Transform player;
    private Rigidbody2D rb;
    private Vector2 moveDir;

    public static UnityAction<GameObject> OnEnemyDeath;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        health = GetComponent<HealthManager>().healthSystem;
        player = FindFirstObjectByType<Player>().transform;
    }
    private void Start()
    {
        sprite.GetComponent<Character>().Active = true;
        canAct = true;
    }
    private void Update()
    {
        if (player)
        {
            moveDir = (player.position - transform.position).normalized;
        }
    }
    private void FixedUpdate()
    {
        if (canAct && player)
        {
            rb.linearVelocity = new Vector2(moveDir.x, moveDir.y) * stats.moveSpeed;
        }
    }
    public void TakeDamage(int damageAmount, float stun) {
        if (!canBeDamaged) return;

        health.Damage(damageAmount);

        if (health.GetHealth() <= 0) {
            Die();
        }
        else {
            StartCoroutine(Invincible(stats.invincibleTime));
            StartCoroutine(StunRecovery(stun));
        }
    }
    public virtual void Die() {
        OnEnemyDeath?.Invoke(gameObject);
        Destroy(gameObject, 0f);
    }
    protected IEnumerator Invincible(float duration) {
        if (canBeDamaged) {
            canBeDamaged = false;
            float timestep = 0;
            while (timestep < duration) {
                timestep += Time.deltaTime;
            }
            yield return new WaitForSeconds(duration);
            canBeDamaged = true;
        }
    }
    protected IEnumerator StunRecovery(float duration) {
        canAct = false;
        yield return new WaitForSeconds(duration);
        canAct = true;
    }
}
