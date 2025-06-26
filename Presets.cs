using System;
using UnityEngine;

namespace OttoIconChanger
{
    public static class Presets
    {
        private static bool DupeName = false;
        private static bool EmptyName = false;
        public static Setting setting;
        public static void PresetsSettings()
        {
            //Preset Label
            GUILayout.Label("Presets");

            if (DupeName) GUILayout.Label("Name Already Exists!");
            if (EmptyName) GUILayout.Label("Name Can't Be Empty!");

            //Save Preset Button
            GUILayout.BeginHorizontal();

            if(GUILayout.Button("Create Default Preset", GUILayout.Width(150f)))
            {
                if (CheckDupeName(setting.PresetName)) return;
                // Create Preset
                setting.PresetLists.Add(Main.setting.PresetName, new PresetStruct());
                setting.PresetName = string.Empty;
            }
            if (GUILayout.Button("Save Preset", GUILayout.Width(90f)))
            {
                if (CheckDupeName(Main.setting.PresetName)) return;
                // Create Preset and assign Name based off of Preset Name
                setting.PresetLists.Add(Main.setting.PresetName, new PresetStruct(Main.setting));
                setting.PresetName = string.Empty;
            }

            //Inputtable Preset Name Field
            Main.setting.PresetName = MoreGUILayout.NamedTextField("Preset Name:", Main.setting.PresetName, 100f, 80f);

            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            //Delete Preset Button & Selected Preset Label
            if (Main.setting.PresetLists.Count > 0)
            {
                if (Main.setting.IsPreset)
                {
                    if (GUILayout.Button("Update Setting", GUILayout.Width(130f)))
                    {
                        var setting = Main.setting;
                        var preset = setting.PresetLists[setting.CurrentPreset];

                        //No Dark Otto When Off
                        preset.OttoGreyOffIsEnabled = setting.OttoGreyOffIsEnabled;

                        //No Nervous Otto
                        preset.NoNervousOttoIsEnabled = setting.NoNervousOttoIsEnabled;

                        //Color Changer
                        preset.OttoColorChangerIsEnabled = setting.OttoColorChangerIsEnabled;
                        preset.OttoColorIndependentIsEnabled = setting.OttoColorIndependentIsEnabled;
                        preset.Ottocolor = setting.Ottocolor;
                        preset.OttocolorOn = setting.OttocolorOn;
                        preset.OttocolorOff = setting.OttocolorOff;

                        //Opacity Changer
                        preset.OttoOpacityChangerIsEnabled = setting.OttoOpacityChangerIsEnabled;
                        preset.OttoOpacityIndependentIsEnabled = setting.OttoOpacityIndependentIsEnabled;
                        preset.OttoOpacityValue = setting.OttoOpacityValue;
                        preset.OttoOpacityValueOn = setting.OttoOpacityValueOn;
                        preset.OttoOpacityValueOff = setting.OttoOpacityValueOff;

                        //Custom Otto Position & Size
                        preset.OttoPosChangerIsEnabled = setting.OttoPosChangerIsEnabled;
                        preset.OttoSizeChangerIsEnabled = setting.OttoSizeChangerIsEnabled;
                        preset.SquareSizeIsEnabled = setting.SquareSizeIsEnabled;
                        preset.PositionNewX = setting.PositionNewX;
                        preset.PositionNewY = setting.PositionNewY;
                        preset.NewOttoSizeX = setting.NewOttoSizeX;
                        preset.NewOttoSizeY = setting.NewOttoSizeY;

                        //Custom Otto Sprite
                        preset.CustomOttoImageIsEnabled = setting.CustomOttoImageIsEnabled;
                        preset.indexCheck = setting.indexCheck;
                        preset.FramesPerSecond = setting.FramesPerSecond;
                        preset.FramesPerSpriteChange = setting.FramesPerSpriteChange;
                        preset.SecondsPerSpriteChange = setting.SecondsPerSpriteChange;
                        preset.FrameBasedValuesIsEnabled = setting.FrameBasedValuesIsEnabled;
                        preset.BlinkDistance = setting.BlinkDistance;

                        //Custom Local Otto Sprite
                        preset.UseLocalImage = setting.UseLocalImage;
                        preset.UseLocalAnimation = setting.UseLocalAnimation;

                        preset.SelectedCharacter = setting.SelectedCharacter;

                        preset.LocalImage = setting.LocalImage;
                        preset.LocalAnimation = setting.LocalAnimation;
                    }

                    if (GUILayout.Button("Unselect Preset", GUILayout.Width(130f))) 
                    {
                        Main.setting.CurrentPreset = string.Empty;
                        Main.setting.IsPreset = false; //Deactivate Preset mode
                        PutBackTemp();
                    }
                    if (GUILayout.Button("Delete Preset", GUILayout.Width(120f)))
                    {
                        Main.setting.PresetLists.Remove(Main.setting.CurrentPreset); //Remove Preset upon click
                        Main.setting.IsPreset = false; //Deactivate Preset mode
                        Main.setting.CurrentPreset = string.Empty;
                        PutBackTemp();
                    }
                }
                GUILayout.Label($"Selected Preset: {(!Main.setting.IsPreset  ? "None" : Main.setting.CurrentPreset)}",
                    GUILayout.Width(200f));
            }
            else
            {
                GUILayout.Label("No Presets Available");
            }
            GUILayout.EndHorizontal();

            //List all available presets as button and load preset upon click
            GUILayout.BeginHorizontal();
            foreach (var preset in Main.setting.PresetLists)
            {
                if (GUILayout.Button(preset.Key, GUILayout.Width(120f)))
                {
                    SaveTemp();
                    Main.setting.IsPreset = true; //Activate Preset mode
                    Main.setting.CurrentPreset = preset.Key; //Assign current Preset index to general variable
                    LoadFromPreset();
                }
            }
            GUILayout.EndHorizontal();
        }
        private static bool CheckDupeName(string nameToCheck)
        {
            // First check: empty or null name
            if (string.IsNullOrWhiteSpace(nameToCheck))
            {
                EmptyName = true;
                DupeName = false;
                return true;
            }

            // Second check: if the name already exists in the dictionary
            if (Main.setting.PresetLists.ContainsKey(nameToCheck))
            {
                EmptyName = false;
                DupeName = true;
                return true;
            }

            // No issues
            EmptyName = false;
            DupeName = false;
            return false;
        }

