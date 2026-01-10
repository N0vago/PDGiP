namespace Input
{
    public class KeyboardPlayerInput : IPlayerInput
    {
        public float GetHorizontal() => UnityEngine.Input.GetAxis("Horizontal");
        public bool GetJumpDown() => UnityEngine.Input.GetButtonDown("Jump");
        public bool GetJumpUp() => UnityEngine.Input.GetButtonUp("Jump");
    }
}