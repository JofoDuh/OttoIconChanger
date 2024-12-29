using System.IO;
using UnityEngine;
using static UnityEngine.ImageConversion;
using System.Linq;
using System;

namespace OttoIconChanger
{
    public static class PathsLoader
    {
        // Method to load a static image or an animation and assign them to the bundle
        public static void LoadCustomSpriteFromPath(string path, int index, bool isFolder)
        {
            // Check if the path is a file (for static image) or a directory (for animation)
            if (isFolder)
            {
                if (Directory.Exists(path))
                {
                    // It's a folder, load the animation
                    LoadAnimationFromFolder(path, index, true);
                }
                else
                {
                    // It's a folder, load the animation
                    LoadAnimationFromFolder(path, index, false);
                }
            }
            else
            {
                if (File.Exists(path))
                {
                    // It's a single file, load the static image
                    LoadStaticImageFromFile(path, index, true);
                }
                else
                {
                    // It's a single file, load the static image
                    LoadStaticImageFromFile(path, index, false);
                }
            }

        }

        // Method to load a static image from file
        private static void LoadStaticImageFromFile(string path, int index, bool Valid)
        {
            if (Valid)
            {
                byte[] imageData = File.ReadAllBytes(path);
                Texture2D texture = new Texture2D(2, 2);
                if (texture.LoadImage(imageData))
                {
                    Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                    // Assign the sprite to CustomOtto
                    BundleLoader.BundleLoader.CustomOttoSprites[index] = sprite;
                    //Main.Logger.Log("Custom static image applied.");
                }
                else
                {
                    Main.Logger.Log("Failed to load static image.");
                }
            }
            else
            {
                BundleLoader.BundleLoader.CustomOttoSprites[index] = null;
            }
        }

        // Method to load animation frames from folder
        private static void LoadAnimationFromFolder(string folderPath, int index, bool Valid)
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

            if (Valid)
            {
                // Load and sort the animation frames for multiple image types
                string[] imagePaths = Directory.GetFiles(folderPath, "*.*")
                    .Where(file => file.EndsWith(".png", StringComparison.OrdinalIgnoreCase) ||
                                   file.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) ||
                                   file.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase))
                    .ToArray();

                if (imagePaths.Length == 0)
                {
                    //Main.Logger.Log("No frames found in folder.");
                    return;
                }

                imagePaths = SortFilesByNumericSuffix(imagePaths);

                // Special condition: if only one image exists, create an array with two identical sprites
                if (imagePaths.Length == 1)
                {
                    // Load the single image
                    byte[] imageData = File.ReadAllBytes(imagePaths[0]);
                    Texture2D texture = new Texture2D(2, 2);
                    if (!texture.LoadImage(imageData))
                    {
                        Main.Logger.Log($"Failed to load image: {imagePaths[0]}");
                        return;
                    }
                    Sprite singleSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

                    // Create an array of two identical sprites
                    Sprite[] animationFrames = new Sprite[] { singleSprite, singleSprite };

                    // Assign the array to the custom animation sprites
                    BundleLoader.BundleLoader.CustomAniOttoSprites[index] = animationFrames;
                    Patch.setting.AmountOfFramesOn = animationFrames.Length;
                    //Main.Logger.Log($"Special case: Only one image found, creating an array with two identical frames. Total {Patch.setting.OttoStates[index]} frames: {animationFrames.Length}");
                }
                else
                {
                    // Load multiple images (normal case)
                    Texture2D[] textures = new Texture2D[imagePaths.Length];
                    for (int i = 0; i < textures.Length; i++)
                    {
                        byte[] imageData = File.ReadAllBytes(imagePaths[i]);
                        textures[i] = new Texture2D(2, 2);
                        if (!textures[i].LoadImage(imageData))
                        {
                            //Main.Logger.Log($"Failed to load image: {imagePaths[i]}");
                        }
                    }
                    Sprite[] animationFrames = textures
                        .Select(texture => Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f)))
                        .ToArray();

                    // Assign the array to the custom animation sprites
                    BundleLoader.BundleLoader.CustomAniOttoSprites[index] = animationFrames;
                    Patch.setting.AmountOfFramesOn = animationFrames.Length;
                    //Main.Logger.Log($"Total {Patch.setting.OttoStates[index]} frames: {animationFrames.Length}");
                }
            }
            else
            {
                // Create a placeholder sprite if the animation is not valid
                Texture2D placeholderTexture = new Texture2D(2, 2);
                Sprite placeholderSprite = Sprite.Create(placeholderTexture, new Rect(0, 0, placeholderTexture.width, placeholderTexture.height), new Vector2(0.5f, 0.5f));

                // Add the sprite to an array
                Sprite[] placeholderArray = new Sprite[] { placeholderSprite };

                // Set the first element (the sprite) to null
                placeholderArray[0] = null;

                // Assign the modified array to CustomAniOttoSprites[index]
                BundleLoader.BundleLoader.CustomAniOttoSprites[index] = placeholderArray;
                //Main.Logger.Log($"Total {Patch.setting.OttoStates[index]} frames: {placeholderArray.Length}");
            }
        }

    }
}