using UnityEngine;
using UnityEngine.UI;
using GK;
using GK.Managers;
using GameUtils = GK.GameUtils;

namespace GK.Examples
{
    /// <summary>
    /// GK工具类综合示例
    /// 展示所有工具类的标准使用方法和最佳实践
    /// </summary>
    public class ComprehensiveExample : MonoBehaviour
    {
        #region UI组件引用
        [Header("UI组件")]
        [SerializeField] private Text statusText;
        [SerializeField] private Button initButton;
        [SerializeField] private Button platformButton;
        [SerializeField] private Button audioButton;
        [SerializeField] private Button dataButton;
        [SerializeField] private Button networkButton;
        [SerializeField] private Button performanceButton;
        [SerializeField] private Slider volumeSlider;
        [SerializeField] private Toggle encryptionToggle;
        #endregion

        #region 配置参数
        [Header("配置参数")]
        [SerializeField] private string testApiUrl = "https://httpbin.org/get";
        [SerializeField] private string testMusicClip = "background_music";
        [SerializeField] private string testSfxClip = "button_click";
        [SerializeField] private bool autoInitialize = true;
        #endregion

        #region 私有变量
        private bool _isInitialized = false;
        private float _updateTimer = 0f;
        private const float UPDATE_INTERVAL = 1f;
        #endregion

        #region Unity生命周期
        private void Start()
        {
            InitializeUI();

            if (autoInitialize)
            {
                InitializeGameUtils();
            }

            RegisterInputEvents();
            LogSystemInfo();
        }

        private void Update()
        {
            UpdateStatusDisplay();
        }

        private void OnDestroy()
        {
            UnregisterInputEvents();
        }
        #endregion

        #region 初始化方法
        /// <summary>
        /// 初始化UI组件
        /// </summary>
        private void InitializeUI()
        {
            // 设置按钮事件
            if (initButton != null)
                initButton.onClick.AddListener(InitializeGameUtils);

            if (platformButton != null)
                platformButton.onClick.AddListener(TestPlatformDetection);

            if (audioButton != null)
                audioButton.onClick.AddListener(TestAudioSystem);

            if (dataButton != null)
                dataButton.onClick.AddListener(TestDataStorage);

            if (networkButton != null)
                networkButton.onClick.AddListener(TestNetworkSystem);

            if (performanceButton != null)
                performanceButton.onClick.AddListener(TestPerformanceSystem);

            // 设置滑块和开关事件
            if (volumeSlider != null)
            {
                volumeSlider.value = 0.8f;
                volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
            }

            if (encryptionToggle != null)
            {
                encryptionToggle.onValueChanged.AddListener(OnEncryptionToggled);
            }

            UpdateStatusText("[GK] UI initialized. Ready to test.");
        }

        /// <summary>
        /// 初始化GameUtils工具类
        /// </summary>
        [ContextMenu("Initialize GameUtils")]
        public void InitializeGameUtils()
        {
            if (_isInitialized)
            {
                UpdateStatusText("[GK] Already initialized.");
                return;
            }

            try
            {
                GameUtils.Initialize();
                _isInitialized = true;
                UpdateStatusText("[GK] GameUtils initialized successfully!");

                // 执行屏幕适配
                GameUtils.Screen.Adapt();

                Debug.Log("[GK] Comprehensive example initialized successfully.");
            }
            catch (System.Exception e)
            {
                UpdateStatusText($"[GK] Initialization failed: {e.Message}");
                Debug.LogError($"[GK] Failed to initialize GameUtils: {e}");
            }
        }
        #endregion

        #region 测试方法
        /// <summary>
        /// 测试平台检测功能
        /// </summary>
        [ContextMenu("Test Platform Detection")]
        public void TestPlatformDetection()
        {
            var platform = GameUtils.Platform.GetCurrentPlatform();
            bool isMobile = GameUtils.Platform.IsMobile();
            bool isMiniGame = GameUtils.Platform.IsMiniGame();
            string deviceInfo = GameUtils.Platform.GetDeviceInfo();

            string result = $"Platform: {platform}\n" +
                           $"Mobile: {isMobile}\n" +
                           $"MiniGame: {isMiniGame}\n" +
                           $"Device: {deviceInfo}";

            UpdateStatusText($"[Platform]\n{result}");
            Debug.Log($"[GK] Platform Detection Result:\n{result}");
        }

