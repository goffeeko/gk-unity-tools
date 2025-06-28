using UnityEngine;
using GK.Platform;
using GK.Managers;
using GK.Adapters;

namespace GK.Utils
{
    /// <summary>
    /// 游戏工具类集合
    /// 提供便捷的静态方法访问各种工具功能
    /// </summary>
    public static class GameUtils
    {
        /// <summary>
        /// 初始化所有工具类
        /// </summary>
        public static void Initialize()
        {
            Debug.Log("Initializing GameUtils...");

            // 确保所有管理器实例化
            var inputManager = InputManager.Instance;
            var audioManager = AudioManager.Instance;
            var networkManager = NetworkManager.Instance;
            var performanceManager = PerformanceManager.Instance;
            var screenAdapter = ScreenAdapter.Instance;

            // 执行初始化
            audioManager.Initialize();
            performanceManager.OptimizeForPlatform();
            screenAdapter.AdaptScreen();

            UnityEngine.Debug.Log($"GameUtils initialized for platform: {PlatformDetector.GetPlatformName()}");
        }

        /// <summary>
        /// 平台相关工具
        /// </summary>
        public static class Platform
        {
            /// <summary>
            /// 获取当前平台类型
            /// </summary>
            public static PlatformDetector.PlatformType GetCurrentPlatform()
            {
                return PlatformDetector.CurrentPlatform;
            }

            /// <summary>
            /// 是否为移动平台
            /// </summary>
            public static bool IsMobile()
            {
                return PlatformDetector.IsMobile;
            }

            /// <summary>
            /// 是否为小游戏平台
            /// </summary>
            public static bool IsMiniGame()
            {
                return PlatformDetector.IsMiniGame;
            }

            /// <summary>
            /// 获取设备信息
            /// </summary>
            public static string GetDeviceInfo()
            {
                return PlatformDetector.GetDeviceInfo();
            }
        }

        /// <summary>
        /// 屏幕适配工具
        /// </summary>
        public static class Screen
        {
            /// <summary>
            /// 执行屏幕适配
            /// </summary>
            public static void Adapt()
            {
                ScreenAdapter.Instance.AdaptScreen();
            }

            /// <summary>
            /// 获取屏幕宽高比
            /// </summary>
            public static float GetAspectRatio()
            {
                return ScreenAdapter.AspectRatio;
            }

            /// <summary>
            /// 是否有安全区域
            /// </summary>
            public static bool HasSafeArea()
            {
                return ScreenAdapter.HasSafeArea;
            }

            /// <summary>
            /// 获取安全区域
            /// </summary>
            public static Rect GetSafeArea()
            {
                return ScreenAdapter.SafeArea;
            }
        }

        /// <summary>
        /// 输入管理工具
        /// </summary>
        public static class Input
        {
            /// <summary>
            /// 获取当前输入位置
            /// </summary>
            public static Vector2 GetCurrentPosition()
            {
                return InputManager.Instance.CurrentInputPosition;
            }

            /// <summary>
            /// 是否正在输入
            /// </summary>
            public static bool IsInputActive()
            {
                return InputManager.Instance.IsInputActive;
            }

            /// <summary>
            /// 设置触摸灵敏度
            /// </summary>
            public static void SetTouchSensitivity(float sensitivity)
            {
                InputManager.Instance.SetTouchSensitivity(sensitivity);
            }
        }

        /// <summary>
        /// 音频管理工具
        /// </summary>
        public static class Audio
        {
            /// <summary>
            /// 播放音乐
            /// </summary>
            public static void PlayMusic(string clipName, bool loop = true)
            {
                AudioManager.Instance.PlayMusic(clipName, loop);
            }

            /// <summary>
            /// 停止音乐
            /// </summary>
            public static void StopMusic()
            {
                AudioManager.Instance.StopMusic();
            }

            /// <summary>
            /// 播放音效
            /// </summary>
            public static void PlaySfx(string clipName, float volume = 1f)
            {
                AudioManager.Instance.PlaySfx(clipName, volume);
            }

            /// <summary>
            /// 设置主音量
            /// </summary>
            public static void SetMasterVolume(float volume)
            {
                AudioManager.Instance.MasterVolume = volume;
            }

            /// <summary>
            /// 设置音乐音量
            /// </summary>
            public static void SetMusicVolume(float volume)
            {
                AudioManager.Instance.MusicVolume = volume;
            }

            /// <summary>
            /// 设置音效音量
            /// </summary>
            public static void SetSfxVolume(float volume)
            {
                AudioManager.Instance.SfxVolume = volume;
            }
        }

