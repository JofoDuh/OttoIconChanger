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
                spriteToReturn = HandleSpecialBlinkCounters(Patch.setting.OttoBlinkState, On, Off, LeftOn, LeftOff, RightOn, RightOff,
                    NervousOn, NervousOff, Pet, Miss);
            }
            else
            {
                spriteToReturn = HandleNonSpecialBlinkCounters(scnEditor, On, Off, LeftOn, LeftOff, RightOn, RightOff,
                    NervousOn, NervousOff, Pet, Miss);
            }
            return spriteToReturn;
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
                            return ReturnSprite(NervousOn, SetDefaultCheck(Check: PresetDefaultCheck(6),
                                On, Off, LeftOn, LeftOff, RightOn, RightOff, NervousOn, NervousOff, Pet, Miss), 
                                useLocalAnimation, useLocalImage, 6, On, Off);
                        }
                        return ReturnSprite(On, SetDefaultCheck(Check: PresetDefaultCheck(0),
                                On, Off, LeftOn, LeftOff, RightOn, RightOff, NervousOn, NervousOff, Pet, Miss),
                                useLocalAnimation, useLocalImage, 0, On, Off);
                    }
                    return ReturnSprite(Pet, SetDefaultCheck(Check: PresetDefaultCheck(8),
                                On, Off, LeftOn, LeftOff, RightOn, RightOff, NervousOn, NervousOff, Pet, Miss),
                                useLocalAnimation, useLocalImage, 8, On, Off);
                }
                return ReturnSprite(Miss, SetDefaultCheck(Check: PresetDefaultCheck(9),
                                On, Off, LeftOn, LeftOff, RightOn, RightOff, NervousOn, NervousOff, Pet, Miss),
                                useLocalAnimation, useLocalImage, 9, On, Off);
            }
            else
            {
                if (Patch.setting.ResultForHighBpm && Patch.setting.ResultForPaused)
                {
                    return ReturnSprite(NervousOff, SetDefaultCheck(Check: PresetDefaultCheck(7),
                                On, Off, LeftOn, LeftOff, RightOn, RightOff, NervousOn, NervousOff, Pet, Miss),
                                useLocalAnimation, useLocalImage, 7, On, Off);
                }
                return ReturnSprite(Off, SetDefaultCheck(Check: PresetDefaultCheck(1),
                                On, Off, LeftOn, LeftOff, RightOn, RightOff, NervousOn, NervousOff, Pet, Miss),
                                useLocalAnimation, useLocalImage, 1, On, Off);
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
                    spriteToReturn = ReturnSprite(LeftOn, SetDefaultCheck(Check: PresetDefaultCheck(2),
                                On, Off, LeftOn, LeftOff, RightOn, RightOff, NervousOn, NervousOff, Pet, Miss),
                                useLocalAnimation, useLocalImage, 2, On, Off);
                    break;
                case 3:
                    spriteToReturn = ReturnSprite(RightOn, SetDefaultCheck(Check: PresetDefaultCheck(4),
                                On, Off, LeftOn, LeftOff, RightOn, RightOff, NervousOn, NervousOff, Pet, Miss),
                                useLocalAnimation, useLocalImage, 4, On, Off);
                    break;
                case 4:
                    spriteToReturn = ReturnSprite(LeftOff, SetDefaultCheck(Check: PresetDefaultCheck(3),
                                On, Off, LeftOn, LeftOff, RightOn, RightOff, NervousOn, NervousOff, Pet, Miss),
                                useLocalAnimation, useLocalImage, 3, On, Off);
                    break;
                case 5:
                    spriteToReturn = ReturnSprite(RightOff, SetDefaultCheck(Check: PresetDefaultCheck(5),
                                On, Off, LeftOn, LeftOff, RightOn, RightOff, NervousOn, NervousOff, Pet, Miss),
                                useLocalAnimation, useLocalImage, 5, On, Off);
                    break;
                default:
                    spriteToReturn = RDC.auto ? ReturnSprite(On, 
                        SetDefaultCheck(Check: PresetDefaultCheck(0), On: On), 
                        useLocalAnimation, useLocalImage, 0, On, Off) : ReturnSprite(Off, 
                        SetDefaultCheck(Check: PresetDefaultCheck(1), Off: Off), 
                        useLocalAnimation, useLocalImage, 1, On, Off);
                    break;
            }
            return spriteToReturn;
        }

        private static int PresetDefaultCheck(int index)
        {
            if (!Patch.setting.IsPreset)
            {
                return Patch.setting.UseLocalAnimation ? Patch.setting.LocalAnimation.LocalSetDefaults[index] : 
                    Patch.setting.LocalImage.LocalSetDefaults[index];
            }
            else
            {
                return Patch.setting.PresetLists[Patch.setting.CurrentIndex].SetDefaults[index];
            }
        }
        public static T SetDefaultCheck<T>(int Check, T On = default, T Off = default, T LeftOn = default, T LeftOff = default, T RightOn = default, T RightOff = default,
    T NervousOn = default, T NervousOff = default, T Pet = default, T Miss = default)
        {
            switch (Check)
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
                    return Pet;
                case 9:
                    return Miss;
                default:
                    return default;
            }
        }
        private static T ReturnSprite<T>(T Sprite, T DefaultSprite, bool LocalAnimation, bool LocalImage, int Index, T On, T Off)
        {
            bool IsNullOrDefault(T value)
            {
                return value == null || (value is int intValue && (intValue == 1 || intValue == 0));
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
                        if (Patch.setting.LocalAnimation.LocalToggles[Index] || Patch.setting.IsPreset)
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
                        if (Patch.setting.LocalImage.LocalToggles[Index] || Patch.setting.IsPreset)
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
                if (LocalImage)
                {
                    return !IsNullOrDefault(DefaultSprite) ? DefaultSprite : default;
                }
                else
                {
                    return RDC.auto ? On : Off;
                }
            }
        }
    }
}