# Unity Scene Builder

This script adds all scene files in the "Assets/Scenes" directory to the build settings in Unity. It can be accessed through the "Tools" menu and is useful for quickly adding multiple scenes to the build settings.

## Usage

1. Add this script to the "Editor" folder in your Unity project.
2. Go to "Tools" in the Unity menu bar and click "Add-ons/Add Scenes to Build Settings".
3. All scene files in the "Assets/Scenes" directory will be added to the build settings.
4. A message will be displayed in the Unity console indicating the number of scenes that have been added to the build settings.

## Notes

- If there are no scenes found in the scenes directory, an error message will be displayed in the Unity console.
- This script only adds scene files to the build settings, it does not change the order of scenes or remove any existing scenes from the build settings.
- You need to manually unchecked unnecessary scenes added in build settings (i.e. a debug scene) to avoid adding unwanted scenes in the build.

## License

This script is licensed under the MIT license. See the `LICENSE` file for more details.
