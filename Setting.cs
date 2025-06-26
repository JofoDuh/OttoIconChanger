using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;
using Newtonsoft.Json;
using UnityEngine;
using UnityModManagerNet;

namespace OttoIconChanger
{
    public class Setting : UnityModManager.ModSettings
    {
        public Setting()
        {
            InitializePathsStorers();
        }
        //General
        public int FirstTimeLoad = 0;
        public int Browsestate = 0;
        public bool FastMode = true;
        public bool EditorIsAwake = false;
        //Hide Autoplay Text
        public bool HideOttoPlayText = false;
        //No Dark Otto When Off
        public bool OttoGreyOffIsEnabled = false;
        //No Nervous Otto
        public bool NoNervousOttoIsEnabled = false;
        public bool ResultForHighBpm = false;
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
        public Vector2 originalOttoImageSizeDelta;
        public Vector2 originalOttoImageAnchoredPosition;
        public Vector2 originalOttoButtonSizeDelta;
        public float PositionNewX;
        public float PositionNewY;
        public float NewOttoSizeX;
        public float NewOttoSizeY;
        //Custom Otto Sprite
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

        public PathsStorer LocalImage = new PathsStorer(false, Enum.GetNames(typeof(Setting.OttoStates)).Length);
        public PathsStorer LocalAnimation = new PathsStorer(true, Enum.GetNames(typeof(Setting.OttoStates)).Length);

        public string PresetName;
        public bool IsPreset;
        public string CurrentPreset;

        // List to hold multiple DataEntry objects
        public Dictionary<string, PresetStruct> PresetLists = new Dictionary<string, PresetStruct>();
        public PresetStruct TempPreset;

        public int AmountOfFramesOn = 0;
        public int AmountOfFramesOff = 0;

        // Otto States
        public enum OttoStates
        {
            On, 
            Off, 
            LeftOn, 
            LeftOff, 
            RightOn, 
            RightOff,
            NervousOn, 
            NervousOff, 
            Pet, 
            Miss
        }
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

        public SettingOld settingold;

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
        public bool IsVideoFile(string path)
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
                        foreach (string path in preset.Value.LocalAnimation.LocalPaths)
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
       
        public override void Save(UnityModManager.ModEntry modEntry)
        {
            var filepath = GetPath(modEntry);
            try
            {
                var settings = new JsonSerializerSettings
                {
                    Formatting = Newtonsoft.Json.Formatting.Indented,
                    Converters = new List<JsonConverter> { new Vector2Converter(), new ColorConverter() }
                };
                var json = JsonConvert.SerializeObject(this, settings);
                File.WriteAllText(filepath, json);
            }
            catch (Exception e) 
            {
                Main.Logger.Log(e.ToString());
            }
        }

        public override string GetPath(UnityModManager.ModEntry modEntry)
        {
            return Path.Combine(modEntry.Path, GetType().Name + ".json");
        }

