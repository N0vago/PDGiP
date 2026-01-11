using MobileInputs;
using UnityEngine;

namespace Web
{
    public sealed class WebAutoInputSwitcher : MonoBehaviour
    {
        [Header("Optional: assign your mobile UI root to toggle visibility")]
        [SerializeField] private GameObject mobileControlsRoot;

        private void Start()
        {
            // У тебя уже есть InputModeManager с режимами Keyboard/MobileUI
            var device = WebPlatformDetector.Detect();

            if (InputModeManager.Instance != null)
            {
                InputModeManager.Instance.SetMode(
                    device == WebDeviceType.Mobile ? InputMode.MobileUI : InputMode.Keyboard
                );
            }

            if (mobileControlsRoot != null)
                mobileControlsRoot.SetActive(device == WebDeviceType.Mobile);
        }
    }
}