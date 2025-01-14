using UnityEngine;
using UnityEngine.UI;

namespace OttoIconChanger
{
    public static class OttoCustomPositionAndSize
    {
        public static void PosAndSizeSettings()
        {
            //Position Size Changer
            GUILayout.Space(5); // Add space between sections
            Main.setting.OttoPosChangerIsEnabled = GUILayout.Toggle(Main.setting.OttoPosChangerIsEnabled, "Otto Position Changer");
            if (Main.setting.OttoPosChangerIsEnabled)
            {
                // Begin an indented section
                GUILayout.BeginHorizontal();
                GUILayout.Space(20f); // Add horizontal space for indentation
                GUILayout.BeginVertical(); // Nested layout for content

                GUILayout.BeginHorizontal();
                float newPosXSlide = MoreGUILayout.NamedSlider("X Pos:", Main.setting.PositionNewX, -1500f, 0, 300f, 1, 50f);
                if (Main.setting.PositionNewX != newPosXSlide)
                {
                    Main.setting.PositionNewX = newPosXSlide;
                }
                // Same for the Off opacity value
                float newPosYSlide = MoreGUILayout.NamedSlider("Y Pos:", Main.setting.PositionNewY, 0, 800f, 300f, 1, 50f);
                if (Main.setting.PositionNewY != newPosYSlide)
                {
                    Main.setting.PositionNewY = newPosYSlide;
                }
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                // Get the text from the opacity field as a string
                string newPosX = MoreGUILayout.NamedTextField("X:", Main.setting.PositionNewX.ToString("F0"), 100f, 20f);
                // Try to parse the text field input to a float
                if (float.TryParse(newPosX, out float parsedPosXNew))
                {
                    // Update setting if the new value is different
                    if (Main.setting.PositionNewX != parsedPosXNew)
                    {
                        Main.setting.PositionNewX = parsedPosXNew;
                    }
                }

                // Get the text from the opacity field as a string
                string newPosY = MoreGUILayout.NamedTextField("Y:", Main.setting.PositionNewY.ToString("F0"), 100f, 20f);
                // Try to parse the text field input to a float
                if (float.TryParse(newPosY, out float parsedPosYNew))
                {
                    // Update setting if the new value is different
                    if (Main.setting.PositionNewY != parsedPosYNew)
                    {
                        Main.setting.PositionNewY = parsedPosYNew;
                    }
                }
                GUILayout.EndHorizontal();
                GUILayout.EndVertical();
                GUILayout.EndHorizontal();
            }
            GUILayout.Space(5); // Add space between sections

            //Size Changer
            Main.setting.OttoSizeChangerIsEnabled = GUILayout.Toggle(Main.setting.OttoSizeChangerIsEnabled, "Otto Size Changer");
            if (Main.setting.OttoSizeChangerIsEnabled)
            {
                // Begin an indented section
                GUILayout.BeginHorizontal();
                GUILayout.Space(20f); // Add horizontal space for indentation
                GUILayout.BeginVertical(); // Nested layout for content

                GUILayout.BeginHorizontal();
                float newSizeXSlide = MoreGUILayout.NamedSlider("X Size:", Main.setting.NewOttoSizeX, 0, 1000f, 300f, 1, 50f);
                if (Main.setting.NewOttoSizeX != newSizeXSlide)
                {
                    Main.setting.NewOttoSizeX = newSizeXSlide;
                    if (Main.setting.SquareSizeIsEnabled)
                    {
                        Main.setting.NewOttoSizeY = newSizeXSlide; // Keep Y same as X for a perfect square
                    }
                }
                // Same for the Off opacity value
                float newSizeYSlide = MoreGUILayout.NamedSlider("Y Size:", Main.setting.NewOttoSizeY, 0, 1000f, 300f, 1, 50f);
                if (Main.setting.NewOttoSizeY != newSizeYSlide)
                {
                    Main.setting.NewOttoSizeY = newSizeYSlide;
                    if (Main.setting.SquareSizeIsEnabled)
                    {
                        Main.setting.NewOttoSizeX = newSizeYSlide; // Keep Y same as X for a perfect square
                    }
                }
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                // Get the text from the X Size field as a string
                string newSizeX = MoreGUILayout.NamedTextField("X:", Main.setting.NewOttoSizeX.ToString("F0"), 100f, 20f);
                // Try to parse the text field input to a float
                if (float.TryParse(newSizeX, out float parsedSizeXNew))
                {
                    // Update X size if the new value is different
                    if (Main.setting.NewOttoSizeX != parsedSizeXNew)
                    {
                        Main.setting.NewOttoSizeX = parsedSizeXNew;

                        if (Main.setting.SquareSizeIsEnabled)
                        {
                            Main.setting.NewOttoSizeY = parsedSizeXNew; // Keep Y same as X for a perfect square
                        }
                    }
                }

                // Get the text from the Y Size field as a string
                string newSizeY = MoreGUILayout.NamedTextField("Y:", Main.setting.NewOttoSizeY.ToString("F0"), 100f, 20f);
                // Try to parse the text field input to a float
                if (float.TryParse(newSizeY, out float parsedSizeYNew))
                {
                    // Update Y size if the new value is different
                    if (Main.setting.NewOttoSizeY != parsedSizeYNew)
                    {
                        Main.setting.NewOttoSizeY = parsedSizeYNew;

                        if (Main.setting.SquareSizeIsEnabled)
                        {
                            Main.setting.NewOttoSizeX = parsedSizeYNew; // Keep X same as Y for a perfect square
                        }
                    }
                }
                GUILayout.EndHorizontal();

                // Add a toggle to enable or disable linking
                Main.setting.SquareSizeIsEnabled = GUILayout.Toggle(Main.setting.SquareSizeIsEnabled, "Link X and Y");
                GUILayout.EndVertical();
                GUILayout.EndHorizontal();
            }
        }
        public static void PositionAndSizeChanger(Image autoImage)
        {
            if (autoImage == null) return;

            var autoButton = autoImage.GetComponentInChildren<Button>();
            if (autoButton == null) return;

            RectTransform ottoImage = autoImage.GetComponent<RectTransform>();
            RectTransform ottoButton = autoButton.GetComponent<RectTransform>();

            if (Patch.setting.OttoPosChangerIsEnabled || Patch.setting.OttoSizeChangerIsEnabled)
            {
                // Modify OttoImage offsets relative to original values
                float newXSize = Patch.setting.NewOttoSizeX;
                float newYSize = Patch.setting.NewOttoSizeY;

                // Calculate half of the new sizes
                float halfNewXSize = newXSize / 2f;
                float halfNewYSize = newYSize / 2f;

                ottoImage.offsetMin = new Vector2(
                    Patch.setting.originalOttoImageOffsetMin.x - (Patch.setting.OttoSizeChangerIsEnabled ? halfNewXSize : 0) + 
                    (Patch.setting.OttoPosChangerIsEnabled ? Patch.setting.PositionNewX : 0),
                    Patch.setting.originalOttoImageOffsetMin.y - (Patch.setting.OttoSizeChangerIsEnabled ? halfNewYSize : 0) + 
                    (Patch.setting.OttoPosChangerIsEnabled ? Patch.setting.PositionNewY : 0)
                );

                ottoImage.offsetMax = new Vector2(
                    Patch.setting.originalOttoImageOffsetMax.x + (Patch.setting.OttoSizeChangerIsEnabled ? halfNewXSize : 0) + 
                    (Patch.setting.OttoPosChangerIsEnabled ? Patch.setting.PositionNewX : 0),
                    Patch.setting.originalOttoImageOffsetMax.y + (Patch.setting.OttoSizeChangerIsEnabled ? halfNewYSize : 0) + 
                    (Patch.setting.OttoPosChangerIsEnabled ? Patch.setting.PositionNewY : 0)
                );

                // Modify OttoButton size to match OttoImage changes
                ottoButton.offsetMin = new Vector2(
                    Patch.setting.originalOttoButtonOffsetMin.x - (Patch.setting.OttoSizeChangerIsEnabled ? halfNewXSize : 0),
                    Patch.setting.originalOttoButtonOffsetMin.y - (Patch.setting.OttoSizeChangerIsEnabled ? halfNewYSize : 0)
                );

                ottoButton.offsetMax = new Vector2(
                    Patch.setting.originalOttoButtonOffsetMax.x + (Patch.setting.OttoSizeChangerIsEnabled ? halfNewXSize : 0),
                    Patch.setting.originalOttoButtonOffsetMax.y + (Patch.setting.OttoSizeChangerIsEnabled ? halfNewYSize : 0)
                );
            }
            else
            {
                // Restore OttoImage and OttoButton to original offsets
                ottoImage.offsetMin = new Vector2(Patch.setting.originalOttoImageOffsetMin.x, Patch.setting.originalOttoImageOffsetMin.y);
                ottoImage.offsetMax = new Vector2(Patch.setting.originalOttoImageOffsetMax.x, Patch.setting.originalOttoImageOffsetMax.y);

                ottoButton.offsetMin = new Vector2(Patch.setting.originalOttoButtonOffsetMin.x, Patch.setting.originalOttoButtonOffsetMin.y);
                ottoButton.offsetMax = new Vector2(Patch.setting.originalOttoButtonOffsetMax.x, Patch.setting.originalOttoButtonOffsetMax.y);
            }
        }
    }
}
