using System;
using UnityEngine;
using UnityEngine.UI;

namespace OttoIconChanger
{
    public static class OttoCustomColor
    {
        public static void ColorAndOpacitySettings()
        {
            GUILayout.Space(5); // Add space between sections

            //Otto Color Changer, many methods are taken from AdofaiTweaks
            Color OttoNewColor;
            string OttoNewHex;
            Color OttoNewColorOn;
            string OttoNewHexOn;
            Color OttoNewColorOff;
            string OttoNewHexOff;

            //Color Changer

            Main.setting.OttoColorChangerIsEnabled = GUILayout.Toggle(Main.setting.OttoColorChangerIsEnabled, "Otto Color Changer");
            if (Main.setting.OttoColorChangerIsEnabled)
            {
                // Begin an indented section
                GUILayout.BeginHorizontal();
                GUILayout.Space(20f); // Add horizontal space for indentation
                GUILayout.BeginVertical(); // Nested layout for content
                if (Main.setting.OttoColorIndependentIsEnabled)
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.BeginVertical();
                    GUILayout.Label("Otto On");
                    OttoNewColorOn = MoreGUILayout.ColorRgbSliders(Main.setting.OttocolorOn);
                    if (Main.setting.OttocolorOn != OttoNewColorOn)
                    {
                        Main.setting.OttocolorOn = OttoNewColorOn;
                    }
                    OttoNewHexOn = MoreGUILayout.NamedTextField("Hex:", Main.setting.OttoColorHexOn, 100f, 40f);
                    if (OttoNewHexOn != Main.setting.OttoColorHexOn && ColorUtility.TryParseHtmlString($"#{OttoNewHexOn}", out OttoNewColorOn))
                    {
                        Main.setting.OttocolorOn = OttoNewColorOn;
                    }
                    Main.setting.OttoColorHexOn = OttoNewHexOn;
                    GUILayout.EndVertical();
                    GUILayout.BeginVertical();
                    GUILayout.Label("Otto Off");
                    OttoNewColorOff = MoreGUILayout.ColorRgbSliders(Main.setting.OttocolorOff);
                    if (Main.setting.OttocolorOff != OttoNewColorOff)
                    {
                        Main.setting.OttocolorOff = OttoNewColorOff;
                    }
                    OttoNewHexOff = MoreGUILayout.NamedTextField("Hex:", Main.setting.OttoColorHexOff, 100f, 40f);
                    if (OttoNewHexOff != Main.setting.OttoColorHexOn && ColorUtility.TryParseHtmlString($"#{OttoNewHexOff}", out OttoNewColorOff))
                    {
                        Main.setting.OttocolorOff = OttoNewColorOff;
                    }
                    Main.setting.OttoColorHexOff = OttoNewHexOff;
                    GUILayout.EndVertical();
                    GUILayout.EndHorizontal();
                }
                else
                {
                    OttoNewColor = MoreGUILayout.ColorRgbSliders(Main.setting.Ottocolor);
                    if (Main.setting.Ottocolor != OttoNewColor)
                    {
                        Main.setting.Ottocolor = OttoNewColor;
                    }
                    OttoNewHex = MoreGUILayout.NamedTextField("Hex:", Main.setting.OttoColorHex, 100f, 40f);
                    if (OttoNewHex != Main.setting.OttoColorHex && ColorUtility.TryParseHtmlString($"#{OttoNewHex}", out OttoNewColor))
                    {
                        Main.setting.Ottocolor = OttoNewColor;
                    }
                    Main.setting.OttoColorHex = OttoNewHex;
                }
                Main.setting.OttoColorIndependentIsEnabled = GUILayout.Toggle(Main.setting.OttoColorIndependentIsEnabled, "Set Color for On and Off Otto");
                GUILayout.EndVertical();
                GUILayout.EndHorizontal();
            }
            GUILayout.Space(5); // Add space between sections

            //Opacity Changer

