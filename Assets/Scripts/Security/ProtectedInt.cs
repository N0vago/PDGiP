using System;
using System.Security.Cryptography;
using System.Text;

namespace Security
{
    [Serializable]
    public struct ProtectedInt
    {
        public int value;
        public int salt;
        public string hash;

        public static ProtectedInt Create(int initialValue)
        {
            var p = new ProtectedInt
            {
                value = initialValue,
                salt = UnityEngine.Random.Range(int.MinValue, int.MaxValue),
            };
            p.hash = ComputeHash(p.value, p.salt);
            return p;
        }

        public bool IsValid()
            => hash == ComputeHash(value, salt);

        public void SetValue(int newValue)
        {
            value = newValue;
            // salt можно менять, чтобы сложнее было “подкрутить”
            salt = UnityEngine.Random.Range(int.MinValue, int.MaxValue);
            hash = ComputeHash(value, salt);
        }

        private static string ComputeHash(int v, int s)
        {
            using var sha = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes($"{v}:{s}");
            var h = sha.ComputeHash(bytes);
            return Convert.ToBase64String(h);
        }
    }
}