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
    private float _defaultFixedDeltaTime; // Store initial fixedDeltaTime

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // Optionally: DontDestroyOnLoad(gameObject); // If needed across scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        _defaultFixedDeltaTime = Time.fixedDeltaTime;
    }

    private void Update()
    {
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
        {
            StopBulletTime();
        }
        else
        {
            StartBulletTime();
        }
    }

    public void StartBulletTime()
    {
        if (_isInBulletTime) return;

        _isInBulletTime = true;
        _bulletTimeTimer = bulletTimeDuration;
        Time.timeScale = bulletTimeScale;
        Time.fixedDeltaTime = _defaultFixedDeltaTime * bulletTimeScale;
        Debug.Log("[TimeManager] Start Bullet Time");
    }

    public void StopBulletTime()
    {
        if (!_isInBulletTime) return;

        _isInBulletTime = false;
        Time.timeScale = 1f;
        Time.fixedDeltaTime = _defaultFixedDeltaTime;
        Debug.Log("[TimeManager] Stop Bullet Time");
    }
}
