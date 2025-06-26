using DG.Tweening;
using HarmonyLib;
using System.Collections;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

namespace OttoIconChanger
{
    public static class Patch
    {
        private static bool isBlinking = false; // Flag to track blinking state
        private static Sequence Timer;
        private static Coroutine runningBlinkCoroutine;

        [HarmonyPatch(typeof(scnEditor), "OttoBlink")]
        public static class OttoBlinkPatch
        {
            // Cache the FieldInfo object for the private 'ottoBlinkCounter' field
            public static readonly FieldInfo ottoBlinkCounterField = typeof(scnEditor).GetField("ottoBlinkCounter", BindingFlags.NonPublic | BindingFlags.Instance);
           
            public static bool Prefix(scnEditor __instance)
            {
                if (Main.setting.CustomOttoImageIsEnabled)
                {
                    // Extract the interval value
                    float interval = ((float)ADOBase.controller.currFloor.nextfloor.entryTime - (float)ADOBase.controller.currFloor.entryTime)
                        * (Main.setting.BlinkDistance / 100);
                    if (ADOBase.controller.currFloor.holdLength > -1 && ADOBase.controller.currFloor.nextfloor != null)
                    {
                        interval = ((float)ADOBase.controller.currFloor.nextfloor.entryTime - (float)ADOBase.controller.currFloor.entryTime);
                    }
                    // Calculate 'num' based on the game logic
                    Main.setting.OttoBlinkState = RDC.auto ? (Main.setting.OttoBlinkCounter % 2 == 0) ? 3 : 2 
                        : (Main.setting.OttoBlinkCounter % 2 == 0) ? 5 : 4;

                    Main.setting.OttoBlinkCounter++;
                    if (Timer != null && Timer.active)
                    {
                        Timer.Kill(false);
                    }
                    Timer = DOTween.Sequence().AppendInterval(interval).OnComplete(delegate
                    {
                        isBlinking = false;
                    }).SetUpdate(true).OnKill(delegate
                    {
                        isBlinking = false;
                    });
                    isBlinking = true;
                    if (runningBlinkCoroutine != null)
                        {
                            __instance.StopCoroutine(runningBlinkCoroutine);
                            runningBlinkCoroutine = null;
                        }
                    if (Main.setting.UseLocalAnimation)
                    {
                        runningBlinkCoroutine = __instance.StartCoroutine(BlinkCoroutine(__instance.autoImage, __instance, __instance.autoSprites));
                    }
                    else
                    {
                        if (OttoCustomSprite.LoadCustomSprite(__instance.autoImage, true, __instance, false))
                        {
                            __instance.autoImage.sprite = __instance.autoSprites[Main.setting.OttoBlinkState];
                        }                      
                    }
                    return false;
                }
                return true;
            }
        }
        private static IEnumerator BlinkCoroutine(Image autoImage, scnEditor __instance, Sprite[] Sprites)
        {
            while (isBlinking)
            {
                // Perform the action (blink start logic)
                if (OttoCustomSprite.LoadCustomSprite(autoImage, true, __instance, false))
                {
                    __instance.autoImage.sprite = __instance.autoSprites[Main.setting.OttoBlinkState];
                }
                ;
                // Continue looping without pausing, actively updating
                yield return null; // Wait until the next frame
            }
        }

