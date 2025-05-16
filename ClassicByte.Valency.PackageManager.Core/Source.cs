using System;

namespace ClassicByte.Valency.PackageManager.Core;

[Serializable]
public class Source
{
    public String Name { get; set; } = "";

    public Dictionary<String, String> Index { get; set; } = [];
}
