using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;
using UnityModManagerNet;

namespace OttoIconChanger
{
    public class Setting : UnityModManager.ModSettings
    {
        //From AdofaiTweaks
        private Color _Ottocolor;
        public Color Ottocolor
        {
            get => _Ottocolor;
            set
            {
                _Ottocolor = value;
                OttoColorHex = ColorUtility.ToHtmlStringRGB(value);
            }
        }
        public string OttoColorHex { get; set; }
        //
        public bool OttoColorChanger = false;
        public bool CustomeOttoImage = false;
        public bool OttoGreyOff = true;
        public bool NoNervousOtto = false;
        public bool ResultForHighBpm = false;

        //Otto Characters enum list
        public enum OttoCharacter
        {
            FurinaNonAni,
            Elysia,
            FurinaAnimated, // Animated
            HuTao,   // Animated
            Sparkle,  // Animated
            FireFly // Animated
        }
        //Set default character to Furina non animated ver.
        public OttoCharacter SelectedCharacter = OttoCharacter.FurinaNonAni;

        // Define animated characters in a HashSet
        private static readonly HashSet<OttoCharacter> AnimatedCharacters = new HashSet<OttoCharacter>
        {
            OttoCharacter.FireFly,
            OttoCharacter.HuTao,
            OttoCharacter.FurinaAnimated,
            OttoCharacter.Sparkle
        };
        //Check if the selected character is animated and if an animated character is selected
        public bool IsAnimatedCharacter(OttoCharacter character) => AnimatedCharacters.Contains(character);
        public bool IsAnimatedCharacterSelected() => AnimatedCharacters.Contains(SelectedCharacter);

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