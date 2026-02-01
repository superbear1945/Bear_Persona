using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class RangeCircle : MonoBehaviour
{
    private LineRenderer _lineRenderer;
    private Transform _target;
    private int _segments = 50;
    private float _radius;

    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.useWorldSpace = true;
        _lineRenderer.loop = true;
        _lineRenderer.positionCount = _segments + 1;
    }

    public void Setup(float radius, float width = 0.1f)
    {
        _radius = radius;
        _lineRenderer.startWidth = width;
        _lineRenderer.endWidth = width;
        UpdateCircle();
    }

    public void SetTarget(Transform target)
    {
        _target = target;
        UpdateCircle();
    }

    private void Update()
    {
        if (_target != null)
        {
            // 跟随目标
            transform.position = _target.position;
            UpdateCircle();
        }
    }

    private void UpdateCircle()
    {
        if (_lineRenderer == null) return;

        float angle = 0f;
        for (int i = 0; i <= _segments; i++)
        {
            float x = Mathf.Sin(Mathf.Deg2Rad * angle) * _radius;
            float y = Mathf.Cos(Mathf.Deg2Rad * angle) * _radius;

            Vector3 pos = transform.position + new Vector3(x, y, 0);
            _lineRenderer.SetPosition(i, pos);

            angle += (360f / _segments);
        }
    }
}
