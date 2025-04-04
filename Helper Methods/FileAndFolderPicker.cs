using SFB;

using UnityEngine;

namespace OttoIconChanger
{
    public class FileAndFolderPicker : MonoBehaviour
    {
        // File picker for selecting an image
        public string OpenFilePickerForImage()
        {
            // Use StandaloneFileBrowser to open the file picker
            string[] paths = StandaloneFileBrowser.OpenFilePanel(
                "Select Image",
                "", // Default directory
                new ExtensionFilter[] { new ExtensionFilter("Image Files", "png", "jpg", "jpeg") },
                false // Single selection only
            );

            if (paths != null && paths.Length > 0 && !string.IsNullOrEmpty(paths[0]))
            {
                return paths[0]; // Return the selected file path
            }
            return string.Empty;
        }

        // Folder picker for selecting a folder containing animation images
        public string PickerForAnimation()
        {
            // Use StandaloneFileBrowser to open the folder picker
            string[] paths = StandaloneFileBrowser.OpenFolderPanel(
                "Select a Folder",
                "", // Default directory
                false // Single selection only
            );

            if (paths != null && paths.Length > 0 && !string.IsNullOrEmpty(paths[0]))
            {
                return paths[0]; // Return the selected folder path
            }
            return string.Empty;
        }

        public string OpenFilePickerForVideoGif()
        {
            ExtensionFilter[] filters = new ExtensionFilter[]
            {
        new ExtensionFilter("Video Files", "mp4", "mov", "avi", "webm"),
        new ExtensionFilter("GIF Files", "gif")
            };

            string[] paths = StandaloneFileBrowser.OpenFilePanel(
                "Select a Video or GIF File",
                "",
                filters,
                false
            );

            return (paths.Length > 0) ? paths[0] : string.Empty;
        }
    }
}
