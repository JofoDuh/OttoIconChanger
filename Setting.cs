using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using UnityEngine;
using UnityModManagerNet;

namespace OttoIconChanger
{
    public class Setting : UnityModManager.ModSettings
    {
        //General
        public int FirstTimeLoad = 0;
        public int Browsestate = 0;
        public bool FastMode = true;
        //From AdofaiTweaks
        public bool EditorIsAwake = false;
        public string OttoColorHex { get; set; } = "FFFFFF";
        private Color _Ottocolor = Color.white;
        public Color Ottocolor
        {
            get => _Ottocolor;
            set
            {
                _Ottocolor = value;
                OttoColorHex = ColorUtility.ToHtmlStringRGB(value);
            }
        }
        public string OttoColorHexOn { get; set; } = "FFFFFF";
        private Color _OttocolorOn = Color.white;
        public Color OttocolorOn
        {
            get => _OttocolorOn;
            set
            {
                _OttocolorOn = value;
                OttoColorHexOn = ColorUtility.ToHtmlStringRGB(value);
            }
        }
        public string OttoColorHexOff { get; set; } = "FFFFFF";
        private Color _OttocolorOff = Color.white;
        public Color OttocolorOff
        {
            get => _OttocolorOff;
            set
            {
                _OttocolorOff = value;
                OttoColorHexOff = ColorUtility.ToHtmlStringRGB(value);
            }
        }
        //No Dark Otto When Off
        public bool OttoGreyOffIsEnabled = false;
        //No Nervous Otto
        public bool NoNervousOttoIsEnabled = false;
        public bool ResultForHighBpm = false;
        //Color Changer
        public bool OttoColorChangerIsEnabled = false;
        public bool OttoColorIndependentIsEnabled = false;
        //Opacity Changer
        public bool OttoOpacityChangerIsEnabled = false;
        public bool OttoOpacityIndependentIsEnabled = false;
        public float OttoOpacityValue = 255f;
        public float OttoOpacityValueOn = 255f;
        public float OttoOpacityValueOff = 255f;
        //Custom Otto Position & Size
        public bool OttoPosChangerIsEnabled = false;
        public bool OttoSizeChangerIsEnabled = false;
        public bool SquareSizeIsEnabled = true;
        public Vector2 originalOttoImageOffsetMin;
        public Vector2 originalOttoImageOffsetMax;
        public Vector2 originalOttoButtonOffsetMin;
        public Vector2 originalOttoButtonOffsetMax;
        public float PositionNewX;
        public float PositionNewY;
        public float NewOttoSizeX;
        public float NewOttoSizeY;
        //Custom Otto Sprite
        public readonly string[] OttoStates = new string[10]{"On", "Off", "Left On", "Left Off", "Right On", "Right Off",
            "Nervous On", "Nervous Off", "Pet", "Miss"};
        public bool CustomOttoImageIsEnabled = false;
        public bool ResultForPaused;
        public int OttoBlinkState;
        public int OttoBlinkCounter = 0;
        public bool HaveBlink;
        public int indexCheck = 0;
        public float ottoPetTime;
        public float FramesPerSecond = 120;
        public float FramesPerSpriteChange = 10;
        public float SecondsPerSpriteChange = 10f / 120f;
        public bool FrameBasedValuesIsEnabled = false;
        public float BlinkDistance = 50;
        //Custom Local Otto Sprite
        public bool UseLocalImage { get; set; } = true; // Toggle between built-in and local images
        public bool UseLocalAnimation {  get; set; } = false;

        public PathsStorer LocalImage;
        public PathsStorer LocalAnimation;
        public Setting()
        {
            LocalImage = new PathsStorer(0, OttoStates.Length);
            LocalAnimation = new PathsStorer(1, OttoStates.Length);
        }

        public string PresetName;
        public bool IsPreset;
        public int CurrentIndex;
        // List to hold multiple DataEntry objects
        public List<PresetList> PresetLists = new List<PresetList>();

        public int AmountOfFramesOn = 0;
        public int AmountOfFramesOff = 0;

        //Otto Characters enum list
        public enum OttoCharacter
        {
            FurinaNonAnimated,
            ElysiaNonAnimated,
            FurinaAnimated, // Animated
            HuTaoAnimated,   // Animated
            SparkleAnimated,  // Animated
            FireFlyAnimated // Animated
        }
        //Set default character to Furina non animated ver.
        public OttoCharacter SelectedCharacter = OttoCharacter.FurinaNonAnimated;

        // Define animated characters in a HashSet
        private static readonly HashSet<OttoCharacter> AnimatedCharacters = new HashSet<OttoCharacter>
        {
            OttoCharacter.FireFlyAnimated,
            OttoCharacter.HuTaoAnimated,
            OttoCharacter.FurinaAnimated,
            OttoCharacter.SparkleAnimated
        };
        //Check if the selected character is animated and if an animated character is selected
        public bool IsAnimatedCharacter(OttoCharacter character) => AnimatedCharacters.Contains(character);
        public bool IsAnimatedCharacterSelected() => AnimatedCharacters.Contains(SelectedCharacter);

