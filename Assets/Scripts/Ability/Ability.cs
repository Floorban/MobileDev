using UnityEngine;

public class Ability : MonoBehaviour
{
    public Transform performPoint;
    public bool canPerform;

    [Header("Cooldown")]
    [SerializeField] protected float cooldownDuration = 0.5f;
    protected float cooldownTimer;

    public virtual void Activation(bool enable)
    {
         canPerform = enable;
    }
    protected virtual void Update()
    {
        if (cooldownTimer > 0f)
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0f)
                canPerform = true;
        }
    }
    protected void StartCooldown()
    {
        canPerform = false;
        cooldownTimer = cooldownDuration;
    }
}
