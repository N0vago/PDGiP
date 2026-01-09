using System;

namespace User
{
    [Serializable]
    public class PlatformUser
    {
        public string PlatformUserId;
        public string DisplayName;
        public bool IsAuthenticated;

        public PlatformUser(string id, string name, bool authed = true)
        {
            PlatformUserId = id;
            DisplayName = name;
            IsAuthenticated = authed;
        }
    }
}