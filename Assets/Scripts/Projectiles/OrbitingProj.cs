using UnityEngine;

[CreateAssetMenu(menuName = "ProjectileBehavior/Orbiting")]
public class OrbitingProj : ProjectileBehaviour
{
    public float radius = 2f;
    public int count = 3;
    private GameObject[] activeProjectiles;

    public override void Activate(Transform target)
    {
        if (hasActivated) return;

        hasActivated = true;
        activeProjectiles = new GameObject[count];

        for (int i = 0; i < count; i++)
        {
            float angleOffset = (360f / count) * i;

            GameObject proj = Instantiate(projectilePrefab, target.position, Quaternion.identity);
            Orbit orbit = proj.AddComponent<Orbit>();
            orbit.Init(target, radius, speed, angleOffset);
            activeProjectiles[i] = proj;
        }
    }

    public override void Deactivate()
    {
        hasActivated = false;
        if (activeProjectiles != null)
        {
            foreach (GameObject proj in activeProjectiles)
            {
                if (proj) Destroy(proj);
            }
        }
    }
}
