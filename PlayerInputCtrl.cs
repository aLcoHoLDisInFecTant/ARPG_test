using UnityEngine;
using System.Collections.Generic;

public enum GestureType
{
    Click,
    Slash_Up,
    Slash_Down,
    ReVshape,
    Slash_Z,
    None
}


public class PlayerInputCtrl : MonoBehaviour
{
    public CombatHandler combatHandler;

    [Header("轨迹识别参数")]
    public float minDragDistance = 50f;
    public float angleTolerance = 25f;

    [Header("路径可视化")]
    public LineRenderer lineRenderer;
    public float pointSpacing = 5f;

    [Header("特效设置")]
    public GameObject trailEffectPrefab;
    public float effectSpawnInterval = 0.05f;

    private List<Vector2> points = new List<Vector2>();
    private float gestureStartTime;
    private float lastEffectTime = 0f;

    void Start()
    {
        if (lineRenderer == null)
        {
            GameObject lineObj = new GameObject("GestureTrail");
            lineObj.transform.SetParent(this.transform);
            lineRenderer = lineObj.AddComponent<LineRenderer>();
            lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
            lineRenderer.widthMultiplier = 0.05f;
            lineRenderer.startColor = Color.yellow;
            lineRenderer.endColor = Color.red;
            lineRenderer.positionCount = 0;
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            points.Clear();
            gestureStartTime = Time.time;
            points.Add(Input.mousePosition);
            lineRenderer.positionCount = 0;
        }

        if (Input.GetMouseButton(0))
        {
            Vector2 current = Input.mousePosition;
            if (Vector2.Distance(points[points.Count - 1], current) > pointSpacing)
            {
                points.Add(current);

                Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(current.x, current.y, 5f));
                lineRenderer.positionCount = points.Count;
                lineRenderer.SetPosition(points.Count - 1, worldPos);

                SpawnTrailEffect(worldPos);
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (Time.time - gestureStartTime < 0.1f || TotalDistance(points) < minDragDistance)
            {
                combatHandler.PerformGesture(GestureType.Click);
            }
            else
            {
                GestureType gesture = RecognizeGesture(points);
                combatHandler.PerformGesture(gesture);
            }

            lineRenderer.positionCount = 0;
        }
    }

    float TotalDistance(List<Vector2> pts)
    {
        float dist = 0f;
        for (int i = 1; i < pts.Count; i++)
            dist += Vector2.Distance(pts[i - 1], pts[i]);
        return dist;
    }

    GestureType RecognizeGesture(List<Vector2> pts)
    {
        if (pts.Count < 5) return GestureType.None;

        Vector2 first = pts[0];
        Vector2 last = pts[pts.Count - 1];
        Vector2 overall = last - first;
        float angle = Vector2.SignedAngle(Vector2.right, overall.normalized);

        // 上挑
        if (Mathf.Abs(angle - 90) < angleTolerance) return GestureType.Slash_Up;

        // 下劈
        if (Mathf.Abs(angle + 90) < angleTolerance) return GestureType.Slash_Down;

        // 圆
        if (IsVShapeMotion(pts)) return GestureType.ReVshape;

        // Z形
        if (IsZShape(pts)) return GestureType.Slash_Z;

        return GestureType.None;
    }

    bool IsVShapeMotion(List<Vector2> pts)
    {
        if (pts.Count < 10) return false;

        int half = pts.Count / 2;
        Vector2 left = pts[0];
        Vector2 mid = pts[half];
        Vector2 right = pts[pts.Count - 1];

        Vector2 seg1 = (mid - left).normalized;
        Vector2 seg2 = (right - mid).normalized;

        float angle1 = Vector2.SignedAngle(Vector2.right, seg1);  // 期望 ~60°
        float angle2 = Vector2.SignedAngle(Vector2.right, seg2);  // 期望 ~-60°

        return Mathf.Abs(angle1 - 60f) < 30f && Mathf.Abs(angle2 + 60f) < 30f;
    }


    bool IsZShape(List<Vector2> pts)
    {
        if (pts.Count < 10) return false;

        int segmentCount = 3;
        List<Vector2> segs = new List<Vector2>();

        int segLength = pts.Count / segmentCount;
        for (int i = 0; i < segmentCount; i++)
        {
            Vector2 start = pts[i * segLength];
            Vector2 end = pts[Mathf.Min((i + 1) * segLength - 1, pts.Count - 1)];
            segs.Add((end - start).normalized);
        }

        float a1 = Vector2.Angle(segs[0], Vector2.right);
        float a2 = Vector2.Angle(segs[1], Vector2.left);
        float a3 = Vector2.Angle(segs[2], Vector2.right);

        return Mathf.Abs(a1) < 45 && Mathf.Abs(a2) < 45 && Mathf.Abs(a3) < 45;
    }

    void SpawnTrailEffect(Vector3 pos)
    {
        if (trailEffectPrefab != null && Time.time - lastEffectTime > effectSpawnInterval)
        {
            Instantiate(trailEffectPrefab, pos, Quaternion.identity);
            lastEffectTime = Time.time;
        }
    }
}