        public static Setting Load(UnityModManager.ModEntry modEntry)
        {
            Main.Logger.Log("Loading Settings");
            var jsonPath = Path.Combine(modEntry.Path, typeof(Setting).Name + ".json");
            var xmlPath = Path.Combine(modEntry.Path, typeof(Setting).Name + ".xml");

            var settings = new JsonSerializerSettings
            {
                Formatting = Newtonsoft.Json.Formatting.Indented,
                Converters = new List<JsonConverter> { new Vector2Converter(), new ColorConverter() },
                ObjectCreationHandling = ObjectCreationHandling.Replace
            };

            // Prefer JSON
            if (File.Exists(jsonPath))
            {
                Main.Logger.Log("Loading Json");

                try
                {
                    var json = File.ReadAllText(jsonPath);
                    var setting = JsonConvert.DeserializeObject<Setting>(json, settings) ?? new Setting();
                    setting.InitializePathsStorers();
                    return setting;
                }
                catch (Exception e)
                {
                    Main.Logger.Log("Failed to load JSON settings: " + e);
                }
            }
            else Main.Logger.Log("Json Not Found");

            // Fallback to XML if JSON not found
            if (File.Exists(xmlPath))
            {
                Main.Logger.Log("Loading Xml");
                try
                {
                    SettingOld xmlSetting = new SettingOld();

                    var doc = new XmlDocument();
                    doc.Load(xmlPath);

                    // Rename root element from <Setting> to <SettingOld>
                    if (doc.DocumentElement != null && doc.DocumentElement.Name == "Setting")
                    {
                        var oldRoot = doc.DocumentElement;

                        var newRoot = doc.CreateElement("SettingOld");
                        foreach (XmlAttribute attr in oldRoot.Attributes)
                        {
                            newRoot.Attributes.Append((XmlAttribute)attr.Clone());
                        }

                        foreach (XmlNode child in oldRoot.ChildNodes)
                        {
                            newRoot.AppendChild(child.Clone());
                        }

                        doc.ReplaceChild(newRoot, oldRoot);
                    }

                    // Serialize the XmlDocument to memory and deserialize
                    var serializer = new XmlSerializer(typeof(SettingOld));
                    using (var reader = new StringReader(doc.OuterXml))
                    {
                        xmlSetting = (SettingOld)serializer.Deserialize(reader);
                    }

                    Main.Logger.Log("Xml Loaded, Assigning");
                    Setting jsonSetting = new Setting();
                    jsonSetting.InitializePathsStorers();
                    jsonSetting.settingold = xmlSetting;

                    // Others
                    jsonSetting.FirstTimeLoad = xmlSetting.FirstTimeLoad;
                    jsonSetting.EditorIsAwake = false;
                    jsonSetting.FastMode = xmlSetting.FastMode;
                    jsonSetting.Browsestate = xmlSetting.Browsestate;

                    // No Dark Otto
                    jsonSetting.OttoGreyOffIsEnabled = xmlSetting.OttoGreyOffIsEnabled;

                    // No Nervous Otto
                    jsonSetting.NoNervousOttoIsEnabled = xmlSetting.NoNervousOttoIsEnabled;

                    // Custom Color
                    jsonSetting.OttoColorChangerIsEnabled = xmlSetting.OttoColorChangerIsEnabled;
                    jsonSetting.Ottocolor = xmlSetting.Ottocolor;
                    jsonSetting.OttocolorOff = xmlSetting.OttocolorOff;
                    jsonSetting.OttocolorOn = xmlSetting.OttocolorOn;
                    jsonSetting.OttoColorIndependentIsEnabled = xmlSetting.OttoColorIndependentIsEnabled;

                    // Custom Opacity
                    jsonSetting.OttoOpacityChangerIsEnabled = xmlSetting.OttoOpacityChangerIsEnabled;
                    jsonSetting.OttoOpacityIndependentIsEnabled = xmlSetting.OttoOpacityIndependentIsEnabled;
                    jsonSetting.OttoOpacityValue = xmlSetting.OttoOpacityValue;
                    jsonSetting.OttoOpacityValueOn = xmlSetting.OttoOpacityValueOn;
                    jsonSetting.OttoOpacityValueOff = xmlSetting.OttoOpacityValueOff;

                    //Custom Otto Position & Size
                    jsonSetting.OttoPosChangerIsEnabled = false;
                    jsonSetting.OttoSizeChangerIsEnabled = false;
                    jsonSetting.SquareSizeIsEnabled = true;

                    jsonSetting.PositionNewX = xmlSetting.PositionNewX;
                    jsonSetting.PositionNewY = xmlSetting.PositionNewY;
                    jsonSetting.NewOttoSizeX = xmlSetting.NewOttoSizeX;
                    jsonSetting.NewOttoSizeY = xmlSetting.NewOttoSizeY;

                    // Custom Sprite
                    jsonSetting.CustomOttoImageIsEnabled = xmlSetting.CustomOttoImageIsEnabled;
                    jsonSetting.AmountOfFramesOff = xmlSetting.AmountOfFramesOff;
                    jsonSetting.AmountOfFramesOn = xmlSetting.AmountOfFramesOn;
                    jsonSetting.BlinkDistance = xmlSetting.BlinkDistance;
                    jsonSetting.FramesPerSecond = 120;
                    jsonSetting.FramesPerSpriteChange = 10;
                    jsonSetting.SecondsPerSpriteChange = 10f / 120f;
                    jsonSetting.FrameBasedValuesIsEnabled = false;
                    jsonSetting.SelectedCharacter = xmlSetting.SelectedCharacter;
                    jsonSetting.UseLocalAnimation = xmlSetting.UseLocalAnimation;
                    jsonSetting.UseLocalImage = xmlSetting.UseLocalImage;

                    // Local Animation
                    jsonSetting.LocalAnimation.LocalPaths = xmlSetting.LocalAnimation.LocalPaths;
                    jsonSetting.LocalAnimation.LocalSetDefaults = xmlSetting.LocalAnimation.LocalSetDefaults;
                    jsonSetting.LocalAnimation.LocalToggles = xmlSetting.LocalAnimation.LocalToggles;
                    jsonSetting.LocalAnimation.IsAnimated = xmlSetting.LocalAnimation.Type == 1;

                    // Local Image
                    jsonSetting.LocalImage.LocalPaths = xmlSetting.LocalImage.LocalPaths;
                    jsonSetting.LocalImage.LocalSetDefaults = xmlSetting.LocalImage.LocalSetDefaults;
                    jsonSetting.LocalImage.LocalToggles = xmlSetting.LocalImage.LocalToggles;
                    jsonSetting.LocalImage.IsAnimated = xmlSetting.LocalImage.Type == 0;

                    // Preset
                    jsonSetting.IsPreset = xmlSetting.IsPreset;
                    foreach (var preset in xmlSetting.PresetLists)
                    {
                        var PresetNew = new PresetStruct(jsonSetting);
                        if (preset.Checker == 0)
                        {
                            PresetNew.LocalImage.IsAnimated = false;
                            PresetNew.LocalImage.LocalPaths = preset.Paths;
                            PresetNew.LocalImage.LocalSetDefaults = preset.SetDefaults;
                            PresetNew.LocalImage.LocalToggles = xmlSetting.LocalImage.LocalToggles;
                        }
                        else
                        {
                            PresetNew.LocalAnimation.IsAnimated = true;
                            PresetNew.LocalAnimation.LocalPaths = preset.Paths;
                            PresetNew.LocalAnimation.LocalSetDefaults = preset.SetDefaults;
                            PresetNew.LocalAnimation.LocalToggles = xmlSetting.LocalAnimation.LocalToggles;
                        }
                        string baseName = string.IsNullOrWhiteSpace(preset.Name) ? "NoName" : preset.Name;
                        string uniqueName = baseName;
                        int i = 1;
                        while (jsonSetting.PresetLists.ContainsKey(uniqueName))
                        {
                            uniqueName = $"{baseName} ({i})";
                            i++;
                        }
                        jsonSetting.PresetLists.Add(uniqueName, PresetNew);
                        if (jsonSetting.PresetLists.Count - 1 == xmlSetting.CurrentIndex)
                        {
                            jsonSetting.CurrentPreset = uniqueName;
                        }
                    }
                    Main.Logger.Log("Migrated old XML settings to JSON. Furina!");
                    // Optionally save it as JSON for future loads
                    File.WriteAllText(jsonPath, JsonConvert.SerializeObject(jsonSetting, settings));
                    File.Delete(xmlPath);
                    Main.Logger.Log("Deleted legacy XML settings after migration.");
                    return jsonSetting;

                }
                catch (Exception e)
                {
                    Main.Logger.Log("Failed to load legacy XML settings: " + e);
                }
            }

            // Return fresh settings if both fail
            var fallbackSetting = new Setting();
            fallbackSetting.InitializePathsStorers();
            return fallbackSetting;
        }

