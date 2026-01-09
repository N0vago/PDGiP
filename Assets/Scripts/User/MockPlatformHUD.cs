using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace User
{
    public class MockPlatformHUD : MonoBehaviour
    {
        [SerializeField] private UserSessionManager session;
        [SerializeField] private TMP_Text statusText;
        [SerializeField] private TMP_Text messageText;
        [SerializeField] private Button switchA;
        [SerializeField] private Button switchB;
        [SerializeField] private Button switchC;
        [SerializeField] private Button signOut;
        [SerializeField] private Button toggleOnline;
        [SerializeField] private Button toggleP2;

        private void Awake()
        {
            if (!session) session = FindFirstObjectByType<UserSessionManager>();
            Refresh();
        }

        private void OnEnable()
        {
            if (session != null)
            {
                session.StateChanged += Refresh;
                session.CriticalMessage += ShowMessage;
            }
            switchA.onClick.AddListener(UI_SwitchUser0);
            switchB.onClick.AddListener(UI_SwitchUser1);
            switchC.onClick.AddListener(UI_SwitchUser2);
            signOut.onClick.AddListener(UI_SignOut);
            toggleOnline.onClick.AddListener(UI_ToggleOnline);
            toggleP2.onClick.AddListener(UI_ToggleP2);
        }

        private void OnDisable()
        {
            if (session != null)
            {
                session.StateChanged -= Refresh;
                session.CriticalMessage -= ShowMessage;
            }
            switchA.onClick.RemoveListener(UI_SwitchUser0);
            switchB.onClick.RemoveListener(UI_SwitchUser1);
            switchC.onClick.RemoveListener(UI_SwitchUser2);
            signOut.onClick.RemoveListener(UI_SignOut);
            toggleOnline.onClick.RemoveListener(UI_ToggleOnline);
            toggleP2.onClick.RemoveListener(UI_ToggleP2);
        }

        private void Update()
        {
            // Горячие клавиши для удобной демонстрации на защите
            if (Input.GetKeyDown(KeyCode.F1)) session.SwitchPrimeOwnerToIndex(0);
            if (Input.GetKeyDown(KeyCode.F2)) session.SwitchPrimeOwnerToIndex(1);
            if (Input.GetKeyDown(KeyCode.F3)) session.SwitchPrimeOwnerToIndex(2);
            if (Input.GetKeyDown(KeyCode.F4)) session.SimulateSignOut();
            if (Input.GetKeyDown(KeyCode.F5)) session.ToggleOnline();
            if (Input.GetKeyDown(KeyCode.F6)) session.TogglePlayer2();
        }

        private void Refresh()
        {
            if (!session || !statusText) return;

            var po = session.PrimeOwner;
            var p2 = session.Player2;

            string poStr = po == null
                ? "PrimeOwner: <none>"
                : $"PrimeOwner: {po.DisplayName} [{po.PlatformUserId}] auth={po.IsAuthenticated}";

            string p2Str = p2 == null
                ? "Player2: <none>"
                : $"Player2: {p2.DisplayName} [{p2.PlatformUserId}]";
            
            statusText.text = $"{poStr}\n{p2Str}\nTimeScale={Time.timeScale}";
        }

        private void ShowMessage(string msg)
        {
            if (!messageText) return;
            messageText.text = msg;
        }

        // Методы для кнопок:
        public void UI_SwitchUser0() => session.SwitchPrimeOwnerToIndex(0);
        public void UI_SwitchUser1() => session.SwitchPrimeOwnerToIndex(1);
        public void UI_SwitchUser2() => session.SwitchPrimeOwnerToIndex(2);
        public void UI_SignOut()      => session.SimulateSignOut();
        public void UI_ToggleOnline() => session.ToggleOnline();
        public void UI_ToggleP2()     => session.TogglePlayer2();
    }
}
