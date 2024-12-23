namespace OttoIconChanger
{
    public static class StateAssigner
    {
        // Custom sprite helper method to apply overflow logic to all states
        public static int ApplyOverflowLogic(int animationIndex, int FramesLength)
        {
            int difference = 0;
            // Check if the animation index exceeds the frame length
            if (FramesLength > 0 && animationIndex >= FramesLength)
            {
               difference = animationIndex - FramesLength + 1;
               animationIndex = animationIndex - difference;
            }
            // Return the calculated difference for further usage
            return animationIndex;
        }
        public static T AssignSprite<T>(scnEditor scnEditor, bool IsBlink, T On = default, T Off = default, T LeftOn = default, T LeftOff = default, T RightOn = default, T RightOff = default, 
            T NervousOn = default, T NervousOff = default, T Pet = default, T Miss = default)
        {
            T spriteToReturn = default;
            if (IsBlink)
            {
                spriteToReturn = HandleSpecialBlinkCounters(Patch.setting.OttoBlinkCounter, On, Off, LeftOn, LeftOff, RightOn, RightOff,
                    NervousOn, NervousOff, Pet, Miss);
            }
            else
            {
                spriteToReturn = HandleNonSpecialBlinkCounters(scnEditor, On, Off, LeftOn, LeftOff, RightOn, RightOff,
                    NervousOn, NervousOff, Pet, Miss);
            }
            return spriteToReturn;
        }
        public static T SetDefaultCheck<T>(int Check, T On = default, T Off = default, T LeftOn = default, T LeftOff = default, T RightOn = default, T RightOff = default,
            T NervousOn = default, T NervousOff = default, T Pet = default, T Miss = default)
        {
            switch(Check)
            {
                case 0:
                    return On;
                case 1:
                    return Off;
                case 2:
                    return LeftOn;
                case 3:
                    return LeftOff;
                case 4:
                    return RightOn;
                case 5:
                    return RightOff;
                case 6:
                    return NervousOn;
                case 7:
                    return NervousOff;
                case 8:
                    return Miss;
                default:
                    return default;
            }
        }
        private static T ReturnSprite<T>(T Sprite, T DefaultSprite, bool LocalAnimation, bool LocalImage, int Index)
        {
            bool IsNullOrDefault(T value)
            {
                return value == null || (value is int intValue && intValue == 1);
            }

            if (!IsNullOrDefault(Sprite))
            {
                if (!LocalImage)
                {
                    return Sprite;
                }
                else
                {
                    if (LocalAnimation)
                    {
                        if (Patch.setting.LocalAnimationToggles[Index])
                        {
                            return Sprite;
                        }
                        else
                        {
                            return !IsNullOrDefault(DefaultSprite) ? DefaultSprite : default;
                        }
                    }
                    else
                    {
                        if (Patch.setting.LocalImageToggles[Index])
                        {
                            return Sprite;
                        }
                        else
                        {
                            return !IsNullOrDefault(DefaultSprite) ? DefaultSprite : default;
                        }
                    }
                }
            }
            else
            {
                return !IsNullOrDefault(DefaultSprite) ? DefaultSprite : default;
            }
        }
        private static T HandleNonSpecialBlinkCounters<T>(scnEditor scnEditor,
            T On, T Off, T LeftOn, T LeftOff, T RightOn, T RightOff, T NervousOn, T NervousOff, T Pet, T Miss)
        {
            bool useLocalAnimation = Patch.setting.UseLocalAnimation;
            bool useLocalImage = Patch.setting.UseLocalImage;
            if (RDEditorUtils.CheckPointerInObject(scnEditor.buttonAuto))
            {
                Patch.OttoPetUpdateHelper.UpdateOttoPetTime(scnEditor);
            }
            else
            {
                Patch.setting.ottoPetTime = 0f;
            }
            if (RDC.auto)
            {
                if (!ADOBase.editor.autoFailed)
                {
                    if (Patch.setting.ottoPetTime < 1.5f)
                    {
                        if (Patch.setting.ResultForHighBpm && Patch.setting.ResultForPaused)
                        {
                            return ReturnSprite(NervousOn, SetDefaultCheck(Check: Patch.setting.UseLocalAnimation ?
                                Patch.setting.LocalAnimationSetDefaults[6] : Patch.setting.LocalImageSetDefaults[6],
                                On, Off, LeftOn, LeftOff, RightOn, RightOff, NervousOn, NervousOff, Pet, Miss), 
                                useLocalAnimation, useLocalImage, 6);
                        }
                        return ReturnSprite(On, SetDefaultCheck(Check: Patch.setting.UseLocalAnimation ?
                                Patch.setting.LocalAnimationSetDefaults[0] : Patch.setting.LocalImageSetDefaults[0],
                                On, Off, LeftOn, LeftOff, RightOn, RightOff, NervousOn, NervousOff, Pet, Miss),
                                useLocalAnimation, useLocalImage, 0);
                    }
                    return ReturnSprite(Pet, SetDefaultCheck(Check: Patch.setting.UseLocalAnimation ?
                                Patch.setting.LocalAnimationSetDefaults[8] : Patch.setting.LocalImageSetDefaults[8],
                                On, Off, LeftOn, LeftOff, RightOn, RightOff, NervousOn, NervousOff, Pet, Miss),
                                useLocalAnimation, useLocalImage, 8);
                }
                return ReturnSprite(Miss, SetDefaultCheck(Check: Patch.setting.UseLocalAnimation ?
                                Patch.setting.LocalAnimationSetDefaults[9] : Patch.setting.LocalImageSetDefaults[9],
                                On, Off, LeftOn, LeftOff, RightOn, RightOff, NervousOn, NervousOff, Pet, Miss),
                                useLocalAnimation, useLocalImage, 9);
            }
            else
            {
                if (Patch.setting.ResultForHighBpm && Patch.setting.ResultForPaused)
                {
                    return ReturnSprite(NervousOff, SetDefaultCheck(Check: Patch.setting.UseLocalAnimation ?
                                Patch.setting.LocalAnimationSetDefaults[7] : Patch.setting.LocalImageSetDefaults[7],
                                On, Off, LeftOn, LeftOff, RightOn, RightOff, NervousOn, NervousOff, Pet, Miss),
                                useLocalAnimation, useLocalImage, 7);
                }
                return ReturnSprite(Off, SetDefaultCheck(Check: Patch.setting.UseLocalAnimation ?
                                Patch.setting.LocalAnimationSetDefaults[1] : Patch.setting.LocalImageSetDefaults[1],
                                On, Off, LeftOn, LeftOff, RightOn, RightOff, NervousOn, NervousOff, Pet, Miss),
                                useLocalAnimation, useLocalImage, 1);
            }
        }
        
        private static T HandleSpecialBlinkCounters<T>(
            int blinkCounter, T On, T Off, T LeftOn, T LeftOff, T RightOn, T RightOff, T NervousOn, T NervousOff, T Pet, T Miss)
        {
            T spriteToReturn;
            bool useLocalAnimation = Patch.setting.UseLocalAnimation;
            bool useLocalImage = Patch.setting.UseLocalImage;
            switch (blinkCounter)
            {
                case 2:
                    spriteToReturn = ReturnSprite(LeftOn, SetDefaultCheck(Check: Patch.setting.UseLocalAnimation ?
                                Patch.setting.LocalAnimationSetDefaults[2] : Patch.setting.LocalImageSetDefaults[2],
                                On, Off, LeftOn, LeftOff, RightOn, RightOff, NervousOn, NervousOff, Pet, Miss),
                                useLocalAnimation, useLocalImage, 2);
                    break;
                case 3:
                    spriteToReturn = ReturnSprite(RightOn, SetDefaultCheck(Check: Patch.setting.UseLocalAnimation ?
                                Patch.setting.LocalAnimationSetDefaults[4] : Patch.setting.LocalImageSetDefaults[4],
                                On, Off, LeftOn, LeftOff, RightOn, RightOff, NervousOn, NervousOff, Pet, Miss),
                                useLocalAnimation, useLocalImage, 4);
                    break;
                case 4:
                    spriteToReturn = ReturnSprite(LeftOff, SetDefaultCheck(Check: Patch.setting.UseLocalAnimation ?
                                Patch.setting.LocalAnimationSetDefaults[3] : Patch.setting.LocalImageSetDefaults[3],
                                On, Off, LeftOn, LeftOff, RightOn, RightOff, NervousOn, NervousOff, Pet, Miss),
                                useLocalAnimation, useLocalImage, 3);
                    break;
                case 5:
                    spriteToReturn = ReturnSprite(RightOff, SetDefaultCheck(Check: Patch.setting.UseLocalAnimation ?
                                Patch.setting.LocalAnimationSetDefaults[5] : Patch.setting.LocalImageSetDefaults[5],
                                On, Off, LeftOn, LeftOff, RightOn, RightOff, NervousOn, NervousOff, Pet, Miss),
                                useLocalAnimation, useLocalImage, 5);
                    break;
                default:
                    spriteToReturn = RDC.auto ? ReturnSprite(On, SetDefaultCheck(Check: Patch.setting.UseLocalAnimation ?
                                Patch.setting.LocalAnimationSetDefaults[0] : Patch.setting.LocalImageSetDefaults[0],
                                On: On), useLocalAnimation, useLocalImage, 0) : ReturnSprite(Off, SetDefaultCheck(Check: Patch.setting.UseLocalAnimation ?
                                Patch.setting.LocalAnimationSetDefaults[1] : Patch.setting.LocalImageSetDefaults[1],
                                Off: Off), useLocalAnimation, useLocalImage, 1);
                    break;
            }
            return spriteToReturn;
        }
    }
}