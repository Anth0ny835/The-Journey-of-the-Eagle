using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class CharacterController : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;       // Rigidbody for movement
    [SerializeField] private FixedJoystick _joystick;   // Joystick for movement
    [SerializeField] private float _moveSpeed = 10f;      // Movement speed
    
    [SerializeField] private AudioSource _audioSource; // 引用 AudioSource 组件
    [SerializeField] private AudioClip _loopClip;      // 要播放和循环的 MP3 文件
    
    [SerializeField] private AudioSource _secondAudioSource; // 第二个 AudioSource
    [SerializeField] private AudioClip _secondLoopClip;     // 第二个音频文件（用来循环）
    [SerializeField] private float _waitTime = 11f;          // 等待时间（秒）

    private Camera _mainCamera;

    private void Start()
    {
        _mainCamera = Camera.main; // Reference to the main camera
    
        // 播放第一个音频并设置循环
    if (_audioSource != null && _loopClip != null)
        {
            _audioSource.clip = _loopClip;
            _audioSource.loop = true;
            _audioSource.Play(); // 开始循环播放
        }

        // 启动第二个音频的播放协程
        if (_secondAudioSource != null && _secondLoopClip != null)
        {
            StartCoroutine(PlaySecondAudioWithDelay());
        }
        else
        {
            Debug.LogWarning("Second AudioSource 或 Second LoopClip 未设置！");
        }
    }

    private void FixedUpdate()
    {
        // 获取摇杆输入
        Vector3 inputDirection = new Vector3(_joystick.Horizontal, 0f, _joystick.Vertical);

        if (inputDirection.magnitude > 0.1f) // 角色只有在移动时才处理
        {
            // 使用相机完整的朝向
            Vector3 camForward = _mainCamera.transform.forward;
            Vector3 camRight = _mainCamera.transform.right;

            // 保留相机 Y 分量
            camForward.Normalize();
            camRight.Normalize();

            // 将输入方向转换为世界空间方向
            Vector3 moveDirection = camForward * inputDirection.z + camRight * inputDirection.x;

            // 角色移动时，保留 Y 分量
            moveDirection.y = Mathf.Clamp(moveDirection.y, -30f, 30f);

            // 移动角色
            _rigidbody.linearVelocity = moveDirection * _moveSpeed;

            // 让角色面向移动方向（保持水平朝向）
            transform.rotation = Quaternion.LookRotation(new Vector3(moveDirection.x, 0f, moveDirection.z));
        }
        else
        {
            // 如果摇杆没有输入，则禁止角色上升或下降，Y 轴速度保持为 0
            _rigidbody.linearVelocity = Vector3.zero;
        }

        // **同步角色的 X 轴旋转**
        // 获取相机的 X 轴旋转
        float camRotationX = _mainCamera.transform.eulerAngles.x;

        // 修改角色的 X 轴旋转
        Vector3 characterRotation = transform.eulerAngles;
        characterRotation.x = camRotationX; // 同步角色 X 轴旋转
        transform.eulerAngles = characterRotation;
    }

    private IEnumerator PlaySecondAudioWithDelay()
    {
        while (true) // 无限循环
        {
            // 播放第二个音频
            _secondAudioSource.clip = _secondLoopClip;
            _secondAudioSource.Play();

            // 等待指定时间
            yield return new WaitForSeconds(_waitTime);

            // 停止第二个音频
            _secondAudioSource.Stop();
        }
    }
}