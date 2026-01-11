using UnityEngine;

namespace Web
{
    public enum WebDeviceType
    {
        Desktop,
        Mobile
    }

    public static class WebPlatformDetector
    {
        public static WebDeviceType Detect()
        {
            // В Editor оставим Desktop (удобно для тестов)
#if UNITY_EDITOR
            return WebDeviceType.Desktop;
#endif

            // На не-WebGL платформах можно пользоваться built-in
            if (Application.platform != RuntimePlatform.WebGLPlayer)
            {
                return Application.isMobilePlatform ? WebDeviceType.Mobile : WebDeviceType.Desktop;
            }

            // WebGL: ориентируемся на userAgent (надёжнее, чем только Screen.dpi)
#if UNITY_WEBGL && !UNITY_EDITOR
        string ua = GetUserAgentSafe().ToLowerInvariant();

        // базовая эвристика
        bool isMobile =
            ua.Contains("android") ||
            ua.Contains("iphone") ||
            ua.Contains("ipad") ||
            ua.Contains("ipod") ||
            ua.Contains("mobile") ||
            ua.Contains("windows phone");

        return isMobile ? WebDeviceType.Mobile : WebDeviceType.Desktop;
#else
            return WebDeviceType.Desktop;
#endif
        }

#if UNITY_WEBGL && !UNITY_EDITOR
    [System.Runtime.InteropServices.DllImport("__Internal")]
    private static extern string GetUserAgent();

    private static string GetUserAgentSafe()
    {
        try { return GetUserAgent(); }
        catch { return ""; }
    }
#else
        private static string GetUserAgentSafe() => "";
#endif
    }
}