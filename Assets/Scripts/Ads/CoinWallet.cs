using UnityEngine;

namespace Ads
{
    public sealed class CoinWallet : MonoBehaviour
    {
        public int Coins { get; private set; }

        public void Add(int amount)
        {
            Coins += amount;
            Debug.Log($"Coins: {Coins}");
        }
    }
}