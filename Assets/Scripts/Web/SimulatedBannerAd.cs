using UnityEngine;
using UnityEngine.UI;

namespace Web
{
    public sealed class SimulatedBannerAd : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private RectTransform bannerArea; // контейнер снизу
        [SerializeField] private GameObject bannerVisual;  // картинка/панель
        [SerializeField] private LayoutElement bannerLayout; // чтобы менять высоту

        [Header("Banner Settings")]
        [SerializeField] private float shownHeight = 120f; // px
        [SerializeField] private bool showOnStart = false;

        public bool IsShown { get; private set; }

        private void Awake()
        {
            if (bannerArea == null)
                bannerArea = GetComponent<RectTransform>();

            if (bannerLayout == null)
                bannerLayout = bannerArea.GetComponent<LayoutElement>();

            SetShown(showOnStart);
        }

        public void Show() => SetShown(true);
        public void Hide() => SetShown(false);

        public void Toggle() => SetShown(!IsShown);

        private void SetShown(bool value)
        {
            IsShown = value;

            if (bannerVisual != null)
                bannerVisual.SetActive(value);

            // Главная фишка: высота меняется => UI сверху сдвигается, ничего не перекрыто
            if (bannerLayout != null)
                bannerLayout.preferredHeight = value ? shownHeight : 0f;

            // Если LayoutElement не используешь — можно менять sizeDelta.y
            else if (bannerArea != null)
            {
                var size = bannerArea.sizeDelta;
                size.y = value ? shownHeight : 0f;
                bannerArea.sizeDelta = size;
            }
        }
    }
}