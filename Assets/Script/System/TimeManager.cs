using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance;

    [Header("时间控制设置")]
    [Tooltip("附身/子弹时间持续时长 (秒)")]
    public float possessionDuration = 5.0f;
    [Tooltip("附身/子弹时间的时间流速 (0~1)")]
    [Range(0.01f, 1f)]
    public float possessionTimeScale = 0.1f;

    private bool _isInPossession;
    public bool IsInPossession => _isInPossession;

    private float _possessionTimer;
    private float _defaultFixedDeltaTime;

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

    private void Update()
    {
        // 简单的计时器
        if (_isInPossession)
        {
            _possessionTimer -= Time.unscaledDeltaTime;
            if (_possessionTimer <= 0)
            {
                StopPossession();
            }
        }
    }

    public void TogglePossession()
    {
        if (_isInPossession)
            StopPossession();
        else
            StartPossession();
    }

    public void StartPossession()
    {
        if (_isInPossession) return;

        _isInPossession = true;
        _possessionTimer = possessionDuration;
        Time.timeScale = possessionTimeScale;
        Time.fixedDeltaTime = _defaultFixedDeltaTime * possessionTimeScale;

        if (_possessionOverlay != null)
            _possessionOverlay.SetActive(true);

        Debug.Log("[TimeManager] Start Possession Mode");
    }

    public void StopPossession()
    {
        if (!_isInPossession) return;

        _isInPossession = false;
        Time.timeScale = 1f;
        Time.fixedDeltaTime = _defaultFixedDeltaTime;

        if (_possessionOverlay != null) _possessionOverlay.SetActive(false);

        Debug.Log("[TimeManager] Stop Possession Mode");
    }
}
