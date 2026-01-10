using UnityEngine;

namespace Security
{
    public class AntiCheatMonitor : MonoBehaviour
    {
        [Header("Example protected vars")]
        public ProtectedInt coins;
        public ProtectedInt lives;

        [Header("Checks")]
        [SerializeField] private float checkInterval = 1.0f;

        private float _t;
        
        public static AntiCheatMonitor Instance;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
            
            coins = ProtectedInt.Create(0);
            lives = ProtectedInt.Create(3);
        }

        private void Update()
        {
            _t += Time.unscaledDeltaTime;
            if (_t < checkInterval) return;
            _t = 0;

            ValidateOrReact(ref coins, "coins");
            ValidateOrReact(ref lives, "lives");
        }

        public void AddCoins(int amount)
        {
            // Любое изменение через методы — чтобы hash обновлялся
            coins.SetValue(coins.value + amount);
        }

        public void TakeDamage()
        {
            lives.SetValue(Mathf.Max(0, lives.value - 1));
        }

        private void ValidateOrReact(ref ProtectedInt p, string name)
        {
            if (p.IsValid()) return;

            Debug.LogError($"[AntiCheat] Tampering detected: {name}");

            // Реакция из задания: предупреждение/сброс/логирование
            // Вариант 1: сброс к безопасному значению
            p = ProtectedInt.Create(0);

            // Вариант 2: показать экран и “заморозить”
            Time.timeScale = 0f;
            // Тут можно включить UI панель "Cheat detected"
        }
    }
}