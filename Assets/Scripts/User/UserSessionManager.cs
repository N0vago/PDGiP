using System;
using UnityEngine;

namespace User
{
    public class UserSessionManager : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private MockPlatformAuthProvider platform;     // можно заменить на IPlatformAuthProvider через DI
        [SerializeField] private ConnectivityService connectivity;

        [Header("Behavior")]
        [SerializeField] private bool returnToTitleOnSignOut = true;

        public PlatformUser PrimeOwner { get; private set; }
        public PlatformUser Player2 { get; private set; }

        public event Action StateChanged;
        public event Action<string> CriticalMessage; // для UI диалогов/баннеров

        private void Awake()
        {
            if (!platform) platform = FindFirstObjectByType<MockPlatformAuthProvider>();
            if (!connectivity) connectivity = FindFirstObjectByType<ConnectivityService>();

            if (!platform) Debug.LogError("UserSessionManager: platform provider not found!");
            if (!connectivity) Debug.LogError("UserSessionManager: connectivity service not found!");
        }

        private void OnEnable()
        {
            if (platform != null)
            {
                platform.PrimeOwnerChanged += OnPrimeOwnerChanged;
                platform.PrimeOwnerSignedOut += OnPrimeOwnerSignedOut;
            }

            if (connectivity != null)
                connectivity.OnlineStateChanged += OnOnlineStateChanged;
        }

        private void OnDisable()
        {
            if (platform != null)
            {
                platform.PrimeOwnerChanged -= OnPrimeOwnerChanged;
                platform.PrimeOwnerSignedOut -= OnPrimeOwnerSignedOut;
            }

            if (connectivity != null)
                connectivity.OnlineStateChanged -= OnOnlineStateChanged;
        }

        private void Start()
        {
            platform.Initialize();
            // connectivity по умолчанию уже выставлен в инспекторе
            StateChanged?.Invoke();
        }

        private void OnPrimeOwnerChanged(PlatformUser user)
        {
            PrimeOwner = user;
            CriticalMessage?.Invoke($"Prime Owner: {user.DisplayName} ({user.PlatformUserId})");
            StateChanged?.Invoke();
        }

        private void OnPrimeOwnerSignedOut()
        {
            CriticalMessage?.Invoke("Prime Owner signed out! Session restricted.");

            // Пример реакции:
            // 1) Выкинуть на титульник
            if (returnToTitleOnSignOut)
            {
                // Для MicroGame чаще всего достаточно "сбросить состояние" или загрузить сцену меню.
                // Если у тебя одна сцена — просто поставь паузу + покажи панель.
                Time.timeScale = 0f;
                CriticalMessage?.Invoke("Signed out → Game paused (Time.timeScale=0). Press Switch User.");
            }

            StateChanged?.Invoke();
        }

        private void OnOnlineStateChanged(bool online)
        {
            if (!online)
                CriticalMessage?.Invoke("Connectivity lost: OFFLINE (cloud features disabled).");
            else
                CriticalMessage?.Invoke("Back ONLINE.");

            StateChanged?.Invoke();
        }

        // --- Public API for UI buttons ---

        public void SwitchPrimeOwnerToIndex(int index)
        {
            var list = platform.AvailableUsers;
            if (index < 0 || index >= list.Count) return;
            platform.SwitchPrimeOwner(list[index].PlatformUserId);

            // Возвращаем игру если была пауза из-за sign-out
            if (Time.timeScale == 0f && PrimeOwner != null && PrimeOwner.IsAuthenticated)
                Time.timeScale = 1f;
        }

        public void SimulateSignOut() => platform.SignOutPrimeOwner();

        public void ToggleOnline() => connectivity.ToggleOnline();

        public void TogglePlayer2()
        {
            if (Player2 == null)
            {
                // Выбираем любого кроме PrimeOwner
                foreach (var u in platform.AvailableUsers)
                {
                    if (PrimeOwner != null && u.PlatformUserId == PrimeOwner.PlatformUserId) continue;
                    Player2 = u;
                    Player2.IsAuthenticated = true;
                    CriticalMessage?.Invoke($"Player2 joined: {Player2.DisplayName} ({Player2.PlatformUserId})");
                    StateChanged?.Invoke();
                    return;
                }

                CriticalMessage?.Invoke("No available user for Player2.");
            }
            else
            {
                CriticalMessage?.Invoke($"Player2 left: {Player2.DisplayName}");
                Player2 = null;
                StateChanged?.Invoke();
            }
        }
    }
}
