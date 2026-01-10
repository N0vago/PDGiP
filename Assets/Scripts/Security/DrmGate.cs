using UnityEngine;

namespace Security
{
    public class DrmGate : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private GameObject blockedPanel; // Canvas-панель с текстом и кнопкой выхода
        [SerializeField] private string mainSceneName = "MainScene"; // название твоей игровой сцены

        private ILicenseProvider _license;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);

            _license = new FileLicenseProvider("license.key");
        }

        private void Start()
        {
            if (_license.HasValidLicense(out var reason))
            {
                blockedPanel?.SetActive(false);
            }
            else
            {
                Debug.LogWarning($"[DRM] Blocked: {reason}");
                if (blockedPanel != null) blockedPanel.SetActive(true);
                Time.timeScale = 0f;
            }
        }
        public void Quit()
        {
            Application.Quit();
        }
    }
}