        private void InitializePathsStorers()
        {
            int statesCount = Enum.GetNames(typeof(OttoStates)).Length;

            // Initialize with proper size
            if (LocalImage == null)
            {
                LocalImage = new PathsStorer(false, statesCount);
            }
            if (LocalAnimation == null)
            {
                LocalAnimation = new PathsStorer(true, statesCount);
            }

            LocalImage.EnsureProperSize();
            LocalAnimation.EnsureProperSize();
        }
    }

    public class SettingOld : UnityModManager.ModSettings
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
        public bool UseLocalAnimation { get; set; } = false;

        public PathsStorer LocalImage;
        public PathsStorer LocalAnimation;
        public SettingOld()
        {
            LocalImage = new PathsStorer(0, Enum.GetValues(typeof(Setting.OttoStates)).Length);
            LocalAnimation = new PathsStorer(1, Enum.GetValues(typeof(Setting.OttoStates)).Length);
        }

        public string PresetName;
        public bool IsPreset;
        public int CurrentIndex;
        // List to hold multiple DataEntry objects
        public List<PresetList> PresetLists = new List<PresetList>();

        public int AmountOfFramesOn = 0;
        public int AmountOfFramesOff = 0;

        //Set default character to Furina non animated ver.
        public Setting.OttoCharacter SelectedCharacter = Setting.OttoCharacter.FurinaNonAnimated;

