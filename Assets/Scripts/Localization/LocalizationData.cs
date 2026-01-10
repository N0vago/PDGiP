using System;

namespace Localization
{
    [Serializable]
    public class LocalizationEntry
    {
        public string key;
        public string value;
    }

    [Serializable]
    public class LocalizationFile
    {
        public string language;
        public LocalizationEntry[] entries;
    }
}