        public static void Apply(bool LoadBoth = false)
        {
            int index;
            if ((setting.UseLocalImage && !setting.UseLocalAnimation) || LoadBoth)
            {
                // Check if file paths are valid for static images
                // Apply the paths for the selected images
                index = 0;
                string[] selectedImagePaths = new string[10];

                for (int i = 0; i < 10; i++)
                {
                    selectedImagePaths[i] = setting.LocalImage.LocalPaths[i];
                }

                // Load images or perform any necessary logic
                foreach (string imagePath in selectedImagePaths)
                {
                    PathsLoader.LoadCustomSpriteFromPath(imagePath, index, false);
                    index++;
                }
            }
            if ((setting.UseLocalAnimation && setting.UseLocalImage) || LoadBoth)
            {
                index = 0;
                // Check if folder paths are valid for animation
                // Apply the folder paths for the selected animation
                string[] selectedFolderPaths = new string[10];

                for (int i = 0; i < 10; i++)
                {
                    selectedFolderPaths[i] = setting.LocalAnimation.LocalPaths[i];
                }

                // Load animation sprites or perform any necessary logic
                foreach (string folderPath in selectedFolderPaths)
                {
                    bool isVideo = setting.IsVideoFile(folderPath);

                    PathsLoader.LoadCustomSpriteFromPath(folderPath, index, true, isVideo);
                    index++;
                }
            }
            setting.FreeSpace(setting.FastMode);
        }

