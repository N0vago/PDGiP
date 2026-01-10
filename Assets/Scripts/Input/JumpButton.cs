using UnityEngine;
using UnityEngine.EventSystems;

namespace Input
{
    public sealed class JumpButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public void OnPointerDown(PointerEventData eventData)
            => InputModeManager.Instance.Mobile.SetJumpDown();

        public void OnPointerUp(PointerEventData eventData)
            => InputModeManager.Instance.Mobile.SetJumpUp();
    }
}