        /// <summary>
        /// 数据存储工具
        /// </summary>
        public static class Data
        {
            /// <summary>
            /// 保存数据
            /// </summary>
            public static void Save<T>(string key, T data)
            {
                DataManager.SaveData(key, data);
            }

            /// <summary>
            /// 加载数据
            /// </summary>
            public static T Load<T>(string key, T defaultValue = default(T))
            {
                return DataManager.LoadData(key, defaultValue);
            }

            /// <summary>
            /// 删除数据
            /// </summary>
            public static void Delete(string key)
            {
                DataManager.DeleteData(key);
            }

            /// <summary>
            /// 检查数据是否存在
            /// </summary>
            public static bool Exists(string key)
            {
                return DataManager.HasData(key);
            }

            /// <summary>
            /// 启用加密
            /// </summary>
            public static void EnableEncryption(string key = null)
            {
                DataManager.UseEncryption = true;
                if (!string.IsNullOrEmpty(key))
                {
                    DataManager.SetEncryptionKey(key);
                }
            }
        }

        /// <summary>
        /// 网络请求工具
        /// </summary>
        public static class Network
        {
            /// <summary>
            /// GET请求
            /// </summary>
            public static void Get(string url, NetworkManager.NetworkCallback callback)
            {
                NetworkManager.Instance.Get(url, callback);
            }

            /// <summary>
            /// POST请求
            /// </summary>
            public static void Post(string url, string jsonData, NetworkManager.NetworkCallback callback)
            {
                NetworkManager.Instance.Post(url, jsonData, callback);
            }

            /// <summary>
            /// 检查网络连接
            /// </summary>
            public static bool IsNetworkAvailable()
            {
                return NetworkManager.Instance.IsNetworkAvailable();
            }

            /// <summary>
            /// 获取网络类型
            /// </summary>
            public static string GetNetworkType()
            {
                return NetworkManager.Instance.GetNetworkType();
            }
        }

        /// <summary>
        /// 性能优化工具
        /// </summary>
        public static class Performance
        {
            /// <summary>
            /// 获取当前FPS
            /// </summary>
            public static float GetCurrentFPS()
            {
                return PerformanceManager.Instance.CurrentFPS;
            }

            /// <summary>
            /// 获取平均FPS
            /// </summary>
            public static float GetAverageFPS()
            {
                return PerformanceManager.Instance.AverageFPS;
            }

            /// <summary>
            /// 获取内存使用量
            /// </summary>
            public static float GetMemoryUsage()
            {
                return PerformanceManager.Instance.CurrentMemoryMB;
            }

            /// <summary>
            /// 强制垃圾回收
            /// </summary>
            public static void ForceGC()
            {
                PerformanceManager.Instance.ForceGarbageCollection();
            }

            /// <summary>
            /// 设置目标帧率
            /// </summary>
            public static void SetTargetFrameRate(int frameRate)
            {
                PerformanceManager.Instance.SetTargetFrameRate(frameRate);
            }
        }

        /// <summary>
        /// UI适配工具
        /// </summary>
        public static class UI
        {
            /// <summary>
            /// 为GameObject添加UI适配
            /// </summary>
            public static UIAdapter AddAdapter(GameObject target, UIAdapter.AdaptType type = UIAdapter.AdaptType.ScaleWithScreenSize)
            {
                return UIAdapter.AddUIAdapter(target, type);
            }

            /// <summary>
            /// 批量适配UI元素
            /// </summary>
            public static void AdaptElements(GameObject[] targets, UIAdapter.AdaptType type = UIAdapter.AdaptType.ScaleWithScreenSize)
            {
                UIAdapter.AdaptUIElements(targets, type);
            }

            /// <summary>
            /// 获取推荐的适配类型
            /// </summary>
            public static UIAdapter.AdaptType GetRecommendedAdaptType()
            {
                return UIAdapter.GetRecommendedAdaptType();
            }
        }

        /// <summary>
        /// 诊断工具
        /// </summary>
        public static class Diagnostics
        {
            /// <summary>
            /// 获取完整的系统信息
            /// </summary>
            public static string GetSystemInfo()
            {
                return $"Platform: {PlatformDetector.GetDeviceInfo()}\n" +
                       $"Performance: {PerformanceManager.Instance.GetPerformanceInfo()}\n" +
                       $"Network: {NetworkManager.Instance.GetNetworkInfo()}\n" +
                       $"Storage: {DataManager.GetStorageInfo()}";
            }

            /// <summary>
            /// 输出所有工具类状态
            /// </summary>
            public static void LogAllStatus()
            {
                UnityEngine.Debug.Log($"[GameUtils] System Info:\n{GetSystemInfo()}");
            }
        }
    }
}
