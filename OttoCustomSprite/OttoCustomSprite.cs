using SA.GoogleDoc;
using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using static OttoIconChanger.Setting;

namespace OttoIconChanger
{
    public static class OttoCustomSprite
    {
        public static void SpritesSettings()
        {
            GUILayout.Space(5); // Add space between sections
            //Custom Otto Sprites

            Main.setting.CustomOttoImageIsEnabled = GUILayout.Toggle(Main.setting.CustomOttoImageIsEnabled, "Custom Otto Sprites");
            if (Main.setting.CustomOttoImageIsEnabled)
            {
                // Begin an indented section
                GUILayout.BeginHorizontal();
                GUILayout.Space(20f); // Add horizontal space for indentation
                GUILayout.BeginVertical(); // Nested layout for content

                //Public Use:
                Main.setting.UseLocalImage = true;
                //Prevents the "On" paths from being loaded even when manually set in setting.xml 
                int[] indicesToClear = { 0, 2, 4, 6 };

                bool pathsAreNonEmpty;
                do
                {
                    pathsAreNonEmpty = false;

                    foreach (int index in indicesToClear)
                    {
                        if (Main.setting.LocalAnimationFolderPaths[index] != string.Empty || Main.setting.LocalImagePaths[index] != string.Empty)
                        {
                            Main.setting.LocalAnimationFolderPaths[index] = string.Empty;
                            Main.setting.LocalImagePaths[index] = string.Empty;
                            pathsAreNonEmpty = true; // At least one path was non-empty
                        }
                    }

                    if (pathsAreNonEmpty)
                    {
                        Main.setting.Apply(true);
                    }

                } while (pathsAreNonEmpty);

                //Friend Use:
                //Main.setting.UseLocalImage = GUILayout.Toggle(Main.setting.UseLocalImage, "Use Local Images");

                // Static Image Selection Section
                if (!Main.setting.UseLocalImage)
                {
                    GUILayout.Label("Non-Animated OttoIcon:");
                    GUILayout.BeginHorizontal();
                    foreach (var character in Enum.GetValues(typeof(OttoCharacter)))
                    {
                        OttoCharacter ottoCharacter = (OttoCharacter)character;

                        // Check if the character is non-animated
                        if (!Main.setting.IsAnimatedCharacter(ottoCharacter))
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
                                Main.setting.SelectedCharacter = ottoCharacter; // Set the selected character to the clicked one
                            }
                        }
                    }
                    GUILayout.EndHorizontal();

                    GUILayout.Space(5); // Add space between sections
                    GUILayout.Label("Animated OttoIcon:");
                    GUILayout.BeginHorizontal();
                    foreach (var character in Enum.GetValues(typeof(OttoCharacter)))
                    {
                        OttoCharacter ottoCharacter = (OttoCharacter)character;

                        // Check if the character is animated
                        if (Main.setting.IsAnimatedCharacter(ottoCharacter))
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
                                Main.setting.SelectedCharacter = ottoCharacter; // Set the selected character to the clicked one
                            }
                        }
                    }
                    GUILayout.EndHorizontal();
                }
                else
                {
                    Main.setting.UseLocalAnimation = GUILayout.Toggle(Main.setting.UseLocalAnimation, "Use Animated Images");
                    if (!Main.setting.UseLocalAnimation)
                    {
                        // "Apply" Button for Static Image Path Selection
                        if (GUILayout.Button("Apply", GUILayout.Width(60f), GUILayout.Height(20f)))
                        {
                            Main.setting.Apply(false);
                        }
                        GUILayout.Label("Image Paths:");
                        int index1 = 0;
                        int index = 0;
                        foreach (string state in Main.setting.OttoStates)
                        {
                            //if (index % 2 == 0) GUILayout.BeginHorizontal();
                            //GUILayout.BeginVertical();
                            if (!(state == "On" || state == "Left On" || state == "Nervous On" || state == "Right On"))
                            {
                                Main.setting.LocalImageToggles[index1] = GUILayout.Toggle(Main.setting.LocalImageToggles[index1],
                                    Main.setting.OttoStates[index1], GUILayout.Width(700f));
                                if (Main.setting.LocalImageToggles[index1])
                                {
                                    GUILayout.BeginHorizontal();
                                    GUILayout.Space(20f);
                                    GUILayout.BeginVertical();

                                    Main.setting.LocalImagePaths[index1] = MoreGUILayout.PathAndBrowse
                                        ("Path:", Main.setting.LocalImagePaths[index1], 500, false);
                                    // Workaround for ref usage with array elements
                                    int tempState = Main.setting.LocalImageSetDefaults[index1]; // Local variable to hold the current state
                                    MoreGUILayout.SetDefaultDropdown(ref tempState, Main.setting.OttoStates, index1); // Pass the local variable by reference
                                    Main.setting.LocalImageSetDefaults[index1] = tempState; // Write back the updated state

                                    GUILayout.EndVertical();
                                    GUILayout.EndHorizontal();
                                }
                            }
                            //GUILayout.EndVertical();
                            index1++;
                            //if (index % 2 == 1 && !(index == 0)) GUILayout.EndHorizontal();
                            index++;
                        }
                    }
                    else
                    {
                        // "Apply" Button for Animation Folder Selection
                        if (GUILayout.Button("Apply", GUILayout.Width(60f), GUILayout.Height(20f)))
                        {
                            Main.setting.Apply(false);
                        }
                        GUILayout.Label("Folder Paths:");
                        int index1 = 0;
                        int index = 0;
                        foreach (string state in Main.setting.OttoStates)
                        {
                            //if (index % 2 == 0) GUILayout.BeginHorizontal();
                            //GUILayout.BeginVertical();
                            if (!(state == "On" || state == "Left On" || state == "Nervous On" || state == "Right On"))
                            {
                                Main.setting.LocalAnimationToggles[index1] = GUILayout.Toggle
                                    (Main.setting.LocalAnimationToggles[index1], Main.setting.OttoStates[index1], GUILayout.Width(700f));
                                if (Main.setting.LocalAnimationToggles[index1])
                                {
                                    GUILayout.BeginHorizontal();
                                    GUILayout.Space(20f);
                                    GUILayout.BeginVertical();

                                    Main.setting.LocalAnimationFolderPaths[index1] = MoreGUILayout.PathAndBrowse
                                        ("Path:", Main.setting.LocalAnimationFolderPaths[index1], 500, true);
                                    // Workaround for ref usage with array elements
                                    int tempState = Main.setting.LocalAnimationSetDefaults[index1]; // Local variable to hold the current state
                                    MoreGUILayout.SetDefaultDropdown(ref tempState, Main.setting.OttoStates, index1); // Pass the local variable by reference
                                    Main.setting.LocalAnimationSetDefaults[index1] = tempState; // Write back the updated state

                                    GUILayout.EndVertical();
                                    GUILayout.EndHorizontal();
                                }

                            }
                            //GUILayout.EndVertical();
                            index1++;
                            //if (index % 2 == 1 && !(index == 0)) GUILayout.EndHorizontal();
                            index++;
                        }
                    }
                }
                if (Main.setting.UseLocalAnimation || (!Main.setting.UseLocalAnimation && !Main.setting.UseLocalImage))
                {
                    GUILayout.Space(10f);
                    // Toggle to switch between Seconds-based or Frame-based values
                    Main.setting.FrameBasedValuesIsEnabled = GUILayout.Toggle(Main.setting.FrameBasedValuesIsEnabled, "Use Frame-Based Values");
                    if (!Main.setting.FrameBasedValuesIsEnabled)
                    {
                        // Seconds per Sprite Change
                        string newSecondsText = MoreGUILayout.NamedTextField("Seconds per Sprite Change:", Main.setting.SecondsPerSpriteChange.ToString("F4"),
                        100f, 170f); // Display two decimal places

                        // Parse and update
                        float result = Main.setting.ParseInput(newSecondsText);
                        if (Mathf.Abs(Main.setting.SecondsPerSpriteChange - result) > 0.0001f) // Small tolerance for float comparison
                        {
                            Main.setting.SecondsPerSpriteChange = result;
                        }
                    }
                    else
                    {
                        GUILayout.BeginHorizontal();
                        // Frames per Sprite Change
                        string newSpriteFrame = MoreGUILayout.NamedTextField("Frames per Sprite Change:", Main.setting.FramesPerSpriteChange.ToString("F4"), 100f, 160f);

                        // Parse and update
                        float result = Main.setting.ParseInput(newSpriteFrame);
                        if (Mathf.Abs(Main.setting.FramesPerSpriteChange - result) > 0.0001f)
                        {
                            Main.setting.FramesPerSpriteChange = result;
                        }

                        // Frames per Second
                        string newFramePerSeconds = MoreGUILayout.NamedTextField("Frames per Second:", Main.setting.FramesPerSecond.ToString("F2"), 100f, 120f);

                        // Parse and update
                        float result1 = Main.setting.ParseInput(newFramePerSeconds);
                        if (Mathf.Abs(Main.setting.FramesPerSecond - result1) > 0.0001f)
                        {
                            Main.setting.FramesPerSecond = result1;
                        }
                        GUILayout.EndHorizontal();
                    }
                }
                GUILayout.EndVertical();
                GUILayout.EndHorizontal();
            }
            if (Main.setting.UseLocalAnimation && !Main.setting.UseLocalImage)
            {
                // If animation is on but image is turned off, disable animation and track state
                Main.setting.UseLocalAnimation = false;
                Main.setting.indexCheck = 1;
            }
            else if (!Main.setting.UseLocalAnimation && Main.setting.indexCheck == 1 && Main.setting.UseLocalImage)
            {
                // If animation was previously disabled due to image being off, re-enable animation when image is turned back on
                Main.setting.UseLocalAnimation = true;
                Main.setting.indexCheck = 0;
            }
        }

        private static int animationIndex = 0;
        private static float lastFrameTime = 0f;

        public static bool LoadCustomSprite(Image autoImage, bool IsBlink, scnEditor scnEditor)
        {
            if (autoImage == null) return true;

            Sprite activeSprite = null;
            int currentMaxFrames = 1;

            // Calculate frame interval dynamically
            float frameInterval = Patch.setting.FrameBasedValuesIsEnabled
                ? (Patch.setting.FramesPerSecond > 0 ? Patch.setting.FramesPerSpriteChange / Patch.setting.FramesPerSecond : float.MaxValue)
                : Patch.setting.SecondsPerSpriteChange;

            if (Main.setting.UseLocalImage)
            {
                if (Main.setting.UseLocalAnimation)
                {
                    activeSprite = StateAssigner.AssignSprite(scnEditor, IsBlink,
                    BundleLoader.BundleLoader.CustomAniOttoSprites[0][StateAssigner.ApplyOverflowLogic(animationIndex,
                    BundleLoader.BundleLoader.CustomAniOttoSprites[0].Length)],
                    BundleLoader.BundleLoader.CustomAniOttoSprites[1][StateAssigner.ApplyOverflowLogic(animationIndex,
                    BundleLoader.BundleLoader.CustomAniOttoSprites[1].Length)],
                    BundleLoader.BundleLoader.CustomAniOttoSprites[2][StateAssigner.ApplyOverflowLogic(animationIndex,
                    BundleLoader.BundleLoader.CustomAniOttoSprites[2].Length)],
                    BundleLoader.BundleLoader.CustomAniOttoSprites[3][StateAssigner.ApplyOverflowLogic(animationIndex,
                    BundleLoader.BundleLoader.CustomAniOttoSprites[3].Length)],
                    BundleLoader.BundleLoader.CustomAniOttoSprites[4][StateAssigner.ApplyOverflowLogic(animationIndex,
                    BundleLoader.BundleLoader.CustomAniOttoSprites[4].Length)],
                    BundleLoader.BundleLoader.CustomAniOttoSprites[5][StateAssigner.ApplyOverflowLogic(animationIndex,
                    BundleLoader.BundleLoader.CustomAniOttoSprites[5].Length)],
                    BundleLoader.BundleLoader.CustomAniOttoSprites[6][StateAssigner.ApplyOverflowLogic(animationIndex,
                    BundleLoader.BundleLoader.CustomAniOttoSprites[6].Length)],
                    BundleLoader.BundleLoader.CustomAniOttoSprites[7][StateAssigner.ApplyOverflowLogic(animationIndex,
                    BundleLoader.BundleLoader.CustomAniOttoSprites[7].Length)],
                    BundleLoader.BundleLoader.CustomAniOttoSprites[8][StateAssigner.ApplyOverflowLogic(animationIndex,
                    BundleLoader.BundleLoader.CustomAniOttoSprites[8].Length)],
                    BundleLoader.BundleLoader.CustomAniOttoSprites[9][StateAssigner.ApplyOverflowLogic(animationIndex,
                    BundleLoader.BundleLoader.CustomAniOttoSprites[9].Length)]);

                    currentMaxFrames = StateAssigner.AssignSprite(scnEditor, IsBlink,
                        BundleLoader.BundleLoader.CustomAniOttoSprites[0].Length,
                        BundleLoader.BundleLoader.CustomAniOttoSprites[1].Length,
                        BundleLoader.BundleLoader.CustomAniOttoSprites[2].Length,
                        BundleLoader.BundleLoader.CustomAniOttoSprites[3].Length,
                        BundleLoader.BundleLoader.CustomAniOttoSprites[4].Length,
                        BundleLoader.BundleLoader.CustomAniOttoSprites[5].Length,
                        BundleLoader.BundleLoader.CustomAniOttoSprites[6].Length,
                        BundleLoader.BundleLoader.CustomAniOttoSprites[7].Length,
                        BundleLoader.BundleLoader.CustomAniOttoSprites[8].Length,
                        BundleLoader.BundleLoader.CustomAniOttoSprites[9].Length);
                }
                else
                {
                    activeSprite = StateAssigner.AssignSprite(scnEditor, IsBlink,
                        BundleLoader.BundleLoader.CustomOttoSprites[0], // OttoOn
                        BundleLoader.BundleLoader.CustomOttoSprites[1], // OttoOff
                        BundleLoader.BundleLoader.CustomOttoSprites[2], // OttoLeftOn
                        BundleLoader.BundleLoader.CustomOttoSprites[3], // OttoLeftOff
                        BundleLoader.BundleLoader.CustomOttoSprites[4], // OttoRightOn
                        BundleLoader.BundleLoader.CustomOttoSprites[5], // OttoRightOff
                        BundleLoader.BundleLoader.CustomOttoSprites[6], // OttoNervousOn
                        BundleLoader.BundleLoader.CustomOttoSprites[7], // OttoNervousOff
                        BundleLoader.BundleLoader.CustomOttoSprites[8], // OttoPet
                        BundleLoader.BundleLoader.CustomOttoSprites[9]  // OttoMiss
                    );
                }
            }
            else
            {
                if (Main.setting.IsAnimatedCharacterSelected())
                {
                    switch (Patch.setting.SelectedCharacter)
                    {
                        case OttoCharacter.FireFlyAnimated:

                            activeSprite = StateAssigner.AssignSprite(scnEditor, IsBlink,
                                BundleLoader.BundleLoader.FireFlyOttoOn[StateAssigner.ApplyOverflowLogic(animationIndex,
                        BundleLoader.BundleLoader.FireFlyOttoOn.Length)],
                                BundleLoader.BundleLoader.FireFlyOttoOff[StateAssigner.ApplyOverflowLogic(animationIndex,
                        BundleLoader.BundleLoader.FireFlyOttoOff.Length)]);
                            currentMaxFrames = 12;
                            break;

                        case OttoCharacter.HuTaoAnimated:

                            activeSprite = StateAssigner.AssignSprite(scnEditor, IsBlink,
                                BundleLoader.BundleLoader.HuTaoOttoOn[StateAssigner.ApplyOverflowLogic(animationIndex,
                        BundleLoader.BundleLoader.HuTaoOttoOn.Length)],
                                BundleLoader.BundleLoader.HuTaoOttoOff[StateAssigner.ApplyOverflowLogic(animationIndex,
                        BundleLoader.BundleLoader.HuTaoOttoOff.Length)]);
                            currentMaxFrames = 8;
                            break;

                        case OttoCharacter.SparkleAnimated:

                            activeSprite = StateAssigner.AssignSprite(scnEditor, IsBlink,
                                BundleLoader.BundleLoader.SparkleOttoOn[StateAssigner.ApplyOverflowLogic(animationIndex,
                        BundleLoader.BundleLoader.SparkleOttoOn.Length)],
                                BundleLoader.BundleLoader.SparkleOttoOff[StateAssigner.ApplyOverflowLogic(animationIndex,
                        BundleLoader.BundleLoader.SparkleOttoOff.Length)]);
                            currentMaxFrames = 12;
                            break;

                        case OttoCharacter.FurinaAnimated:
                            activeSprite = StateAssigner.AssignSprite(scnEditor, IsBlink,
                                BundleLoader.BundleLoader.FurinaAniOttoOn[StateAssigner.ApplyOverflowLogic(animationIndex,
                        BundleLoader.BundleLoader.FurinaAniOttoOn.Length)],
                                BundleLoader.BundleLoader.FurinaAniOttoOff[StateAssigner.ApplyOverflowLogic(animationIndex,
                        BundleLoader.BundleLoader.FurinaAniOttoOff.Length)]
                            );
                            currentMaxFrames = StateAssigner.AssignSprite(scnEditor, IsBlink,
                                BundleLoader.BundleLoader.FurinaAniOttoOn.Length,
                                BundleLoader.BundleLoader.FurinaAniOttoOff.Length); // Set max frames for Furina animation
                            break;
                            // Add additional character cases if needed, following the same pattern
                    }
                }
                else
                {
                    switch (Patch.setting.SelectedCharacter)
                    {
                        case OttoCharacter.FurinaNonAnimated:
                            activeSprite = StateAssigner.AssignSprite(scnEditor, IsBlink, BundleLoader.BundleLoader.FurinaOttoOn, BundleLoader.BundleLoader.FurinaOttoOff);
                            break;
                        case OttoCharacter.ElysiaNonAnimated:
                            activeSprite = StateAssigner.AssignSprite(scnEditor, IsBlink, BundleLoader.BundleLoader.ElysiaOttoOn, BundleLoader.BundleLoader.ElysiaOttoOff);
                            break;
                    }
                }
            }
            if ((Main.setting.UseLocalImage && Main.setting.UseLocalAnimation) || (Main.setting.IsAnimatedCharacterSelected() && !Main.setting.UseLocalImage))
            {
                if (currentMaxFrames <= 0) currentMaxFrames++;
                // Update animation index based on time
                if (Time.realtimeSinceStartup - lastFrameTime >= frameInterval)
                {
                    animationIndex = (animationIndex + 1) % currentMaxFrames; // Increment and wrap animationIndex
                    lastFrameTime = Time.realtimeSinceStartup; // Update to the current real time
                }
            }
            // Animation logic
            if (activeSprite != null)
            {
                autoImage.sprite = activeSprite; // Override the image sprite of Otto
                return false; //Prevent OttoUpdate from assigning otto sprite
            }
            else return true;
        }
    }
}