        public static void SaveTemp()
        {
            if (setting.IsPreset) return;
            setting.TempPreset = new PresetStruct(setting);
        }
        public static void LoadFromPreset()
        {
            if (!setting.IsPreset) return;

            var preset = setting.PresetLists[setting.CurrentPreset];
            //No Dark Otto When Off
            setting.OttoGreyOffIsEnabled = setting.PresetLists[setting.CurrentPreset].OttoGreyOffIsEnabled;

            //No Nervous Otto
            setting.NoNervousOttoIsEnabled = setting.PresetLists[setting.CurrentPreset].NoNervousOttoIsEnabled;

            //Color Changer
            setting.OttoColorChangerIsEnabled = setting.PresetLists[setting.CurrentPreset].OttoColorChangerIsEnabled;
            setting.OttoColorIndependentIsEnabled = setting.PresetLists[setting.CurrentPreset].OttoColorIndependentIsEnabled;
            setting.Ottocolor = setting.PresetLists[setting.CurrentPreset].Ottocolor;
            setting.OttocolorOn = setting.PresetLists[setting.CurrentPreset].OttocolorOn;
            setting.OttocolorOff = setting.PresetLists[setting.CurrentPreset].OttocolorOff;

            //Opacity Changer
            setting.OttoOpacityChangerIsEnabled = setting.PresetLists[setting.CurrentPreset].OttoOpacityChangerIsEnabled;
            setting.OttoOpacityIndependentIsEnabled = setting.PresetLists[setting.CurrentPreset].OttoOpacityIndependentIsEnabled;
            setting.OttoOpacityValue = setting.PresetLists[setting.CurrentPreset].OttoOpacityValue;
            setting.OttoOpacityValueOn = setting.PresetLists[setting.CurrentPreset].OttoOpacityValueOn;
            setting.OttoOpacityValueOff = setting.PresetLists[setting.CurrentPreset].OttoOpacityValueOff;

            //Custom Otto Position & Size
            setting.OttoPosChangerIsEnabled = setting.PresetLists[setting.CurrentPreset].OttoPosChangerIsEnabled;
            setting.OttoSizeChangerIsEnabled = setting.PresetLists[setting.CurrentPreset].OttoSizeChangerIsEnabled;
            setting.SquareSizeIsEnabled = setting.PresetLists[setting.CurrentPreset].SquareSizeIsEnabled;
            setting.PositionNewX = setting.PresetLists[setting.CurrentPreset].PositionNewX;
            setting.PositionNewY = setting.PresetLists[setting.CurrentPreset].PositionNewY;
            setting.NewOttoSizeX = setting.PresetLists[setting.CurrentPreset].NewOttoSizeX;
            setting.NewOttoSizeY = setting.PresetLists[setting.CurrentPreset].NewOttoSizeY;

            //Custom Otto Sprite
            setting.CustomOttoImageIsEnabled = setting.PresetLists[setting.CurrentPreset].CustomOttoImageIsEnabled;
            setting.indexCheck = setting.PresetLists[setting.CurrentPreset].indexCheck;
            setting.FramesPerSecond = setting.PresetLists[setting.CurrentPreset].FramesPerSecond;
            setting.FramesPerSpriteChange = setting.PresetLists[setting.CurrentPreset].FramesPerSpriteChange;
            setting.SecondsPerSpriteChange = setting.PresetLists[setting.CurrentPreset].SecondsPerSpriteChange;
            setting.FrameBasedValuesIsEnabled = setting.PresetLists[setting.CurrentPreset].FrameBasedValuesIsEnabled;
            setting.BlinkDistance = setting.PresetLists[setting.CurrentPreset].BlinkDistance;

            //Custom Local Otto Sprite
            setting.UseLocalImage = setting.PresetLists[setting.CurrentPreset].UseLocalImage; // Toggle between built-in and local images
            setting.UseLocalAnimation = setting.PresetLists[setting.CurrentPreset].UseLocalAnimation;

            setting.LocalImage = setting.PresetLists[setting.CurrentPreset].LocalImage;
            setting.LocalAnimation = setting.PresetLists[setting.CurrentPreset].LocalAnimation;

            setting.SelectedCharacter = setting.PresetLists[setting.CurrentPreset].SelectedCharacter;
            Apply();
        }

