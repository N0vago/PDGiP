using System;

namespace RoamingSave
{
    [Serializable]
    public class SaveData
    {
        public int totalPoints;
        public long unixTimeUtc;
        public int version;

        public static SaveData New(int points, int version = 1)
        {
            return new SaveData
            {
                totalPoints = points,
                unixTimeUtc = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                version = version
            };
        }
    }
}