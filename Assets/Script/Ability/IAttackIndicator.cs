using UnityEngine;

/// <summary>
/// 攻击指示器接口
/// 所有类型的攻击范围指示器都实现此接口
/// </summary>
public interface IAttackIndicator
{
    /// <summary>
    /// 设置指示器方向
    /// </summary>
    void SetDirection(Vector2 direction);

    /// <summary>
    /// 设置填充进度 (0~1)
    /// </summary>
    void SetFillProgress(float progress);

    /// <summary>
    /// 显示指示器
    /// </summary>
    void Show();

    /// <summary>
    /// 隐藏指示器
    /// </summary>
    void Hide();

    /// <summary>
    /// 获取当前进度
    /// </summary>
    float FillProgress { get; }
}
