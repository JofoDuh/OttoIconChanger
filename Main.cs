using System;
using System.IO;
using System.Reflection;
using HarmonyLib;
using UnityEngine;
using UnityModManagerNet;
using static OttoIconChanger.Setting;

namespace OttoIconChanger
{
    public static class Main
    {
        public static bool IsEnabled = false;
        public static UnityModManager.ModEntry.ModLogger Logger;
        public static Harmony harmony;
        public static Setting setting;

        //EntryMethod At startup
        internal static void Setup(UnityModManager.ModEntry modEntry)
        {
            //Load settings
            setting = new Setting();
            setting = UnityModManager.ModSettings.Load<Setting>(modEntry);
            Patch.setting = setting;
            Logger = modEntry.Logger;
            modEntry.OnToggle = OnToggle;
        }
        private static bool OnToggle(UnityModManager.ModEntry modEntry, bool value)
        {
            IsEnabled = value;
            if (value)
            {
                //Mod Make
                harmony = new Harmony(modEntry.Info.Id);
                harmony.PatchAll(Assembly.GetExecutingAssembly());

                //Load the Assets
                BundleLoader.BundleLoader.LoadCustomOttoSprite();
                modEntry.OnGUI = OnGUI;
                modEntry.OnSaveGUI = OnSaveGUI;

                // One-time invocation if path is already set and valid
                // Check if folder paths are valid for animation
                if (!string.IsNullOrEmpty(setting.LocalAnimationFolderPathOn) &&
                    !string.IsNullOrEmpty(setting.LocalAnimationFolderPathOff) &&
                    Directory.Exists(setting.LocalAnimationFolderPathOn) &&
                    Directory.Exists(setting.LocalAnimationFolderPathOff))
                {
                    // Apply the folder paths for the selected animation
                    string selectedFolderPathOn = setting.LocalAnimationFolderPathOn;
                    string selectedFolderPathOff = setting.LocalAnimationFolderPathOff;

                    // Load animation sprites or perform any necessary logic
                    OttoCustomSprite.LoadCustomSpriteFromPath(selectedFolderPathOn, selectedFolderPathOff);
                }
                // Check if file paths are valid for static images
                if (!string.IsNullOrEmpty(setting.LocalImagePathOn) &&
                    !string.IsNullOrEmpty(setting.LocalImagePathOff) &&
                    File.Exists(setting.LocalImagePathOn) &&
                    File.Exists(setting.LocalImagePathOff))
                {
                    // Apply the paths for the selected images
                    string selectedImagePathOn = setting.LocalImagePathOn;
                    string selectedImagePathOff = setting.LocalImagePathOff;

                    // Load images or perform any necessary logic
                    OttoCustomSprite.LoadCustomSpriteFromPath(selectedImagePathOn, selectedImagePathOff);
                }
            }
            else
            {
                //Turning off the mod
                harmony.UnpatchAll(modEntry.Info.Id);
            }
            return true;
        }
        private static void OnGUI(UnityModManager.ModEntry modEntry)
        {
            // Mod settings window
            GUILayout.Label("OttoIconChanger");
            setting.NoNervousOttoIsEnabled = GUILayout.Toggle(setting.NoNervousOttoIsEnabled, "No Nervous Otto"); //Toggle to make Otto never nervous

            GUILayout.Space(5); // Add space between sections
            setting.OttoGreyOffIsEnabled = GUILayout.Toggle(setting.OttoGreyOffIsEnabled, "No dark Otto when off"); //Toggle to make Otto dark when off

            GUILayout.Space(5); // Add space between sections

            //Otto Color Changer, many methods are taken from AdofaiTweaks
            Color OttoNewColor;
            string OttoNewHex;
            Color OttoNewColorOn;
            string OttoNewHexOn;
            Color OttoNewColorOff;
            string OttoNewHexOff;
            setting.OttoColorChangerIsEnabled = GUILayout.Toggle(setting.OttoColorChangerIsEnabled, "Otto Color Changer"); 
            if (setting.OttoColorChangerIsEnabled)
            {
                // Begin an indented section
                GUILayout.BeginHorizontal();
                GUILayout.Space(20f); // Add horizontal space for indentation
                GUILayout.BeginVertical(); // Nested layout for content
                if (setting.OttoColorIndependentIsEnabled)
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.BeginVertical();
                    GUILayout.Label("Otto On");
                    OttoNewColorOn = MoreGUILayout.ColorRgbSliders(setting.OttocolorOn);
                    if (setting.OttocolorOn != OttoNewColorOn)
                    {
                        setting.OttocolorOn = OttoNewColorOn;
                    }
                    OttoNewHexOn = MoreGUILayout.NamedTextField("Hex:", setting.OttoColorHexOn, 100f, 40f);
                    if (OttoNewHexOn != setting.OttoColorHexOn && ColorUtility.TryParseHtmlString($"#{OttoNewHexOn}", out OttoNewColorOn))
                    {
                        setting.OttocolorOn = OttoNewColorOn;
                    }
                    setting.OttoColorHexOn = OttoNewHexOn;
                    GUILayout.EndVertical();
                    GUILayout.BeginVertical();
                    GUILayout.Label("Otto Off");
                    OttoNewColorOff = MoreGUILayout.ColorRgbSliders(setting.OttocolorOff);
                    if (setting.OttocolorOff != OttoNewColorOff)
                    {
                        setting.OttocolorOff = OttoNewColorOff;
                    }
                    OttoNewHexOff = MoreGUILayout.NamedTextField("Hex:", setting.OttoColorHexOff, 100f, 40f);
                    if (OttoNewHexOff != setting.OttoColorHexOn && ColorUtility.TryParseHtmlString($"#{OttoNewHexOff}", out OttoNewColorOff))
                    {
                        setting.OttocolorOff = OttoNewColorOff;
                    }
                    setting.OttoColorHexOff = OttoNewHexOff;
                    GUILayout.EndVertical();
                    GUILayout.EndHorizontal();
                }
                else
                {
                    OttoNewColor = MoreGUILayout.ColorRgbSliders(setting.Ottocolor);
                    if (setting.Ottocolor != OttoNewColor)
                    {
                        setting.Ottocolor = OttoNewColor;
                    }
                    OttoNewHex = MoreGUILayout.NamedTextField("Hex:", setting.OttoColorHexOn, 100f, 40f);
                    if (OttoNewHex != setting.OttoColorHexOn && ColorUtility.TryParseHtmlString($"#{OttoNewHex}", out OttoNewColor))
                    {
                        setting.OttocolorOn = OttoNewColor;
                    }
                    setting.OttoColorHexOn = OttoNewHex;
                }
                setting.OttoColorIndependentIsEnabled = GUILayout.Toggle(setting.OttoColorIndependentIsEnabled, "Set Color for On and Off Otto");
                GUILayout.EndVertical();
                GUILayout.EndHorizontal();
            }
            GUILayout.Space(5); // Add space between sections
            setting.OttoOpacityChangerIsEnabled = GUILayout.Toggle(setting.OttoOpacityChangerIsEnabled, "Otto Opacity Changer");
            if (setting.OttoOpacityChangerIsEnabled)
            {
                // Begin an indented section
                GUILayout.BeginHorizontal();
                GUILayout.Space(20f); // Add horizontal space for indentation
                GUILayout.BeginVertical(); // Nested layout for content
                if (!setting.OttoOpacityIndependentIsEnabled)
                {
                    // Create a slider for alpha, with a label "Opacity:" and range from 0 (transparent) to 255 (opaque)
                    float newAlpha = MoreGUILayout.NamedSlider("Opacity:", setting.OttoOpacityValue, 0, 255, 300f, 1, 50f);
                    // Check if the opacity slider value has changed
                    if (setting.OttoOpacityValue != newAlpha)
                    {
                    // Set new opacity value (normalized from 0 to 1) and apply it to the color
                    setting.OttoOpacityValue = newAlpha;
                    }
                    // Get the text from the opacity field as a string
                    string newAlphaText = MoreGUILayout.NamedTextField("A:", setting.OttoOpacityValue.ToString("F0"), 100f, 40f);

                    // Try to parse the text field input to a float
                    if (float.TryParse(newAlphaText, out float parsedAlpha))
                    {
                        // Clamp the parsed value to ensure it's within the 0-255 range
                        parsedAlpha = Mathf.Clamp(parsedAlpha, 0f, 255f);

                        // Update setting if the new value is different
                        if (Mathf.Abs(setting.OttoOpacityValue - parsedAlpha) > 0.01f) // Small tolerance to avoid floating-point precision issues
                        {
                            setting.OttoOpacityValue = parsedAlpha;
                        }
                    }
                }
                if (setting.OttoOpacityIndependentIsEnabled)
                {
                    // If OttoOpacityIndependent is true, handle opacity for each state separately (On / Off)
                    // Use the existing sliders for single opacity values
                    GUILayout.BeginHorizontal();
                    float newOpacityOn = MoreGUILayout.NamedSlider("Opacity Otto On:", setting.OttoOpacityValueOn, 0, 255, 300f, 1, 100f);
                    if (setting.OttoOpacityValueOn != newOpacityOn)
                    {
                        setting.OttoOpacityValueOn = Mathf.Clamp(newOpacityOn, 0f, 255f);
                    }
                    // Same for the Off opacity value
                    float newOpacityOff = MoreGUILayout.NamedSlider("Opacity Otto Off:", setting.OttoOpacityValueOff, 0, 255, 300f, 1, 100f);
                    if (setting.OttoOpacityValueOff != newOpacityOff)
                    {
                        setting.OttoOpacityValueOff = Mathf.Clamp(newOpacityOff, 0f, 255f);
                    }
                    GUILayout.EndHorizontal();
                    // Get the text from the opacity field as a string
                    GUILayout.BeginHorizontal();
                    string newAlphaTextOn = MoreGUILayout.NamedTextField("A:", setting.OttoOpacityValueOn.ToString("F0"), 100f, 40f);
                    // Try to parse the text field input to a float
                    if (float.TryParse(newAlphaTextOn, out float parsedAlphaOn))
                    {
                        // Clamp the parsed value to ensure it's within the 0-255 range
                        parsedAlphaOn = Mathf.Clamp(parsedAlphaOn, 0f, 255f);

                        // Update setting if the new value is different
                        if (Mathf.Abs(setting.OttoOpacityValue - parsedAlphaOn) > 0f) // Small tolerance to avoid floating-point precision issues
                        {
                            setting.OttoOpacityValueOn = parsedAlphaOn;
                        }
                    }
                    // Get the text from the opacity field as a string
                    string newAlphaTextOff = MoreGUILayout.NamedTextField("A:", setting.OttoOpacityValueOff.ToString("F0"), 100f, 40f);
                    // Try to parse the text field input to a float
                    if (float.TryParse(newAlphaTextOff, out float parsedAlphaOff))
                    {
                        // Clamp the parsed value to ensure it's within the 0-255 range
                        parsedAlphaOff = Mathf.Clamp(parsedAlphaOff, 0f, 255f);

                        // Update setting if the new value is different
                        if (Mathf.Abs(setting.OttoOpacityValue - parsedAlphaOff) > 0f) // Small tolerance to avoid floating-point precision issues
                        {
                            setting.OttoOpacityValueOff = parsedAlphaOff;
                        }
                    }
                    GUILayout.EndHorizontal();
                }
                setting.OttoOpacityIndependentIsEnabled = GUILayout.Toggle(setting.OttoOpacityIndependentIsEnabled, "Set Opacity for On and Off Otto");
                GUILayout.EndVertical();
                GUILayout.EndHorizontal();

            }
            GUILayout.Space(5); // Add space between sections
            setting.OttoPosChangerIsEnabled = GUILayout.Toggle(setting.OttoPosChangerIsEnabled, "Otto Position Changer");
            if (setting.OttoPosChangerIsEnabled)
            {
                // Begin an indented section
                GUILayout.BeginHorizontal();
                GUILayout.Space(20f); // Add horizontal space for indentation
                GUILayout.BeginVertical(); // Nested layout for content

                GUILayout.BeginHorizontal();
                float newPosXSlide = MoreGUILayout.NamedSlider("X Pos:", setting.PositionNewX, 0, 500, 300f, 1, 50f);
                if (setting.PositionNewX != newPosXSlide)
                {
                    setting.PositionNewX = newPosXSlide;
                }
                // Same for the Off opacity value
                float newPosYSlide = MoreGUILayout.NamedSlider("Y Pos:", setting.PositionNewY, 0, 500, 300f, 1, 50f);
                if (setting.PositionNewY != newPosYSlide)
                {
                    setting.PositionNewY = newPosYSlide;
                }
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                // Get the text from the opacity field as a string
                string newPosX = MoreGUILayout.NamedTextField("X:", setting.PositionNewX.ToString("F0"), 100f, 20f);
                // Try to parse the text field input to a float
                if (float.TryParse(newPosX, out float parsedPosXNew))
                {
                    // Update setting if the new value is different
                    if (setting.PositionNewX != parsedPosXNew)
                    {
                        setting.PositionNewX = parsedPosXNew;
                    }
                }

                // Get the text from the opacity field as a string
                string newPosY = MoreGUILayout.NamedTextField("Y:", setting.PositionNewY.ToString("F0"), 100f, 20f);
                // Try to parse the text field input to a float
                if (float.TryParse(newPosY, out float parsedPosYNew))
                {
                    // Update setting if the new value is different
                    if (setting.PositionNewY != parsedPosYNew)
                    {
                        setting.PositionNewY = parsedPosYNew;
                    }
                }
                GUILayout.EndHorizontal();
                GUILayout.EndVertical();
                GUILayout.EndHorizontal();
            }
            GUILayout.Space(5); // Add space between sections
            setting.OttoSizeChangerIsEnabled = GUILayout.Toggle(setting.OttoSizeChangerIsEnabled, "Otto Size Changer");
            if (setting.OttoSizeChangerIsEnabled)
            {
                // Begin an indented section
                GUILayout.BeginHorizontal();
                GUILayout.Space(20f); // Add horizontal space for indentation
                GUILayout.BeginVertical(); // Nested layout for content

                GUILayout.BeginHorizontal();
                float newSizeXSlide = MoreGUILayout.NamedSlider("X Size:", setting.NewOttoSizeX, 0, 500, 300f, 1, 50f);
                if (setting.NewOttoSizeX != newSizeXSlide)
                {
                    setting.NewOttoSizeX = newSizeXSlide;
                    if (setting.SquareSizeIsEnabled)
                    {
                        setting.NewOttoSizeY = newSizeXSlide; // Keep Y same as X for a perfect square
                    }
                }
                // Same for the Off opacity value
                float newSizeYSlide = MoreGUILayout.NamedSlider("Y Size:", setting.NewOttoSizeY, 0, 500, 300f, 1, 50f);
                if (setting.NewOttoSizeY != newSizeYSlide)
                {
                    setting.NewOttoSizeY = newSizeYSlide;
                    if (setting.SquareSizeIsEnabled)
                    {
                        setting.NewOttoSizeX = newSizeYSlide; // Keep Y same as X for a perfect square
                    }
                }
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                // Get the text from the X Size field as a string
                string newSizeX = MoreGUILayout.NamedTextField("X:", setting.NewOttoSizeX.ToString("F0"), 100f, 20f);
                // Try to parse the text field input to a float
                if (float.TryParse(newSizeX, out float parsedSizeXNew))
                {
                    // Update X size if the new value is different
                    if (setting.NewOttoSizeX != parsedSizeXNew)
                    {
                        setting.NewOttoSizeX = parsedSizeXNew;

                        if (setting.SquareSizeIsEnabled)
                        {
                            setting.NewOttoSizeY = parsedSizeXNew; // Keep Y same as X for a perfect square
                        }
                    }
                }

                // Get the text from the Y Size field as a string
                string newSizeY = MoreGUILayout.NamedTextField("Y:", setting.NewOttoSizeY.ToString("F0"), 100f, 20f);
                // Try to parse the text field input to a float
                if (float.TryParse(newSizeY, out float parsedSizeYNew))
                {
                    // Update Y size if the new value is different
                    if (setting.NewOttoSizeY != parsedSizeYNew)
                    {
                        setting.NewOttoSizeY = parsedSizeYNew;

                        if (setting.SquareSizeIsEnabled)
                        {
                            setting.NewOttoSizeX = parsedSizeYNew; // Keep X same as Y for a perfect square
                        }
                    }
                }
                GUILayout.EndHorizontal();

                // Add a toggle to enable or disable linking
                setting.SquareSizeIsEnabled = GUILayout.Toggle(setting.SquareSizeIsEnabled, "Link X an Y");
                GUILayout.EndVertical();
                GUILayout.EndHorizontal();
            }
            GUILayout.Space(5); // Add space between sections
            setting.CustomeOttoImageIsEnabled = GUILayout.Toggle(setting.CustomeOttoImageIsEnabled, "Custom Otto Sprites");
            if (setting.CustomeOttoImageIsEnabled)
            {
                // Begin an indented section
                GUILayout.BeginHorizontal();
                GUILayout.Space(20f); // Add horizontal space for indentation
                GUILayout.BeginVertical(); // Nested layout for content
                setting.UseLocalImage = GUILayout.Toggle(setting.UseLocalImage, "Use Local Images");

                // Static Image Selection Section
                if (!setting.UseLocalImage)
                {
                    GUILayout.Label("Non-Animated OttoIcon:");
                    GUILayout.BeginHorizontal();
                    foreach (var character in Enum.GetValues(typeof(OttoCharacter)))
                    {
                        OttoCharacter ottoCharacter = (OttoCharacter)character;

                        // Check if the character is non-animated
                        if (!setting.IsAnimatedCharacter(ottoCharacter))
                        {
                            string ottoCharacterName = ottoCharacter.ToString();
                            switch (ottoCharacterName)
                            {
                                case "FurinaNonAnimated":
                                    ottoCharacterName = "Furina";
                                    break;
                                case "ElysiaNonAnimated":
                                    ottoCharacterName = "Elysia";
                                    break;
                            }
                            if (GUILayout.Button(ottoCharacterName, GUILayout.Width(80f), GUILayout.Height(20f)))
                            {
                                setting.SelectedCharacter = ottoCharacter; // Set the selected character to the clicked one
                            }
                        }
                    }
                    GUILayout.EndHorizontal();

                    GUILayout.Space(5); // Add space between sections
                    GUILayout.Label("Animated OttoIcon:");
                    GUILayout.BeginHorizontal();
                    foreach (var character in Enum.GetValues(typeof(OttoCharacter)))
                    {
                        OttoCharacter ottoCharacter = (OttoCharacter)character;

                        // Check if the character is animated
                        if (setting.IsAnimatedCharacter(ottoCharacter))
                        {
                            string ottoCharacterName = ottoCharacter.ToString();
                            switch (ottoCharacterName)
                            {
                                case "FurinaAnimated":
                                    ottoCharacterName = "Furina";
                                    break;
                                case "SparkleAnimated":
                                    ottoCharacterName = "Sparkle";
                                    break;
                                case "HuTaoAnimated":
                                    ottoCharacterName = "Hu Tao";
                                    break;
                                case "FireFlyAnimated":
                                    ottoCharacterName = "Firefly";
                                    break;
                            }
                            if (GUILayout.Button(ottoCharacterName, GUILayout.Width(80f), GUILayout.Height(20f)))
                            {
                                setting.SelectedCharacter = ottoCharacter; // Set the selected character to the clicked one
                            }
                        }
                    }
                    GUILayout.EndHorizontal();
                }
                else
                {
                    setting.UseLocalAnimation = GUILayout.Toggle(setting.UseLocalAnimation, "Use Animated Images");
                    FileAndFolderPicker picker = new FileAndFolderPicker();

                    if (!setting.UseLocalAnimation)
                    {
                        GUILayout.BeginHorizontal();
                        GUILayout.BeginVertical();
                        // Static Image Path
                        setting.LocalImagePathOn = MoreGUILayout.NamedTextField("Static Otto On Path:", setting.LocalImagePathOn, 300);
                        if (GUILayout.Button("Browse", GUILayout.Width(70f), GUILayout.Height(20f)))
                        {
                            // For file selection:
                            picker.OpenFilePickerForImageOn();
                        }
                        GUILayout.EndVertical();

                        GUILayout.BeginVertical();
                        // Static Image Path
                        setting.LocalImagePathOff = MoreGUILayout.NamedTextField("Static Otto Off Path:", setting.LocalImagePathOff, 300);
                        if (GUILayout.Button("Browse", GUILayout.Width(70f), GUILayout.Height(20f)))
                        {
                            // For file selection:
                            picker.OpenFilePickerForImageOff();
                        }
                        GUILayout.EndVertical();
                        GUILayout.EndHorizontal();
                        // "Apply" Button for Static Image Path Selection
                        if (GUILayout.Button("Apply", GUILayout.Width(60f), GUILayout.Height(20f)))
                        {
                            // Apply the paths for the selected images
                            string selectedImagePathOn = setting.LocalImagePathOn;
                            string selectedImagePathOff = setting.LocalImagePathOff;

                            // Load images or perform any necessary logic
                            OttoCustomSprite.LoadCustomSpriteFromPath(selectedImagePathOn, selectedImagePathOff);
                        }
                    }
                    else
                    {
                        GUILayout.BeginHorizontal();
                        GUILayout.BeginVertical();
                        // Static Image Path
                        Patch.setting.LocalAnimationFolderPathOn = MoreGUILayout.NamedTextField("Otto On Animation Folder Path :", setting.LocalAnimationFolderPathOn, 300);
                        if (GUILayout.Button("Browse", GUILayout.Width(70f), GUILayout.Height(20f)))
                        {
                            // For folder selection:
                            picker.OpenFolderPickerForAnimationOn();
                        }
                        GUILayout.EndVertical();

                        GUILayout.BeginVertical();
                        // Static Image Path
                        Patch.setting.LocalAnimationFolderPathOff = MoreGUILayout.NamedTextField("Otto Off Animation Folder Path:", setting.LocalAnimationFolderPathOff, 300);
                        if (GUILayout.Button("Browse", GUILayout.Width(70f), GUILayout.Height(20f)))
                        {
                            // For folder selection:
                            picker.OpenFolderPickerForAnimationOff();
                        }
                        GUILayout.EndVertical();
                        GUILayout.EndHorizontal();
                        // "Apply" Button for Animation Folder Selection
                        if (GUILayout.Button("Apply", GUILayout.Width(60f), GUILayout.Height(20f)))
                        {
                            // Apply the folder paths for the selected animation
                            string selectedFolderPathOn = setting.LocalAnimationFolderPathOn;
                            string selectedFolderPathOff = setting.LocalAnimationFolderPathOff;

                            // Load animation sprites or perform any necessary logic
                            OttoCustomSprite.LoadCustomSpriteFromPath(selectedFolderPathOn, selectedFolderPathOff);
                        }
                    }
                }

                // Toggle to switch between Seconds-based or Frame-based values
                setting.FrameBasedValuesIsEnabled = GUILayout.Toggle(setting.FrameBasedValuesIsEnabled, "Use Frame-Based Values");
                if (!setting.FrameBasedValuesIsEnabled)
                {
                    // Seconds per Sprite Change
                    string newSecondsText = MoreGUILayout.NamedTextField("Seconds per Sprite Change:", setting.SecondsPerSpriteChange.ToString("F4"),
                    100f, 170f); // Display two decimal places

                    // Parse and update
                    float result = setting.ParseInput(newSecondsText);
                    if (Mathf.Abs(setting.SecondsPerSpriteChange - result) > 0.0001f) // Small tolerance for float comparison
                    {
                        setting.SecondsPerSpriteChange = result;
                    }
                }
                else
                {
                    GUILayout.BeginHorizontal();
                    // Frames per Sprite Change
                    string newSpriteFrame = MoreGUILayout.NamedTextField("Frames per Sprite Change:", setting.FramesPerSpriteChange.ToString("F4"), 100f, 160f);

                    // Parse and update
                    float result = setting.ParseInput(newSpriteFrame);
                    if (Mathf.Abs(setting.FramesPerSpriteChange - result) > 0.0001f)
                    {
                        setting.FramesPerSpriteChange = result;
                    }

                    // Frames per Second
                    string newFramePerSeconds = MoreGUILayout.NamedTextField("Frames per Second:", setting.FramesPerSecond.ToString("F2"), 100f, 120f);

                    // Parse and update
                    float result1 = setting.ParseInput(newFramePerSeconds);
                    if (Mathf.Abs(setting.FramesPerSecond - result1) > 0.0001f)
                    {
                        setting.FramesPerSecond = result1;
                    }
                    GUILayout.EndHorizontal();
                }
                GUILayout.EndVertical();
                GUILayout.EndHorizontal();
            }
        }
        private static void OnSaveGUI(UnityModManager.ModEntry modEntry)
        {
            //Save settings
            setting.Save(modEntry);
        }
    }
}