using UnityEngine;

public class AutoFlyAndCircle : MonoBehaviour
{
    public float radius = 10f; // 盘旋的半径
    public float speed = 3f; // 飞行速度
    public float heightAmplitude = 3.5f; // 上下浮动的振幅
    public float heightFrequency = 1.5f; // 上下浮动的频率

    private Vector3 centerPoint; // 飞行的圆心
    private float angle = 0f; // 当前角度

    void Start()
    {
        // 设置飞行的圆心为角色初始位置
        centerPoint = transform.position;
    }

    void Update()
    {
        // 计算当前位置在圆周上的坐标
        angle += speed * Time.deltaTime; // 增加角度，控制飞行速度
        float x = Mathf.Cos(angle) * radius; // 计算X坐标
        float z = Mathf.Sin(angle) * radius; // 计算Z坐标

        // 计算高度的上下浮动
        float y = centerPoint.y + Mathf.Sin(Time.time * heightFrequency) * heightAmplitude;

        // 更新角色位置
        transform.position = new Vector3(centerPoint.x + x, y, centerPoint.z + z);

        // 让角色始终朝向飞行方向
        Vector3 direction = new Vector3(-Mathf.Sin(angle), 0, Mathf.Cos(angle));
        transform.rotation = Quaternion.LookRotation(direction);
    }
}
