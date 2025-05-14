using UnityEngine;

public class Ability : MonoBehaviour
{
    public Transform performPoint;
    public bool canPerform;
    private bool connected;

    [Header("Cooldown")]
    [SerializeField] protected float cooldownDuration = 0.5f;

    public virtual void Activation(bool enable)
    {
         canPerform = enable;
         connected = enable;
    }

    protected void StartCooldown()
    {
        canPerform = false;
        Invoke(nameof(ResetCD), cooldownDuration);
    }
    private void ResetCD()
    {
        if (connected)
            canPerform = true;
    }
}
