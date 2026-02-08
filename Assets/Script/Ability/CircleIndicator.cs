using UnityEngine;

/// <summary>
/// 圆形 AOE 攻击范围指示器
/// 以单位为圆心，显示圆形攻击范围
/// 填充效果从中心向外扩展
/// </summary>
[RequireComponent(typeof(LineRenderer))]
public class CircleIndicator : BaseAttackIndicator
{
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private SpriteRenderer _fillRenderer;
    [SerializeField] private int _segments = 32;

    private float _radius;

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
            _lineRenderer.positionCount = _segments;
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
            _fillRenderer.sprite = CreateCircleSprite();
            _fillRenderer.color = _fillColor;
            _fillRenderer.sortingOrder = -1;
        }
    }

    /// <summary>
    /// 设置圆形尺寸
    /// </summary>
    public void Setup(float radius, float lineWidth = 0.05f)
    {
        EnsureInitialized();

        if (_lineRenderer == null)
        {
            Debug.LogError($"[CircleIndicator] {gameObject.name} 缺少 LineRenderer 组件！");
            return;
        }

        _radius = radius;
        _lineRenderer.startWidth = lineWidth;
        _lineRenderer.endWidth = lineWidth;
        GenerateCirclePoints();

        SetFillProgress(0f);
    }

    public override void SetDirection(Vector2 direction)
    {
        // 圆形 AOE 不需要方向，但可以用于视觉效果
    }

    protected override void UpdateFillVisual()
    {
        if (_fillRenderer == null) return;

        // 从中心向外扩展
        float currentRadius = _radius * _fillProgress * 2f;
        _fillRenderer.transform.localScale = new Vector3(currentRadius, currentRadius, 1f);
    }

    private void GenerateCirclePoints()
    {
        if (_lineRenderer == null) return;

        Vector3[] points = new Vector3[_segments];
        float angleStep = 360f / _segments;

        for (int i = 0; i < _segments; i++)
        {
            float angle = i * angleStep * Mathf.Deg2Rad;
            points[i] = new Vector3(
                Mathf.Cos(angle) * _radius,
                Mathf.Sin(angle) * _radius,
                0
            );
        }

        _lineRenderer.SetPositions(points);
    }

    /// <summary>
    /// 创建圆形 Sprite
    /// </summary>
    private Sprite CreateCircleSprite()
    {
        int size = 64;
        Texture2D texture = new Texture2D(size, size);
        float center = size / 2f;
        float radius = size / 2f;

        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                float distance = Vector2.Distance(new Vector2(x, y), new Vector2(center, center));
                if (distance <= radius)
                    texture.SetPixel(x, y, Color.white);
                else
                    texture.SetPixel(x, y, Color.clear);
            }
        }

        texture.Apply();
        return Sprite.Create(texture, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f), size);
    }
}
