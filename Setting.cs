using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using UnityEngine;
using UnityModManagerNet;

namespace OttoIconChanger
{
    public class Setting : UnityModManager.ModSettings
    {
        //From AdofaiTweaks
        public bool EditorIsAwake = false;
        public string OttoColorHex { get; set; } = "FFFFFF";
        private Color _Ottocolor = Color.white;
        public Color Ottocolor
        {
            get => _Ottocolor;
            set
            {
                _Ottocolor = value;
                OttoColorHex = ColorUtility.ToHtmlStringRGB(value);
            }
        }
        public string OttoColorHexOn { get; set; } = "FFFFFF";
        private Color _OttocolorOn = Color.white;
        public Color OttocolorOn
        {
            get => _OttocolorOn;
            set
            {
                _OttocolorOn = value;
                OttoColorHexOn = ColorUtility.ToHtmlStringRGB(value);
            }
        }
        public string OttoColorHexOff { get; set; } = "FFFFFF";
        private Color _OttocolorOff = Color.white;
        public Color OttocolorOff
        {
            get => _OttocolorOff;
            set
            {
                _OttocolorOff = value;
                OttoColorHexOff = ColorUtility.ToHtmlStringRGB(value);
            }
        }
        //Color Changer
        public bool OttoColorChangerIsEnabled = false;
        public bool OttoColorIndependentIsEnabled = false;
        //Opacity Changer
        public bool OttoOpacityChangerIsEnabled = false;
        public bool OttoOpacityIndependentIsEnabled = false;
        public float OttoOpacityValue = 255f;
        public float OttoOpacityValueOn = 255f;
        public float OttoOpacityValueOff = 255f;
        //Custom Otto Sprite
        public bool CustomeOttoImageIsEnabled = false;
        public float FramesPerSecond = 120;
        public float FramesPerSpriteChange = 10;
        public float SecondsPerSpriteChange = 10f / 120f;
        public bool FrameBasedValuesIsEnabled = false;
        //Custom Local Otto Sprite
        public bool UseLocalImage { get; set; } = false; // Toggle between built-in and local images
        public bool UseLocalAnimation {  get; set; } = false;
        public string LocalImagePathOn { get; set; } = string.Empty; // Path for the local image
        public string LocalAnimationFolderPathOn { get; set; } = string.Empty; // Path for the local animation folder
        public string LocalImagePathOff { get; set; } = string.Empty; // Path for the local image
        public string LocalAnimationFolderPathOff { get; set; } = string.Empty; // Path for the local animation folder
        public int AmountOfFramesOn = 0;
        public int AmountOfFramesOff = 0;
        //No Dark Otto When Off
        public bool OttoGreyOffIsEnabled = false;
        //No Nervous Otto
        public bool NoNervousOttoIsEnabled = false;
        public bool ResultForHighBpm = false;
        //Custom Otto Position & Size
        public bool OttoPosChangerIsEnabled = false;
        public bool OttoSizeChangerIsEnabled = false;
        public bool LinkSizeIsEnabled = false;
        public bool SquareSizeIsEnabled = false;
        public Vector2 originalOttoImageOffsetMin;
        public Vector2 originalOttoImageOffsetMax;
        public Vector2 originalOttoButtonOffsetMin;
        public Vector2 originalOttoButtonOffsetMax;
        public float PositionNewX = 0f;
        public float PositionNewY = 0f;
        public float NewOttoSizeX = 0f; 
        public float NewOttoSizeY = 0f; 

        //Otto Characters enum list
        public enum OttoCharacter
        {
            FurinaNonAnimated,
            ElysiaNonAnimated,
            FurinaAnimated, // Animated
            HuTaoAnimated,   // Animated
            SparkleAnimated,  // Animated
            FireFlyAnimated // Animated
        }
        //Set default character to Furina non animated ver.
        public OttoCharacter SelectedCharacter = OttoCharacter.FurinaNonAnimated;

        // Define animated characters in a HashSet
        private static readonly HashSet<OttoCharacter> AnimatedCharacters = new HashSet<OttoCharacter>
        {
            OttoCharacter.FireFlyAnimated,
            OttoCharacter.HuTaoAnimated,
            OttoCharacter.FurinaAnimated,
            OttoCharacter.SparkleAnimated
        };
        //Check if the selected character is animated and if an animated character is selected
        public bool IsAnimatedCharacter(OttoCharacter character) => AnimatedCharacters.Contains(character);
        public bool IsAnimatedCharacterSelected() => AnimatedCharacters.Contains(SelectedCharacter);

        //Parse method for floats to remove non intergers
        public float ParseInput(string input)
        {
            // Allow digits, decimal points, and the '-' sign
            string sanitizedInput = Regex.Replace(input, @"[^0-9.]", "");

            // Prevent invalid cases like "."
            if (string.IsNullOrEmpty(sanitizedInput) || sanitizedInput == ".")
            {
                return 0f;
            }

            // Try parsing as a float
            if (float.TryParse(sanitizedInput, out float result))
            {
                return result;
            }

            // Return 0 if parsing fails
            return 0f;
        }

        public override void Save(UnityModManager.ModEntry modEntry)
        {
            var filepath = GetPath(modEntry);
            try
            {
                using (var writer = new StreamWriter(filepath))
                {
                    var serializer = new XmlSerializer(GetType());
                    serializer.Serialize(writer, this);
                }
            }
            catch
            {
            }
        }
        public override string GetPath(UnityModManager.ModEntry modEntry)
        {
            return Path.Combine(modEntry.Path, GetType().Name + ".xml");
        }
    }
}