            Main.setting.OttoOpacityChangerIsEnabled = GUILayout.Toggle(Main.setting.OttoOpacityChangerIsEnabled, "Otto Opacity Changer");
            if (Main.setting.OttoOpacityChangerIsEnabled)
            {
                // Begin an indented section
                GUILayout.BeginHorizontal();
                GUILayout.Space(20f); // Add horizontal space for indentation
                GUILayout.BeginVertical(); // Nested layout for content
                if (!Main.setting.OttoOpacityIndependentIsEnabled)
                {
                    // Create a slider for alpha, with a label "Opacity:" and range from 0 (transparent) to 255 (opaque)
                    float newAlpha = MoreGUILayout.NamedSlider("Opacity:", Main.setting.OttoOpacityValue, 0, 255, 300f, 1, 50f);
                    // Check if the opacity slider value has changed
                    if (Main.setting.OttoOpacityValue != newAlpha)
                    {
                        // Set new opacity value (normalized from 0 to 1) and apply it to the color
                        Main.setting.OttoOpacityValue = newAlpha;
                    }
                    // Get the text from the opacity field as a string
                    string newAlphaText = MoreGUILayout.NamedTextField("A:", Main.setting.OttoOpacityValue.ToString("F0"), 100f, 40f);

                    // Try to parse the text field input to a float
                    if (float.TryParse(newAlphaText, out float parsedAlpha))
                    {
                        // Clamp the parsed value to ensure it's within the 0-255 range
                        parsedAlpha = Mathf.Clamp(parsedAlpha, 0f, 255f);

                        // Update setting if the new value is different
                        if (Mathf.Abs(Main.setting.OttoOpacityValue - parsedAlpha) > 0.01f) // Small tolerance to avoid floating-point precision issues
                        {
                            Main.setting.OttoOpacityValue = parsedAlpha;
                        }
                    }
                }
                if (Main.setting.OttoOpacityIndependentIsEnabled)
                {
                    // If OttoOpacityIndependent is true, handle opacity for each state separately (On / Off)
                    // Use the existing sliders for single opacity values
                    GUILayout.BeginHorizontal();
                    float newOpacityOn = MoreGUILayout.NamedSlider("Opacity Otto On:", Main.setting.OttoOpacityValueOn, 0, 255, 300f, 1, 100f);
                    if (Main.setting.OttoOpacityValueOn != newOpacityOn)
                    {
                        Main.setting.OttoOpacityValueOn = Mathf.Clamp(newOpacityOn, 0f, 255f);
                    }
                    // Same for the Off opacity value
                    float newOpacityOff = MoreGUILayout.NamedSlider("Opacity Otto Off:", Main.setting.OttoOpacityValueOff, 0, 255, 300f, 1, 100f);
                    if (Main.setting.OttoOpacityValueOff != newOpacityOff)
                    {
                        Main.setting.OttoOpacityValueOff = Mathf.Clamp(newOpacityOff, 0f, 255f);
                    }
                    GUILayout.EndHorizontal();
                    // Get the text from the opacity field as a string
                    GUILayout.BeginHorizontal();
                    string newAlphaTextOn = MoreGUILayout.NamedTextField("A:", Main.setting.OttoOpacityValueOn.ToString("F0"), 100f, 40f);
                    // Try to parse the text field input to a float
                    if (float.TryParse(newAlphaTextOn, out float parsedAlphaOn))
                    {
                        // Clamp the parsed value to ensure it's within the 0-255 range
                        parsedAlphaOn = Mathf.Clamp(parsedAlphaOn, 0f, 255f);

                        // Update setting if the new value is different
                        if (Mathf.Abs(Main.setting.OttoOpacityValue - parsedAlphaOn) > 0f) // Small tolerance to avoid floating-point precision issues
                        {
                            Main.setting.OttoOpacityValueOn = parsedAlphaOn;
                        }
                    }
                    // Get the text from the opacity field as a string
                    string newAlphaTextOff = MoreGUILayout.NamedTextField("A:", Main.setting.OttoOpacityValueOff.ToString("F0"), 100f, 40f);
                    // Try to parse the text field input to a float
                    if (float.TryParse(newAlphaTextOff, out float parsedAlphaOff))
                    {
                        // Clamp the parsed value to ensure it's within the 0-255 range
                        parsedAlphaOff = Mathf.Clamp(parsedAlphaOff, 0f, 255f);

                        // Update setting if the new value is different
                        if (Mathf.Abs(Main.setting.OttoOpacityValue - parsedAlphaOff) > 0f) // Small tolerance to avoid floating-point precision issues
                        {
                            Main.setting.OttoOpacityValueOff = parsedAlphaOff;
                        }
                    }
                    GUILayout.EndHorizontal();
                }
                Main.setting.OttoOpacityIndependentIsEnabled = GUILayout.Toggle(Main.setting.OttoOpacityIndependentIsEnabled, "Set Opacity for On and Off Otto");
                GUILayout.EndVertical();
                GUILayout.EndHorizontal();
            }
        }
        public static void OttoColorChanger(Image autoImage)
        {
            Color OttoColor = Patch.setting.ResultForHighBpm ? Color.red : Color.white;
            if (autoImage == null) return;

            // Determine the color to apply based on the independent color setting
            if (Patch.setting.OttoColorChangerIsEnabled)
            {
                if (Patch.setting.OttoColorIndependentIsEnabled)
                {
                    // Independent colors for "On" and "Off" states
                    OttoColor = RDC.auto ? Patch.setting.OttocolorOn : Patch.setting.OttocolorOff;
                }
                else
                {
                    // Single color for both states
                    OttoColor = Patch.setting.Ottocolor;
                }
            }
            if (!RDC.auto)
            {
                if (!Patch.setting.OttoGreyOffIsEnabled)
                {
                    // Darken if No Dark Otto is disabled and Otto is off
                    OttoColor *= Color.gray;
                }
            }
            // Set the final color to autoImage
            autoImage.color = OttoColor;
        }

        public static void OttoOpacityChanger(Image autoImage)
        {
            if (autoImage == null) return;

            if (Patch.setting.OttoOpacityChangerIsEnabled && !Patch.setting.OttoOpacityIndependentIsEnabled)
            {
                // Create a new color with the same RGB values but with updated alpha
                Color newColor = autoImage.color;
                newColor.a = Patch.setting.OttoOpacityValue / 255f; // Normalize from 0–255 to 0–1 range
                autoImage.color = newColor; // Assign the modified color back to autoImage
            }
            else if (Patch.setting.OttoOpacityChangerIsEnabled && Patch.setting.OttoOpacityIndependentIsEnabled)
            {
                // Create a new color with the same RGB values but with updated alpha
                Color newColorOn = autoImage.color, newColorOff = autoImage.color;
                newColorOn.a = Patch.setting.OttoOpacityValueOn / 255f; // Normalize from 0–255 to 0–1 range
                newColorOff.a = Patch.setting.OttoOpacityValueOff / 255f; // Normalize from 0–255 to 0–1 range
                autoImage.color = RDC.auto ? newColorOn : newColorOff; // Assign the modified color back to autoImage
            }
        }
    }
}