        //Parse method for floats to remove non intergers
        public void PresetListInitializer(PresetList list)
        {
            while (list.SetDefaults.Count < Enum.GetValues(typeof(Setting.OttoStates)).Length
                || list.Paths.Count < Enum.GetValues(typeof(Setting.OttoStates)).Length)
            {
                if (list.SetDefaults.Count < Enum.GetValues(typeof(Setting.OttoStates)).Length)
                {
                    list.SetDefaults.Add(0);
                }
                if (list.Paths.Count < Enum.GetValues(typeof(Setting.OttoStates)).Length)
                {
                    list.Paths.Add(string.Empty);
                }
            }
        }
        public override string GetPath(UnityModManager.ModEntry modEntry)
        {
            return Path.Combine(modEntry.Path, GetType().Name + ".xml");
        }
        public class PresetList
        {
            public string Name { get; set; }
            public List<int> SetDefaults { get; set; }
            public List<string> Paths { get; set; }
            public int Checker { get; set; } // Can only be 0 or 1

            // **Parameterless constructor is required for XML serialization**
            public PresetList()
            {
                Name = string.Empty;
                SetDefaults = new List<int>();
                Paths = new List<string>();
                Checker = 0;
            }

            // Constructor for easier initialization
            public PresetList(string name) : this() // Calls the parameterless constructor first
            {
                Name = name;
            }
        }

        public class PathsStorer
        {
            public int Type { get; set; } //0 for Image, 1 for Animated
            public List<string> LocalPaths { get; set; }
            public List<int> LocalSetDefaults { get; set; }
            public List<bool> LocalToggles { get; set; }

            public PathsStorer()
            {
                LocalPaths = new List<string>();
                LocalSetDefaults = new List<int>();
                LocalToggles = new List<bool>();
                Type = 0;
            }

            public PathsStorer(int type, int Count) : this()
            {
                Type = type;
                InitList(Count);
                SetDefaultListValues();
            }

            //Initializes the lists
            public void InitList(int targetCount)
            {
                while (LocalPaths.Count < targetCount || LocalSetDefaults.Count < targetCount || LocalToggles.Count < targetCount)
                {
                    if (LocalPaths.Count < targetCount)
                        LocalPaths.Add(string.Empty);

                    if (LocalToggles.Count < targetCount)
                        LocalToggles.Add(false);

                    if (LocalSetDefaults.Count < targetCount)
                        LocalSetDefaults.Add(0);
                }
            }

            //Method to set the Default States of state to specific ones
            public void SetDefaultListValues()
            {
                if (LocalSetDefaults.Count < 10) return;
                LocalSetDefaults[0] = 10;
                LocalSetDefaults[1] = 10;
                LocalSetDefaults[2] = 0;
                LocalSetDefaults[3] = 1;
                LocalSetDefaults[4] = 0;
                LocalSetDefaults[5] = 1;
                LocalSetDefaults[6] = 0;
                LocalSetDefaults[7] = 1;
                LocalSetDefaults[8] = 1;
                LocalSetDefaults[9] = 1;
            }
        }
    }
}
