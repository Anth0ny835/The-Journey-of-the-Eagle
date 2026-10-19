using UnityEngine;
using UnityEngine.UI;  // 用于 debugText

public class Target : MonoBehaviour
{
    [Header("Target Settings")]
    public float health = 30f;

    // 用来重生的预制体（在 Inspector 中指定，通常就是该目标的 Prefab）
    public GameObject targetPrefab;

    // 静态杀敌计数
    public static int killCount = 0;

    public void TakeDamage(float damage)
    {
        health -= damage;

        if (health <= 0)
        {
            killCount++;

            // 更新 UI 显示（通过 ScoreManager 处理）
            ScoreManager.UpdateKillCount(killCount);

            // 安排重生：调用 Respawner 管理器
            if (TargetRespawner.Instance != null && targetPrefab != null)
            {
                TargetRespawner.Instance.RespawnTarget(targetPrefab, transform.position, transform.rotation, 20f);
            }

            Destroy(gameObject);
        }
    }
}
