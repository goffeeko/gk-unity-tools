using UnityEngine;
using UnityEngine.UI;
using GK;
using GK.Managers;
using GameUtils = GK.GameUtils;

namespace GK.Examples
{
    /// <summary>
    /// 游戏工具类使用示例
    /// 展示如何使用各种工具类功能
    /// </summary>
    public class GameUtilsExample : MonoBehaviour
    {
        [Header("UI引用")]
        [SerializeField] private Text infoText;
        [SerializeField] private Button[] testButtons;
        [SerializeField] private Slider volumeSlider;
        [SerializeField] private Toggle encryptionToggle;

        [Header("测试数据")]
        [SerializeField] private string testMusicClip = "background_music";
        [SerializeField] private string testSfxClip = "button_click";
        [SerializeField] private string testApiUrl = "https://api.example.com/test";

        private void Start()
        {
            // 初始化游戏工具类
            GameUtils.Initialize();

            // 设置UI事件
            SetupUI();

            // 注册输入事件
            RegisterInputEvents();

            // 开始更新信息显示
            InvokeRepeating(nameof(UpdateInfoDisplay), 1f, 1f);

            // 示例：保存一些测试数据
            SaveExampleData();
        }

        /// <summary>
        /// 设置UI
        /// </summary>
        private void SetupUI()
        {
            // 为所有按钮添加UI适配
            if (testButtons != null)
            {
                GameUtils.UI.AdaptElements(System.Array.ConvertAll(testButtons, b => b.gameObject));
            }

            // 设置音量滑块
            if (volumeSlider != null)
            {
                volumeSlider.value = 0.8f;
                volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
            }

            // 设置加密开关
            if (encryptionToggle != null)
            {
                encryptionToggle.onValueChanged.AddListener(OnEncryptionToggled);
            }

            // 为按钮设置点击事件
            SetupButtonEvents();
        }

