using HarmonyLib;
using UnityEngine;

namespace OttoIconChanger
{
    public static class Patch
    {
        public static Setting setting;
        public static RectTransform OttoImage;


        [HarmonyPatch(typeof(scnEditor), "OttoUpdate")]
        public static class OttoUpdate
        {
            public static void Postfix(scnEditor __instance)
            {
                if (setting.CustomeOttoImage)
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
                if (setting.OttoColorChanger || setting.OttoGreyOff)
                {
                    OttoCustomColor.OttoColorChanger(__instance);
                }
                if (setting.OttoOpacityChanger)
                {
                    OttoCustomColor.OttoOpacityChanger(__instance);
                }
            }
            //[HarmonyPatch(typeof(scnEditor), "Awake")]
            //public static class AwakePatch
            //{
            //    public static void Postfix()
            //    {
            //        OttoSizeChanger(setting.OttoSizeChanger);
            //    }
            //}
            //private static void OttoSizeChanger(bool IsEnabled)
            //{

            //    var autoImage = scnEditor.instance.autoImage;
            //    var OttoImageRectTransform = autoImage.GetComponent<RectTransform>();
            //    var OttoButton = autoImage.GetComponentInChildren<Button>();
            //    var OttoButtonRectTransform = OttoButton.GetComponent<RectTransform>();

            //    if (IsEnabled)
            //    {
            //        if (!setting.StoreOriginalValue)
            //        {
            //            setting.originalOttoButtonSize = (OttoButtonRectTransform.sizeDelta.x, OttoButtonRectTransform.sizeDelta.y);
            //            setting.originalOttoSize = (OttoImageRectTransform.sizeDelta.x, OttoImageRectTransform.sizeDelta.y);
            //            setting.StoreOriginalValue = true;
            //        }
            //        OttoButtonRectTransform.sizeDelta = new Vector2(setting.originalOttoButtonSize.Item1 * setting.NewOttoSizeMultiplier,
            //            setting.originalOttoButtonSize.Item2 * setting.NewOttoSizeMultiplier);
            //        OttoImageRectTransform.sizeDelta = new Vector2(setting.originalOttoSize.Item1 * setting.NewOttoSizeMultiplier,
            //            setting.originalOttoSize.Item2 * setting.NewOttoSizeMultiplier);

            //    }
            //    else
            //    {
            //        if (!setting.StoreOriginalValue)
            //            return;
            //        OttoButtonRectTransform.sizeDelta = new Vector2(setting.originalOttoButtonSize.Item1, setting.originalOttoButtonSize.Item2);
            //        OttoImageRectTransform.sizeDelta = new Vector2(setting.originalOttoSize.Item1, setting.originalOttoSize.Item2);
            //    }
            //}
        }
        // Patch to set result of highBPM to false to prevent red Otto
        [HarmonyPatch(typeof(scnEditor), "get_highBPM")]
        public static class HighBPMPatch
        {
            public static void Postfix(ref bool __result)
            {
                setting.ResultForHighBpm = __result ? true : false;
                if (setting.NoNervousOtto)
                {
                    __result = false;
                }
            }
        }
    }
}