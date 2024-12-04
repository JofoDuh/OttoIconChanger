using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;

namespace OttoIconChanger
{
    public static class Patch
    {
        public static Setting setting;


        [HarmonyPatch(typeof(scnEditor), "OttoUpdate")]
        public static class OttoUpdate
        {
            public static void Postfix(scnEditor __instance)
            {
                if (setting.CustomeOttoImageIsEnabled)
                {
                    if (setting.IsAnimatedCharacterSelected()) //If selected character is animated load animation logic else static image logic
                    {
                        OttoCustomSprite.LoadImageAnimation(__instance);
                    }
                    else
                    {
                        OttoCustomSprite.LoadImage(__instance);
                    }
                }
                if (setting.OttoColorChangerIsEnabled || setting.OttoGreyOffIsEnabled)
                {
                    OttoCustomColor.OttoColorChanger(__instance);
                }
                if (setting.OttoOpacityChangerIsEnabled)
                {
                    OttoCustomColor.OttoOpacityChanger(__instance);
                }
                OttoCustomPositionAndSize.PositionAndSizeChanger(__instance);
            }
        }
        // Patch to set result of highBPM to false to prevent red Otto
        [HarmonyPatch(typeof(scnEditor), "get_highBPM")]
        public static class HighBPMPatch
        {
            public static void Postfix(ref bool __result)
            {
                setting.ResultForHighBpm = __result ? true : false;
                if (setting.NoNervousOttoIsEnabled)
                {
                    __result = false;
                }
            }
        }
        [HarmonyPatch(typeof(scnEditor), "Start")]
        public static class StartPatch
        {
            public static void Postfix()
            {
                bool HaveStoreOriginalValue = false;
                var autoImageRect = scnEditor.instance.autoImage.GetComponent<RectTransform>();
                var buttonRect = scnEditor.instance.autoImage.GetComponentInChildren<Button>()?.GetComponent<RectTransform>();

                // Store original values if not already done
                if (!HaveStoreOriginalValue)
                {
                    setting.originalOttoImageOffsetMin = autoImageRect.offsetMin;
                    setting.originalOttoImageOffsetMax = autoImageRect.offsetMax;
                    setting.originalOttoButtonOffsetMax = buttonRect.offsetMax;
                    setting.originalOttoButtonOffsetMin = buttonRect.offsetMin;
                    HaveStoreOriginalValue = true;
                }
            }
        }
    }
}