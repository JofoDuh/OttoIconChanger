using System.IO;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.ImageConversion;
using static OttoIconChanger.Setting;
using System.Linq;

namespace OttoIconChanger
{
    public static class OttoCustomSprite
    {
        private static int animationIndex = 0;
        private static float lastFrameTime = 0f;

        // Method to load a static image or an animation and assign them to the bundle
        public static void LoadCustomSpriteFromPath(string pathOn, string pathOff)
        {
            if (string.IsNullOrEmpty(pathOn) || string.IsNullOrEmpty(pathOff)) return;

            // Check if the path is a file (for static image) or a directory (for animation)
            if (Directory.Exists(pathOn) && Directory.Exists(pathOff))
            {
                // It's a folder, load the animation
                LoadAnimationFromFolder(pathOn, pathOff);
            }
            else if (File.Exists(pathOn) && File.Exists(pathOff))
            {
                // It's a single file, load the static image
                LoadStaticImageFromFile(pathOn, pathOff);
            }
            else
            {
                Debug.LogError("Invalid path selected.");
            }
        }

        // Method to load a static image from file
        private static void LoadStaticImageFromFile(string pathOn, string pathOff)
        {
            byte[] imageDataOn = File.ReadAllBytes(pathOn);
            byte[] imageDataOff = File.ReadAllBytes(pathOff);
            Texture2D textureOn = new Texture2D(2, 2);
            Texture2D textureOff = new Texture2D(2, 2);
            if (textureOn.LoadImage(imageDataOn) && textureOff.LoadImage(imageDataOff))
            {
                Sprite spriteOn = Sprite.Create(textureOn, new Rect(0, 0, textureOn.width, textureOn.height), new Vector2(0.5f, 0.5f));
                Sprite spriteOff = Sprite.Create(textureOff, new Rect(0, 0, textureOff.width, textureOff.height), new Vector2(0.5f, 0.5f));
                // Assign the sprite to CustomOttoOn and CustomOttoOFF
                BundleLoader.BundleLoader.CustomOttoOn = spriteOn;
                BundleLoader.BundleLoader.CustomOttoOff = spriteOff;
                Main.Logger.Log("Custom static image applied.");
            }
            else
            {
                Main.Logger.Log("Failed to load static image.");
            }
        }

        // Method to load animation frames from folder
        private static void LoadAnimationFromFolder(string folderPathOn, string folderPathOff)
        {
            // Helper function to sort files based on numeric suffix
            string[] SortFilesByNumericSuffix(string[] filePaths)
            {
                return filePaths
                    .OrderBy(path =>
                    {
                        string fileName = Path.GetFileNameWithoutExtension(path);
                        string numericPart = new string(fileName.Reverse().TakeWhile(char.IsDigit).Reverse().ToArray());
                        return int.TryParse(numericPart, out int result) ? result : int.MaxValue;
                    })
                    .ToArray();
            }

            // Load and sort the "On" state animation frames
            string[] imagePathsOn = Directory.GetFiles(folderPathOn, "*.png");
            if (imagePathsOn.Length == 0)
            {
                Main.Logger.Log("No frames found in 'On' folder.");
                return;
            }
            imagePathsOn = SortFilesByNumericSuffix(imagePathsOn);

            Texture2D[] texturesOn = new Texture2D[imagePathsOn.Length];
            for (int i = 0; i < texturesOn.Length; i++)
            {
                byte[] imageDataOn = File.ReadAllBytes(imagePathsOn[i]);
                texturesOn[i] = new Texture2D(2, 2);
                if (!texturesOn[i].LoadImage(imageDataOn))
                {
                    Main.Logger.Log($"Failed to load image: {imagePathsOn[i]}");
                }
                else
                {
                    Main.Logger.Log($"Loaded 'On' frame: {Path.GetFileName(imagePathsOn[i])}");
                }
            }

            Sprite[] animationFramesOn = texturesOn
                .Select(texture => Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f)))
                .ToArray();

