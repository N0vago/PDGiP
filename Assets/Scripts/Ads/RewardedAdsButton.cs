using UnityEngine;
using UnityEngine.UI;

namespace Ads
{
    public sealed class RewardedAdsButton : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private Button button;

        [Header("Reward (example)")]
        [SerializeField] private int coinsReward = 10;

        private void Awake()
        {
            if (button == null) button = GetComponent<Button>();
            button.onClick.AddListener(OnClick);
        }

        private void OnEnable()
        {
            UpdateInteractable();
        }

        private void UpdateInteractable()
        {
            if (AdsService.Instance == null) return;
            button.interactable = AdsService.Instance.Rewarded.IsReady;
        }

        private void OnClick()
        {
            var rewarded = AdsService.Instance.Rewarded;
            if (!rewarded.IsReady) return;

            button.interactable = false;

            rewarded.Show(result =>
            {
                button.interactable = true;

                if (result == RewardResult.Completed)
                {
                    GiveReward();
                }
            });
        }

        private void GiveReward()
        {
            var wallet = FindFirstObjectByType<CoinWallet>();
            if (wallet != null)
                wallet.Add(coinsReward);

            Debug.Log($"[Rewarded] Granted coins: {coinsReward}");
        }
    }
}