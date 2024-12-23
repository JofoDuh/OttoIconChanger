using HarmonyLib;
using System.Collections;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

namespace OttoIconChanger
{
    public static class Patch
    {
        public static Setting setting;
        private static bool isBlinking = false; // Flag to track blinking state

        [HarmonyPatch(typeof(scnEditor), "OttoBlink")]
        public static class OttoBlinkPatch
        {
            // Cache the FieldInfo object for the private 'ottoBlinkCounter' field
            public static readonly FieldInfo ottoBlinkCounterField = typeof(scnEditor).GetField("ottoBlinkCounter", BindingFlags.NonPublic | BindingFlags.Instance);

            // Postfix method that runs after OttoBlink is called
            public static void Postfix(scnEditor __instance)
            {
                if (ottoBlinkCounterField == null) return;
                // Get the current value of the 'ottoBlinkCounter' using the cached FieldInfo
                int ottoBlinkCounter = (int)ottoBlinkCounterField.GetValue(__instance);
                // Extract the interval value
                float interval = 60f / (ADOBase.conductor.bpm * ADOBase.conductor.song.pitch * (float)ADOBase.controller.speed) / 2f;
                if (ADOBase.controller.currFloor.holdLength > -1 && ADOBase.controller.currFloor.nextfloor != null)
                {
                    interval = ((float)ADOBase.controller.currFloor.nextfloor.entryTime - (float)ADOBase.controller.currFloor.entryTime);
                }
                // Calculate 'num' based on the game logic
                if (RDC.auto)
                {
                    setting.OttoBlinkCounter = (ottoBlinkCounter % 2 == 0) ? 3 : 2;
                }
                else
                {
                    setting.OttoBlinkCounter = (ottoBlinkCounter % 2 == 0) ? 5 : 4;
                }
                if (setting.CustomeOttoImageIsEnabled)
                {
                    // Call BlinkCoroutine from OttoBlink
                    __instance.StartCoroutine(BlinkCoroutine(__instance.autoImage, interval, __instance));
                }
            }
        }
        private static IEnumerator BlinkCoroutine(Image autoImage, float duration, scnEditor scnEditor)
        {
            isBlinking = true;
            float startTime = Time.realtimeSinceStartup;

            while (isBlinking)
            {
                // Perform the action (blink start logic)
                if (setting.IsAnimatedCharacterSelected() && !setting.UseLocalImage ||
                    setting.UseLocalAnimation && setting.UseLocalImage)
                {
                    OttoCustomSprite.LoadImageAnimation(autoImage, true, scnEditor);
                }
                else
                {
                    OttoCustomSprite.LoadImage(autoImage, true, scnEditor);
                }

                // Check if the duration has elapsed
                if (Time.realtimeSinceStartup - startTime >= duration)
                {
                    isBlinking = false;
                }

                // Continue looping without pausing, actively updating
                yield return null; // Wait until the next frame
            }

            // Perform the ending action (blink end logic)
            if (setting.IsAnimatedCharacterSelected() && !setting.UseLocalImage ||
                setting.UseLocalAnimation && setting.UseLocalImage)
            {
                OttoCustomSprite.LoadImageAnimation(autoImage, false, scnEditor);
            }
            else
            {
                OttoCustomSprite.LoadImage(autoImage, false, scnEditor);
            }
        }