        //Parse method for floats to remove non intergers
        public float ParseInput(string input)
        {
            // Allow digits, decimal points, and the '-' sign
            string sanitizedInput = Regex.Replace(input, @"[^0-9.]", "");

            // Prevent invalid cases like "."
            if (string.IsNullOrEmpty(sanitizedInput) || sanitizedInput == ".")
            {
                return 0f;
            }

            // Try parsing as a float
            if (float.TryParse(sanitizedInput, out float result))
            {
                return result;
            }

            // Return 0 if parsing fails
            return 0f;
        }
        public void PresetListInitializer(PresetList list)
        {
            while (list.SetDefaults.Count < OttoStates.Length || list.Paths.Count < OttoStates.Length)
            {
                if (list.SetDefaults.Count < OttoStates.Length)
                {
                    list.SetDefaults.Add(0);
                }
                if (list.Paths.Count < OttoStates.Length)
                {
                    list.Paths.Add(string.Empty);
                }
            }
        }
        private bool IsVideoFile(string path)
        {
            if (string.IsNullOrEmpty(path)) return false;

            string extension = System.IO.Path.GetExtension(path)?.ToLower();
            return extension == ".mp4" || extension == ".mov" ||
                   extension == ".avi" || extension == ".webm" ||
                   extension == ".gif";
        }

        public void FreeSpace(bool FastMode = true)
        {
            List<string> Excludes = new List<string>();
            if (FastMode)
            {

                if (PresetLists.Count > 0)
                {
                    foreach (var preset in PresetLists)
                    {
                        foreach (string path in preset.Paths)
                        {
                            bool isVideo = IsVideoFile(path);
                            if (isVideo)
                            {
                                string FolderName = System.IO.Path.GetFileNameWithoutExtension(path);
                                Excludes.Add(FolderName);
                            }
                        }
                    }
                }
            }
            foreach (var path in LocalAnimation.LocalPaths)
            {
                bool isVideo = IsVideoFile(path);
                if (isVideo)
                {
                    string FolderName = System.IO.Path.GetFileNameWithoutExtension(path);
                    Excludes.Add(FolderName);
                }
            }

            // Get the main directory where the folders are stored
            string mainDirectory = Main.ModEntry.Path;

            // Get all folders within the main directory
            string[] allFolders = Directory.GetDirectories(mainDirectory);

            foreach (string folderPath in allFolders)
            {
                string folderName = Path.GetFileName(folderPath);
                // If the folder is not in the Excludes list, delete it
                if (!Excludes.Contains(folderName))
                {
                    try
                    {
                        Directory.Delete(folderPath, true); // 'true' forces deletion of all contents
                        //Main.Logger.Log($"Deleted folder: {folderPath}");
                    }
                    catch (Exception ex)
                    {
                        Main.Logger.Log($"Failed to delete folder: {folderPath}. Error: {ex.Message}");
                    }
                }
            }
        }
        public void Apply(bool LoadBoth = false, int PresetTypeChecker = 0)
        {
            if (!IsPreset)
            {
                int index;
                if ((Main.setting.UseLocalImage && !Main.setting.UseLocalAnimation) || LoadBoth)
                {
                    // Check if file paths are valid for static images
                    // Apply the paths for the selected images
                    index = 0;
                    string[] selectedImagePaths = new string[10];

                    for (int i = 0; i < 10; i++)
                    {
                        selectedImagePaths[i] = Main.setting.LocalImage.LocalPaths[i];
                    }

                    // Load images or perform any necessary logic
                    foreach (string imagePath in selectedImagePaths)
                    {
                        PathsLoader.LoadCustomSpriteFromPath(imagePath, index, false);
                        index++;
                    }
                }
                if ((Main.setting.UseLocalAnimation && Main.setting.UseLocalImage) || LoadBoth)
                {
                    index = 0;
                    // Check if folder paths are valid for animation
                    // Apply the folder paths for the selected animation
                    string[] selectedFolderPaths = new string[10];

                    for (int i = 0; i < 10; i++)
                    {
                        selectedFolderPaths[i] = Main.setting.LocalAnimation.LocalPaths[i];
                    }

                    // Load animation sprites or perform any necessary logic
                    foreach (string folderPath in selectedFolderPaths)
                    {
                        bool isVideo = IsVideoFile(folderPath);

                        PathsLoader.LoadCustomSpriteFromPath(folderPath, index, true, isVideo);
                        index++;
                    }
                }
            }
            else
            {
                int index;
                if (PresetTypeChecker == 0)
                {
                    // Check if file paths are valid for static images
                    // Apply the paths for the selected images
                    index = 0;
                    string[] selectedImagePaths = new string[10];

                    for (int i = 0; i < 10; i++)
                    {
                        selectedImagePaths[i] = Main.setting.PresetLists[CurrentIndex].Paths[i];
                    }

                    // Load images or perform any necessary logic
                    foreach (string imagePath in selectedImagePaths)
                    {
                        PathsLoader.LoadCustomSpriteFromPath(imagePath, index, false);
                        index++;
                    }
                }
                else
                {
                    index = 0;
                    // Check if folder paths are valid for animation
                    // Apply the folder paths for the selected animation
                    string[] selectedFolderPaths = new string[10];

                    for (int i = 0; i < 10; i++)
                    {
                        selectedFolderPaths[i] = Main.setting.PresetLists[CurrentIndex].Paths[i];
                    }

                    // Load animation sprites or perform any necessary logic
                    foreach (string folderPath in selectedFolderPaths)
                    {
                        bool isVideo = IsVideoFile(folderPath);

                        PathsLoader.LoadCustomSpriteFromPath(folderPath, index, true, isVideo);
                        index++;
                    }
                }
            }
            FreeSpace(FastMode);
        }
        //Method to set the Default States of state to specific ones at fire-time launch
        public override void Save(UnityModManager.ModEntry modEntry)
        {
            var filepath = GetPath(modEntry);
            try
            {
                using (var writer = new StreamWriter(filepath))
                {
                    var serializer = new XmlSerializer(GetType());
                    serializer.Serialize(writer, this);
                }
            }
            catch
            {
            }
        }
        public override string GetPath(UnityModManager.ModEntry modEntry)
        {
            return Path.Combine(modEntry.Path, GetType().Name + ".xml");
        }
    }
}