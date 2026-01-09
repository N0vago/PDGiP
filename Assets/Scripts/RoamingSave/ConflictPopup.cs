using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RoamingSave
{
    public class ConflictPopup : MonoBehaviour
    {
        [Header("UI")]
        public GameObject root;
        public TMP_Text infoText;
        public Button useLocalButton;
        public Button useCloudButton;

        private Action _onUseLocal;
        private Action _onUseCloud;

        private void Awake()
        {
            Hide();
        }

        public void Show(string message, Action onUseLocal, Action onUseCloud)
        {
            _onUseLocal = onUseLocal;
            _onUseCloud = onUseCloud;

            infoText.text = message;
            root.SetActive(true);

            useLocalButton.onClick.RemoveAllListeners();
            useCloudButton.onClick.RemoveAllListeners();

            useLocalButton.onClick.AddListener(() => { root.SetActive(false); _onUseLocal?.Invoke(); });
            useCloudButton.onClick.AddListener(() => { root.SetActive(false); _onUseCloud?.Invoke(); });
        }

        public void Hide()
        {
            root.SetActive(false);
        }
    }
}