        [HarmonyPatch(typeof(scnEditor), "OttoUpdate")]
        public static class OttoUpdate
        {
            public static void Postfix(scnEditor __instance)
            {
                Image autoImage = __instance.autoImage;
                if (setting.CustomeOttoImageIsEnabled)
                {
                    if (!isBlinking)
                    {
                        if (setting.IsAnimatedCharacterSelected() && !setting.UseLocalImage ||
                            setting.UseLocalAnimation && setting.UseLocalImage)
                        {
                            OttoCustomSprite.LoadImageAnimation(autoImage, false, __instance);
                        }
                        else
                        {
                            OttoCustomSprite.LoadImage(autoImage, false, __instance);
                        }
                    }
                }
                if (setting.OttoColorChangerIsEnabled || setting.OttoGreyOffIsEnabled)
                {
                    OttoCustomColor.OttoColorChanger(autoImage);
                }
                if (setting.OttoOpacityChangerIsEnabled)
                {
                    OttoCustomColor.OttoOpacityChanger(autoImage);
                }
                OttoCustomPositionAndSize.PositionAndSizeChanger(autoImage);
            }
        }
        public static class OttoPetUpdateHelper
        {
            private static readonly FieldInfo autoPetTimeField = typeof(scnEditor).GetField("autoPetTime", BindingFlags.NonPublic | BindingFlags.Instance);
            private static readonly FieldInfo lastOttoPetPositionField = typeof(scnEditor).GetField("lastOttoPetPosition", BindingFlags.NonPublic | BindingFlags.Instance);
            private static readonly FieldInfo lastOttoPetTimeField = typeof(scnEditor).GetField("lastOttoPetTime", BindingFlags.NonPublic | BindingFlags.Instance);

            public static void UpdateOttoPetTime(scnEditor editorInstance)
            {
                // Access private fields
                float autoPetTime = (float)autoPetTimeField.GetValue(editorInstance);
                Vector3 lastOttoPetPosition = (Vector3)lastOttoPetPositionField.GetValue(editorInstance);
                float lastOttoPetTime = (float)lastOttoPetTimeField.GetValue(editorInstance);

                // Check if the pointer is over the button
                if (RDC.auto)
                {
                    // Pointer is over the button
                    if (lastOttoPetPosition != Input.mousePosition)
                    {
                        // Pointer moved: increase pet time
                        autoPetTime += Time.unscaledDeltaTime;
                        lastOttoPetTime = Time.unscaledTime;
                        lastOttoPetPosition = Input.mousePosition;

                        // Update the private fields back
                        autoPetTimeField.SetValue(editorInstance, autoPetTime);
                        lastOttoPetPositionField.SetValue(editorInstance, lastOttoPetPosition);
                        lastOttoPetTimeField.SetValue(editorInstance, lastOttoPetTime);
                    }
                }
                else
                {
                    // Pointer moved away: reset pet time
                    autoPetTime = 0f;
                    autoPetTimeField.SetValue(editorInstance, autoPetTime);
                }

                // Update the custom mod's state
                setting.ottoPetTime = autoPetTime;
            }
        }
        // Patch to set result of highBPM to false to prevent red Otto
        [HarmonyPatch(typeof(scnEditor), "get_highBPM")]
        public static class HighBPMPatch
        {
            public static void Postfix(ref bool __result)
            {
                setting.ResultForHighBpm = __result ? true : false;
                if (setting.NoNervousOttoIsEnabled)
                {
                    __result = false;
                }
            }
        }
        // Patch to set result of highBPM to false to prevent red Otto
        [HarmonyPatch(typeof(scnEditor), "get_paused")]
        public static class PausedPatch
        {
            public static void Postfix(ref bool __result)
            {
                setting.ResultForPaused = __result;
            }
        }
        [HarmonyPatch(typeof(scnEditor), "Start")]
        public static class StartPatch
        {
            public static void Postfix()
            {
                bool HaveStoreOriginalValue = false;
                var autoImageRect = scnEditor.instance.autoImage.GetComponent<RectTransform>();
                var buttonRect = scnEditor.instance.autoImage.GetComponentInChildren<Button>()?.GetComponent<RectTransform>();

                // Store original values if not already done
                if (!HaveStoreOriginalValue)
                {
                    setting.originalOttoImageOffsetMin = autoImageRect.offsetMin;
                    setting.originalOttoImageOffsetMax = autoImageRect.offsetMax;
                    setting.originalOttoButtonOffsetMax = buttonRect.offsetMax;
                    setting.originalOttoButtonOffsetMin = buttonRect.offsetMin;
                    HaveStoreOriginalValue = true;
                }
            }
        }
    }
}