using UnityEngine;

namespace GK.Platform
{
    /// <summary>
    /// 平台检测工具类
    /// 用于检测当前运行的平台类型，支持移动设备、微信小游戏、抖音小游戏等
    /// </summary>
    public static class PlatformDetector
    {
        /// <summary>
        /// 平台类型枚举
        /// </summary>
        public enum PlatformType
        {
            Unknown,
            Windows,
            Mac,
            Linux,
            Android,
            iOS,
            WebGL,
            WeixinMiniGame,
            DouyinMiniGame,
            Editor
        }

        private static PlatformType? _currentPlatform;

        /// <summary>
        /// 获取当前平台类型
        /// </summary>
        public static PlatformType CurrentPlatform
        {
            get
            {
                if (_currentPlatform == null)
                {
                    _currentPlatform = DetectPlatform();
                }
                return _currentPlatform.Value;
            }
        }

        /// <summary>
        /// 是否为移动平台
        /// </summary>
        public static bool IsMobile => CurrentPlatform == PlatformType.Android || CurrentPlatform == PlatformType.iOS;

        /// <summary>
        /// 是否为小游戏平台
        /// </summary>
        public static bool IsMiniGame => CurrentPlatform == PlatformType.WeixinMiniGame || CurrentPlatform == PlatformType.DouyinMiniGame;

        /// <summary>
        /// 是否为桌面平台
        /// </summary>
        public static bool IsDesktop => CurrentPlatform == PlatformType.Windows || CurrentPlatform == PlatformType.Mac || CurrentPlatform == PlatformType.Linux;

        /// <summary>
        /// 是否为Web平台
        /// </summary>
        public static bool IsWeb => CurrentPlatform == PlatformType.WebGL || IsMiniGame;

        /// <summary>
        /// 是否为编辑器
        /// </summary>
        public static bool IsEditor => CurrentPlatform == PlatformType.Editor;

        /// <summary>
        /// 检测当前平台
        /// </summary>
        private static PlatformType DetectPlatform()
        {
#if UNITY_EDITOR
            return PlatformType.Editor;
#elif UNITY_ANDROID
            return PlatformType.Android;
#elif UNITY_IOS
            return PlatformType.iOS;
#elif UNITY_WEBGL
            // 检测是否为小游戏平台
            if (IsWeixinMiniGame())
                return PlatformType.WeixinMiniGame;
            else if (IsDouyinMiniGame())
                return PlatformType.DouyinMiniGame;
            else
                return PlatformType.WebGL;
#elif UNITY_STANDALONE_WIN
            return PlatformType.Windows;
#elif UNITY_STANDALONE_OSX
            return PlatformType.Mac;
#elif UNITY_STANDALONE_LINUX
            return PlatformType.Linux;
#else
            return PlatformType.Unknown;
#endif
        }

        /// <summary>
        /// 检测是否为微信小游戏
        /// </summary>
        private static bool IsWeixinMiniGame()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            try
            {
                // 检测微信小游戏特有的API
                return Application.platform == RuntimePlatform.WebGLPlayer && 
                       HasJavaScriptObject("wx");
            }
            catch
            {
                return false;
            }
#else
            return false;
#endif
        }

        /// <summary>
        /// 检测是否为抖音小游戏
        /// </summary>
        private static bool IsDouyinMiniGame()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            try
            {
                // 检测抖音小游戏特有的API
                return Application.platform == RuntimePlatform.WebGLPlayer && 
                       HasJavaScriptObject("tt");
            }
            catch
            {
                return false;
            }
#else
            return false;
#endif
        }

        /// <summary>
        /// 检测JavaScript对象是否存在
        /// </summary>
        private static bool HasJavaScriptObject(string objectName)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            try
            {
                return !string.IsNullOrEmpty(Application.ExternalEval($"typeof {objectName} !== 'undefined' ? 'true' : 'false'")) &&
                       Application.ExternalEval($"typeof {objectName} !== 'undefined' ? 'true' : 'false'") == "true";
            }
            catch
            {
                return false;
            }
#else
            return false;
#endif
        }

        /// <summary>
        /// 获取平台名称
        /// </summary>
        public static string GetPlatformName()
        {
            return CurrentPlatform.ToString();
        }

        /// <summary>
        /// 获取设备信息
        /// </summary>
        public static string GetDeviceInfo()
        {
            return $"Platform: {GetPlatformName()}, " +
                   $"Device: {SystemInfo.deviceModel}, " +
                   $"OS: {SystemInfo.operatingSystem}, " +
                   $"Memory: {SystemInfo.systemMemorySize}MB";
        }

        /// <summary>
        /// 强制重新检测平台
        /// </summary>
        public static void RefreshPlatform()
        {
            _currentPlatform = null;
        }
    }
}
