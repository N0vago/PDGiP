using TMPro;
using UnityEngine;

namespace Localization
{
    [RequireComponent(typeof(TMP_Text))]
    public class LocalizedTextTMP : MonoBehaviour
    {
        [SerializeField] private string key;
        [SerializeField] private bool applyLocalizedFont = true;

        private TMP_Text _text;

        private void Awake()
        {
            _text = GetComponent<TMP_Text>();
        }

        private void OnEnable()
        {
            if (LocalizationManager.Instance != null)
                LocalizationManager.Instance.LanguageChanged += OnLanguageChanged;

            Refresh();
        }

        private void OnDisable()
        {
            if (LocalizationManager.Instance != null)
                LocalizationManager.Instance.LanguageChanged -= OnLanguageChanged;
        }

        private void OnLanguageChanged(string lang) => Refresh();

        public void Refresh()
        {
            if (_text == null) return;
            if (LocalizationManager.Instance == null) return;

            _text.text = LocalizationManager.Instance.Get(key);

            if (applyLocalizedFont)
            {
                var font = LocalizationManager.Instance.GetFontForCurrentLanguage();
                if (font != null) _text.font = font;
            }
        }

        public void SetKey(string newKey)
        {
            key = newKey;
            Refresh();
        }
    }
}