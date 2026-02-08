using UnityEngine;

/// <summary>
/// 矩形攻击范围指示器
/// 外框使用 LineRenderer 绘制
/// 填充效果使用 SpriteRenderer（逐渐从左向右填满）
/// </summary>
[RequireComponent(typeof(LineRenderer))]
public class RectIndicator : BaseAttackIndicator
{
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private SpriteRenderer _fillRenderer;

    private float _length;
    private float _width;

    protected override void EnsureInitialized()
    {
        if (_initialized) return;
        _initialized = true;

        if (_lineRenderer == null)
            _lineRenderer = GetComponent<LineRenderer>();

        if (_lineRenderer != null)
        {
            _lineRenderer.useWorldSpace = false;
            _lineRenderer.loop = true;
            _lineRenderer.positionCount = 4;
            _lineRenderer.startColor = _outlineColor;
            _lineRenderer.endColor = _outlineColor;
        }

        // 自动创建填充用的 SpriteRenderer
        if (_fillRenderer == null)
        {
            GameObject fillObj = new GameObject("Fill");
            fillObj.transform.SetParent(transform);
            fillObj.transform.localPosition = Vector3.zero;
            fillObj.transform.localRotation = Quaternion.identity;
            _fillRenderer = fillObj.AddComponent<SpriteRenderer>();
            _fillRenderer.sprite = CreateWhiteSquareSprite();
            _fillRenderer.color = _fillColor;
            _fillRenderer.sortingOrder = -1;
        }
    }

    /// <summary>
    /// 设置矩形尺寸
    /// </summary>
    public void Setup(float length, float width, float lineWidth = 0.05f)
    {
        EnsureInitialized();

        if (_lineRenderer == null)
        {
            Debug.LogError($"[RectIndicator] {gameObject.name} 缺少 LineRenderer 组件！");
            return;
        }

        _length = length;
        _width = width;
        _lineRenderer.startWidth = lineWidth;
        _lineRenderer.endWidth = lineWidth;
        GenerateRectPoints();

        SetFillProgress(0f);
    }

    public override void SetDirection(Vector2 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.localRotation = Quaternion.Euler(0, 0, angle);
    }

    protected override void UpdateFillVisual()
    {
        if (_fillRenderer == null) return;

        float currentLength = _length * _fillProgress;
        _fillRenderer.transform.localPosition = new Vector3(currentLength / 2f, 0, 0);
        _fillRenderer.transform.localScale = new Vector3(currentLength, _width, 1f);
    }

    private void GenerateRectPoints()
    {
        if (_lineRenderer == null) return;

        float halfWidth = _width / 2f;

        Vector3[] points = new Vector3[4]
        {
            new Vector3(0, -halfWidth, 0),
            new Vector3(_length, -halfWidth, 0),
            new Vector3(_length, halfWidth, 0),
            new Vector3(0, halfWidth, 0)
        };

        _lineRenderer.SetPositions(points);
    }
}
