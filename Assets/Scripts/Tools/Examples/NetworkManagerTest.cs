using UnityEngine;
using GK.Managers;

namespace GK.Examples
{
    /// <summary>
    /// NetworkManager 测试脚本
    /// 用于验证修复后的NetworkManager是否正常工作
    /// </summary>
    public class NetworkManagerTest : MonoBehaviour
    {
        [Header("测试设置")]
        [SerializeField] private string testUrl = "https://httpbin.org/get";
        [SerializeField] private bool autoTest = false;

        private void Start()
        {
            if (autoTest)
            {
                TestNetworkManager();
            }
        }

        /// <summary>
        /// 测试NetworkManager功能
        /// </summary>
        [ContextMenu("Test Network Manager")]
        public void TestNetworkManager()
        {
            Debug.Log("[NetworkTest] Starting NetworkManager test...");

            // 测试GET请求
            NetworkManager.Instance.Get(testUrl, OnTestResponse);
        }

        /// <summary>
        /// 测试响应回调
        /// </summary>
        private void OnTestResponse(bool success, string data, string error)
        {
            if (success)
            {
                Debug.Log($"[NetworkTest] Request successful! Data length: {data.Length}");
                Debug.Log($"[NetworkTest] Response preview: {data.Substring(0, Mathf.Min(200, data.Length))}...");
            }
            else
            {
                Debug.LogError($"[NetworkTest] Request failed: {error}");
            }
        }

        /// <summary>
        /// 测试POST请求
        /// </summary>
        [ContextMenu("Test POST Request")]
        public void TestPostRequest()
        {
            string postUrl = "https://httpbin.org/post";
            string jsonData = "{\"test\": \"data\", \"timestamp\": \"" + System.DateTime.Now.ToString() + "\"}";

            Debug.Log("[NetworkTest] Testing POST request...");
            NetworkManager.Instance.Post(postUrl, jsonData, OnTestResponse);
        }

        /// <summary>
        /// 测试网络状态
        /// </summary>
        [ContextMenu("Test Network Status")]
        public void TestNetworkStatus()
        {
            bool isAvailable = NetworkManager.Instance.IsNetworkAvailable();
            string networkType = NetworkManager.Instance.GetNetworkType();
            string networkInfo = NetworkManager.Instance.GetNetworkInfo();

            Debug.Log($"[NetworkTest] Network Available: {isAvailable}");
            Debug.Log($"[NetworkTest] Network Type: {networkType}");
            Debug.Log($"[NetworkTest] Network Info: {networkInfo}");
        }

        /// <summary>
        /// 测试重试机制
        /// </summary>
        [ContextMenu("Test Retry Mechanism")]
        public void TestRetryMechanism()
        {
            // 使用一个无效的URL来测试重试机制
            string invalidUrl = "https://invalid-url-for-testing-retry.com/test";

            Debug.Log("[NetworkTest] Testing retry mechanism with invalid URL...");
            NetworkManager.Instance.Get(invalidUrl, (success, data, error) =>
            {
                Debug.Log($"[NetworkTest] Retry test completed. Success: {success}, Error: {error}");
            });
        }
    }
}
