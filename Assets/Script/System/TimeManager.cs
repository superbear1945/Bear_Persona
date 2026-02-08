using UnityEngine;
using System;
using System.Collections;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance;

    [Header("时间控制设置")]
    [Tooltip("手动附身持续时长 (秒)")]
    public float possessionDuration = 5.0f;
    [Tooltip("死亡附身持续时长 (秒)")]
    public float deathPossessionDuration = 3.0f;
    [Tooltip("附身/子弹时间的时间流速 (0~1)")]
    [Range(0.01f, 1f)]
    public float possessionTimeScale = 0.1f;

    private bool _isInPossession;
    public bool IsInPossession => _isInPossession;

    private bool _isDeathPossession;
    public bool IsDeathPossession => _isDeathPossession;

    private float _defaultFixedDeltaTime;

    // 当前运行的计时协程
    private Coroutine _possessionCoroutine;

    // 死亡附身超时回调
    public event Action OnDeathPossessionTimeout;

    [Header("UI 视觉反馈")]
    [SerializeField] private GameObject _possessionOverlay;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        _defaultFixedDeltaTime = Time.fixedDeltaTime;
        if (_possessionOverlay != null) _possessionOverlay.SetActive(false);
    }

    public void TogglePossession()
    {
        if (_isInPossession)
            StopPossession();
        else
            StartPossession();
    }

    /// <summary>
    /// 开始手动附身模式
    /// </summary>
    public void StartPossession()
    {
        StartPossession(possessionDuration, false);
    }

    /// <summary>
    /// 开始死亡附身模式
    /// </summary>
    public void StartDeathPossession()
    {
        StartPossession(deathPossessionDuration, true);
    }

    /// <summary>
    /// 开始附身模式（指定时长）
    /// </summary>
    public void StartPossession(float duration, bool isDeathPossession)
    {
        if (_isInPossession) return;

        _isInPossession = true;
        _isDeathPossession = isDeathPossession;
        Time.timeScale = possessionTimeScale;
        Time.fixedDeltaTime = _defaultFixedDeltaTime * possessionTimeScale;

        if (_possessionOverlay != null)
            _possessionOverlay.SetActive(true);

        // 启动计时协程
        _possessionCoroutine = StartCoroutine(PossessionTimerCoroutine(duration, isDeathPossession));

        Debug.Log($"[TimeManager] Start Possession Mode (duration={duration}, death={isDeathPossession})");
    }

    /// <summary>
    /// 附身计时协程
    /// </summary>
    private IEnumerator PossessionTimerCoroutine(float duration, bool isDeathPossession)
    {
        // 使用 WaitForSecondsRealtime，不受 Time.timeScale 影响
        yield return new WaitForSecondsRealtime(duration);

        // 超时，停止附身
        StopPossession();

        // 如果是死亡附身，触发游戏结束
        if (isDeathPossession)
        {
            OnDeathPossessionTimeout?.Invoke();
        }
    }

    public void StopPossession()
    {
        if (!_isInPossession) return;

        // 停止计时协程
        if (_possessionCoroutine != null)
        {
            StopCoroutine(_possessionCoroutine);
            _possessionCoroutine = null;
        }

        _isInPossession = false;
        _isDeathPossession = false;
        Time.timeScale = 1f;
        Time.fixedDeltaTime = _defaultFixedDeltaTime;

        if (_possessionOverlay != null) _possessionOverlay.SetActive(false);

        Debug.Log("[TimeManager] Stop Possession Mode");
    }
}
