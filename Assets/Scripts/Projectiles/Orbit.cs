using UnityEngine;

public class Orbit : MonoBehaviour
{
    Transform origin;
    float radius;
    float angle;
    float speed;

    public void Init(Transform target, float r, float s, float startAngle = 0f)
    {
        origin = target;
        radius = r;
        speed = s;
        angle = startAngle;
    }

    void Update()
    {
        if (!origin) return;
        angle += speed * Time.deltaTime;
        float rad = angle * Mathf.Deg2Rad;

        Vector3 offset = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0f) * radius;
        transform.position = origin.position + offset;
    }
}
