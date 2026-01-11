using UnityEngine;

namespace MobileInputs
{
    public enum InputMode
    {
        Keyboard,
        MobileUI
    }

    public sealed class InputModeManager : MonoBehaviour
    {
        public static InputModeManager Instance { get; private set; }

        public InputMode CurrentMode { get; private set; } = InputMode.Keyboard;

        public readonly KeyboardPlayerInput Keyboard = new KeyboardPlayerInput();
        public readonly MobilePlayerInput Mobile = new MobilePlayerInput();

        private void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public IPlayerInput GetActiveInput()
            => CurrentMode == InputMode.MobileUI ? Mobile : Keyboard;

        public void SetMode(InputMode mode) => CurrentMode = mode;

        // Удобные методы для UI-кнопок переключения
        public void UseKeyboard() => SetMode(InputMode.Keyboard);
        public void UseMobileUI() => SetMode(InputMode.MobileUI);
    }
}