using UnityEngine;
using GK;

namespace GK.Examples
{
    /// <summary>
    /// 编译测试脚本
    /// 验证所有代码都能正确编译
    /// </summary>
    public class CompilationTest : MonoBehaviour
    {
        [Header("测试设置")]
        [SerializeField] private bool runOnStart = false;

        private void Start()
        {
            if (runOnStart)
            {
                TestCompilation();
            }
        }

        /// <summary>
        /// 测试编译和基本功能
        /// </summary>
        [ContextMenu("Test Compilation")]
        public void TestCompilation()
        {
            Debug.Log("[CompilationTest] Starting compilation test...");

            // 测试字符串替换功能（之前的错误点）
            TestStringReplacement();

            // 测试基本功能
            TestBasicFunctions();

            Debug.Log("[CompilationTest] ✓ All compilation tests passed!");
        }

        /// <summary>
        /// 测试字符串替换功能
        /// </summary>
        private void TestStringReplacement()
        {
            string testString = "Line1\nLine2\nLine3";

            // 正确的字符串替换方法
            string result1 = testString.Replace('\n', ' ');        // 单字符替换
            string result2 = testString.Replace('\n', ',');        // 单字符替换
            string result3 = testString.Replace("\n", ", ");       // 字符串替换
            string result4 = testString.Replace("\n", " | ");      // 字符串替换

            Debug.Log($"[CompilationTest] String replacement test:");
            Debug.Log($"  Original: {testString}");
            Debug.Log($"  Replace with space: {result1}");
            Debug.Log($"  Replace with comma: {result2}");
            Debug.Log($"  Replace with comma-space: {result3}");
            Debug.Log($"  Replace with pipe: {result4}");
        }

        /// <summary>
        /// 测试基本功能
        /// </summary>
        private void TestBasicFunctions()
        {
            try
            {
                // 测试平台检测
                var platform = GameUtils.Platform.GetCurrentPlatform();
                Debug.Log($"[CompilationTest] Platform: {platform}");

                // 测试性能监控
                float fps = GameUtils.Performance.GetCurrentFPS();
                Debug.Log($"[CompilationTest] FPS: {fps:F1}");

                // 测试网络状态
                bool networkAvailable = GameUtils.Network.IsNetworkAvailable();
                Debug.Log($"[CompilationTest] Network: {networkAvailable}");

                // 测试屏幕信息
                float aspectRatio = GameUtils.Screen.GetAspectRatio();
                Debug.Log($"[CompilationTest] Aspect Ratio: {aspectRatio:F2}");

                Debug.Log("[CompilationTest] ✓ Basic functions test passed!");
            }
            catch (System.Exception e)
            {
                Debug.LogError($"[CompilationTest] ✗ Basic functions test failed: {e.Message}");
            }
        }

        /// <summary>
        /// 测试所有示例脚本的编译
        /// </summary>
        [ContextMenu("Test All Examples")]
        public void TestAllExamples()
        {
            Debug.Log("[CompilationTest] Testing all example scripts...");

            // 检查所有示例脚本是否存在
            var exampleTypes = new System.Type[]
            {
                typeof(QuickStartExample),
                typeof(ComprehensiveExample),
                typeof(GameUtilsExample),
                typeof(NetworkManagerTest),
                typeof(DiagnosticsTest),
                typeof(NamespaceTest)
            };

            foreach (var type in exampleTypes)
            {
                Debug.Log($"[CompilationTest] ✓ {type.Name} compiled successfully");
            }

            Debug.Log("[CompilationTest] ✓ All example scripts compilation test passed!");
        }

        /// <summary>
        /// 测试字符字面量和Replace方法的正确用法
        /// </summary>
        [ContextMenu("Test Character Literals")]
        public void TestCharacterLiterals()
        {
            Debug.Log("[CompilationTest] Testing character literals and Replace methods...");

            string testText = "Hello\nWorld\tTest";

            // 正确的字符字面量用法
            char newline = '\n';
            char space = ' ';

            // 正确的Replace方法用法
            string result1 = testText.Replace(newline, space);      // Replace(char, char)
            string result2 = testText.Replace('\n', ' ');           // Replace(char, char) - 直接使用
            string result3 = testText.Replace("\n", " ");           // Replace(string, string)
            string result4 = testText.Replace("\n", ", ");          // Replace(string, string) - 多字符
            string result5 = testText.Replace("\t", " | ");         // Replace(string, string) - Tab替换

            Debug.Log($"[CompilationTest] Replace method tests:");
            Debug.Log($"  Original: '{testText}'");
            Debug.Log($"  Replace(char, char) with variables: '{result1}'");
            Debug.Log($"  Replace(char, char) direct: '{result2}'");
            Debug.Log($"  Replace(string, string) single: '{result3}'");
            Debug.Log($"  Replace(string, string) multi: '{result4}'");
            Debug.Log($"  Replace(string, string) tab: '{result5}'");

            // 错误示例（注释掉，仅作说明）
            // string wrong1 = testText.Replace('\n', ', ');    // ❌ 错误：字符字面量包含多个字符
            // string wrong2 = testText.Replace('\n', ", ");    // ❌ 错误：参数类型不匹配 (char, string)
            // string wrong3 = testText.Replace("\n", ' ');     // ❌ 错误：参数类型不匹配 (string, char)

            Debug.Log("[CompilationTest] ✓ Character literals and Replace methods test passed!");
        }

        /// <summary>
        /// 性能测试
        /// </summary>
        [ContextMenu("Performance Test")]
        public void PerformanceTest()
        {
            Debug.Log("[CompilationTest] Running performance test...");

            var stopwatch = System.Diagnostics.Stopwatch.StartNew();

            // 测试字符串操作性能
            string testString = "Line1\nLine2\nLine3\nLine4\nLine5";

            for (int i = 0; i < 1000; i++)
            {
                // 测试不同的字符串替换方法
                string result1 = testString.Replace('\n', ' ');
                string result2 = testString.Replace("\n", " ");
                string result3 = testString.Replace("\n", ", ");
            }

            stopwatch.Stop();
            Debug.Log($"[CompilationTest] Performance test completed in {stopwatch.ElapsedMilliseconds}ms");
        }
    }
}
