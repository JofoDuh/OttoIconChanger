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
        //Color Changer
        public bool OttoColorChanger = false;
        //Opacity Changer
        public bool OttoOpacityChanger = false;
        public bool OttoOpacityIndependent = false;
        public float OttoOpacityValue = 255f;
        public float OttoOpacityValueOn = 255f;
        public float OttoOpacityValueOff = 255f;
        //Custom Otto Sprite
        public bool CustomeOttoImage = false;
        //No Dark Otto When Off
        public bool OttoGreyOff = false;
        //No Nervous Otto
        public bool NoNervousOtto = false;
        public bool ResultForHighBpm = false;
        ////OttoSizeChanger
        //public bool OttoSizeChanger = false;
        //public bool StoreOriginalValue = false;
        //public (float, float) originalOttoSize;
        //public (float, float) originalOttoButtonSize;
        //public float NewOttoSizeMultiplier = 1f;

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
            // Remove non-numeric characters (keeping only digits, '.', and '-')
            string sanitizedInput = Regex.Replace(input, @"[^0-9-]", "");

            // If the result is empty, return 0 as the default value
            if (string.IsNullOrEmpty(sanitizedInput))
            {
                return 0f;
            }

            // Attempt to parse the sanitized string as a float
            if (float.TryParse(sanitizedInput, out float result))
            {
                return result;
            }
            // Default to 0 if parsing fails
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