        /// <summary>
        /// 设置按钮事件
        /// </summary>
        private void SetupButtonEvents()
        {
            if (testButtons == null || testButtons.Length < 6) return;

            testButtons[0].onClick.AddListener(TestPlatformDetection);
            testButtons[1].onClick.AddListener(TestAudioPlayback);
            testButtons[2].onClick.AddListener(TestDataStorage);
            testButtons[3].onClick.AddListener(TestNetworkRequest);
            testButtons[4].onClick.AddListener(TestPerformanceInfo);
            testButtons[5].onClick.AddListener(TestScreenAdaptation);
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
        /// 更新信息显示
        /// </summary>
        private void UpdateInfoDisplay()
        {
            if (infoText == null) return;

            string info = $"平台: {GameUtils.Platform.GetCurrentPlatform()}\n" +
                         $"FPS: {GameUtils.Performance.GetCurrentFPS():F1}\n" +
                         $"内存: {GameUtils.Performance.GetMemoryUsage():F1}MB\n" +
                         $"网络: {GameUtils.Network.GetNetworkType()}\n" +
                         $"屏幕: {Screen.width}x{Screen.height}\n" +
                         $"安全区域: {GameUtils.Screen.HasSafeArea()}";

            infoText.text = info;
        }

        /// <summary>
        /// 测试平台检测
        /// </summary>
        public void TestPlatformDetection()
        {
            string platformInfo = $"当前平台: {GameUtils.Platform.GetCurrentPlatform()}\n" +
                                 $"是否移动设备: {GameUtils.Platform.IsMobile()}\n" +
                                 $"是否小游戏: {GameUtils.Platform.IsMiniGame()}\n" +
                                 $"设备信息: {GameUtils.Platform.GetDeviceInfo()}";

            Debug.Log($"[平台检测] {platformInfo}");
            ShowMessage("平台检测完成，查看控制台输出");
        }

        /// <summary>
        /// 测试音频播放
        /// </summary>
        public void TestAudioPlayback()
        {
            // 播放音效
            GameUtils.Audio.PlaySfx(testSfxClip);

            // 播放背景音乐
            GameUtils.Audio.PlayMusic(testMusicClip);

            ShowMessage("音频播放测试完成");
        }

        /// <summary>
        /// 测试数据存储
        /// </summary>
        public void TestDataStorage()
        {
            // 保存测试数据
            var testData = new TestData
            {
                playerName = "TestPlayer",
                level = 10,
                score = 12345,
                lastPlayTime = System.DateTime.Now.ToString()
            };

            GameUtils.Data.Save("test_data", testData);

            // 加载数据
            var loadedData = GameUtils.Data.Load<TestData>("test_data");

            string message = $"数据存储测试完成\n" +
                           $"玩家: {loadedData.playerName}\n" +
                           $"等级: {loadedData.level}\n" +
                           $"分数: {loadedData.score}";

            ShowMessage(message);
        }

        /// <summary>
        /// 测试网络请求
        /// </summary>
        public void TestNetworkRequest()
        {
            if (!GameUtils.Network.IsNetworkAvailable())
            {
                ShowMessage("网络不可用");
                return;
            }

            ShowMessage("发送网络请求中...");

            GameUtils.Network.Get(testApiUrl, (success, data, error) =>
            {
                if (success)
                {
                    ShowMessage($"网络请求成功: {data.Substring(0, Mathf.Min(50, data.Length))}...");
                }
                else
                {
                    ShowMessage($"网络请求失败: {error}");
                }
            });
        }

        /// <summary>
        /// 测试性能信息
        /// </summary>
        public void TestPerformanceInfo()
        {
            string perfInfo = $"当前FPS: {GameUtils.Performance.GetCurrentFPS():F1}\n" +
                             $"平均FPS: {GameUtils.Performance.GetAverageFPS():F1}\n" +
                             $"内存使用: {GameUtils.Performance.GetMemoryUsage():F1}MB";

            ShowMessage(perfInfo);

            // 强制垃圾回收
            GameUtils.Performance.ForceGC();
        }

        /// <summary>
        /// 测试屏幕适配
        /// </summary>
        public void TestScreenAdaptation()
        {
            GameUtils.Screen.Adapt();

            string screenInfo = $"屏幕尺寸: {Screen.width}x{Screen.height}\n" +
                               $"宽高比: {GameUtils.Screen.GetAspectRatio():F2}\n" +
                               $"安全区域: {GameUtils.Screen.GetSafeArea()}";

            ShowMessage(screenInfo);
        }

        /// <summary>
        /// 音量改变事件
        /// </summary>
        private void OnVolumeChanged(float value)
        {
            GameUtils.Audio.SetMasterVolume(value);
        }

        /// <summary>
        /// 加密开关事件
        /// </summary>
        private void OnEncryptionToggled(bool enabled)
        {
            if (enabled)
            {
                GameUtils.Data.EnableEncryption("MySecretKey2024");
                ShowMessage("数据加密已启用");
            }
            else
            {
                DataManager.UseEncryption = false;
                ShowMessage("数据加密已禁用");
            }
        }

        /// <summary>
        /// 点击事件
        /// </summary>
        private void OnTap(Vector2 position)
        {
            Debug.Log($"点击位置: {position}");
        }

        /// <summary>
        /// 滑动事件
        /// </summary>
        private void OnSwipe(Vector2 startPos, Vector2 endPos)
        {
            Vector2 direction = InputManager.GetSwipeDirection(startPos, endPos);
            Debug.Log($"滑动方向: {direction}");
        }

        /// <summary>
        /// 保存示例数据
        /// </summary>
        private void SaveExampleData()
        {
            var gameSettings = new GameSettings
            {
                musicVolume = 0.8f,
                sfxVolume = 1.0f,
                language = "zh-CN",
                quality = 2
            };

            GameUtils.Data.Save("game_settings", gameSettings);
        }

        /// <summary>
        /// 显示消息
        /// </summary>
        private void ShowMessage(string message)
        {
            Debug.Log($"[示例] {message}");
            // 这里可以显示UI提示
        }

        private void OnDestroy()
        {
            // 取消注册事件
            InputManager.OnTap -= OnTap;
            InputManager.OnSwipe -= OnSwipe;
        }

        /// <summary>
        /// 测试数据结构
        /// </summary>
        [System.Serializable]
        public class TestData
        {
            public string playerName;
            public int level;
            public int score;
            public string lastPlayTime;
        }

        /// <summary>
        /// 游戏设置数据结构
        /// </summary>
        [System.Serializable]
        public class GameSettings
        {
            public float musicVolume;
            public float sfxVolume;
            public string language;
            public int quality;
        }
    }
}
