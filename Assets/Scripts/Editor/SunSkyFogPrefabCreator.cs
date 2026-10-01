using UnityEngine;
using UnityEditor;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

/// <summary>
/// Editor utility that creates reusable Prefabs for Sun (Directional Light),
/// Sky & Fog Volume, and a combined "Sun Sky Fog" setup.
/// 
/// Usage: Unity Menu → Tools → Create Sun/Sky/Fog Prefabs
/// 
/// The prefabs and Volume Profile are saved under Assets/Prefabs/SunSkyFog/.
/// Copy that entire folder into any other HDRP project to reuse the setup.
/// </summary>
public static class SunSkyFogPrefabCreator
{
    private const string OutputFolder = "Assets/Prefabs/SunSkyFog";

    [MenuItem("Tools/Create Sun Sky Fog Prefabs")]
    public static void CreatePrefabs()
    {
        // Ensure output folder exists
        EnsureFolder("Assets/Prefabs");
        EnsureFolder(OutputFolder);

        // --- 1. Create the Volume Profile asset ---
        VolumeProfile profile = CreateVolumeProfile();

        // --- 2. Create Sun Prefab ---
        CreateSunPrefab();

        // --- 3. Create Sky & Fog Volume Prefab ---
        CreateSkyFogVolumePrefab(profile);

        // --- 4. Create Combined Prefab ---
        CreateCombinedPrefab(profile);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        EditorUtility.DisplayDialog(
            "Sun/Sky/Fog Prefabs Created",
            $"Prefabs and Volume Profile saved to:\n{OutputFolder}\n\n" +
            "You can now copy this folder to any HDRP project.",
            "OK");

        // Ping the folder in Project window
        var folder = AssetDatabase.LoadAssetAtPath<Object>(OutputFolder);
        EditorGUIUtility.PingObject(folder);
    }

    // ─────────────────────────────────────────────
    // Volume Profile
    // ─────────────────────────────────────────────
    private static VolumeProfile CreateVolumeProfile()
    {
        string profilePath = $"{OutputFolder}/SunSkyFog_VolumeProfile.asset";

        VolumeProfile profile = ScriptableObject.CreateInstance<VolumeProfile>();

        // --- Visual Environment ---
        var visualEnv = profile.Add<VisualEnvironment>(true);
        visualEnv.skyType.Override(4);               // PhysicallyBasedSky
        visualEnv.cloudType.Override(1);             // VolumetricClouds
        visualEnv.skyAmbientMode.Override(SkyAmbientMode.Dynamic);

        // --- Physically Based Sky ---
        var pbSky = profile.Add<PhysicallyBasedSky>(true);
        pbSky.type.Override(PhysicallyBasedSkyModel.EarthAdvanced);
        pbSky.aerosolDensity.Override(0.025f);
        pbSky.groundTint.Override(new Color(0.88f, 0.78f, 0.65f, 1f));
        pbSky.horizonTint.Override(new Color(1f, 0.88f, 0.72f, 1f));
        pbSky.zenithTint.Override(new Color(0.18f, 0.44f, 0.88f, 1f));

        // --- Volumetric Clouds ---
        var clouds = profile.Add<VolumetricClouds>(true);
        clouds.enable.Override(true);

        // --- Exposure ---
        var exposure = profile.Add<Exposure>(true);
        exposure.mode.Override(ExposureMode.Automatic);
        exposure.compensation.Override(0.4f);

        // --- Color Adjustments ---
        var colorAdj = profile.Add<ColorAdjustments>(true);
        colorAdj.contrast.Override(12f);
        colorAdj.colorFilter.Override(new Color(1f, 0.97f, 0.92f, 1f));
        colorAdj.saturation.Override(18f);

        // --- Tonemapping ---
        var tonemap = profile.Add<Tonemapping>(true);
        tonemap.mode.Override(TonemappingMode.ACES);

        // --- Bloom ---
        var bloom = profile.Add<Bloom>(true);
        bloom.intensity.Override(0.35f);
        bloom.scatter.Override(0.72f);
        bloom.tint.Override(new Color(1f, 0.95f, 0.85f, 1f));

        // --- Vignette ---
        var vignette = profile.Add<Vignette>(true);
        vignette.intensity.Override(0.22f);
        vignette.smoothness.Override(0.8f);

        AssetDatabase.CreateAsset(profile, profilePath);
        Debug.Log($"[SunSkyFog] Volume Profile created: {profilePath}");

        return profile;
    }

