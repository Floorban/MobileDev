using UnityEngine;

public abstract class ProjectileBehaviour : ScriptableObject
{
    public GameObject projectilePrefab;
    public float cooldown = 1f;

    public abstract void Activate(Vector3 position, Transform player);
}
