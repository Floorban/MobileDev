using UnityEngine;

[CreateAssetMenu(menuName = "ProjectileBehavior/StraightShot")]
public class StraightShot : ProjectileBehaviour
{
    public override void Activate(Vector2 position, Vector2 shootDir)
    {
        GameObject proj = Instantiate(projectilePrefab, position, Quaternion.identity);
        proj.GetComponent<Rigidbody2D>().linearVelocity = shootDir * speed;
        Destroy(proj, lifeTime);
    }
}
