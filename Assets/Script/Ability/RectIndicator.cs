using UnityEngine;

/// <summary>
/// 矩形攻击范围指示器
/// 外框使用 LineRenderer 绘制
/// 填充效果使用 SpriteRenderer（逐渐从左向右填满）
/// </summary>
[RequireComponent(typeof(LineRenderer))]
public class RectIndicator : MonoBehaviour
{
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private SpriteRenderer _fillRenderer;

    [Header("颜色配置")]
    [SerializeField] private Color _outlineColor = Color.red;
    [SerializeField] private Color _fillColor = new Color(1f, 0f, 0f, 0.5f);

    private float _length;
    private float _width;
    private float _fillProgress;

    private void Awake()
    {
        EnsureInitialized();
    }

    private bool _initialized = false;

    /// <summary>
    /// 确保组件已初始化
    /// </summary>
    private void EnsureInitialized()
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
            _fillRenderer.sortingOrder = -1; // 在线框后面
        }
    }

    /// <summary>
    /// 设置矩形尺寸
    /// </summary>
    public void Setup(float length, float width, float lineWidth = 0.05f)
    {
        // 确保初始化（如果对象之前是禁用的，Awake 可能还没调用）
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

        // 重置填充进度
        SetFillProgress(0f);
    }

    /// <summary>
    /// 设置指示器方向
    /// </summary>
    public void SetDirection(Vector2 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.localRotation = Quaternion.Euler(0, 0, angle);
    }

    /// <summary>
    /// 设置填充进度 (0~1)
    /// 矩形从左向右逐渐填满
    /// </summary>
    public void SetFillProgress(float progress)
    {
        _fillProgress = Mathf.Clamp01(progress);

        if (_fillRenderer == null) return;

        float halfWidth = _width / 2f;
        float currentLength = _length * _fillProgress;

        // 调整填充区域的位置和大小
        _fillRenderer.transform.localPosition = new Vector3(currentLength / 2f, 0, 0);
        _fillRenderer.transform.localScale = new Vector3(currentLength, _width, 1f);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void GenerateRectPoints()
    {
        if (_lineRenderer == null) return;

        float halfWidth = _width / 2f;

        // 矩形四个顶点（本地坐标，从原点向右延伸）
        Vector3[] points = new Vector3[4]
        {
            new Vector3(0, -halfWidth, 0),
            new Vector3(_length, -halfWidth, 0),
            new Vector3(_length, halfWidth, 0),
            new Vector3(0, halfWidth, 0)
        };

        _lineRenderer.SetPositions(points);
    }

    /// <summary>
    /// 创建一个 1x1 的白色方形 Sprite 用于填充
    /// </summary>
    private Sprite CreateWhiteSquareSprite()
    {
        Texture2D tex = new Texture2D(1, 1);
        tex.SetPixel(0, 0, Color.white);
        tex.Apply();
        return Sprite.Create(tex, new Rect(0, 0, 1, 1), new Vector2(0.5f, 0.5f), 1f);
    }
}
