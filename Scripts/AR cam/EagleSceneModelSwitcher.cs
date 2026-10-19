using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Collections;

public class EagleModelSwitcher : MonoBehaviour
{
    [Header("AR Components")]
    public ARTrackedImageManager trackedImageManager;

    [Header("Default Eagle")]
    public GameObject defaultEagle;

    [Header("Eagle Models")]
    public GameObject ealgeRed;
    public GameObject ealgeRGB;
    public GameObject ealgeBlue;

    [Header("Target Scene")]
    public string targetSceneName = "Color Confirmation";

    private Dictionary<string, GameObject> eagleModels = new Dictionary<string, GameObject>();

    private void Start()
    {
        // 记录不同图片对应的老鹰模型
        eagleModels["ealgeRed"] = ealgeRed;
        eagleModels["ealgeRGB"] = ealgeRGB;
        eagleModels["ealgeBlue"] = ealgeBlue;
        eagleModels["ealgeNormal"] = defaultEagle;
    }

    private void OnEnable()
    {
        trackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
    }

    private void OnDisable()
    {
        trackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
    }

    private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (var trackedImage in eventArgs.added)
        {
            UpdateEagleModel(trackedImage);
        }

        foreach (var trackedImage in eventArgs.updated)
        {
            UpdateEagleModel(trackedImage);
        }
    }

    private void UpdateEagleModel(ARTrackedImage trackedImage)
    {
        if (trackedImage.trackingState == TrackingState.Tracking)
        {
            string imageName = trackedImage.referenceImage.name;

            if (eagleModels.ContainsKey(imageName))
            {
                // 记录当前选中的老鹰模型
                string selectedEagle = eagleModels[imageName].name;
                PlayerPrefs.SetString("SelectedEagle", selectedEagle);
                PlayerPrefs.Save();

                StartCoroutine(SwitchSceneAfterDelay(1f));
            }
        }
    }
    private IEnumerator SwitchSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(targetSceneName);
    }
}