            BundleLoader.BundleLoader.CustomAniOttoOn = animationFramesOn;
            Patch.setting.AmountOfFramesOn = animationFramesOn.Length;
            Main.Logger.Log($"Total 'On' frames: {animationFramesOn.Length}");

            // Load and sort the "Off" state animation frames
            string[] imagePathsOff = Directory.GetFiles(folderPathOff, "*.png");
            if (imagePathsOff.Length == 0)
            {
                Main.Logger.Log("No frames found in 'Off' folder.");
                return;
            }
            imagePathsOff = SortFilesByNumericSuffix(imagePathsOff);

            Texture2D[] texturesOff = new Texture2D[imagePathsOff.Length];
            for (int i = 0; i < texturesOff.Length; i++)
            {
                byte[] imageDataOff = File.ReadAllBytes(imagePathsOff[i]);
                texturesOff[i] = new Texture2D(2, 2);
                if (!texturesOff[i].LoadImage(imageDataOff))
                {
                    Main.Logger.Log($"Failed to load image: {imagePathsOff[i]}");
                }
                else
                {
                    Main.Logger.Log($"Loaded 'Off' frame: {Path.GetFileName(imagePathsOff[i])}");
                }
            }

            Sprite[] animationFramesOff = texturesOff
                .Select(texture => Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f)))
                .ToArray();

            BundleLoader.BundleLoader.CustomAniOttoOff = animationFramesOff;
            Patch.setting.AmountOfFramesOff = animationFramesOff.Length;
            Main.Logger.Log($"Total 'Off' frames: {animationFramesOff.Length}");
        }

        public static void LoadImageAnimation(scnEditor __instance)
        {
            // Image validation check
            Image autoImage = __instance.autoImage;
            if (autoImage == null) return;

            Sprite[] activeSprites = null;
            int currentMaxFrames = 0;

            // Check which character is selected
            if (Patch.setting.UseLocalAnimation && Patch.setting.UseLocalImage)
            {
                activeSprites = RDC.auto ? BundleLoader.BundleLoader.CustomAniOttoOn : BundleLoader.BundleLoader.CustomAniOttoOff;
                currentMaxFrames = RDC.auto ? Patch.setting.AmountOfFramesOn : Patch.setting.AmountOfFramesOff;
            }
            else
            {
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
            }
            // Calculate frame interval dynamically
            float frameInterval = Patch.setting.FrameBasedValuesIsEnabled
                ? (Patch.setting.FramesPerSpriteChange > 0 ? 1f / (Patch.setting.FramesPerSecond / Patch.setting.FramesPerSpriteChange) : float.MaxValue)
                : Patch.setting.SecondsPerSpriteChange;

            // Animation logic
            if (activeSprites != null)
            {
                // Use Time.realtimeSinceStartup for consistent time tracking
                if (Time.realtimeSinceStartup - lastFrameTime >= frameInterval)
                {
                    animationIndex = (animationIndex + 1) % currentMaxFrames;
                    lastFrameTime = Time.realtimeSinceStartup; // Update to the current real time
                }
                autoImage.sprite = activeSprites[animationIndex]; // Override the image sprite of Otto
            }
        }

        public static void LoadImage(scnEditor __instance)
        {
            Image autoImage = __instance.autoImage;
            if (autoImage == null) return;

            Sprite selectedSpriteOn = null;
            Sprite selectedSpriteOff = null;

            if (Patch.setting.UseLocalImage)
            {
                selectedSpriteOn = BundleLoader.BundleLoader.CustomOttoOn;
                selectedSpriteOff = BundleLoader.BundleLoader.CustomOttoOff;
            }
            else
            {
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
            }
            if (selectedSpriteOn != null && selectedSpriteOff != null)
            {
                autoImage.sprite = RDC.auto ? selectedSpriteOn : selectedSpriteOff;
            }
        }
    }
}
