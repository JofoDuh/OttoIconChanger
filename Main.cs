using System;
using System.Reflection;
using System.Runtime;
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
            if (setting.OttoColorHex == "")
            {
                setting.OttoColorHex = "FFFFFF";
            }
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

            Color OttoNewColor;
            string OttoNewHex;
            GUILayout.Space(5); // Add space between sections
            setting.OttoColorChanger = GUILayout.Toggle(setting.OttoColorChanger, "Otto Color Changer"); //Otto Color Changer, many methods are taken from AdofaiTweaks
            if (setting.OttoColorChanger)
            {
                OttoNewColor = MoreGUILayout.ColorRgbSliders(setting.Ottocolor);
                if (setting.Ottocolor != OttoNewColor)
                {
                    setting.Ottocolor = OttoNewColor;
                }
                OttoNewHex = MoreGUILayout.NamedTextField("Hex:", setting.OttoColorHex, 100f, 40f);
                if (OttoNewHex != setting.OttoColorHex
                    && ColorUtility.TryParseHtmlString($"#{OttoNewHex}", out OttoNewColor))
                {
                    setting.Ottocolor = OttoNewColor;
                }
            }

            GUILayout.Space(5); // Add space between sections
            setting.CustomeOttoImage = GUILayout.Toggle(setting.CustomeOttoImage, "Custom Otto Sprites");
            if (setting.CustomeOttoImage)
            {
                GUILayout.Label("Non-Animated OttoIcon:");
                // Display buttons for non-animated characters
                foreach (var character in Enum.GetValues(typeof(OttoCharacter)))
                {
                    OttoCharacter ottoCharacter = (OttoCharacter)character;

                    // Check if the character is non-animated
                    if (!setting.IsAnimatedCharacter(ottoCharacter))
                    {
                        if (GUILayout.Button(ottoCharacter.ToString()))
                        {
                            setting.SelectedCharacter = ottoCharacter;
                        }
                    }
                }
                GUILayout.Space(5); // Add space between sections
                GUILayout.Label("Animated OttoIcon:");

                // Display buttons for animated characters
                foreach (var character in Enum.GetValues(typeof(OttoCharacter)))
                {
                    OttoCharacter ottoCharacter = (OttoCharacter)character;

                    // Check if the character is animated
                    if (setting.IsAnimatedCharacter(ottoCharacter))
                    {
                        if (GUILayout.Button(ottoCharacter.ToString()))
                        {
                            setting.SelectedCharacter = ottoCharacter;
                        }
                    }
                }
            }
        }
        private static void OnSaveGUI(UnityModManager.ModEntry modEntry)
        {
            //Save settings
            setting.Save(modEntry);
        }
    }
}