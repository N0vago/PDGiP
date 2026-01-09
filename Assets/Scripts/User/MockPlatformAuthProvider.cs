using System;
using System.Collections.Generic;
using UnityEngine;

namespace User
{
    public class MockPlatformAuthProvider : MonoBehaviour, IPlatformAuthProvider
    {
        [Header("Mock Users (edit in Inspector)")]
        [SerializeField] private List<PlatformUser> users = new()
        {
            new PlatformUser("1001", "User_A"),
            new PlatformUser("1002", "User_B"),
            new PlatformUser("1003", "User_C"),
        };

        [Header("Startup")]
        [SerializeField] private int defaultPrimeOwnerIndex = 0;

        public event Action<PlatformUser> PrimeOwnerChanged;
        public event Action PrimeOwnerSignedOut;

        public PlatformUser PrimeOwner { get; private set; }
        public IReadOnlyList<PlatformUser> AvailableUsers => users;

        public void Initialize()
        {
            if (users == null || users.Count == 0)
            {
                Debug.LogError("[MockPlatform] No users configured!");
                return;
            }

            defaultPrimeOwnerIndex = Mathf.Clamp(defaultPrimeOwnerIndex, 0, users.Count - 1);
            PrimeOwner = users[defaultPrimeOwnerIndex];
            PrimeOwner.IsAuthenticated = true;

            PrimeOwnerChanged?.Invoke(PrimeOwner);
        }

        public void SwitchPrimeOwner(string platformUserId)
        {
            var u = users.Find(x => x.PlatformUserId == platformUserId);
            if (u == null)
            {
                Debug.LogWarning($"[MockPlatform] User id not found: {platformUserId}");
                return;
            }

            u.IsAuthenticated = true;
            PrimeOwner = u;
            PrimeOwnerChanged?.Invoke(PrimeOwner);
        }

        public void SignOutPrimeOwner()
        {
            if (PrimeOwner == null) return;

            PrimeOwner.IsAuthenticated = false;
            PrimeOwnerSignedOut?.Invoke();
        }
    }
}