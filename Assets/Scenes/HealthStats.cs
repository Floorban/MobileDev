using UnityEngine;

[CreateAssetMenu(fileName = "HealthStats", menuName = "Scriptable Objects/HealthStats")]
public class HealthStats : ScriptableObject
{
    private float value;
    public float Value {
        get => health/maxHealth;
        set { value = health / maxHealth; }
    }

    [SerializeField] private float health;
    public float maxHealth;
    public void UpdateHealth() {

    }
}
