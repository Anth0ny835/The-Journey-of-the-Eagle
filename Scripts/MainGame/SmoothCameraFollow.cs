using UnityEngine;
using UnityEngine.InputSystem; // 引入新的 Input System 命名空间

public class SmoothCameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target; // 角色的 Transform
    [SerializeField] private float smoothTime = 0.3f; // 相机平滑移动时间
    [SerializeField] private float rotationSpeed = 15f; // 相机旋转速度
    [SerializeField] private float positionHigh = 7f; // 相机旋转速度
    [SerializeField] private float positionLow = -7f; // 相机旋转速度

    private Vector3 _offset; // 相机与角色之间的偏移
    private Vector3 _currentVelocity = Vector3.zero; // 平滑移动的速度
    private float _rotationX = 0f; // 垂直旋转
    private float _rotationY = 0f; // 水平旋转

    private Vector2 _previousTouchPosition; // 记录上一次触摸的位置
    private bool _isTouching; // 是否正在触摸屏幕

    private void Awake()
    {
        // 计算初始偏移量
        _offset = transform.position - target.position;

        // 初始化旋转角度为当前相机角度
        _rotationY = transform.eulerAngles.y;
        _rotationX = transform.eulerAngles.x;
    }

    private void LateUpdate()
    {
        HandleRotation(); // 处理屏幕触摸或鼠标旋转逻辑
        FollowTarget();   // 平滑跟随角色
    }

    private void HandleRotation()
    {
        // PC 和触摸设备的输入支持
#if UNITY_EDITOR || UNITY_STANDALONE
        HandleMouseRotation();
#elif UNITY_IOS || UNITY_ANDROID
        HandleTouchRotation();
#endif
    }

    private void HandleMouseRotation()
    {
        // 获取当前鼠标或触摸的位置
        Vector2 mousePosition = Mouse.current.position.ReadValue();

        // 获取屏幕的宽度（假设全屏显示）
        float screenWidth = Screen.width;

        // 只允许在屏幕右边进行旋转
        if (mousePosition.x > screenWidth * 0.35f && Mouse.current.leftButton.isPressed)
        {
            Vector2 delta = Mouse.current.delta.ReadValue(); // 获取鼠标移动增量

            // 更新旋转角度
            _rotationX -= delta.y * rotationSpeed * Time.deltaTime;
            _rotationY += delta.x * rotationSpeed * Time.deltaTime;

            // 限制垂直旋转角度（防止翻转）
            _rotationX = Mathf.Clamp(_rotationX, -30f, 30f);

            // 应用旋转到相机
            transform.rotation = Quaternion.Euler(_rotationX, _rotationY, 0f);
        }
    }

    private void HandleTouchRotation()
    {
        if (Touchscreen.current == null) return;

        foreach (var touch in Touchscreen.current.touches)
        {
            if (touch.press.isPressed)
            {
                Vector2 touchPosition = touch.position.ReadValue();
                float screenWidth = Screen.width;

                // 右边屏幕负责旋转
                if (touchPosition.x > screenWidth * 0.35f)
                {
                    if (touch.phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Began)
                    {
                        _previousTouchPosition = touchPosition;
                        _isTouching = true;
                    }
                    else if (touch.phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Moved && _isTouching)
                    {
                        Vector2 delta = touchPosition - _previousTouchPosition;

                        // 更新旋转逻辑
                        _rotationX -= delta.y * rotationSpeed * Time.deltaTime * 0.1f;
                        _rotationY += delta.x * rotationSpeed * Time.deltaTime * 0.1f;

                        _rotationX = Mathf.Clamp(_rotationX, -30f, 30f);
                        transform.rotation = Quaternion.Euler(_rotationX, _rotationY, 0f);

                        _previousTouchPosition = touchPosition;
                    }
                    else if (touch.phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Ended)
                    {
                        _isTouching = false;
                    }
                }
            }
        }
    }


    private void FollowTarget()
    {
        // 计算目标位置
        Vector3 targetPosition = target.position + Quaternion.Euler(0f, _rotationY, 0f) * _offset;

        // 根据仰角调整相机的 Y 轴高度
        float yOffset = Mathf.Lerp(positionHigh, positionLow, (_rotationX + 30f) / 60f); // 仰角越高，Y 越低
        targetPosition.y -= yOffset;

        // 平滑过渡到目标位置
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref _currentVelocity, smoothTime);
    }
}