        [HarmonyPatch(typeof(scnEditor), "OttoUpdate")]
        public static class OttoUpdate
        {
            public static bool Prefix (scnEditor __instance)
            {
                if (!Main.setting.CustomOttoImageIsEnabled) return true;
                return OttoCustomSprite.LoadCustomSprite(__instance.autoImage, false, __instance, isBlinking);
            }
            public static void Postfix(scnEditor __instance)
            {
                Image autoImage = __instance.autoImage;

                OttoCustomColor.OttoColorChanger(autoImage);
                if (Main.setting.OttoOpacityChangerIsEnabled)
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

            public static void UpdateOttoPetTime(scnEditor scnEditor)
            {
                // Access private fields
                float autoPetTime = (float)autoPetTimeField.GetValue(scnEditor);
                Vector3 lastOttoPetPosition = (Vector3)lastOttoPetPositionField.GetValue(scnEditor);
                float lastOttoPetTime = (float)lastOttoPetTimeField.GetValue(scnEditor);

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
                        autoPetTimeField.SetValue(scnEditor, autoPetTime);
                        lastOttoPetPositionField.SetValue(scnEditor, lastOttoPetPosition);
                        lastOttoPetTimeField.SetValue(scnEditor, lastOttoPetTime);
                    }
                }
                else
                {
                    // Pointer moved away: reset pet time
                    autoPetTime = 0f;
                    autoPetTimeField.SetValue(scnEditor, autoPetTime);
                }
                // Update the custom mod's state
                Main.setting.ottoPetTime = autoPetTime;
            }
        }
        // Patch to set result of highBPM to false to prevent red Otto, as well as getting the current result
        [HarmonyPatch(typeof(scnEditor), "get_highBPM")]
        public static class HighBPMPatch
        {
            public static void Postfix(ref bool __result)
            {
                if (Main.setting.NoNervousOttoIsEnabled)
                {
                    __result = false;
                }
                Main.setting.ResultForHighBpm = __result;
            }
        }

        // Patch to get result for if the game is in play mode or editor
        [HarmonyPatch(typeof(scnEditor), "get_paused")]
        public static class PausedPatch
        {
            public static void Postfix(ref bool __result)
            {
                Main.setting.ResultForPaused = __result;
            }
        }

        [HarmonyPatch(typeof(scrShowIfDebug), "Update")]
        private static class HideAutoplayTextPatch
        {
            private static bool prevVal;

            public static void Prefix()
            {
                prevVal = RDC.auto;
                if (Main.setting.HideOttoPlayText)
                {
                    RDC.auto = false;
                }
            }

            public static void Postfix()
            {
                RDC.auto = prevVal;
            }
        }

        [HarmonyPatch(typeof(scrController), nameof(scrController.OnLandOnPortal))]
        private static class OnLandOnPortalPatch
        {
            static void Postfix()
            {
                if (Main.setting.HideOttoPlayText && RDC.auto)
                {
                    scrController.instance.txtAllStrictClear.gameObject.SetActive(false);
                    scrController.instance.txtCongrats.text = "Autoplayed";
                }
            }
        }

        [HarmonyPatch(typeof(scnGame), nameof(scnGame.instance.Play))]
        private static class PlayGamePatch
        {
            static void Postfix()
            {
                if (Main.setting.HideOttoPlayText)
                {
                    if (scnEditor.instance.buttonAuto == null) return;
                    scnEditor.instance.buttonAuto.interactable = false;
                }
            }
        }


        [HarmonyPatch(typeof(scnEditor), "ResetScene")]
        internal static class scnEditor_ResetScene_Patch
        {
            private static void Postfix()
            {
                if (scnEditor.instance.buttonAuto == null) return;
                scnEditor.instance.buttonAuto.interactable = true;
            }
        }

        [HarmonyPatch(typeof(scrController), "StartLoadingScene")]
        internal static class scrController_StartLoadingScene_Patch
        {
            private static void Postfix()
            {
                if (scnEditor.instance.buttonAuto == null) return;
                scnEditor.instance.buttonAuto.interactable = true;
            }
        }

        // Store original offset values of Otto, if not already done, via start Patch of editor scene, and make disabled Otto not grey
        [HarmonyPatch(typeof(scnEditor), "Start")]
        public static class StartPatch
        {
            public static void Postfix()
            {
                bool HaveStoreOriginalValue = false;
                var autoImageRect = scnEditor.instance.autoImage.GetComponent<RectTransform>();
                var buttonRect = scnEditor.instance.buttonAuto.GetComponent<RectTransform>();

                if (!HaveStoreOriginalValue)
                {
                    Main.setting.originalOttoButtonSizeDelta = buttonRect.sizeDelta;
                    Main.setting.originalOttoImageAnchoredPosition = autoImageRect.anchoredPosition;
                    Main.setting.originalOttoImageSizeDelta = autoImageRect.sizeDelta;
                    HaveStoreOriginalValue = true;
                }
                var colors = scnEditor.instance.buttonAuto.colors;
                colors.disabledColor = Color.white;
                scnEditor.instance.buttonAuto.colors = colors;
            }
        }
    }
}