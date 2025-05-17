using UnityEngine;

public abstract class ProjectileBehaviour : ScriptableObject
{
    public GameObject projectilePrefab;
    protected GameObject projectile;
    public bool hasActivated;
    public float speed = 10f;
    public float lifeTime = 1f;
    public float cooldown = 1f;

    public abstract void Activate(Transform target);
    public abstract void Deactivate();

}
