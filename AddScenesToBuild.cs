using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

public class AddScenesToBuild
{
   // Configuration settings
    private const string ScenesDirectoryKey = "ScenesDirectory";
    private const string DefaultScenesDirectory = "Assets/Scenes";
    private const string SceneFilter = "t:Scene";

    [MenuItem("Tools/Add-ons/Add Scenes to Build Settings")]
    private static void AddScenesToBuildSettings()
    {
        try
        {
            // Find all scenes in the scenes directory
            string scenesDirectory = EditorPrefs.GetString(ScenesDirectoryKey, DefaultScenesDirectory);
            string[] sceneGUIDs = AssetDatabase.FindAssets(SceneFilter, new[] { scenesDirectory });

            // Check if there are any scenes in the scenes directory
            if (sceneGUIDs.Length == 0)
            {
                Debug.LogError($"No scene files found in {scenesDirectory}. " +
                               $"Add at least one scene file to the directory and try again.");
                return;
            }

            // Create the build scene array
            EditorBuildSettingsScene[] buildScenes = new EditorBuildSettingsScene[sceneGUIDs.Length];

            for (int i = 0; i < sceneGUIDs.Length; i++)
            {
                // Get the path of the scene file
                string scenePath = AssetDatabase.GUIDToAssetPath(sceneGUIDs[i]);

                // Add the scene to the build settings
                buildScenes[i] = new EditorBuildSettingsScene(scenePath, true);
            }

            // Check if there are existing scenes in the build settings
            if (EditorBuildSettings.scenes.Length > 0)
            {
                // Show warning message to the user
                bool replaceScenes = EditorUtility.DisplayDialog("Warning",
                    "There are existing scenes in the build settings. " +
                    "Adding new scenes may replace existing scenes. " +
                    "Do you want to continue?",
                    "Yes", "No");

                // If the user wants to replace the scenes, proceed
                if (replaceScenes)
                {
                    switch (EditorUtility.DisplayDialogComplex("Add Scenes to Build Settings",
                        "What do you want to do with the new scenes?",
                        "Append", "Replace", "Cancel"))
                    {
                        case 0: // Append
                                // Get all scenes in the build settings
                            List<EditorBuildSettingsScene> allScenes = new List<EditorBuildSettingsScene>(EditorBuildSettings.scenes);
                            foreach (EditorBuildSettingsScene buildScene in buildScenes)
                            {
                                // Check if a scene with the same path as the new scene already exists in the build settings
                                EditorBuildSettingsScene existingScene = allScenes.Find(scene => scene.path == buildScene.path);
                                if (existingScene != null)
                                {
                                    // Remove the existing scene before adding the new scene
                                    allScenes.Remove(existingScene);
                                }
                                allScenes.Add(buildScene);
                            }
                            EditorBuildSettings.scenes = allScenes.ToArray();
                            Debug.Log($"Added {buildScenes.Length} scenes to the build settings.");
                            break;
                        case 1: // Replace
                                // Set the build scenes in the EditorBuildSettings
                            EditorBuildSettings.scenes = buildScenes;
                            Debug.Log($"Replaced existing scenes in the build settings with {sceneGUIDs.Length} new scenes.");
                            break;
                        case 2: // Cancel
                                // Do nothing
                            Debug.Log("Cancelled adding scenes to build settings.");
                            break;
                    }
                }
            }
            else
            {
                // Set the build scenes in the EditorBuildSettings
                EditorBuildSettings.scenes = buildScenes;
                Debug.Log($"Added {sceneGUIDs.Length} scenes to the build settings.");
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"An error occurred while adding scenes to build settings: {ex.Message}");
        }
    }
}
