using UnityEngine;

[CreateAssetMenu(fileName = "HealthStats", menuName = "Scriptable Objects/HealthStats")]
public class HealthStats : ScriptableObject
{
    [SerializeField] private float health;
    public float maxHealth;
    public void UpdateHealth(float healthChange) {
        health += healthChange;
    }
}
