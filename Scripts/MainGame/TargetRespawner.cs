using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TargetRespawner : MonoBehaviour
{
    public static TargetRespawner Instance;

    private void Awake()
    {
        // 单例模式：如果已有实例，则销毁当前对象，否则赋值为单例
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            // 如需跨场景持续存在，则取消注释下一行
            // DontDestroyOnLoad(gameObject);
        }
    }

    /// <summary>
    /// 安排重生目标
    /// </summary>
    /// <param name="prefab">目标预制体</param>
    /// <param name="position">重生位置</param>
    /// <param name="rotation">重生旋转</param>
    /// <param name="delay">延迟时间（秒）</param>
    public void RespawnTarget(GameObject prefab, Vector3 position, Quaternion rotation, float delay)
    {
        StartCoroutine(RespawnCoroutine(prefab, position, rotation, delay));
    }

    private IEnumerator RespawnCoroutine(GameObject prefab, Vector3 position, Quaternion rotation, float delay)
    {
        yield return new WaitForSeconds(delay);
        GameObject newTarget = Instantiate(prefab, position, rotation);
    }

}
