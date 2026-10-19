using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class SimpleEagleSwitcher : MonoBehaviour
{
    [Header("AR Components (AR Scene Only)")]
    public ARTrackedImageManager trackedImageManager;

    [Header("Eagle Models (Prefabs)")]
    // 默认老鹰对象，必须是场景中实际存在的 GameObject（已经实例化的），它的外观会被替换
    public GameObject defaultEagle;
    // 预设的不同颜色的 FBX 模型（预制体），用来替换 defaultEagle 的外观
    public GameObject eagleRedPrefab;
    public GameObject eagleRGBPrefab;
    public GameObject eagleBluePrefab;
    // 可根据需要添加更多

    [Header("Scene Settings")]
    public string targetSceneName = "Color Confirmation"; // 目标场景名称（如果需要切换场景）

    // 用于存储图片名称和对应预制体的字典（Key 要与 AR Reference Image 的名称一致）
    private Dictionary<string, GameObject> eagleModels = new Dictionary<string, GameObject>();

    private void Start()
    {
        // 初始化字典：注意 Key 必须与 AR Reference Image Library 中图片的名称匹配
        eagleModels["eagleRed"] = eagleRedPrefab;
        eagleModels["eagleRGB"] = eagleRGBPrefab;
        eagleModels["eagleBlue"] = eagleBluePrefab;
        eagleModels["eagleNormal"] = defaultEagle;  // “原版”皮肤

        // 如果当前有 ARTrackedImageManager 组件，则说明在 AR 场景中
        if (trackedImageManager != null)
        {
            trackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
        }
        else
        {
            // 如果没有，则说明在 Color Confirmation 场景
            LoadSavedEagle();
        }
    }

    private void OnDestroy()
    {
        if (trackedImageManager != null)
        {
            trackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
        }
    }

    // AR场景下：当识别到图片时调用
    private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        // 处理新增和更新的图像
        foreach (var trackedImage in eventArgs.added)
        {
            ProcessTrackedImage(trackedImage);
        }
        foreach (var trackedImage in eventArgs.updated)
        {
            ProcessTrackedImage(trackedImage);
        }
    }

    private void ProcessTrackedImage(ARTrackedImage trackedImage)
    {
        // 如果该图片正被追踪
        if (trackedImage.trackingState == TrackingState.Tracking)
        {
            string imageName = trackedImage.referenceImage.name;

            if (eagleModels.ContainsKey(imageName))
            {
                // 保存选中的老鹰模型名称到 PlayerPrefs（直接保存图片名称）
                PlayerPrefs.SetString("SelectedEagle", imageName);
                PlayerPrefs.Save();

                // 可选择是否切换场景（这里直接切换，如果需要保持在当前场景可以不调用切换场景代码）
                StartCoroutine(SwitchSceneAfterDelay(1f));
            }
        }
    }

    private IEnumerator SwitchSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(targetSceneName);
    }

    // Color Confirmation 场景下：加载保存的老鹰模型外观
    private void LoadSavedEagle()
    {
        // 从 PlayerPrefs 读取保存的选中模型名称，默认 "eagleNormal"
        string savedEagle = PlayerPrefs.GetString("SelectedEagle", "eagleNormal");

        if (eagleModels.ContainsKey(savedEagle))
        {
            ReplaceEagleModel(eagleModels[savedEagle]);
        }
    }

    // 替换 defaultEagle 的 Mesh 和材质，保持 Transform 不变
    private void ReplaceEagleModel(GameObject newEaglePrefab)
    {
        // 获取 defaultEagle 的 SkinnedMeshRenderer
        SkinnedMeshRenderer defaultRenderer = defaultEagle.GetComponentInChildren<SkinnedMeshRenderer>();
        // 获取新模型的 SkinnedMeshRenderer
        SkinnedMeshRenderer newRenderer = newEaglePrefab.GetComponentInChildren<SkinnedMeshRenderer>();

        if (defaultRenderer != null && newRenderer != null)
        {
            // 替换 Mesh 和 材质
            defaultRenderer.sharedMesh = newRenderer.sharedMesh;
            defaultRenderer.sharedMaterials = newRenderer.sharedMaterials;
            
            // 替换 Root Bone，如果需要
            defaultRenderer.rootBone = newRenderer.rootBone;
            
            // 强制刷新更新（关键：让 SkinnedMeshRenderer 即使离屏也更新）
            defaultRenderer.updateWhenOffscreen = true;
        }
    }
}
