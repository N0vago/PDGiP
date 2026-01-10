using UnityEngine;
using UnityEngine.UI;

namespace Localization
{
    [RequireComponent(typeof(Image))]
    public class LocalizedImage : MonoBehaviour
    {
        [SerializeField] private Sprite defaultSprite;
        [SerializeField] private Sprite spriteEN;
        [SerializeField] private Sprite spritePL;
        [SerializeField] private Sprite spriteRU;

        private Image _image;

        private void Awake()
        {
            _image = GetComponent<Image>();
            if (defaultSprite == null) defaultSprite = _image.sprite;
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
            if (_image == null || LocalizationManager.Instance == null) return;

            Sprite chosen = defaultSprite;
            switch (LocalizationManager.Instance.CurrentLanguage)
            {
                case "en": if (spriteEN != null) chosen = spriteEN; break;
                case "pl": if (spritePL != null) chosen = spritePL; break;
                case "ru": if (spriteRU != null) chosen = spriteRU; break;
            }

            _image.sprite = chosen;
        }
    }
}