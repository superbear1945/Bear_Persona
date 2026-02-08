using UnityEngine;

/// <summary>
/// 攻击指示器抽象基类
/// 提供通用的初始化、填充进度、显示隐藏逻辑
/// </summary>
public abstract class BaseAttackIndicator : MonoBehaviour, IAttackIndicator
{
    [Header("颜色配置")]
    [SerializeField] protected Color _outlineColor = Color.red;
    [SerializeField] protected Color _fillColor = new Color(1f, 0f, 0f, 0.5f);

    protected float _fillProgress;
    protected bool _initialized = false;

    public float FillProgress => _fillProgress;

    protected virtual void Awake()
    {
        EnsureInitialized();
    }

    /// <summary>
    /// 确保组件已初始化（子类必须实现）
    /// </summary>
    protected abstract void EnsureInitialized();

    /// <summary>
    /// 设置指示器方向
    /// </summary>
    public abstract void SetDirection(Vector2 direction);

    /// <summary>
    /// 设置填充进度 (0~1)
    /// </summary>
    public virtual void SetFillProgress(float progress)
    {
        _fillProgress = Mathf.Clamp01(progress);
        UpdateFillVisual();
    }

    /// <summary>
    /// 更新填充视觉效果（子类实现）
    /// </summary>
    protected abstract void UpdateFillVisual();

    public virtual void Show()
    {
        gameObject.SetActive(true);
    }

    public virtual void Hide()
    {
        gameObject.SetActive(false);
    }

    /// <summary>
    /// 创建一个 1x1 的白色方形 Sprite 用于填充
    /// </summary>
    protected Sprite CreateWhiteSquareSprite()
    {
        Texture2D texture = new Texture2D(1, 1);
        texture.SetPixel(0, 0, Color.white);
        texture.Apply();
        return Sprite.Create(texture, new Rect(0, 0, 1, 1), new Vector2(0.5f, 0.5f), 1f);
    }
}
