using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 蓄力进度条 UI
/// 使用 World Space Canvas 显示在单位上方
/// </summary>
public class ChargeBar : MonoBehaviour
{
    [SerializeField] private Image _fillImage;
    [SerializeField] private Vector3 _offset = new Vector3(0, 1f, 0);

    private Transform _followTarget;

    /// <summary>
    /// 设置跟随目标
    /// </summary>
    public void SetTarget(Transform target)
    {
        _followTarget = target;
    }

    /// <summary>
    /// 设置进度 (0~1)
    /// </summary>
    public void SetProgress(float progress)
    {
        if (_fillImage != null)
        {
            _fillImage.fillAmount = Mathf.Clamp01(progress);
        }
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        SetProgress(0f);
    }

    private void LateUpdate()
    {
        if (_followTarget != null)
        {
            transform.position = _followTarget.position + _offset;
        }
    }
}
