using System;
using UnityEngine;

namespace User
{
    public class ConnectivityService : MonoBehaviour
    {
        [SerializeField] private bool isOnline = true;

        public event Action<bool> OnlineStateChanged;

        public bool IsOnline
        {
            get => isOnline;
            set
            {
                if (isOnline == value) return;
                isOnline = value;
                OnlineStateChanged?.Invoke(isOnline);
            }
        }

        public void ToggleOnline() => IsOnline = !IsOnline;
    }
}