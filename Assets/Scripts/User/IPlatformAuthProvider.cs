using System;
using System.Collections.Generic;

namespace User
{
    public interface IPlatformAuthProvider
    {
        event Action<PlatformUser> PrimeOwnerChanged;
        event Action PrimeOwnerSignedOut;

        PlatformUser PrimeOwner { get; }
        IReadOnlyList<PlatformUser> AvailableUsers { get; }

        void Initialize();
        void SwitchPrimeOwner(string platformUserId);
        void SignOutPrimeOwner();
    }
}