        public static void PutBackTemp()
        {
            if (setting.IsPreset) setting.IsPreset = false;
            if (setting.TempPreset == null) return;
            //No Dark Otto When Off
            setting.OttoGreyOffIsEnabled = setting.TempPreset.OttoGreyOffIsEnabled;

            //No Nervous Otto
            setting.NoNervousOttoIsEnabled = setting.TempPreset.NoNervousOttoIsEnabled;

            //Color Changer
            setting.OttoColorChangerIsEnabled = setting.TempPreset.OttoColorChangerIsEnabled;
            setting.OttoColorIndependentIsEnabled = setting.TempPreset.OttoColorIndependentIsEnabled;
            setting.Ottocolor = setting.TempPreset.Ottocolor;
            setting.OttocolorOn = setting.TempPreset.OttocolorOn;
            setting.OttocolorOff = setting.TempPreset.OttocolorOff;


            //Opacity Changer
            setting.OttoOpacityChangerIsEnabled = setting.TempPreset.OttoOpacityChangerIsEnabled;
            setting.OttoOpacityIndependentIsEnabled = setting.TempPreset.OttoOpacityIndependentIsEnabled;
            setting.OttoOpacityValue = setting.TempPreset.OttoOpacityValue;
            setting.OttoOpacityValueOn = setting.TempPreset.OttoOpacityValueOn;
            setting.OttoOpacityValueOff = setting.TempPreset.OttoOpacityValueOff;

            //Custom Otto Position & Size
            setting.OttoPosChangerIsEnabled = setting.TempPreset.OttoPosChangerIsEnabled;
            setting.OttoSizeChangerIsEnabled = setting.TempPreset.OttoSizeChangerIsEnabled;
            setting.SquareSizeIsEnabled = setting.TempPreset.SquareSizeIsEnabled;
            setting.PositionNewX = setting.TempPreset.PositionNewX;
            setting.PositionNewY = setting.TempPreset.PositionNewY;
            setting.NewOttoSizeX = setting.TempPreset.NewOttoSizeX;
            setting.NewOttoSizeY = setting.TempPreset.NewOttoSizeY;

            //Custom Otto Sprite
            setting.CustomOttoImageIsEnabled = setting.TempPreset.CustomOttoImageIsEnabled;
            setting.indexCheck = setting.TempPreset.indexCheck;
            setting.FramesPerSecond = setting.TempPreset.FramesPerSecond;
            setting.FramesPerSpriteChange = setting.TempPreset.FramesPerSpriteChange;
            setting.SecondsPerSpriteChange = setting.TempPreset.SecondsPerSpriteChange;
            setting.FrameBasedValuesIsEnabled = setting.TempPreset.FrameBasedValuesIsEnabled;
            setting.BlinkDistance = setting.TempPreset.BlinkDistance;

            //Custom Local Otto Sprite
            setting.UseLocalImage = setting.TempPreset.UseLocalImage; // Toggle between built-in and local images
            setting.UseLocalAnimation = setting.TempPreset.UseLocalAnimation;

            setting.LocalImage = setting.TempPreset.LocalImage;
            setting.LocalAnimation = setting.TempPreset.LocalAnimation;

            setting.SelectedCharacter = setting.TempPreset.SelectedCharacter;

            setting.IsPreset = false;
            Apply();
        }
    }

    public class PresetStruct
    {
        //Hide Autoplay Text
        public bool HideOttoPlayText = false;

        //No Dark Otto When Off
        public bool OttoGreyOffIsEnabled = false;

        //No Nervous Otto
        public bool NoNervousOttoIsEnabled = false;

