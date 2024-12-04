using System;
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
            setting.OttoColorChangerIsEnabled = GUILayout.Toggle(setting.OttoColorChangerIsEnabled, "Otto Color Changer"); 
            if (setting.OttoColorChangerIsEnabled)
            {
                // Begin an indented section
                GUILayout.BeginHorizontal();
                GUILayout.Space(20f); // Add horizontal space for indentation
                GUILayout.BeginVertical(); // Nested layout for content

                OttoNewColor = MoreGUILayout.ColorRgbSliders(setting.Ottocolor);
                if (setting.Ottocolor != OttoNewColor)
                {
                    setting.Ottocolor = OttoNewColor;
                }
                OttoNewHex = MoreGUILayout.NamedTextField("Hex:", setting.OttoColorHex, 100f, 40f);
                if (OttoNewHex != setting.OttoColorHex && ColorUtility.TryParseHtmlString($"#{OttoNewHex}", out OttoNewColor))
                {
                    setting.Ottocolor = OttoNewColor;
                }
                setting.OttoColorHex = OttoNewHex;
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
                    float newAlpha = MoreGUILayout.NamedSlider("A:", setting.OttoOpacityValue, 0, 255, 300f, 1, 40f);
                    // Check if the opacity slider value has changed
                    if (setting.OttoOpacityValue != newAlpha)
                    {
                    // Set new opacity value (normalized from 0 to 1) and apply it to the color
                    setting.OttoOpacityValue = newAlpha;
                    }
                    // Get the text from the opacity field as a string
                    string newAlphaText = MoreGUILayout.NamedTextField("Opacity:", setting.OttoOpacityValue.ToString("F0"), 100f, 50f);

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
                    float newOpacityOn = MoreGUILayout.NamedSlider("A:", setting.OttoOpacityValueOn, 0, 255, 300f, 1, 40f);
                    if (setting.OttoOpacityValueOn != newOpacityOn)
                    {
                        setting.OttoOpacityValueOn = Mathf.Clamp(newOpacityOn, 0f, 255f);
                    }
                    // Same for the Off opacity value
                    float newOpacityOff = MoreGUILayout.NamedSlider("A:", setting.OttoOpacityValueOff, 0, 255, 300f, 1, 40f);
                    if (setting.OttoOpacityValueOff != newOpacityOff)
                    {
                        setting.OttoOpacityValueOff = Mathf.Clamp(newOpacityOff, 0f, 255f);
                    }
                    GUILayout.EndHorizontal();
                    // Get the text from the opacity field as a string
                    GUILayout.BeginHorizontal();
                    string newAlphaTextOn = MoreGUILayout.NamedTextField("Opacity Otto On:", setting.OttoOpacityValueOn.ToString("F0"), 100f, 100f);
                    // Try to parse the text field input to a float
                    if (float.TryParse(newAlphaTextOn, out float parsedAlphaOn))
                    {
                        // Clamp the parsed value to ensure it's within the 0-255 range
                        parsedAlphaOn = Mathf.Clamp(parsedAlphaOn, 0f, 255f);

                        // Update setting if the new value is different
                        if (Mathf.Abs(setting.OttoOpacityValue - parsedAlphaOn) > 0.01f) // Small tolerance to avoid floating-point precision issues
                        {
                            setting.OttoOpacityValueOn = parsedAlphaOn;
                        }
                    }
                    // Get the text from the opacity field as a string
                    string newAlphaTextOff = MoreGUILayout.NamedTextField("Opacity Otto Off:", setting.OttoOpacityValueOff.ToString("F0"), 100f, 100f);
                    // Try to parse the text field input to a float
                    if (float.TryParse(newAlphaTextOff, out float parsedAlphaOff))
                    {
                        // Clamp the parsed value to ensure it's within the 0-255 range
                        parsedAlphaOff = Mathf.Clamp(parsedAlphaOff, 0f, 255f);

                        // Update setting if the new value is different
                        if (Mathf.Abs(setting.OttoOpacityValue - parsedAlphaOff) > 0.01f) // Small tolerance to avoid floating-point precision issues
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
                float newPosXSlide = MoreGUILayout.NamedSlider("X:", setting.PositionNewX, 0, 500, 300f, 1, 40f);
                if (setting.PositionNewX != newPosXSlide)
                {
                    setting.PositionNewX = newPosXSlide;
                }
                // Same for the Off opacity value
                float newPosYSlide = MoreGUILayout.NamedSlider("Y:", setting.PositionNewY, 0, 500, 300f, 1, 40f);
                if (setting.PositionNewY != newPosYSlide)
                {
                    setting.PositionNewY = newPosYSlide;
                }
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                // Get the text from the opacity field as a string
                string newPosX = MoreGUILayout.NamedTextField("X Pos:", setting.PositionNewX.ToString("F0"), 100f, 50f);
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
                string newPosY = MoreGUILayout.NamedTextField("Y Pos:", setting.PositionNewY.ToString("F0"), 100f, 50f);
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
                float newSizeXSlide = MoreGUILayout.NamedSlider("X:", setting.NewOttoSizeX, 0, 500, 300f, 1, 40f);
                if (setting.NewOttoSizeX != newSizeXSlide)
                {
                    setting.NewOttoSizeX = newSizeXSlide;
                    if (setting.SquareSizeIsEnabled)
                    {
                        setting.NewOttoSizeY = newSizeXSlide; // Keep Y same as X for a perfect square
                    }
                }
                // Same for the Off opacity value
                float newSizeYSlide = MoreGUILayout.NamedSlider("Y:", setting.NewOttoSizeY, 0, 500, 300f, 1, 40f);
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
                string newSizeX = MoreGUILayout.NamedTextField("X Size:", setting.NewOttoSizeX.ToString("F0"), 100f, 50f);
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
                string newSizeY = MoreGUILayout.NamedTextField("Y Size:", setting.NewOttoSizeY.ToString("F0"), 100f, 50f);
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

                GUILayout.Label("Non-Animated OttoIcon:");
                // Display buttons for non-animated characters
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
                            setting.SelectedCharacter = ottoCharacter;
                        }
                    }
                }
                GUILayout.EndHorizontal();
                GUILayout.Space(5); // Add space between sections
                GUILayout.Label("Animated OttoIcon:");
                // Display buttons for animated characters
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
                            setting.SelectedCharacter = ottoCharacter;
                        }
                    }
                }
                GUILayout.EndHorizontal();
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