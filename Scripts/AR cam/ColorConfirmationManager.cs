using UnityEngine;
using TMPro;

public class ColorConfirmationManager : MonoBehaviour
{
    [Header("Eagle Model Components")]
    public Renderer eagleRenderer; // 老鹰模型的渲染器

    [Header("UI Components")]
    public TextMeshProUGUI debugText; // DebugText 显示调试信息

    private void Start()
    {
        // 检查 PlayerPrefs 里是否有颜色数据
        if (PlayerPrefs.HasKey("ColorR") && PlayerPrefs.HasKey("ColorG") && PlayerPrefs.HasKey("ColorB"))
        {
            float r = PlayerPrefs.GetFloat("ColorR");
            float g = PlayerPrefs.GetFloat("ColorG");
            float b = PlayerPrefs.GetFloat("ColorB");

            Color extractedColor = new Color(r, g, b);
            ApplyColorToEagle(extractedColor);

            UpdateDebugText($"Retrieved Color: R={r}, G={g}, B={b}");
        }
        else
        {
            UpdateDebugText("❌ No color data found in PlayerPrefs.");
        }
    }

    private void ApplyColorToEagle(Color color)
    {
        if (eagleRenderer != null)
        {
            // 尝试设置颜色，优先检查 `_BaseColor`
            if (eagleRenderer.material.HasProperty("_BaseColor"))
            {
                eagleRenderer.material.SetColor("_BaseColor", color);
            }
            else if (eagleRenderer.material.HasProperty("_Color"))
            {
                eagleRenderer.material.SetColor("_Color", color);
            }
            else
            {
                UpdateDebugText("❌ Material does not support color changes.");
            }

            UpdateDebugText($"✅ Applied color {color} to Eagle Model");
        }
        else
        {
            UpdateDebugText("❌ Eagle Renderer is not assigned.");
        }
    }

    private void UpdateDebugText(string message)
    {
        if (debugText != null)
        {
            debugText.text = message;
        }
    }
}
