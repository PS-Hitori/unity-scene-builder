// Author: Hitori
// Date: 26th April 2023 (Wednesday) 
// Description: This script adds all scene files in the "Assets/Scenes" directory to the build settings in Unity. 
// It can be accessed through the "Tools" menu and is useful for quickly adding multiple scenes to the build settings.
// Usage: Add this script to the "Editor" folder in your project. Then, go to "Tools" in the Unity menu bar and click "QOL Addons/Add All Scenes to Build Settings".

using UnityEditor;
using UnityEngine;
public class AddScenesToBuild
{
    // Constant for the scenes directory
    private const string ScenesDirectory = "Assets/Scenes"; // Let us assume this is the default directory, change it if your Scenes are in a different directory

    [MenuItem("Tools/QOL Addons/Add All Scenes to Build Settings")]
    public static void AddScenesToBuildSettings()
    {
        // Get all scene files in the scenes directory using AssetDatabase and the ScenesDirectory constant
        string[] sceneBuilds = AssetDatabase.FindAssets("t:Scene", new[] { ScenesDirectory });
        
        // If there are no scenes in the scenes directory, log an error and return
        if (sceneBuilds.Length == 0)
        {
            Debug.LogError("No scenes found in " + ScenesDirectory + ".");
            return;
        }

        // Create the build scene array
        EditorBuildSettingsScene[] buildScenes = new EditorBuildSettingsScene[sceneBuilds.Length];
        for(int sceneIndex = 0; sceneIndex < sceneBuilds.Length; sceneIndex++){
            string scenePath = AssetDatabase.GUIDToAssetPath(sceneBuilds[sceneIndex]);
            buildScenes[sceneIndex] = new EditorBuildSettingsScene(scenePath, true);
        }

        // Set the build scenes in the EditorBuildSettings
        EditorBuildSettings.scenes = buildScenes;

        // Log the number of scenes added to the build settings
        Debug.Log("Added " + sceneBuilds.Length + " scenes to the build settings.");
    }
}
