using Newtonsoft.Json;
using OttoIconChanger;
using System.Collections.Generic;
using System;

public class PathsStorer
{
    public bool IsAnimated { get; set; }
    [JsonProperty(ItemTypeNameHandling = TypeNameHandling.None)]
    public List<string> LocalPaths { get; set; }
    [JsonProperty(ItemTypeNameHandling = TypeNameHandling.None)]
    public List<int> LocalSetDefaults { get; set; }
    [JsonProperty(ItemTypeNameHandling = TypeNameHandling.None)]
    public List<bool> LocalToggles { get; set; }

    public PathsStorer() : this(false, Enum.GetNames(typeof(Setting.OttoStates)).Length) { }

    public PathsStorer(bool isAnimated, int count)
    {
        IsAnimated = isAnimated;
        InitializeLists(count);
    }

    private void InitializeLists(int count)
    {
        LocalPaths = new List<string>(count);
        LocalSetDefaults = new List<int>(count);
        LocalToggles = new List<bool>(count);

        // Fill with default values
        for (int i = 0; i < count; i++)
        {
            LocalPaths.Add(string.Empty);
            LocalToggles.Add(false);
            LocalSetDefaults.Add(i < 10 ? GetDefaultValue(i) : 0);
        }
    }

    private int GetDefaultValue(int index)
    {
        int[] defaults = { 10, 10, 0, 1, 0, 1, 0, 1, 1, 1 };
        return index < defaults.Length ? defaults[index] : 0;
    }

    public void EnsureProperSize()
    {
        int targetCount = Enum.GetNames(typeof(Setting.OttoStates)).Length;
        if (LocalPaths.Count != targetCount)
        {
            InitializeLists(targetCount);
        }
    }

    public PathsStorer Clone()
    {
        var clone = new PathsStorer(IsAnimated, LocalPaths.Count)
        {
            LocalPaths = new List<string>(LocalPaths),
            LocalToggles = new List<bool>(LocalToggles),
            LocalSetDefaults = new List<int>(LocalSetDefaults)
        };
        return clone;
    }
}