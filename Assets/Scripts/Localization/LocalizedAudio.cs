using UnityEngine;

namespace Localization
{
    [RequireComponent(typeof(AudioSource))]
    public class LocalizedAudio : MonoBehaviour
    {
        [Header("Clips per language")]
        [SerializeField] private AudioClip clipEN;
        [SerializeField] private AudioClip clipPL;
        [SerializeField] private AudioClip clipRU;

        private AudioSource _source;

        private void Awake()
        {
            _source = GetComponent<AudioSource>();
        }

        public void PlayLocalizedOneShot(float volumeScale = 1f)
        {
            if (LocalizationManager.Instance == null || _source == null) return;

            AudioClip chosen = null;
            switch (LocalizationManager.Instance.CurrentLanguage)
            {
                case "en": chosen = clipEN; break;
                case "pl": chosen = clipPL; break;
                case "ru": chosen = clipRU; break;
            }

            if (chosen != null)
                _source.PlayOneShot(chosen, volumeScale);
        }
    }
}
