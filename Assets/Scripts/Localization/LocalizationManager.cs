// ... остальные using такие же

using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Localization
{
    public class LocalizationManager : MonoBehaviour
    {
        public static LocalizationManager Instance { get; private set; }
        public event Action<string> LanguageChanged;

        [SerializeField] private string defaultLanguage = "en";

        [Header("Fonts (optional)")]
        [SerializeField] private TMP_FontAsset fontEN;
        [SerializeField] private TMP_FontAsset fontPL;
        [SerializeField] private TMP_FontAsset fontRU;

        public string CurrentLanguage { get; private set; }

        private Dictionary<string, string> _dict = new Dictionary<string, string>(256);
        private const string PlayerPrefsLangKey = "lang";

        private void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            Instance = this;
            DontDestroyOnLoad(gameObject);

            var saved = PlayerPrefs.GetString(PlayerPrefsLangKey, string.Empty);
            var lang = string.IsNullOrEmpty(saved) ? defaultLanguage : saved;
            SetLanguage(lang, notify: false);
        }

        public void SetLanguage(string languageCode, bool notify = true)
        {
            if (string.IsNullOrWhiteSpace(languageCode))
                languageCode = defaultLanguage;

            languageCode = languageCode.Trim().ToLowerInvariant();

            TextAsset json = Resources.Load<TextAsset>($"Localization/{languageCode}");
            if (json == null)
            {
                Debug.LogWarning($"[Localization] Missing Resources/Localization/{languageCode}.json. Falling back to {defaultLanguage}.");
                languageCode = defaultLanguage;
                json = Resources.Load<TextAsset>($"Localization/{languageCode}");
            }

            if (json == null)
            {
                Debug.LogError("[Localization] No localization JSON found.");
                _dict.Clear();
                CurrentLanguage = languageCode;
                return;
            }

            LocalizationFile file;
            try { file = JsonUtility.FromJson<LocalizationFile>(json.text); }
            catch (Exception e)
            {
                Debug.LogError($"[Localization] JSON parse error ({languageCode}): {e.Message}");
                return;
            }

            _dict.Clear();
            if (file?.entries != null)
            {
                foreach (var e in file.entries)
                {
                    if (!string.IsNullOrEmpty(e.key))
                        _dict[e.key] = e.value ?? string.Empty;
                }
            }

            CurrentLanguage = languageCode;
            PlayerPrefs.SetString(PlayerPrefsLangKey, CurrentLanguage);
            PlayerPrefs.Save();

            if (notify) LanguageChanged?.Invoke(CurrentLanguage);
        }

        public string Get(string key)
        {
            if (string.IsNullOrEmpty(key)) return string.Empty;
            return _dict.TryGetValue(key, out var v) ? v : $"#{key}";
        }

        public TMP_FontAsset GetFontForCurrentLanguage()
        {
            switch (CurrentLanguage)
            {
                case "ru": return fontRU != null ? fontRU : fontEN;
                case "pl": return fontPL != null ? fontPL : fontEN;
                case "en":
                default:   return fontEN;
            }
        }
    }
}
