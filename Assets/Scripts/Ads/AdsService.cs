using UnityEngine;

namespace Ads
{
    public sealed class AdsService : MonoBehaviour
    {
        public static AdsService Instance { get; private set; }

        [SerializeField] private SimulatedRewardedAds simulatedRewarded;

        public IRewardedAds Rewarded => simulatedRewarded;

        private void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            Instance = this;
            DontDestroyOnLoad(gameObject);

            if (simulatedRewarded == null)
                simulatedRewarded = gameObject.AddComponent<SimulatedRewardedAds>();
        }
    }
}