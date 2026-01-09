using System.IO;
using UnityEngine;

namespace RoamingSave
{
    public class CloudSaveSimulator
    {
        private readonly string _cloudPath;
        
        public CloudSaveSimulator(string cloudSlotId = "deviceA", string fileName = "cloud_save.json")
        {
            var dir = Path.Combine(Application.persistentDataPath, "CloudSim", cloudSlotId);
            Directory.CreateDirectory(dir);
            _cloudPath = Path.Combine(dir, fileName);
        }

        public bool Exists() => File.Exists(_cloudPath);

        public SaveData Download()
        {
            if (!Exists()) return null;
            var json = File.ReadAllText(_cloudPath);
            return JsonUtility.FromJson<SaveData>(json);
        }

        public void Upload(SaveData data)
        {
            var json = JsonUtility.ToJson(data, true);
            File.WriteAllText(_cloudPath, json);
        }

        public string DebugPath() => _cloudPath;
    }
}