        /// <summary>
        /// 测试音频系统
        /// </summary>
        [ContextMenu("Test Audio System")]
        public void TestAudioSystem()
        {
            try
            {
                // 播放音效
                GameUtils.Audio.PlaySfx(testSfxClip);

                // 播放背景音乐
                GameUtils.Audio.PlayMusic(testMusicClip);

                // 获取音量信息
                float masterVolume = AudioManager.Instance.MasterVolume;
                float musicVolume = AudioManager.Instance.MusicVolume;
                float sfxVolume = AudioManager.Instance.SfxVolume;

                string result = $"Master: {masterVolume:F2}\n" +
                               $"Music: {musicVolume:F2}\n" +
                               $"SFX: {sfxVolume:F2}";

                UpdateStatusText($"[Audio]\n{result}");
                Debug.Log($"[GK] Audio test completed. Volumes - {result.Replace("\n", ", ")}");
            }
            catch (System.Exception e)
            {
                UpdateStatusText($"[Audio] Error: {e.Message}");
                Debug.LogError($"[GK] Audio test failed: {e}");
            }
        }

        /// <summary>
        /// 测试数据存储
        /// </summary>
        [ContextMenu("Test Data Storage")]
        public void TestDataStorage()
        {
            try
            {
                // 创建测试数据
                var testData = new TestGameData
                {
                    playerName = "TestPlayer",
                    level = Random.Range(1, 100),
                    score = Random.Range(1000, 99999),
                    lastPlayTime = System.DateTime.Now.ToString()
                };

                // 保存数据
                GameUtils.Data.Save("test_game_data", testData);

                // 加载数据
                var loadedData = GameUtils.Data.Load<TestGameData>("test_game_data");

                string result = $"Name: {loadedData.playerName}\n" +
                               $"Level: {loadedData.level}\n" +
                               $"Score: {loadedData.score}\n" +
                               $"Time: {loadedData.lastPlayTime}";

                UpdateStatusText($"[Data]\n{result}");
                Debug.Log($"[GK] Data storage test completed: {result.Replace("\n", ", ")}");
            }
            catch (System.Exception e)
            {
                UpdateStatusText($"[Data] Error: {e.Message}");
                Debug.LogError($"[GK] Data storage test failed: {e}");
            }
        }

        /// <summary>
        /// 测试网络系统
        /// </summary>
        [ContextMenu("Test Network System")]
        public void TestNetworkSystem()
        {
            if (!GameUtils.Network.IsNetworkAvailable())
            {
                UpdateStatusText("[Network] No network available");
                return;
            }

            UpdateStatusText("[Network] Testing...");

            GameUtils.Network.Get(testApiUrl, (success, data, error) =>
            {
                if (success)
                {
                    string preview = data.Length > 100 ? data.Substring(0, 100) + "..." : data;
                    UpdateStatusText($"[Network] Success\nLength: {data.Length}\nPreview: {preview}");
                    Debug.Log($"[GK] Network test successful. Data length: {data.Length}");
                }
                else
                {
                    UpdateStatusText($"[Network] Failed\nError: {error}");
                    Debug.LogError($"[GK] Network test failed: {error}");
                }
            });
        }

