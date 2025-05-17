using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[CreateAssetMenu(menuName = "ProjectileBehavior/StraightShot")]
public class StraightShot : ProjectileBehaviour
{
    Vector2[] directions = new Vector2[]
    {
        Vector2.up,
        Vector2.down,
        Vector2.left,
        Vector2.right
    };
    public override void Activate(Transform target)
    {
        ShootStraight(target);
    }

    public override void Deactivate()
    {
        hasActivated = false;
    }
    private void ShootStraight(Transform target)
    {
        foreach (Vector2 dir in directions)
        {
            GameObject proj = Instantiate(projectilePrefab, target.position, Quaternion.identity);
            Rigidbody2D rb = proj.GetComponent<Rigidbody2D>();
            rb.linearVelocity = dir * speed;
            Destroy(proj, lifeTime);
        }
    }
}
