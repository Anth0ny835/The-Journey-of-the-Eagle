using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using TMPro;
using System.Collections;

public class ARImageColorSceneSwitcher : MonoBehaviour
{
    [Header("AR Components")]
    public ARTrackedImageManager trackedImageManager;

    [Header("UI Components")]
    public TextMeshProUGUI statusText;
    public TextMeshProUGUI debugText;

    [Header("Target Scene")]
    public string targetSceneName = "Color Confirmation";

    private bool colorExtracted = false;

    private void Update()
    {
        bool isAnyImageTracked = false;

        foreach (var trackedImage in trackedImageManager.trackables)
        {
            if (trackedImage.trackingState == TrackingState.Tracking && !colorExtracted)
            {
                UpdateDebugText($"Tracking image: {trackedImage.referenceImage.name}");
                
                Color extractedColor = ExtractColorFromImage(trackedImage);
                if (extractedColor != Color.clear)
                {
                    UpdateDebugText($"Extracted color: R={extractedColor.r:F2}, G={extractedColor.g:F2}, B={extractedColor.b:F2}");
                    SaveColorToPrefs(extractedColor);
                    colorExtracted = true;
                    StartCoroutine(SwitchSceneAfterDelay(3f));
                }
                else
                {
                    UpdateDebugText("Failed to extract color. Texture might not be readable.");
                }
            }

            string trackingInfo = $"Image: {trackedImage.referenceImage.name}  State: {trackedImage.trackingState}";
            UpdateStatusText(trackingInfo);
            isAnyImageTracked = true;
        }

        if (!isAnyImageTracked)
        {
            UpdateStatusText("No images currently being tracked.");
        }
    }

    private Color ExtractColorFromImage(ARTrackedImage trackedImage)
    {
        Texture2D texture = trackedImage.referenceImage.texture;
        if (texture != null && texture.isReadable)
        {
            int centerX = texture.width / 2;
            int centerY = texture.height / 2;
            Color centerColor = texture.GetPixel(centerX, centerY);
            return centerColor;
        }
        return Color.clear;
    }

    private void SaveColorToPrefs(Color color)
    {
        PlayerPrefs.SetFloat("ColorR", color.r);
        PlayerPrefs.SetFloat("ColorG", color.g);
        PlayerPrefs.SetFloat("ColorB", color.b);
        PlayerPrefs.Save();
        UpdateDebugText($"Color saved to PlayerPrefs: R={color.r:F2}, G={color.g:F2}, B={color.b:F2}");
    }

    private IEnumerator SwitchSceneAfterDelay(float delay)
    {
        UpdateDebugText("Switching scene in 3 seconds...");
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(targetSceneName);
    }

    private void UpdateStatusText(string message)
    {
        if (statusText != null)
            statusText.text = message;
    }

    private void UpdateDebugText(string message)
    {
        if (debugText != null)
           debugText.text = "/n" + message;
    }
}


public class EagleColorChanger : MonoBehaviour
{
    [Header("Material Options")]
    public Renderer eagleRenderer;
    public TextMeshProUGUI debugText;

    private void Start()
    {
        ApplySavedColor();
    }

    private void ApplySavedColor()
    {
        if (PlayerPrefs.HasKey("ColorR") && PlayerPrefs.HasKey("ColorG") && PlayerPrefs.HasKey("ColorB"))
        {
            float r = PlayerPrefs.GetFloat("ColorR");
            float g = PlayerPrefs.GetFloat("ColorG");
            float b = PlayerPrefs.GetFloat("ColorB");
            Color color = new Color(r, g, b);
            UpdateDebugText($"Retrieved Color: R={r:F2}, G={g:F2}, B={b:F2}");
            ApplyColorToModel(color);
        }
        else
        {
            UpdateDebugText("No color data found in PlayerPrefs.");
        }
    }

    private void ApplyColorToModel(Color color)
    {
        if (eagleRenderer != null)
        {
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
                UpdateDebugText("Material does not have a color property to modify.");
                return;
            }
            UpdateDebugText($"Applied color {color} to {gameObject.name}");
        }
        else
        {
            UpdateDebugText("Eagle Renderer is not assigned.");
        }
    }

    private void UpdateDebugText(string message)
    {
        if (debugText != null)
           debugText.text = "/n" + message;
    }
}


public class EagleDebugManager : MonoBehaviour
{
    public TextMeshProUGUI debugText;
    
    public void LogDebugMessage(string message)
    {
        if (debugText != null)
           debugText.text = "/n" + message;
    }
}
