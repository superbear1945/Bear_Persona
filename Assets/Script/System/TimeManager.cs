using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance;

    [Header("子弹时间配置")]
    [Tooltip("子弹时间持续时间 (秒)")]
    public float bulletTimeDuration = 5f;
    [Tooltip("子弹时间的时间流速 (0.01 - 1)")]
    [Range(0.01f, 1f)]
    public float bulletTimeScale = 0.1f;

    private bool _isInBulletTime;
    private float _bulletTimeTimer;
    private float _defaultFixedDeltaTime;

    [Header("开启子弹时间时的视觉蒙版效果ui")]
    [SerializeField] private GameObject _bulletTimeOverlay;

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
        if (_bulletTimeOverlay != null) _bulletTimeOverlay.SetActive(false);
    }

    private void Update()
    {
        // 简单的计时器
        if (_isInBulletTime)
        {
            _bulletTimeTimer -= Time.unscaledDeltaTime;
            if (_bulletTimeTimer <= 0)
            {
                StopBulletTime();
            }
        }
    }

    public void ToggleBulletTime()
    {
        if (_isInBulletTime)
            StopBulletTime();
        else
            StartBulletTime();
    }

    public void StartBulletTime()
    {
        if (_isInBulletTime) return;

        _isInBulletTime = true;
        _bulletTimeTimer = bulletTimeDuration;
        Time.timeScale = bulletTimeScale;
        Time.fixedDeltaTime = _defaultFixedDeltaTime * bulletTimeScale;

        if (_bulletTimeOverlay != null)
            _bulletTimeOverlay.SetActive(true);

    }

    public void StopBulletTime()
    {
        if (!_isInBulletTime) return;

        _isInBulletTime = false;
        Time.timeScale = 1f;
        Time.fixedDeltaTime = _defaultFixedDeltaTime;

        if (_bulletTimeOverlay != null) _bulletTimeOverlay.SetActive(false);
    }
}