        //Color Changer
        public bool OttoColorChangerIsEnabled = false;
        public bool OttoColorIndependentIsEnabled = false;
        //From AdofaiTweaks
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
        public float PositionNewX;
        public float PositionNewY;
        public float NewOttoSizeX;
        public float NewOttoSizeY;

        //Custom Otto Sprite
        public bool CustomOttoImageIsEnabled = false;
        public int indexCheck = 0;
        public float FramesPerSecond = 120;
        public float FramesPerSpriteChange = 10;
        public float SecondsPerSpriteChange = 10f / 120f;
        public bool FrameBasedValuesIsEnabled = false;
        public float BlinkDistance = 50;

        //Custom Local Otto Sprite
        public bool UseLocalImage = true; // Toggle between built-in and local images
        public bool UseLocalAnimation = false;

        public PathsStorer LocalImage = new PathsStorer(false, Enum.GetNames(typeof(Setting.OttoStates)).Length);
        public PathsStorer LocalAnimation = new PathsStorer(true, Enum.GetNames(typeof(Setting.OttoStates)).Length);

        public Setting.OttoCharacter SelectedCharacter = Setting.OttoCharacter.FurinaNonAnimated;

        public PresetStruct() { }
        public PresetStruct (Setting setting)
        {
            // Custom Sprite

            UseLocalAnimation = setting.UseLocalAnimation;
            UseLocalImage = setting.UseLocalImage;

            LocalAnimation = setting.LocalAnimation?.Clone() ?? new PathsStorer(true, Enum.GetNames(typeof(Setting.OttoStates)).Length);
            LocalImage = setting.LocalImage?.Clone() ?? new PathsStorer(false, Enum.GetNames(typeof(Setting.OttoStates)).Length);

            SelectedCharacter = setting.SelectedCharacter;

            BlinkDistance = setting.BlinkDistance;
            CustomOttoImageIsEnabled = setting.CustomOttoImageIsEnabled;
            indexCheck = setting.indexCheck;
            FramesPerSecond = setting.FramesPerSecond;
            FramesPerSpriteChange = setting.FramesPerSpriteChange;
            SecondsPerSpriteChange = setting.SecondsPerSpriteChange;
            FrameBasedValuesIsEnabled = setting.FrameBasedValuesIsEnabled;

            // Opacity
            OttoOpacityChangerIsEnabled = setting.OttoOpacityChangerIsEnabled;
            OttoOpacityIndependentIsEnabled = setting.OttoOpacityIndependentIsEnabled;
            OttoOpacityValue = setting.OttoOpacityValue;
            OttoOpacityValueOn = setting.OttoOpacityValueOn;
            OttoOpacityValueOff = setting.OttoOpacityValueOff;

            // No Nervous
            NoNervousOttoIsEnabled = setting.NoNervousOttoIsEnabled;

            // No Dark Otto
            OttoGreyOffIsEnabled = setting.OttoGreyOffIsEnabled;

            // Hide Text
            HideOttoPlayText = setting.HideOttoPlayText;

            // Color
            OttoColorChangerIsEnabled = setting.OttoColorChangerIsEnabled;
            OttoColorIndependentIsEnabled = setting.OttoColorIndependentIsEnabled;
            Ottocolor = setting.Ottocolor;
            OttocolorOff = setting.OttocolorOff;
            OttocolorOn = setting.OttocolorOn;

            // Position & Size
            OttoPosChangerIsEnabled = setting.OttoPosChangerIsEnabled;
            OttoSizeChangerIsEnabled = setting.OttoSizeChangerIsEnabled;
            SquareSizeIsEnabled = setting.SquareSizeIsEnabled;
            NewOttoSizeX = setting.NewOttoSizeX;
            NewOttoSizeY = setting.NewOttoSizeY;
            PositionNewX = setting.PositionNewX;
            PositionNewY = setting.PositionNewY;
        }
    }
}