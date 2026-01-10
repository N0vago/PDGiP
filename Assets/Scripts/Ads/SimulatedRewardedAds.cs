using System;
using System.Collections;
using UnityEngine;

namespace Ads
{
    public sealed class SimulatedRewardedAds : MonoBehaviour, IRewardedAds
    {
        [Header("Simulation")]
        [SerializeField] private float watchTimeSeconds = 2.5f;
        [SerializeField] private bool alwaysReady = true;

        // Вариант: для дебага можно переключать результат
        public RewardResult simulatedResult = RewardResult.Completed;

        public bool IsReady => alwaysReady;

        private bool _isShowing;

        public void Show(Action<RewardResult> onCompleted)
        {
            if (_isShowing)
            {
                onCompleted?.Invoke(RewardResult.Failed);
                return;
            }

            if (!IsReady)
            {
                onCompleted?.Invoke(RewardResult.Failed);
                return;
            }

            StartCoroutine(ShowRoutine(onCompleted));
        }

        private IEnumerator ShowRoutine(Action<RewardResult> onCompleted)
        {
            _isShowing = true;

            // “Пауза геймплея”, если надо
            // Time.timeScale = 0f;  // <- включай, если хочешь заморозить игру
            // но тогда WaitForSeconds не сработает, нужно WaitForSecondsRealtime
            float t = 0f;
            while (t < watchTimeSeconds)
            {
                t += Time.unscaledDeltaTime;
                yield return null;
            }

            // Time.timeScale = 1f;

            _isShowing = false;
            onCompleted?.Invoke(simulatedResult);
        }
    }
}