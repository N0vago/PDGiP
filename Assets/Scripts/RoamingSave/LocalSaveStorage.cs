using System.IO;
using UnityEngine;

namespace RoamingSave
{
    public class LocalSaveStorage
    {
        private readonly string _path;

        public LocalSaveStorage(string fileName = "local_save.json")
        {
            _path = Path.Combine(Application.persistentDataPath, fileName);
        }

        public bool Exists() => File.Exists(_path);

        public SaveData Load()
        {
            if (!Exists()) return null;
            var json = File.ReadAllText(_path);
            return JsonUtility.FromJson<SaveData>(json);
        }

        public void Save(SaveData data)
        {
            var json = JsonUtility.ToJson(data, true);
            File.WriteAllText(_path, json);
        }

        public void Clear()
        {
            if (Exists()) File.Delete(_path);
        }

        public string DebugPath() => _path;
    }
}