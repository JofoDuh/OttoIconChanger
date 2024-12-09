using System.Windows.Forms;
using UnityEngine;

namespace OttoIconChanger
{
    public class FileAndFolderPicker : MonoBehaviour
    {
        // File picker for selecting an image (Windows-specific example)
        public void OpenFilePickerForImageOn()
        {
            // Open file picker using Windows Forms (only on Windows)
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files|*.png;*.jpg;*.jpeg";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string selectedFile = openFileDialog.FileName;
                Debug.Log("Selected Image: " + selectedFile);
                Patch.setting.LocalImagePathOn = selectedFile;
            }
            else
            {
                Debug.Log("No image selected.");
            }
        }
        // Folder picker for selecting a folder containing animation images
        public void OpenFolderPickerForAnimationOn()
        {
            // Open folder picker using Windows Forms (only on Windows)
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                string selectedFolder = folderBrowserDialog.SelectedPath;
                Debug.Log("Selected Folder: " + selectedFolder);
                Patch.setting.LocalAnimationFolderPathOn = selectedFolder;
            }
            else
            {
                Debug.Log("No folder selected.");
            }
        }
        public void OpenFilePickerForImageOff()
        {
            // Open file picker using Windows Forms (only on Windows)
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files|*.png;*.jpg;*.jpeg";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string selectedFile = openFileDialog.FileName;
                Debug.Log("Selected Image: " + selectedFile);
                Patch.setting.LocalImagePathOff = selectedFile;
            }
            else
            {
                Debug.Log("No image selected.");
            }
        }

        // Folder picker for selecting a folder containing animation images
        public void OpenFolderPickerForAnimationOff()
        {
            // Open folder picker using Windows Forms (only on Windows)
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                string selectedFolder = folderBrowserDialog.SelectedPath;
                Debug.Log("Selected Folder: " + selectedFolder);
                Patch.setting.LocalAnimationFolderPathOff = selectedFolder;
            }
            else
            {
                Debug.Log("No folder selected.");
            }
        }
    }
}
