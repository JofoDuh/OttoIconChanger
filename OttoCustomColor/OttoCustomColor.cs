using UnityEngine;
using UnityEngine.UI;

namespace OttoIconChanger
{
    public static class OttoCustomColor
    {
        public static void OttoColorChanger(scnEditor __instance)
        {

            Image autoImage = __instance.autoImage;
            if (autoImage == null) return;

            //Apply OttoColorChanger if enabled
            Color OttoColor = Patch.setting.OttoColorChangerIsEnabled ? Patch.setting.Ottocolor : autoImage.color;

            //Check No Dark Otto and apply color based on RDC.auto
            if (!Patch.setting.OttoGreyOffIsEnabled && !RDC.auto)
            {
                //Darken if No Dark Otto is disabled and Otto is off
                OttoColor *= Color.gray;
            }

            //Apply the final color based on if Otto is nervous or not and High BPM or not
            if (Patch.setting.OttoGreyOffIsEnabled && !RDC.auto)
            {
                if (Patch.setting.NoNervousOttoIsEnabled)
                {
                    OttoColor = Patch.setting.OttoColorChangerIsEnabled ? Patch.setting.Ottocolor : Color.white;
                }
                else
                {
                    OttoColor = Patch.setting.OttoColorChangerIsEnabled ? Patch.setting.Ottocolor : Patch.setting.ResultForHighBpm ? Color.red : Color.white;
                }
            }
            // Set the final color to autoImage
            autoImage.color = OttoColor;
        }
        public static void OttoOpacityChanger(scnEditor __instance)
        {
            Image autoImage = __instance.autoImage;
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
