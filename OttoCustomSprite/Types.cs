using System.Collections.Generic;

namespace OttoIconChanger
{
    public class PresetList
    {
        public string Name { get; set; }
        public List<int> SetDefaults { get; set; }
        public List<string> Paths { get; set; }
        public int Checker { get; set; } // Can only be 0 or 1

        // **Parameterless constructor is required for XML serialization**
        public PresetList()
        {
            Name = string.Empty;
            SetDefaults = new List<int>();
            Paths = new List<string>();
            Checker = 0;
        }

        // Constructor for easier initialization
        public PresetList(string name) : this() // Calls the parameterless constructor first
        {
            Name = name;
        }
    }

    public class PathsStorer
    {
        public int Type { get; set; } //0 for Image, 1 for Animated
        public List<string> LocalPaths { get; set; }
        public List<int> LocalSetDefaults { get; set; }
        public List<bool> LocalToggles { get; set; }

        public PathsStorer()
        {
            LocalPaths = new List<string>();
            LocalSetDefaults = new List<int>();
            LocalToggles = new List<bool>();
            Type = 0;
        }

        public PathsStorer(int type, int Count) : this()
        {
            Type = type;
            InitList(Count);
            SetDefaultListValues();
        }

        //Initializes the lists
        public void InitList(int targetCount)
        {
            while (LocalPaths.Count < targetCount || LocalSetDefaults.Count < targetCount || LocalToggles.Count < targetCount)
            {
                if (LocalPaths.Count < targetCount)
                    LocalPaths.Add(string.Empty);

                if (LocalToggles.Count < targetCount)
                    LocalToggles.Add(false);

                if (LocalSetDefaults.Count < targetCount)
                    LocalSetDefaults.Add(0);
            }
        }

        //Method to set the Default States of state to specific ones
        public void SetDefaultListValues()
        {
            if (LocalSetDefaults.Count < 10) return;
            LocalSetDefaults[0] = 10;
            LocalSetDefaults[1] = 10;
            LocalSetDefaults[2] = 0;
            LocalSetDefaults[3] = 1;
            LocalSetDefaults[4] = 0;
            LocalSetDefaults[5] = 1;
            LocalSetDefaults[6] = 0;
            LocalSetDefaults[7] = 1;
            LocalSetDefaults[8] = 1;
            LocalSetDefaults[9] = 1;
        }
    }
}
