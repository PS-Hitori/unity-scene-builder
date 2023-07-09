using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

// È possibile modificare le impostazioni di configurazione di seguito per cambiare la directory predefinita delle scene.
// You can edit the configuration settings below to change the default scenes directory.
public class ConfigurationSettings 
{
    private const string ScenesDirectoryKey = "ScenesDirectory";
    private const string DefaultScenesDirectory = "Assets/Scenes";
    private const string SceneFilter = "t:Scene";    
}

public class AddScenesToBuild
{
    [MenuItem("Strumenti/Componenti aggiuntivi/Aggiungi scene alle impostazioni di compilazione")]
    // Add scenes to the build settings
    private static void AddScenesToBuildSettings()
    {
        try
        {
            // Trova tutte le scene nella directory delle scene
            // Find all scenes in the scenes directory
            string scenesDirectory = EditorPrefs.GetString(ScenesDirectoryKey, DefaultScenesDirectory);
            string[] sceneGUIDs = AssetDatabase.FindAssets(SceneFilter, new[] { scenesDirectory });

            // Verifica se ci sono scene nella directory delle scene
            // Check if there are any scenes in the scenes directory
            if (sceneGUIDs.Length == 0)
            {
                Debug.LogError($"Nessun file di scena trovato in {scenesDirectory}. " +
                               $"Aggiungi almeno un file di scena alla directory e riprova.");
                // No scene files found in the directory. Add at least one scene file to the directory and try again.
                return;
            }

            // Crea l'array delle scene di compilazione
            // Create the build scene array
            EditorBuildSettingsScene[] buildScenes = new EditorBuildSettingsScene[sceneGUIDs.Length];

            for (int i = 0; i < sceneGUIDs.Length; i++)
            {
                // Ottieni il percorso del file di scena
                // Get the path of the scene file
                string scenePath = AssetDatabase.GUIDToAssetPath(sceneGUIDs[i]);

                // Aggiungi la scena alle impostazioni di compilazione
                // Add the scene to the build settings
                buildScenes[i] = new EditorBuildSettingsScene(scenePath, true);
            }

            // Verifica se ci sono scene esistenti nelle impostazioni di compilazione
            // Check if there are existing scenes in the build settings
            if (EditorBuildSettings.scenes.Length > 0)
            {
                // Mostra un messaggio di avviso all'utente
                // Show a warning message to the user
                bool replaceScenes = EditorUtility.DisplayDialog("Avviso",
                    "Ci sono scene esistenti nelle impostazioni di compilazione. " +
                    "L'aggiunta di nuove scene potrebbe sostituire le scene esistenti. " +
                    "Vuoi continuare?",
                    // There are existing scenes in the build settings. Adding new scenes may replace existing scenes. Do you want to continue?
                    "Sì", "No");

                // Se l'utente vuole sostituire le scene, procedi
                // If the user wants to replace the scenes, proceed
                if (replaceScenes)
                {
                    switch (EditorUtility.DisplayDialogComplex("Aggiungi scene alle impostazioni di compilazione",
                        "Cosa desideri fare con le nuove scene?",
                        "Aggiungi", "Sostituisci", "Annulla"))
                    {
                        case 0: // Aggiungi
                            // Ottieni tutte le scene nelle impostazioni di compilazione
                            // Get all scenes in the build settings
                            List<EditorBuildSettingsScene> allScenes = new List<EditorBuildSettingsScene>(EditorBuildSettings.scenes);
                            foreach (EditorBuildSettingsScene buildScene in buildScenes)
                            {
                                // Verifica se una scena con lo stesso percorso della nuova scena esiste già nelle impostazioni di compilazione
                                // Check if a scene with the same path as the new scene already exists in the build settings
                                EditorBuildSettingsScene existingScene = allScenes.Find(scene => scene.path == buildScene.path);
                                if (existingScene != null)
                                {
                                    // Rimuovi la scena esistente prima di aggiungere la nuova scena
                                    // Remove the existing scene before adding the new scene
                                    allScenes.Remove(existingScene);
                                }
                                allScenes.Add(buildScene);
                            }
                            EditorBuildSettings.scenes = allScenes.ToArray();
                            Debug.Log($"Aggiunte {buildScenes.Length} scene alle impostazioni di compilazione.");
                            // Added {buildScenes.Length} scenes to the build settings.
                            break;
                        case 1: // Sostituisci
                            // Imposta le scene di compilazione nelle impostazioni di compilazione dell'Editor
                            // Set the build scenes in the EditorBuildSettings
                            EditorBuildSettings.scenes = buildScenes;
                            Debug.Log($"Sostituite le scene esistenti nelle impostazioni di compilazione con {sceneGUIDs.Length} nuove scene.");
                            // Replaced existing scenes in the build settings with {sceneGUIDs.Length} new scenes.
                            break;
                        case 2: // Annulla
                            // Non fare nulla
                            // Do nothing
                            Debug.Log("Aggiunta delle scene alle impostazioni di compilazione annullata.");
                            // Cancelled adding scenes to build settings.
                            break;
                    }
                }
            }
            else
            {
                // Imposta le scene di compilazione nelle impostazioni di compilazione dell'Editor
                // Set the build scenes in the EditorBuildSettings
                EditorBuildSettings.scenes = buildScenes;
                Debug.Log($"Aggiunte {sceneGUIDs.Length} scene alle impostazioni di compilazione.");
                // Added {sceneGUIDs.Length} scenes to the build settings.
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Si è verificato un errore durante l'aggiunta delle scene alle impostazioni di compilazione: {ex.Message}");
            // An error occurred while adding scenes to build settings: {ex.Message}
        }
    }
}
