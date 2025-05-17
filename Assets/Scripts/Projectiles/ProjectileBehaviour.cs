using UnityEngine;

public abstract class ProjectileBehaviour : ScriptableObject
{
    public GameObject projectilePrefab;
    public float speed = 10f;
    public float lifeTime = 1f;
    public float cooldown = 1f;

    public abstract void Activate(Vector2 position, Vector2 shootDir);
}
