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
        public int OttoBlinkCounter;
        public bool HaveBlink;
        public int indexCheck = 0;
        public float ottoPetTime;
        public float FramesPerSecond = 120;
        public float FramesPerSpriteChange = 10;
        public float SecondsPerSpriteChange = 10f / 120f;
        public bool FrameBasedValuesIsEnabled = false;
        //Custom Local Otto Sprite
        public bool UseLocalImage { get; set; } = true; // Toggle between built-in and local images
        public bool UseLocalAnimation {  get; set; } = false;

        public List<string> LocalImagePaths = new List<string>();
        public List<int> LocalImageSetDefaults = new List<int>();
        public List<bool> LocalImageToggles = new List<bool>();

        public List<string> LocalAnimationFolderPaths = new List<string>();
        public List<int> LocalAnimationSetDefaults = new List<int>();
        public List<bool> LocalAnimationToggles = new List<bool>();

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

        //Initializes the list of all states' path
        public void InitializeList()
        {
            int targetCount = OttoStates.Length;

            // Add items to each list until their counts match OttoStates.Count
            while (LocalImagePaths.Count < targetCount ||
                   LocalAnimationFolderPaths.Count < targetCount ||
                   LocalImageToggles.Count < targetCount ||
                   LocalAnimationToggles.Count < targetCount ||
                   LocalImageSetDefaults.Count < targetCount ||
                   LocalAnimationSetDefaults.Count < targetCount)
            {
                if (LocalImagePaths.Count < targetCount)
                    LocalImagePaths.Add(string.Empty);

                if (LocalAnimationFolderPaths.Count < targetCount)
                    LocalAnimationFolderPaths.Add(string.Empty);

                if (LocalImageToggles.Count < targetCount)
                    LocalImageToggles.Add(false);

                if (LocalAnimationToggles.Count < targetCount)
                    LocalAnimationToggles.Add(false);

                if (LocalImageSetDefaults.Count < targetCount)
                    LocalImageSetDefaults.Add(0);

                if (LocalAnimationSetDefaults.Count < targetCount)
                    LocalAnimationSetDefaults.Add(0);
            }
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
                        selectedImagePaths[i] = Main.setting.LocalImagePaths[i];
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
                        selectedFolderPaths[i] = Main.setting.LocalAnimationFolderPaths[i];
                    }

                    // Load animation sprites or perform any necessary logic
                    foreach (string folderPath in selectedFolderPaths)
                    {
                        PathsLoader.LoadCustomSpriteFromPath(folderPath, index, true);
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
                        PathsLoader.LoadCustomSpriteFromPath(folderPath, index, true);
                        index++;
                    }
                }
            }
        }
        //Method to set the Default States of state to specific ones at fire-time launch
        public void SetDefaultListValues()
        {
            LocalAnimationSetDefaults[0] = 10;
            LocalImageSetDefaults[0] = 10;
            LocalAnimationSetDefaults[1] = 10;
            LocalImageSetDefaults[1] = 10;
            LocalAnimationSetDefaults[3] = 1;
            LocalImageSetDefaults[3] = 1;
            LocalAnimationSetDefaults[5] = 1;
            LocalImageSetDefaults[5] = 1;
            LocalAnimationSetDefaults[7] = 1;
            LocalImageSetDefaults[7] = 1;
            LocalAnimationSetDefaults[8] = 10;
            LocalImageSetDefaults[8] = 10;
            LocalAnimationSetDefaults[9] = 10;
            LocalImageSetDefaults[8] = 10;
        }
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