        /// <summary>
        /// 测试性能系统
        /// </summary>
        [ContextMenu("Test Performance System")]
        public void TestPerformanceSystem()
        {
            float currentFPS = GameUtils.Performance.GetCurrentFPS();
            float averageFPS = GameUtils.Performance.GetAverageFPS();
            float memoryUsage = GameUtils.Performance.GetMemoryUsage();

            string result = $"Current FPS: {currentFPS:F1}\n" +
                           $"Average FPS: {averageFPS:F1}\n" +
                           $"Memory: {memoryUsage:F1}MB";

            UpdateStatusText($"[Performance]\n{result}");
            Debug.Log($"[GK] Performance info: {result.Replace("\n", ", ")}");

            // 强制垃圾回收
            GameUtils.Performance.ForceGC();
        }
        #endregion

        #region 事件处理
        /// <summary>
        /// 音量改变事件
        /// </summary>
        private void OnVolumeChanged(float value)
        {
            GameUtils.Audio.SetMasterVolume(value);
            Debug.Log($"[GK] Master volume changed to: {value:F2}");
        }

        /// <summary>
        /// 加密开关事件
        /// </summary>
        private void OnEncryptionToggled(bool enabled)
        {
            if (enabled)
            {
                GameUtils.Data.EnableEncryption("GK_SecretKey_2024");
                Debug.Log("[GK] Data encryption enabled");
            }
            else
            {
                DataManager.UseEncryption = false;
                Debug.Log("[GK] Data encryption disabled");
            }
        }

        /// <summary>
        /// 注册输入事件
        /// </summary>
        private void RegisterInputEvents()
        {
            InputManager.OnTap += OnTap;
            InputManager.OnSwipe += OnSwipe;
        }

        /// <summary>
        /// 取消注册输入事件
        /// </summary>
        private void UnregisterInputEvents()
        {
            InputManager.OnTap -= OnTap;
            InputManager.OnSwipe -= OnSwipe;
        }

        /// <summary>
        /// 点击事件处理
        /// </summary>
        private void OnTap(Vector2 position)
        {
            Debug.Log($"[GK] Tap detected at: {position}");
        }

        /// <summary>
        /// 滑动事件处理
        /// </summary>
        private void OnSwipe(Vector2 startPos, Vector2 endPos)
        {
            Vector2 direction = InputManager.GetSwipeDirection(startPos, endPos);
            Debug.Log($"[GK] Swipe detected: {startPos} -> {endPos}, Direction: {direction}");
        }
        #endregion

        #region 辅助方法
        /// <summary>
        /// 更新状态显示
        /// </summary>
        private void UpdateStatusDisplay()
        {
            _updateTimer += Time.deltaTime;
            if (_updateTimer >= UPDATE_INTERVAL && _isInitialized)
            {
                _updateTimer = 0f;

                // 更新实时信息
                if (statusText != null && statusText.text.StartsWith("[Status]"))
                {
                    string info = $"[Status]\n" +
                                 $"FPS: {GameUtils.Performance.GetCurrentFPS():F1}\n" +
                                 $"Memory: {GameUtils.Performance.GetMemoryUsage():F1}MB\n" +
                                 $"Platform: {GameUtils.Platform.GetCurrentPlatform()}\n" +
                                 $"Network: {GameUtils.Network.GetNetworkType()}";

                    statusText.text = info;
                }
            }
        }

        /// <summary>
        /// 更新状态文本
        /// </summary>
        private void UpdateStatusText(string text)
        {
            if (statusText != null)
            {
                statusText.text = text;
            }
        }

        /// <summary>
        /// 输出系统信息
        /// </summary>
        private void LogSystemInfo()
        {
            string systemInfo = GameUtils.Diagnostics.GetSystemInfo();
            Debug.Log($"[GK] System Information:\n{systemInfo}");
        }

        /// <summary>
        /// 显示实时状态
        /// </summary>
        [ContextMenu("Show Real-time Status")]
        public void ShowRealtimeStatus()
        {
            UpdateStatusText("[Status]\nReal-time monitoring...");
        }
        #endregion

        #region 数据结构
        /// <summary>
        /// 测试游戏数据结构
        /// </summary>
        [System.Serializable]
        public class TestGameData
        {
            public string playerName;
            public int level;
            public int score;
            public string lastPlayTime;
        }
        #endregion
    }
}
