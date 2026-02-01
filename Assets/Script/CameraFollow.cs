using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("跟随设置")]
    [Tooltip("相机与目标的偏移量")]
    public Vector3 offset = new Vector3(0, 0, -10);

    [Tooltip("相机移动的平滑速度")]
    public float smoothSpeed = 5f;

    private void LateUpdate()
    {
        // 检查 PlayerController 实例是否存在
        if (PlayerController.Instance == null)
            return;

        // 获取当前控制的单位
        var targetUnit = PlayerController.Instance.currentUnit;

        // 确保目标单位不为空，且是一个 MonoBehaviour (以便获取 Transform)
        if (targetUnit != null)
        {
            Transform targetTransform = targetUnit.transform;

            // 计算目标位置
            Vector3 desiredPosition = targetTransform.position + offset;

            // 平滑移动相机
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

            // 更新相机位置
            transform.position = smoothedPosition;
        }
    }
}
