using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Localization
{
    public class LanguageDropdown : MonoBehaviour
    {
        [SerializeField] private TMP_Dropdown dropdown;

        // Порядок важен: индекс -> код языка
        private readonly List<string> _languages = new List<string> { "en", "pl", "ru" };

        private void Reset()
        {
            dropdown = GetComponent<TMP_Dropdown>();
        }

        private void Start()
        {
            if (dropdown == null) return;

            dropdown.ClearOptions();
            dropdown.AddOptions(new List<string> { "English", "Polski", "Русский" });

            // выставляем текущее значение
            var current = LocalizationManager.Instance != null ? LocalizationManager.Instance.CurrentLanguage : "en";
            int index = Mathf.Max(0, _languages.IndexOf(current));
            dropdown.SetValueWithoutNotify(index);

            dropdown.onValueChanged.AddListener(OnChanged);
        }

        private void OnDestroy()
        {
            if (dropdown != null)
                dropdown.onValueChanged.RemoveListener(OnChanged);
        }

        private void OnChanged(int index)
        {
            if (LocalizationManager.Instance == null) return;
            if (index < 0 || index >= _languages.Count) return;

            LocalizationManager.Instance.SetLanguage(_languages[index]);
        }
    }
}