    // ─────────────────────────────────────────────
    // Sun (Directional Light with HDAdditionalLightData)
    // ─────────────────────────────────────────────
    private static void CreateSunPrefab()
    {
        string prefabPath = $"{OutputFolder}/Sun.prefab";

        var go = new GameObject("Sun");

        // Light component
        var light = go.AddComponent<Light>();
        light.type = LightType.Directional;
        light.color = new Color(0.45f, 0.65f, 0.95f, 1f);
        light.intensity = 1.5f;
        light.useColorTemperature = true;
        light.colorTemperature = 6500f;
        light.shadows = LightShadows.Soft;

        // HDRP additional data is auto-added; configure it
        var hdLight = go.GetComponent<HDAdditionalLightData>();
        if (hdLight != null)
        {
            hdLight.SetIntensity(130000f, LightUnit.Lux);
            hdLight.angularDiameter = 2.5f;
            hdLight.interactsWithSky = true;
            hdLight.volumetricDimmer = 1f;
        }

        // Transform – 45° sun angle
        go.transform.rotation = Quaternion.Euler(45f, 0f, 0f);
        go.transform.position = new Vector3(0f, 3f, 0f);

        PrefabUtility.SaveAsPrefabAsset(go, prefabPath);
        Object.DestroyImmediate(go);

        Debug.Log($"[SunSkyFog] Sun prefab created: {prefabPath}");
    }

    // ─────────────────────────────────────────────
    // Sky & Fog Volume
    // ─────────────────────────────────────────────
    private static void CreateSkyFogVolumePrefab(VolumeProfile profile)
    {
        string prefabPath = $"{OutputFolder}/SkyAndFogVolume.prefab";

        var go = new GameObject("Sky and Fog Volume");

        var volume = go.AddComponent<Volume>();
        volume.isGlobal = true;
        volume.priority = 10f;
        volume.weight = 0.5f;
        volume.sharedProfile = profile;

        PrefabUtility.SaveAsPrefabAsset(go, prefabPath);
        Object.DestroyImmediate(go);

        Debug.Log($"[SunSkyFog] Sky and Fog Volume prefab created: {prefabPath}");
    }

    // ─────────────────────────────────────────────
    // Combined: Sun + Sky & Fog Volume under one root
    // ─────────────────────────────────────────────
    private static void CreateCombinedPrefab(VolumeProfile profile)
    {
        string prefabPath = $"{OutputFolder}/SunSkyFog_Combined.prefab";

        var root = new GameObject("Sun Sky Fog");

        // --- Sun child ---
        var sunGo = new GameObject("Sun");
        sunGo.transform.SetParent(root.transform);

        var light = sunGo.AddComponent<Light>();
        light.type = LightType.Directional;
        light.color = new Color(0.45f, 0.65f, 0.95f, 1f);
        light.intensity = 1.5f;
        light.useColorTemperature = true;
        light.colorTemperature = 6500f;
        light.shadows = LightShadows.Soft;

        var hdLight = sunGo.GetComponent<HDAdditionalLightData>();
        if (hdLight != null)
        {
            hdLight.SetIntensity(130000f, LightUnit.Lux);
            hdLight.angularDiameter = 2.5f;
            hdLight.interactsWithSky = true;
            hdLight.volumetricDimmer = 1f;
        }

        sunGo.transform.localRotation = Quaternion.Euler(45f, 0f, 0f);
        sunGo.transform.localPosition = new Vector3(0f, 3f, 0f);

        // --- Volume child ---
        var volGo = new GameObject("Sky and Fog Volume");
        volGo.transform.SetParent(root.transform);

        var volume = volGo.AddComponent<Volume>();
        volume.isGlobal = true;
        volume.priority = 10f;
        volume.weight = 0.5f;
        volume.sharedProfile = profile;

        PrefabUtility.SaveAsPrefabAsset(root, prefabPath);
        Object.DestroyImmediate(root);

        Debug.Log($"[SunSkyFog] Combined prefab created: {prefabPath}");
    }

    // ─────────────────────────────────────────────
    // Utility
    // ─────────────────────────────────────────────
    private static void EnsureFolder(string path)
    {
        if (!AssetDatabase.IsValidFolder(path))
        {
            string parent = System.IO.Path.GetDirectoryName(path).Replace("\\", "/");
            string folderName = System.IO.Path.GetFileName(path);
            AssetDatabase.CreateFolder(parent, folderName);
        }
    }
}
