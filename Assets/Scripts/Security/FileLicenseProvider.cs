using System.IO;
using UnityEngine;

namespace Security
{
    public class FileLicenseProvider : ILicenseProvider
    {
        private readonly string _path;

        public FileLicenseProvider(string fileName = "license.key")
        {
            // Для билдов: рядом с exe. Для Editor: можно тоже использовать persistentDataPath.
#if UNITY_EDITOR
            _path = Path.Combine(Application.persistentDataPath, fileName);
#else
        _path = Path.Combine(Application.dataPath, "..", fileName);
#endif
        }

        public bool HasValidLicense(out string reason)
        {
            if (!File.Exists(_path))
            {
                reason = $"Missing license file: {_path}";
                return false;
            }

            var key = File.ReadAllText(_path).Trim();

            // Симуляция: ключ должен быть не пустой и начинаться с "MICROGAME-"
            if (string.IsNullOrWhiteSpace(key) || !key.StartsWith("MICROGAME-1234"))
            {
                reason = "Invalid license key format.";
                return false;
            }

            reason = "OK";
            return true;
        }
    }
}