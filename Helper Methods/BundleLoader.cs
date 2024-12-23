using System.IO;
using UnityEngine;

namespace OttoIconChanger.BundleLoader
{
    public static class BundleLoader
    {
        //Furina Non Animated
        public static Sprite FurinaOttoOn;
        public static Sprite FurinaOttoOff;
        //Elysia Non Animated
        public static Sprite ElysiaOttoOn;
        public static Sprite ElysiaOttoOff;
        //FireFly Animated
        public static Sprite[] FireFlyOttoOn = new Sprite[12];
        public static Sprite[] FireFlyOttoOff = new Sprite[12];
        //HuTao Animated
        public static Sprite[] HuTaoOttoOn = new Sprite[8];
        public static Sprite[] HuTaoOttoOff = new Sprite[8];
        //Sparkle Animated
        public static Sprite[] SparkleOttoOn = new Sprite[12];
        public static Sprite[] SparkleOttoOff = new Sprite[12];
        //FurinaAni Animated
        public static Sprite[] FurinaAniOttoOn = new Sprite[15];
        public static Sprite[] FurinaAniOttoOff = new Sprite[18];

        public static Sprite CustomOttoOn;
        public static Sprite CustomOttoOff;
        public static Sprite CustomOttoRightOn;
        public static Sprite CustomOttoRightOff;
        public static Sprite CustomOttoLeftOn;
        public static Sprite CustomOttoLeftOff;
        public static Sprite CustomOttoNervousOn;
        public static Sprite CustomOttoNervousOff;
        public static Sprite CustomOttoPet;
        public static Sprite CustomOttoMiss;

        public static Sprite[] CustomAniOttoOn;
        public static Sprite[] CustomAniOttoOff;
        public static Sprite[] CustomAniOttoRightOn;
        public static Sprite[] CustomAniOttoRightOff;
        public static Sprite[] CustomAniOttoLeftOn;
        public static Sprite[] CustomAniOttoLeftOff;
        public static Sprite[] CustomAniOttoNervousOn;
        public static Sprite[] CustomAniOttoNervousOff;
        public static Sprite[] CustomAniOttoPet;
        public static Sprite[] CustomAniOttoMiss;

        // Single frame sprites
        public static Sprite[] CustomOttoSprites = new Sprite[10] {
            CustomOttoOn,
            CustomOttoOff,
            CustomOttoLeftOn,
            CustomOttoLeftOff,
            CustomOttoRightOn,
            CustomOttoRightOff,
            CustomOttoNervousOn,
            CustomOttoNervousOff,
            CustomOttoPet,
            CustomOttoMiss};
        // Animated sprites
        public static Sprite[][] CustomAniOttoSprites = new Sprite[][] {
            CustomAniOttoOn,
            CustomAniOttoOff,
            CustomAniOttoLeftOn,
            CustomAniOttoLeftOff,
            CustomAniOttoRightOn,
            CustomAniOttoRightOff,
            CustomAniOttoNervousOn,
            CustomAniOttoNervousOff,
            CustomAniOttoPet,
            CustomAniOttoMiss};
        //Load the sprites from Bundle and assign into varibles
        public static void LoadCustomOttoSprite()
        {

            var bundle = AssetBundle.LoadFromFile(Path.Combine("Mods", "OttoIconChanger", "ottoreplacement"));
            if (bundle != null)
            {
                //Furina assets non animated
                FurinaOttoOn = bundle.LoadAsset<Sprite>("FurinaOtto_on1");
                FurinaOttoOff = bundle.LoadAsset<Sprite>("FurinaOtto_off1"); 

                //Elysia assets non animated
                ElysiaOttoOn = bundle.LoadAsset<Sprite>("ElysiaOtto_on");
                ElysiaOttoOff = bundle.LoadAsset<Sprite>("ElysiaOtto_off");

                //FireFly Off frames
                for (int i = 0; i < 12; i++)
                {
                    FireFlyOttoOff[i] = bundle.LoadAsset<Sprite>($"FireFlyOtto_off{i + 1}");
                }

                //FireFly On frames
                for (int i = 0; i < 12; i++)
                {
                    FireFlyOttoOn[i] = bundle.LoadAsset<Sprite>($"FireFlyOtto_on{i + 1}");
                }
                //Hu Tao Off frames
                for (int i = 0; i < 8; i++)
                {
                    HuTaoOttoOff[i] = bundle.LoadAsset<Sprite>($"HuTaoOff{i + 1}");
                }

                //Hu Tao On frames
                for (int i = 0; i < 8; i++)
                {
                    HuTaoOttoOn[i] = bundle.LoadAsset<Sprite>($"HuTaoOn{i + 1}");
                }
                //Sparkle Off frames
                for (int i = 0; i < 12; i++)
                {
                    SparkleOttoOff[i] = bundle.LoadAsset<Sprite>($"SparkleOtto_off{i + 1}");
                }

                //Sparkle On frames
                for (int i = 0; i < 12; i++)
                {
                    SparkleOttoOn[i] = bundle.LoadAsset<Sprite>($"SparkleOtto_on{i + 1}");
                }
                //FurinaAni Off frames
                for (int i = 0; i < 18; i++)
                {
                    FurinaAniOttoOff[i] = bundle.LoadAsset<Sprite>($"FurinaOtto_off{i + 1}");
                }

                //FurinaAni On frames
                for (int i = 0; i < 15; i++)
                {
                    FurinaAniOttoOn[i] = bundle.LoadAsset<Sprite>($"FurinaOtto_on{i + 1}");
                }
            }
        }
    }
}
