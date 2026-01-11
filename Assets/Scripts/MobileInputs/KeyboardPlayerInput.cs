using UnityEngine;

namespace MobileInputs
{
    public class KeyboardPlayerInput : IPlayerInput
    {
        public float GetHorizontal() => Input.GetAxis("Horizontal");
        public bool GetJumpDown() => Input.GetButtonDown("Jump");
        public bool GetJumpUp() => Input.GetButtonUp("Jump");
    }
}