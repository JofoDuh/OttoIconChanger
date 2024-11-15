using UnityEngine;
using UnityEngine.UI;
using static OttoIconChanger.Setting;

namespace OttoIconChanger
{
    public static class OttoCustomSprite 
    {
        private static int animationIndex = 0;
        private static float lastFrameTime = 0f;
        //1s = 120 frames
        //Aim = 10 frames per 120 frames
        //120/10 = 12
        //We get 1/12
        private static readonly float frameInterval = 1f / 12f; // 120 FPS base interval
        //Animated Otto Change Method
        public static void LoadImageAnimation(scnEditor __instance)
        {
            //Image validation check
            Image autoImage = __instance.autoImage;
            if (autoImage == null) return;

            Sprite[] activeSprites = null;
            int currentMaxFrames = 0;

            //Check which character is slected
            switch (Patch.setting.SelectedCharacter)
            {
                case OttoCharacter.FireFlyAnimated:
                    activeSprites = RDC.auto ? BundleLoader.BundleLoader.FireFlyOttoOn : BundleLoader.BundleLoader.FireFlyOttoOff;
                    currentMaxFrames = 12;
                    break;
                case OttoCharacter.HuTaoAnimated:
                    activeSprites = RDC.auto ? BundleLoader.BundleLoader.HuTaoOttoOn : BundleLoader.BundleLoader.HuTaoOttoOff;
                    currentMaxFrames = 8;
                    break;
                case OttoCharacter.SparkleAnimated:
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
        public static void LoadImage(scnEditor __instance)
        {
            Image autoImage = __instance.autoImage;
            if (autoImage == null) return;

            Sprite selectedSpriteOn = null;
            Sprite selectedSpriteOff = null;

            switch (Patch.setting.SelectedCharacter)
            {
                case OttoCharacter.FurinaNonAnimated:
                    selectedSpriteOn = BundleLoader.BundleLoader.FurinaOttoOn;
                    selectedSpriteOff = BundleLoader.BundleLoader.FurinaOttoOff;
                    break;
                case OttoCharacter.ElysiaNonAnimated:
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
}
