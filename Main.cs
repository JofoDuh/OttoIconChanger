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
            setting.NoNervousOtto = GUILayout.Toggle(setting.NoNervousOtto, "No Nervous Otto"); //Toggle to make Otto never nervous

            GUILayout.Space(5); // Add space between sections
            setting.OttoGreyOff = GUILayout.Toggle(setting.OttoGreyOff, "No dark Otto when off"); //Toggle to make Otto dark when off

            GUILayout.Space(5); // Add space between sections

            //Otto Color Changer, many methods are taken from AdofaiTweaks
            Color OttoNewColor;
            string OttoNewHex;
            setting.OttoColorChanger = GUILayout.Toggle(setting.OttoColorChanger, "Otto Color Changer"); 
            if (setting.OttoColorChanger)
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
            setting.OttoOpacityChanger = GUILayout.Toggle(setting.OttoOpacityChanger, "Otto Opacity Changer");
            if (setting.OttoOpacityChanger)
            {
                // Begin an indented section
                GUILayout.BeginHorizontal();
                GUILayout.Space(20f); // Add horizontal space for indentation
                GUILayout.BeginVertical(); // Nested layout for content
                if (!setting.OttoOpacityIndependent)
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
                if (setting.OttoOpacityIndependent)
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
                    if (float.TryParse(newAlphaTextOn, out float parsedAlpha))
                    {
                        // Clamp the parsed value to ensure it's within the 0-255 range
                        parsedAlpha = Mathf.Clamp(parsedAlpha, 0f, 255f);

                        // Update setting if the new value is different
                        if (Mathf.Abs(setting.OttoOpacityValue - parsedAlpha) > 0.01f) // Small tolerance to avoid floating-point precision issues
                        {
                            setting.OttoOpacityValueOn = parsedAlpha;
                        }
                    }
                    // Get the text from the opacity field as a string
                    string newAlphaTextOff = MoreGUILayout.NamedTextField("Opacity Otto Off:", setting.OttoOpacityValueOff.ToString("F0"), 100f, 100f);
                    // Try to parse the text field input to a float
                    if (float.TryParse(newAlphaTextOff, out float parsedAlpha1))
                    {
                        // Clamp the parsed value to ensure it's within the 0-255 range
                        parsedAlpha1 = Mathf.Clamp(parsedAlpha1, 0f, 255f);

                        // Update setting if the new value is different
                        if (Mathf.Abs(setting.OttoOpacityValue - parsedAlpha1) > 0.01f) // Small tolerance to avoid floating-point precision issues
                        {
                            setting.OttoOpacityValueOff = parsedAlpha1;
                        }
                    }
                    GUILayout.EndHorizontal();
                }
                setting.OttoOpacityIndependent = GUILayout.Toggle(setting.OttoOpacityIndependent, "Set Opacity for On and Off Otto");
                GUILayout.EndVertical();
                GUILayout.EndHorizontal();

            }
            //GUILayout.Space(5); // Add space between sections
            //setting.OttoSizeChanger = GUILayout.Toggle(setting.OttoSizeChanger, "Otto Size Changer");
            //if (setting.OttoSizeChanger)
            //{
            //    X Position Input
            //    string newMultipler = MoreGUILayout.NamedTextField("Multiplier:", setting.NewOttoSizeMultiplier.ToString(), 100f, 40f);
            //    float parsedMultipler = setting.ParseInput(newMultipler);
            //    if (Math.Abs(parsedMultipler - setting.NewOttoSizeMultiplier) > Mathf.Epsilon) // Checks if the parsed value is different
            //    {
            //        setting.NewOttoSizeMultiplier = parsedMultipler;
            //    }
            //    if (GUILayout.Button("Set Default"))
            //    {
            //        setting.NewOttoSizeMultiplier = 1f;
            //    }
            //    if (GUILayout.Button("Get Values"))
            //    {

            //        Logger.Log("Hi");
            //        Logger.Log($"Original Position: {setting.originalOttoButtonSize}");
            //        Logger.Log($"Original Position: {setting.originalOttoSize}");
            //    }
            //}

            GUILayout.Space(5); // Add space between sections
            setting.CustomeOttoImage = GUILayout.Toggle(setting.CustomeOttoImage, "Custom Otto Sprites");
            if (setting.CustomeOttoImage)
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