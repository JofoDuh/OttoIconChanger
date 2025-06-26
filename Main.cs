using System.Reflection;
using HarmonyLib;
using UnityEngine;
using UnityModManagerNet;

namespace OttoIconChanger
{
    public static class Main
    {
        public static bool IsEnabled = false;
        public static UnityModManager.ModEntry.ModLogger Logger;
        public static UnityModManager.ModEntry ModEntry;
        public static Harmony harmony;
        public static Setting setting;

        //EntryMethod At startup
        internal static void Setup(UnityModManager.ModEntry modEntry)
        {
            //Load settings        
            ModEntry = modEntry;
            Logger = modEntry.Logger;
            modEntry.OnToggle = OnToggle;
            setting = Setting.Load(modEntry);
            Presets.setting = setting;
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
                Presets.LoadFromPreset();
                Presets.Apply(true);

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
            Presets.PresetsSettings();
            GUILayout.Space(5); // Add space between sections
            setting.HideOttoPlayText = GUILayout.Toggle(setting.HideOttoPlayText, "Hide Autoplay Text");

            GUILayout.Space(5); // Add space between sections
            setting.NoNervousOttoIsEnabled = GUILayout.Toggle(setting.NoNervousOttoIsEnabled, "No Nervous Otto"); //Toggle to make Otto never nervous

            GUILayout.Space(5); // Add space between sections
            setting.OttoGreyOffIsEnabled = GUILayout.Toggle(setting.OttoGreyOffIsEnabled, "No Dark Otto"); //Toggle to make Otto dark when off

            OttoCustomColor.ColorAndOpacitySettings();
            OttoCustomPositionAndSize.PosAndSizeSettings();
            OttoCustomSprite.SpritesSettings();
        }
        private static void OnSaveGUI(UnityModManager.ModEntry modEntry)
        {
            //Save settings
            setting.Save(modEntry);
        }
    }
}