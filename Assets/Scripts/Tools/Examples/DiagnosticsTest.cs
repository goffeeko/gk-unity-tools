using UnityEngine;
using GK;
using GameUtils = GK.GameUtils;

namespace GK.Examples
{
    /// <summary>
    /// 诊断工具测试脚本
    /// 验证Debug类重命名为Diagnostics后的功能
    /// </summary>
    public class DiagnosticsTest : MonoBehaviour
    {
        [Header("测试设置")]
        [SerializeField] private bool autoTest = false;

        private void Start()
        {
            if (autoTest)
            {
                TestDiagnostics();
            }
        }

        /// <summary>
        /// 测试诊断功能
        /// </summary>
        [ContextMenu("Test Diagnostics")]
        public void TestDiagnostics()
        {
            Debug.Log("[DiagnosticsTest] Testing GK.Diagnostics...");

            // 测试获取系统信息
            string systemInfo = GameUtils.Diagnostics.GetSystemInfo();
            Debug.Log($"[DiagnosticsTest] System Info:\n{systemInfo}");

            // 测试输出所有状态
            GameUtils.Diagnostics.LogAllStatus();

            Debug.Log("[DiagnosticsTest] Diagnostics test completed successfully!");
        }

        /// <summary>
        /// 测试各个工具类的状态
        /// </summary>
        [ContextMenu("Test Individual Tools")]
        public void TestIndividualTools()
        {
            Debug.Log("[DiagnosticsTest] Testing individual tool status...");

            // 平台信息
            Debug.Log($"Platform: {GameUtils.Platform.GetCurrentPlatform()}");
            Debug.Log($"Is Mobile: {GameUtils.Platform.IsMobile()}");
            Debug.Log($"Is Mini Game: {GameUtils.Platform.IsMiniGame()}");

            // 性能信息
            Debug.Log($"Current FPS: {GameUtils.Performance.GetCurrentFPS():F1}");
            Debug.Log($"Memory Usage: {GameUtils.Performance.GetMemoryUsage():F1}MB");

            // 网络信息
            Debug.Log($"Network Available: {GameUtils.Network.IsNetworkAvailable()}");
            Debug.Log($"Network Type: {GameUtils.Network.GetNetworkType()}");

            // 屏幕信息
            Debug.Log($"Screen Aspect Ratio: {GameUtils.Screen.GetAspectRatio():F2}");
            Debug.Log($"Has Safe Area: {GameUtils.Screen.HasSafeArea()}");

            Debug.Log("[DiagnosticsTest] Individual tools test completed!");
        }

        /// <summary>
        /// 验证命名空间冲突已解决
        /// </summary>
        [ContextMenu("Test Debug Namespace")]
        public void TestDebugNamespace()
        {
            // 这应该使用Unity的Debug类，不会有命名冲突
            Debug.Log("[DiagnosticsTest] Unity Debug.Log works correctly!");
            Debug.LogWarning("[DiagnosticsTest] Unity Debug.LogWarning works correctly!");
            Debug.LogError("[DiagnosticsTest] Unity Debug.LogError works correctly!");

            // 这应该使用GK的Diagnostics类
            string info = GameUtils.Diagnostics.GetSystemInfo();
            Debug.Log($"[DiagnosticsTest] GK.Diagnostics works correctly! Info length: {info.Length}");

            Debug.Log("[DiagnosticsTest] No namespace conflicts detected!");
        }
    }
}
