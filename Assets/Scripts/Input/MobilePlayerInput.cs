namespace Input
{
    public class MobilePlayerInput : IPlayerInput
    {
        public float Horizontal { get; set; }

        // флаги “событий” (одноразовые)
        private bool _jumpDown;
        private bool _jumpUp;

        // Эти методы дергай из UI-кнопки (EventTrigger / IPointerDown/Up)
        public void SetJumpDown() => _jumpDown = true;
        public void SetJumpUp() => _jumpUp = true;

        public float GetHorizontal() => Horizontal;

        public bool GetJumpDown()
        {
            if (!_jumpDown) return false;
            _jumpDown = false;
            return true;
        }

        public bool GetJumpUp()
        {
            if (!_jumpUp) return false;
            _jumpUp = false;
            return true;
        }
    }
}