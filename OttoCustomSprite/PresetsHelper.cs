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
}
