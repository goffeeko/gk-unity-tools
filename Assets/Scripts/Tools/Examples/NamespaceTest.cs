using UnityEngine;
using GK;
using GK.Platform;
using GK.Managers;
using GameUtils = GK.GameUtils;

namespace GK.Examples
{
    /// <summary>
    /// 命名空间测试脚本
    /// 验证GK命名空间重构是否成功
    /// </summary>
    public class NamespaceTest : MonoBehaviour
    {
        [Header("测试设置")]
        [SerializeField] private bool autoTest = false;

        private void Start()
        {
            if (autoTest)
            {
                TestNamespaces();
            }
        }

        /// <summary>
        /// 测试命名空间功能
        /// </summary>
        [ContextMenu("Test Namespaces")]
        public void TestNamespaces()
        {
            Debug.Log("[NamespaceTest] Testing GK namespace structure...");

            // 测试1: 直接使用GameUtils (推荐方式)
            TestDirectGameUtils();

            // 测试2: 使用GK命名空间
            TestGKNamespace();

            // 测试3: 直接使用具体类
            TestDirectClasses();

            Debug.Log("[NamespaceTest] All namespace tests completed successfully!");
        }

        /// <summary>
        /// 测试直接使用GameUtils
        /// </summary>
        private void TestDirectGameUtils()
        {
            Debug.Log("[NamespaceTest] Testing direct GameUtils usage...");

            // 这些调用应该正常工作，无需额外using语句
            var platform = GameUtils.Platform.GetCurrentPlatform();
            bool isMobile = GameUtils.Platform.IsMobile();
            bool isMiniGame = GameUtils.Platform.IsMiniGame();

            Debug.Log($"Platform: {platform}, Mobile: {isMobile}, MiniGame: {isMiniGame}");

            // 测试其他功能
            float aspectRatio = GameUtils.Screen.GetAspectRatio();
            bool hasNetwork = GameUtils.Network.IsNetworkAvailable();

            Debug.Log($"Aspect Ratio: {aspectRatio:F2}, Network: {hasNetwork}");
        }

        /// <summary>
        /// 测试GK命名空间
        /// </summary>
        private void TestGKNamespace()
        {
            Debug.Log("[NamespaceTest] Testing GK namespace usage...");

            // 使用GK命名空间下的GameUtils
            var systemInfo = GameUtils.Diagnostics.GetSystemInfo();
            Debug.Log($"System Info Length: {systemInfo.Length}");

            // 测试性能功能
            float fps = GameUtils.Performance.GetCurrentFPS();
            float memory = GameUtils.Performance.GetMemoryUsage();

            Debug.Log($"FPS: {fps:F1}, Memory: {memory:F1}MB");
        }

        /// <summary>
        /// 测试直接使用具体类
        /// </summary>
        private void TestDirectClasses()
        {
            Debug.Log("[NamespaceTest] Testing direct class usage...");

            // 直接使用GK命名空间下的类
            var currentPlatform = PlatformDetector.CurrentPlatform;
            var deviceInfo = PlatformDetector.GetDeviceInfo();

            Debug.Log($"Direct Platform: {currentPlatform}");
            Debug.Log($"Device Info: {deviceInfo}");

            // 测试管理器实例
            var audioManager = AudioManager.Instance;
            var inputManager = InputManager.Instance;
            var networkManager = NetworkManager.Instance;

            Debug.Log($"Managers initialized: Audio={audioManager != null}, " +
                     $"Input={inputManager != null}, Network={networkManager != null}");
        }

        /// <summary>
        /// 测试命名空间隔离
        /// </summary>
        [ContextMenu("Test Namespace Isolation")]
        public void TestNamespaceIsolation()
        {
            Debug.Log("[NamespaceTest] Testing namespace isolation...");

            // Unity的Debug应该正常工作
            Debug.Log("Unity Debug.Log works!");
            Debug.LogWarning("Unity Debug.LogWarning works!");
            Debug.LogError("Unity Debug.LogError works!");

            // GK的Diagnostics应该正常工作
            GameUtils.Diagnostics.LogAllStatus();

            Debug.Log("[NamespaceTest] Namespace isolation test completed!");
        }

        /// <summary>
        /// 测试不同的using方式
        /// </summary>
        [ContextMenu("Test Different Using Styles")]
        public void TestDifferentUsingStyles()
        {
            Debug.Log("[NamespaceTest] Testing different using styles...");

            // 方式1: 无需using，直接使用
            GameUtils.Initialize();

            // 方式2: 使用GK命名空间 (已在文件顶部using GK)
            var platform1 = GameUtils.Platform.GetCurrentPlatform();

            // 方式3: 使用别名 (已在文件顶部using GameUtils = GK.GameUtils)
            var platform2 = GameUtils.Platform.GetCurrentPlatform();

            // 方式4: 直接使用具体类 (已在文件顶部using GK.Platform)
            var platform3 = PlatformDetector.CurrentPlatform;

            Debug.Log($"All methods return same result: {platform1 == platform2 && platform2 == platform3}");
            Debug.Log("[NamespaceTest] Different using styles test completed!");
        }

        /// <summary>
        /// 性能测试
        /// </summary>
        [ContextMenu("Test Performance")]
        public void TestPerformance()
        {
            Debug.Log("[NamespaceTest] Testing performance...");

            var stopwatch = System.Diagnostics.Stopwatch.StartNew();

            // 测试1000次调用的性能
            for (int i = 0; i < 1000; i++)
            {
                var platform = GameUtils.Platform.GetCurrentPlatform();
                var isMobile = GameUtils.Platform.IsMobile();
            }

            stopwatch.Stop();
            Debug.Log($"1000 calls took: {stopwatch.ElapsedMilliseconds}ms");
            Debug.Log("[NamespaceTest] Performance test completed!");
        }
    }
}
