using System.Reflection;
using UnityEngine;

namespace OttoIconChanger
{
    public static class MoreGUILayout
    {
        // Copyright (c) 2021 PizzaLovers007
        public static Color ColorRgbSliders(Color color)
        {
            float oldR = Mathf.Round(color.r * 255);
            float oldG = Mathf.Round(color.g * 255);
            float oldB = Mathf.Round(color.b * 255);
            float newR = NamedSlider("R:", oldR, 0, 255, 300f, 1, 40f);
            float newG = NamedSlider("G:", oldG, 0, 255, 300f, 1, 40f);
            float newB = NamedSlider("B:", oldB, 0, 255, 300f, 1, 40f);
            if (oldR != newR || oldG != newG || oldB != newB)
            {
                return new Color(newR / 255, newG / 255, newB / 255);
            }
            return color;
        }
        public static float NamedSlider(string name, float value, float leftValue, float rightValue, float sliderWidth, 
            float roundNearest = 0, float labelWidth = 0, string valueFormat = "{0}")
        {
            GUILayout.BeginHorizontal();
            float newValue = NamedSliderContent(
                    name,
                    value,
                    leftValue,
                    rightValue,
                    sliderWidth,
                    roundNearest,
                    labelWidth,
                    valueFormat);
            GUILayout.EndHorizontal();
            return newValue;
        }
        private static float NamedSliderContent(string name, float value, float leftValue, float rightValue, float sliderWidth,
            float roundNearest = 0, float labelWidth = 0, string valueFormat = "{0}")
        {
            if (labelWidth == 0)
            {
                GUILayout.Label(name);
                GUILayout.Space(4f);
            }
            else
            {
                GUILayout.Label(name, GUILayout.Width(labelWidth));
            }
            float newValue = GUILayout.HorizontalSlider(
                    value, leftValue, rightValue, GUILayout.Width(sliderWidth));
            if (roundNearest != 0)
            {
                newValue = Mathf.Round(newValue / roundNearest) * roundNearest;
            }
            GUILayout.Space(8f);
            GUILayout.Label(string.Format(valueFormat, newValue), GUILayout.Width(40f));
            GUILayout.FlexibleSpace();
            return newValue;
        }
        public static string NamedTextField(
            string name,
            string value,
            float fieldWidth,
            float labelWidth = 0)
        {
            GUILayout.BeginHorizontal();
            string newValue = NamedTextFieldContent(name, value, fieldWidth, labelWidth);
            GUILayout.EndHorizontal();
            return newValue;
        }
        private static string NamedTextFieldContent(
            string name,
            string value,
            float fieldWidth,
            float labelWidth = 0)
            {
                if (labelWidth == 0)
                {
                    GUILayout.Label(name);
                    GUILayout.Space(4f);
                }
                else
                {
                GUILayout.Label(name, GUILayout.Width(labelWidth));
                }
            string newValue = GUILayout.TextField(value, GUILayout.Width(fieldWidth));
            GUILayout.FlexibleSpace();
            return newValue;
        }

        //Jofo's codes from here
        public static string PathAndBrowse(string label, string path, float fieldWidth, bool isFolder)
        {

            FileAndFolderPicker picker = new FileAndFolderPicker();
            // Render the text field and store the updated value
            string updatedPath = NamedTextField(label, path, fieldWidth);
            // Handle browsing for file or folder
            if (!isFolder)
            {
                if (GUILayout.Button("Browse", GUILayout.Width(70f), GUILayout.Height(20f)))
                {
                    updatedPath = picker.OpenFilePickerForImage(); // File selection
                }
            }
            else
            {
                if (GUILayout.Button("Browse", GUILayout.Width(70f), GUILayout.Height(20f)))
                {
                    updatedPath = picker.OpenFolderPickerForAnimation(); // Folder selection
                }
            }
            return updatedPath; // Return the updated path
        }

        // Dropdown logic
        static Vector2 scrollPosition;
        static bool[] displayDropdowns = new bool[10];
        public static void SetDefaultDropdown(ref int selectedValue, string[] options, int index)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Default State:", GUILayout.Width(80f)); // Set a fixed width for the label to align properly
                                                                      // Main dropdown button
            if (GUILayout.Button(selectedValue == 10 ? "Default" : options[selectedValue], GUILayout.Width(100f), GUILayout.Height(20f)))
            {
                displayDropdowns[index] = !displayDropdowns[index]; // Toggle dropdown visibility
            }
            GUILayout.EndHorizontal();
            // Render the dropdown if it is displayed
            if (displayDropdowns[index])
            {
                GUILayout.BeginHorizontal();
                GUILayout.Space(85f);
                GUILayout.BeginVertical();
                GUILayout.Space(5f); // Reduce excessive space between the button and dropdown
                scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Height(100f)); // Increased dropdown height for better UX
                for (int i = 0; i < options.Length; i++)
                {
                    if (GUILayout.Button(options[i], GUILayout.Height(20f), GUILayout.Width(100f))) // Match dropdown button size
                    {
                        selectedValue = i; // Update the selected value
                        displayDropdowns[index] = false; // Close the dropdown
                    }
                }
                if (GUILayout.Button("Default", GUILayout.Height(20f), GUILayout.Width(100f)))
                {
                    selectedValue = 10;
                    displayDropdowns[index] = false;
                }
                GUILayout.EndScrollView();
                GUILayout.EndVertical();
                GUILayout.EndHorizontal();
            }
        }
    }
}