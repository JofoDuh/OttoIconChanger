using UnityEngine;
using UnityEngine.UI;

namespace OttoIconChanger
{
    public static class OttoCustomPositionAndSize
    {
        public static void PositionAndSizeChanger(scnEditor __instance)
        {
            // Ensure `autoImage` and its components are valid
            Image autoImage = __instance.autoImage;
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
                    Patch.setting.originalOttoImageOffsetMin.x - (Patch.setting.OttoSizeChangerIsEnabled ? halfNewXSize : 0) + (Patch.setting.OttoPosChangerIsEnabled ? Patch.setting.PositionNewX : 0),
                    Patch.setting.originalOttoImageOffsetMin.y - (Patch.setting.OttoSizeChangerIsEnabled ? halfNewYSize : 0) + (Patch.setting.OttoPosChangerIsEnabled ? Patch.setting.PositionNewY : 0)
                );

                ottoImage.offsetMax = new Vector2(
                    Patch.setting.originalOttoImageOffsetMax.x + (Patch.setting.OttoSizeChangerIsEnabled ? halfNewXSize : 0) + (Patch.setting.OttoPosChangerIsEnabled ? Patch.setting.PositionNewX : 0),
                    Patch.setting.originalOttoImageOffsetMax.y + (Patch.setting.OttoSizeChangerIsEnabled ? halfNewYSize : 0) + (Patch.setting.OttoPosChangerIsEnabled ? Patch.setting.PositionNewY : 0)
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
