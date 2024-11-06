using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;
using static OttoIconChanger.Setting;

namespace OttoIconChanger
{
    public static class Patch
    {
        public static Setting setting;
        private static int animationIndex = 0;
        private static float lastFrameTime = 0f;
        private static readonly float frameInterval = 1f / 12f; // 120 FPS base interval

        [HarmonyPatch(typeof(scnEditor), "OttoUpdate")]
        public static class OttoUpdate
        {
            public static void Postfix(scnEditor __instance)
            {
                if (setting.IsAnimatedCharacterSelected()) //If selected character is animted load animation logic else static image logic
                {
                    LoadImageAnimation(__instance);
                }
                else
                {
                    LoadImage(__instance);
                }
            }

            //Animated Otto Change Method
            private static void LoadImageAnimation(scnEditor __instance)
            {
                //Image validation check
                Image autoImage = __instance.autoImage;
                if (autoImage == null) return;

                Sprite[] activeSprites = null;
                int currentMaxFrames = 0;

                //Check which character is slected
                switch (Patch.setting.SelectedCharacter)
                {
                    case OttoCharacter.FireFly:
                        activeSprites = RDC.auto ? BundleLoader.BundleLoader.FireFlyOttoOn : BundleLoader.BundleLoader.FireFlyOttoOff;
                        currentMaxFrames = 12;
                        break;
                    case OttoCharacter.HuTao:
                        activeSprites = RDC.auto ? BundleLoader.BundleLoader.HuTaoOttoOn : BundleLoader.BundleLoader.HuTaoOttoOff;
                        currentMaxFrames = 8;
                        break;
                    case OttoCharacter.Sparkle:
                        activeSprites = RDC.auto ? BundleLoader.BundleLoader.SparkleOttoOn : BundleLoader.BundleLoader.SparkleOttoOff;
                        currentMaxFrames = 12;
                        break;
                    case OttoCharacter.FurinaAnimated:
                        activeSprites = RDC.auto ? BundleLoader.BundleLoader.FurinaAniOttoOn : BundleLoader.BundleLoader.FurinaAniOttoOff;
                        currentMaxFrames = RDC.auto ? 15 : 18;
                        break;
                }

                //Animation logic
                if (activeSprites != null)
                {
                    // Use Time.realtimeSinceStartup for consistent time tracking across mode changes
                    if (Time.realtimeSinceStartup - lastFrameTime >= frameInterval)
                    {
                        animationIndex = (animationIndex + 1) % currentMaxFrames;
                        lastFrameTime = Time.realtimeSinceStartup; // Update to the current real time
                    }
                    autoImage.sprite = activeSprites[animationIndex]; //Overrides the image sprite of Otto
                }
            }
            //Static Image Otto Change Method
            private static void LoadImage(scnEditor __instance)
            {
                Image autoImage = __instance.autoImage;
                if (autoImage == null) return;

                Sprite selectedSpriteOn = null;
                Sprite selectedSpriteOff = null;

                switch (Patch.setting.SelectedCharacter)
                {
                    case OttoCharacter.FurinaNonAni:
                        selectedSpriteOn = BundleLoader.BundleLoader.FurinaOttoOn;
                        selectedSpriteOff = BundleLoader.BundleLoader.FurinaOttoOff;
                        break;
                    case OttoCharacter.Elysia:
                        selectedSpriteOn = BundleLoader.BundleLoader.ElysiaOttoOn;
                        selectedSpriteOff = BundleLoader.BundleLoader.ElysiaOttoOff;
                        break;
                }
                if (selectedSpriteOn != null && selectedSpriteOff != null)
                {
                    autoImage.sprite = RDC.auto ? selectedSpriteOn : selectedSpriteOff;
                }
            }
        }
        // Patch to set result of highBPM to false to prevent red Otto
        [HarmonyPatch(typeof(scnEditor), "get_highBPM")]
        public static class HighBPMPatch
        {
            public static void Postfix(ref bool __result)
            {
                __result = false;
            }
        }
    }
}