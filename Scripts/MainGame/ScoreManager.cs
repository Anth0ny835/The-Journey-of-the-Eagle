using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    // 单例模式，方便其他脚本调用
    public static ScoreManager Instance;

    [Header("UI Components")]
    public Text killCountText;  // 在 Canvas 上放置一个 UI Text 用于显示杀敌数量

    private void Awake()
    {
        Instance = this;
    }

    // 静态方法，更新杀敌计数的显示
    public static void UpdateKillCount(int count)
    {
        if (Instance != null && Instance.killCountText != null)
        {
            Instance.killCountText.text = "Kills: " + count;
        }
    }
}
