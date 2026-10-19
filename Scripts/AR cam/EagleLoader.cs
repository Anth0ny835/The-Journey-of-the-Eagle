using UnityEngine;
using System.Collections.Generic;

public class EagleLoader : MonoBehaviour
{
    [Header("Default Eagle")]
    public GameObject defaultEagle;

    [Header("Eagle Models")]
    public GameObject ealgeRed;
    public GameObject ealgeRGB;
    public GameObject ealgeBlue;

    private Dictionary<string, GameObject> eagleModels = new Dictionary<string, GameObject>();

    private void Start()
    {
        // 记录不同老鹰模型
        eagleModels["ealgeRed"] = ealgeRed;
        eagleModels["ealgeRGB"] = ealgeRGB;
        eagleModels["ealgeBlue"] = ealgeBlue;
        eagleModels["ealgeNormal"] = defaultEagle;

        // 读取之前存的老鹰
        string savedEagle = PlayerPrefs.GetString("SelectedEagle", "defaultEagle");

        if (eagleModels.ContainsKey(savedEagle))
        {
            ReplaceEagleModel(eagleModels[savedEagle]);
        }
    }

    private void ReplaceEagleModel(GameObject newEaglePrefab)
    {
        // 替换默认老鹰的 Mesh
        MeshFilter defaultMeshFilter = defaultEagle.GetComponent<MeshFilter>();
        MeshFilter newMeshFilter = newEaglePrefab.GetComponent<MeshFilter>();

        if (defaultMeshFilter != null && newMeshFilter != null)
        {
            defaultMeshFilter.mesh = newMeshFilter.mesh;
        }

        // 替换默认老鹰的材质
        MeshRenderer defaultRenderer = defaultEagle.GetComponent<MeshRenderer>();
        MeshRenderer newRenderer = newEaglePrefab.GetComponent<MeshRenderer>();

        if (defaultRenderer != null && newRenderer != null)
        {
            defaultRenderer.materials = newRenderer.materials;
        }
    }
}
