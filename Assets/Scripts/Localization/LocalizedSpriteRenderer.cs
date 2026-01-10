using UnityEngine;

namespace Localization
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class LocalizedSpriteRenderer : MonoBehaviour
    {
        [SerializeField] private Sprite defaultSprite;
        [SerializeField] private Sprite spriteEN;
        [SerializeField] private Sprite spritePL;
        [SerializeField] private Sprite spriteRU;

        private SpriteRenderer _sr;

        private void Awake()
        {
            _sr = GetComponent<SpriteRenderer>();
            if (defaultSprite == null) defaultSprite = _sr.sprite;
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
            if (_sr == null || LocalizationManager.Instance == null) return;

            Sprite chosen = defaultSprite;
            switch (LocalizationManager.Instance.CurrentLanguage)
            {
                case "en": if (spriteEN != null) chosen = spriteEN; break;
                case "pl": if (spritePL != null) chosen = spritePL; break;
                case "ru": if (spriteRU != null) chosen = spriteRU; break;
            }

            _sr.sprite = chosen;
        }
    }
}