using UnityEngine;

// 该脚本用于绘制圆，目前用于显示附身范围，后续或许可以用于范围攻击的攻击指示器
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
        _lineRenderer.useWorldSpace = false; // 使用相对坐标
        _lineRenderer.loop = true;
        _lineRenderer.positionCount = _segments + 1;
    }

    public void Setup(float radius, float width = 0.1f)
    {
        _radius = radius;
        _lineRenderer.startWidth = width;
        _lineRenderer.endWidth = width;
        GenerateCirclePoints(); // 只需计算一次点
    }

    private void Start()
    {
        if (PlayerController.Instance != null)
        {
            PlayerController.Instance.OnPossessionChanged += OnPossessionChanged;
        }
    }

    private void OnDestroy()
    {
        if (PlayerController.Instance != null)
        {
            PlayerController.Instance.OnPossessionChanged -= OnPossessionChanged;
        }
    }

    private void OnPossessionChanged(BearUnit unit)
    {
        SetTarget(unit.transform);
    }

    public void SetTarget(Transform target)
    {
        _target = target;
        // 初始位置同步
        if (_target != null)
        {
            transform.SetParent(_target, false);
            transform.localPosition = Vector3.zero;
        }
    }

    private void GenerateCirclePoints()
    {
        if (_lineRenderer == null) return;

        float angle = 0f;
        for (int i = 0; i <= _segments; i++)
        {
            float x = Mathf.Sin(Mathf.Deg2Rad * angle) * _radius;
            float y = Mathf.Cos(Mathf.Deg2Rad * angle) * _radius;

            // 本地坐标，相对于 (0,0)
            Vector3 pos = new Vector3(x, y, 0);
            _lineRenderer.SetPosition(i, pos);

            angle += (360f / _segments);
        }
    }
}
