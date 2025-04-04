using System.IO;
using UnityEngine;
using static UnityEngine.ImageConversion;
using System.Linq;
using System;
using System.Diagnostics;

namespace OttoIconChanger
{
    public static class PathsLoader
    {
        private static readonly string ffmpegPath = Path.Combine(Main.ModEntry.Path, "ffmpeg.exe");

        // Method to load a static image, animation, or video and assign them to the bundle
        public static void LoadCustomSpriteFromPath(string path, int index, bool isFolder, bool isVideo = false)
        {
            if (isVideo)
            {
                if (File.Exists(path) && File.Exists(ffmpegPath))
                {
                    string outputFolder = Path.Combine(Main.ModEntry.Path, Path.GetFileNameWithoutExtension(path));

                    if (!Directory.Exists(outputFolder))
                    {
                        Directory.CreateDirectory(outputFolder);
                    }

                    // Extract frames using FFmpeg
                    ExtractFramesFromVideo(path, outputFolder);

                    // Load the extracted frames as an animation
                    LoadAnimationFromFolder(outputFolder, index, true);
                }
                else
                {
                    LoadAnimationFromFolder(path, index, false);
                }
            }
            else if (isFolder)
            {
                if (Directory.Exists(path))
                {
                    LoadAnimationFromFolder(path, index, true);
                }
                else
                {
                    LoadAnimationFromFolder(path, index, false);
                }
            }
            else
            {
                if (File.Exists(path))
                {
                    LoadStaticImageFromFile(path, index, true);
                }
                else
                {
                    LoadStaticImageFromFile(path, index, false);
                }
            }
        }
        private static void ExtractFramesFromVideo(string videoPath, string outputFolder)
        {
            string outputPattern = Path.Combine(outputFolder, "frame_%04d.png"); // frame_0001.png, frame_0002.png, etc.

            ProcessStartInfo processInfo = new ProcessStartInfo
            {
                FileName = ffmpegPath,
                Arguments = $"-i \"{videoPath}\" -vf \"fps=30\" \"{outputPattern}\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (Process process = new Process { StartInfo = processInfo })
            {
                process.Start();
                process.WaitForExit();
            }
        }
        private static void LoadStaticImageFromFile(string path, int index, bool Valid)
        {
            if (Valid)
            {
                // Destroy the old sprite if it exists
                if (BundleLoader.BundleLoader.CustomOttoSprites[index] != null)
                {
                    UnityEngine.Object.Destroy(BundleLoader.BundleLoader.CustomOttoSprites[index].texture);
                    UnityEngine.Object.Destroy(BundleLoader.BundleLoader.CustomOttoSprites[index]);
                }

                byte[] imageData = File.ReadAllBytes(path);
                Texture2D texture = new Texture2D(2, 2);
                if (texture.LoadImage(imageData))
                {
                    Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                    BundleLoader.BundleLoader.CustomOttoSprites[index] = sprite;
                }
                else
                {
                    Main.Logger.Log("Failed to load static image.");
                }
            }
            else
            {
                // Destroy the old sprite if invalid
                if (BundleLoader.BundleLoader.CustomOttoSprites[index] != null)
                {
                    UnityEngine.Object.Destroy(BundleLoader.BundleLoader.CustomOttoSprites[index].texture);
                    UnityEngine.Object.Destroy(BundleLoader.BundleLoader.CustomOttoSprites[index]);
                }
                BundleLoader.BundleLoader.CustomOttoSprites[index] = null;
            }
        }


        private static void LoadAnimationFromFolder(string folderPath, int index, bool Valid)
        {
            // Helper function to sort files by numeric suffix
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
                // Destroy old animation sprites
                if (BundleLoader.BundleLoader.CustomAniOttoSprites[index] != null)
                {
                    foreach (var sprite in BundleLoader.BundleLoader.CustomAniOttoSprites[index])
                    {
                        if (sprite != null)
                        {
                            UnityEngine.Object.Destroy(sprite.texture);
                            UnityEngine.Object.Destroy(sprite);
                        }
                    }
                }

                // Load and sort the animation frames
                string[] imagePaths = Directory.GetFiles(folderPath, "*.*")
                    .Where(file => file.EndsWith(".png", StringComparison.OrdinalIgnoreCase) ||
                                   file.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) ||
                                   file.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase))
                    .ToArray();

                if (imagePaths.Length == 0)
                {
                    return;
                }

                imagePaths = SortFilesByNumericSuffix(imagePaths);

                // Create sprites
                Texture2D[] textures = new Texture2D[imagePaths.Length];
                for (int i = 0; i < textures.Length; i++)
                {
                    byte[] imageData = File.ReadAllBytes(imagePaths[i]);
                    textures[i] = new Texture2D(2, 2);
                    if (!textures[i].LoadImage(imageData))
                    {
                        Main.Logger.Log($"Failed to load image: {imagePaths[i]}");
                    }
                }

                Sprite[] animationFrames = textures
                    .Select(texture => Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f)))
                    .ToArray();

                // Assign new animation frames
                BundleLoader.BundleLoader.CustomAniOttoSprites[index] = animationFrames;
                Patch.setting.AmountOfFramesOn = animationFrames.Length;
            }
            else
            {
                // Create placeholder animation if invalid
                if (BundleLoader.BundleLoader.CustomAniOttoSprites[index] != null)
                {
                    foreach (var sprite in BundleLoader.BundleLoader.CustomAniOttoSprites[index])
                    {
                        if (sprite != null)
                        {
                            UnityEngine.Object.Destroy(sprite.texture);
                            UnityEngine.Object.Destroy(sprite);
                        }
                    }
                }

                Texture2D placeholderTexture = new Texture2D(2, 2);
                Sprite placeholderSprite = Sprite.Create(placeholderTexture, new Rect(0, 0, placeholderTexture.width, placeholderTexture.height), new Vector2(0.5f, 0.5f));

                Sprite[] placeholderArray = new Sprite[] { placeholderSprite };
                placeholderArray[0] = null;

                BundleLoader.BundleLoader.CustomAniOttoSprites[index] = placeholderArray;
            }
        }
    }
}