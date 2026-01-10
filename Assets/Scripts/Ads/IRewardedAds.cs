using System;

namespace Ads
{
    public interface IRewardedAds
    {
        bool IsReady { get; }
        void Show(Action<RewardResult> onCompleted);
    }

    public enum RewardResult
    {
        Completed,
        Skipped,
        Failed
    }
}