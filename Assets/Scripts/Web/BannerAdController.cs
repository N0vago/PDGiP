using UnityEngine;

namespace Web
{
    public sealed class BannerAdController : MonoBehaviour
    {
        [SerializeField] private SimulatedBannerAd banner;

        public void ShowBanner() => banner.Show();
        public void HideBanner() => banner.Hide();
        public void ToggleBanner() => banner.Toggle();
    }
}