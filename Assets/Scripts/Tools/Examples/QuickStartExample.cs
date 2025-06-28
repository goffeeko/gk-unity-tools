using UnityEngine;
using GK;

namespace GK.Examples
{
    /// <summary>
    /// GK工具类快速开始示例
    /// 展示最基本的使用方法，适合新手快速上手
    /// </summary>
    public class QuickStartExample : MonoBehaviour
    {
        [Header("快速测试设置")]
        [SerializeField] private bool enableAutoTest = true;
        [SerializeField] private float testInterval = 5f;

        private void Start()
        {
            // 第一步：初始化GK工具类
            InitializeGK();

            // 第二步：检测平台
            DetectPlatform();

            // 第三步：适配屏幕
            AdaptScreen();

            // 第四步：设置音频
            SetupAudio();

            // 第五步：测试数据存储
            TestDataStorage();

            // 第六步：开始自动测试（可选）
            if (enableAutoTest)
            {
                InvokeRepeating(nameof(AutoTest), testInterval, testInterval);
            }
        }

        /// <summary>
        /// 第一步：初始化GK工具类
        /// </summary>
        private void InitializeGK()
        {
            Debug.Log("[QuickStart] Step 1: Initializing GK...");
            
            // 一行代码初始化所有工具
            GameUtils.Initialize();
            
            Debug.Log("[QuickStart] ✓ GK initialized successfully!");
        }

        /// <summary>
        /// 第二步：检测平台
        /// </summary>
        private void DetectPlatform()
        {
            Debug.Log("[QuickStart] Step 2: Detecting platform...");
            
            // 获取当前平台
            var platform = GameUtils.Platform.GetCurrentPlatform();
            
            // 检查平台类型
            if (GameUtils.Platform.IsMobile())
            {
                Debug.Log("[QuickStart] ✓ Running on mobile device");
                // 移动设备特定设置
                GameUtils.Performance.SetTargetFrameRate(30);
            }
            else if (GameUtils.Platform.IsMiniGame())
            {
                Debug.Log("[QuickStart] ✓ Running on mini-game platform");
                // 小游戏特定设置
                GameUtils.Performance.SetTargetFrameRate(60);
            }
            else
            {
                Debug.Log($"[QuickStart] ✓ Running on {platform}");
                // 桌面平台设置
                GameUtils.Performance.SetTargetFrameRate(60);
            }
        }

        /// <summary>
        /// 第三步：适配屏幕
        /// </summary>
        private void AdaptScreen()
        {
            Debug.Log("[QuickStart] Step 3: Adapting screen...");
            
            // 执行屏幕适配
            GameUtils.Screen.Adapt();
            
            // 获取屏幕信息
            float aspectRatio = GameUtils.Screen.GetAspectRatio();
            bool hasSafeArea = GameUtils.Screen.HasSafeArea();
            
            Debug.Log($"[QuickStart] ✓ Screen adapted. Aspect ratio: {aspectRatio:F2}, Safe area: {hasSafeArea}");
        }

        /// <summary>
        /// 第四步：设置音频
        /// </summary>
        private void SetupAudio()
        {
            Debug.Log("[QuickStart] Step 4: Setting up audio...");
            
            // 设置音量
            GameUtils.Audio.SetMasterVolume(0.8f);
            GameUtils.Audio.SetMusicVolume(0.7f);
            GameUtils.Audio.SetSfxVolume(1.0f);
            
            // 播放测试音效（如果有的话）
            GameUtils.Audio.PlaySfx("test_sound");
            
            Debug.Log("[QuickStart] ✓ Audio system configured");
        }

        /// <summary>
        /// 第五步：测试数据存储
        /// </summary>
        private void TestDataStorage()
        {
            Debug.Log("[QuickStart] Step 5: Testing data storage...");
            
            // 创建测试数据
            var gameSettings = new GameSettings
            {
                playerName = "QuickStartPlayer",
                volume = 0.8f,
                difficulty = 1,
                lastPlayTime = System.DateTime.Now.ToString()
            };
            
            // 保存数据
            GameUtils.Data.Save("quick_start_settings", gameSettings);
            
            // 加载数据验证
            var loadedSettings = GameUtils.Data.Load<GameSettings>("quick_start_settings");
            
            if (loadedSettings != null && loadedSettings.playerName == gameSettings.playerName)
            {
                Debug.Log("[QuickStart] ✓ Data storage working correctly");
            }
            else
            {
                Debug.LogWarning("[QuickStart] ⚠ Data storage test failed");
            }
        }

        /// <summary>
        /// 自动测试（每隔几秒运行一次）
        /// </summary>
        private void AutoTest()
        {
            Debug.Log("[QuickStart] Running auto test...");
            
            // 显示性能信息
            float fps = GameUtils.Performance.GetCurrentFPS();
            float memory = GameUtils.Performance.GetMemoryUsage();
            
            Debug.Log($"[QuickStart] Performance - FPS: {fps:F1}, Memory: {memory:F1}MB");
            
            // 检查网络状态
            bool networkAvailable = GameUtils.Network.IsNetworkAvailable();
            string networkType = GameUtils.Network.GetNetworkType();
            
            Debug.Log($"[QuickStart] Network - Available: {networkAvailable}, Type: {networkType}");
            
            // 输出诊断信息
            GameUtils.Diagnostics.LogAllStatus();
        }

        /// <summary>
        /// 手动测试所有功能
        /// </summary>
        [ContextMenu("Run Full Test")]
        public void RunFullTest()
        {
            Debug.Log("[QuickStart] Running full test suite...");
            
            // 重新初始化
            InitializeGK();
            
            // 测试平台检测
            DetectPlatform();
            
            // 测试屏幕适配
            AdaptScreen();
            
            // 测试音频
            SetupAudio();
            
            // 测试数据存储
            TestDataStorage();
            
            // 测试网络（如果可用）
            if (GameUtils.Network.IsNetworkAvailable())
            {
                GameUtils.Network.Get("https://httpbin.org/get", (success, data, error) =>
                {
                    if (success)
                    {
                        Debug.Log($"[QuickStart] ✓ Network test successful. Data length: {data.Length}");
                    }
                    else
                    {
                        Debug.LogWarning($"[QuickStart] ⚠ Network test failed: {error}");
                    }
                });
            }
            
            Debug.Log("[QuickStart] ✓ Full test completed!");
        }

        /// <summary>
        /// 显示使用提示
        /// </summary>
        [ContextMenu("Show Usage Tips")]
        public void ShowUsageTips()
        {
            Debug.Log(@"[QuickStart] GK工具类使用提示：

1. 初始化：GameUtils.Initialize()
2. 平台检测：GameUtils.Platform.IsMobile()
3. 屏幕适配：GameUtils.Screen.Adapt()
4. 音频播放：GameUtils.Audio.PlayMusic(""music"")
5. 数据存储：GameUtils.Data.Save(""key"", data)
6. 网络请求：GameUtils.Network.Get(url, callback)
7. 性能监控：GameUtils.Performance.GetCurrentFPS()
8. 系统诊断：GameUtils.Diagnostics.LogAllStatus()

更多详细信息请查看其他示例文件和文档。");
        }

        /// <summary>
        /// 游戏设置数据结构
        /// </summary>
        [System.Serializable]
        public class GameSettings
        {
            public string playerName;
            public float volume;
            public int difficulty;
            public string lastPlayTime;
        }

        private void OnDestroy()
        {
            // 清理资源
            CancelInvoke();
        }
    }
}
