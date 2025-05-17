using UnityEngine;

[RequireComponent(typeof(ChefStation))]
[RequireComponent(typeof(LineRenderer))]
public class RangeVisualizer : MonoBehaviour
{
    public Color inRangeColor = Color.green;
    public Color outOfRangeColor = Color.gray;
    private ChefStation station;
    private LineRenderer line;
    public int segments = 64;
    private void Awake()
    {
        InitComponents();
        InitLineRend(line);
    }
    private void InitComponents()
    {
        station = GetComponent<ChefStation>();
        line = GetComponent<LineRenderer>();
    }
    private void InitLineRend(LineRenderer line)
    {
        line.useWorldSpace = false;
        line.loop = true;
        line.widthMultiplier = 0.05f;
        line.positionCount = segments;
    }
    private void DrawCircle(float radius)
    {
        float angle = 0f;
        for (int i = 0; i < segments; i++)
        {
            float x = Mathf.Cos(Mathf.Deg2Rad * angle) * radius;
            float y = Mathf.Sin(Mathf.Deg2Rad * angle) * radius;
            if(line) line.SetPosition(i, new Vector3(x, y, 0));
            angle += 360f / segments;
        }
    }
    public void SetRadius(float newRadius)
    {
        DrawCircle(newRadius);
    }
    public void UpdateRange(bool inRange)
    {
        line.startColor = line.endColor = inRange ? inRangeColor : outOfRangeColor;
    }
}
