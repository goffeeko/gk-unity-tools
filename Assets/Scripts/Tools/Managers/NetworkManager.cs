using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System;
using GK.Platform;

namespace GK.Managers
{
    /// <summary>
    /// 网络管理器
    /// 处理HTTP请求和网络连接，适配小游戏平台限制
    /// </summary>
    public class NetworkManager : MonoBehaviour
    {
        [Header("网络设置")]
        [SerializeField] private float requestTimeout = 30f;
        [SerializeField] private int maxRetryCount = 3;
        [SerializeField] private float retryDelay = 1f;
        [SerializeField] private bool enableLogging = true;

        private static NetworkManager _instance;
        private Dictionary<string, string> _defaultHeaders;
        private Queue<NetworkRequest> _requestQueue;
        private bool _isProcessingQueue;

        public static NetworkManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<NetworkManager>();
                    if (_instance == null)
                    {
                        GameObject go = new GameObject("NetworkManager");
                        _instance = go.AddComponent<NetworkManager>();
                        DontDestroyOnLoad(go);
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// 网络请求结果回调
        /// </summary>
        public delegate void NetworkCallback(bool success, string data, string error);

        /// <summary>
        /// 网络请求类
        /// </summary>
        private class NetworkRequest
        {
            public string url;
            public string method;
            public string data;
            public Dictionary<string, string> headers;
            public NetworkCallback callback;
            public int retryCount;
        }

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
                Initialize();
            }
            else if (_instance != this)
            {
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// 初始化网络管理器
        /// </summary>
        private void Initialize()
        {
            _defaultHeaders = new Dictionary<string, string>();
            _requestQueue = new Queue<NetworkRequest>();

            // 设置默认请求头
            _defaultHeaders["Content-Type"] = "application/json";
            _defaultHeaders["User-Agent"] = GetUserAgent();

            // 适配小游戏平台
            AdaptToPlatform();
        }

        /// <summary>
        /// 适配不同平台
        /// </summary>
        private void AdaptToPlatform()
        {
            if (PlatformDetector.IsMiniGame)
            {
                // 小游戏平台网络限制
                requestTimeout = Mathf.Min(requestTimeout, 10f); // 限制超时时间
                maxRetryCount = Mathf.Min(maxRetryCount, 2); // 限制重试次数

                // 微信小游戏需要配置域名白名单
                if (PlatformDetector.CurrentPlatform == PlatformDetector.PlatformType.WeixinMiniGame)
                {
                    LogMessage("注意：微信小游戏需要在后台配置request域名白名单");
                }
            }
        }

        /// <summary>
        /// GET请求
        /// </summary>
        public void Get(string url, NetworkCallback callback, Dictionary<string, string> headers = null)
        {
            SendRequest(url, "GET", null, callback, headers);
        }

        /// <summary>
        /// POST请求
        /// </summary>
        public void Post(string url, string jsonData, NetworkCallback callback, Dictionary<string, string> headers = null)
        {
            SendRequest(url, "POST", jsonData, callback, headers);
        }

        /// <summary>
        /// PUT请求
        /// </summary>
        public void Put(string url, string jsonData, NetworkCallback callback, Dictionary<string, string> headers = null)
        {
            SendRequest(url, "PUT", jsonData, callback, headers);
        }

        /// <summary>
        /// DELETE请求
        /// </summary>
        public void Delete(string url, NetworkCallback callback, Dictionary<string, string> headers = null)
        {
            SendRequest(url, "DELETE", null, callback, headers);
        }

        /// <summary>
        /// 发送网络请求
        /// </summary>
        private void SendRequest(string url, string method, string data, NetworkCallback callback, Dictionary<string, string> headers)
        {
            var request = new NetworkRequest
            {
                url = url,
                method = method,
                data = data,
                headers = MergeHeaders(headers),
                callback = callback,
                retryCount = 0
            };

            _requestQueue.Enqueue(request);

            if (!_isProcessingQueue)
            {
                StartCoroutine(ProcessRequestQueue());
            }
        }

        /// <summary>
        /// 处理请求队列
        /// </summary>
        private IEnumerator ProcessRequestQueue()
        {
            _isProcessingQueue = true;

            while (_requestQueue.Count > 0)
            {
                var request = _requestQueue.Dequeue();
                yield return StartCoroutine(ExecuteRequest(request));

                // 小游戏平台需要间隔时间避免请求过于频繁
                if (PlatformDetector.IsMiniGame)
                {
                    yield return new WaitForSeconds(0.1f);
                }
            }

            _isProcessingQueue = false;
        }

        /// <summary>
        /// 执行网络请求
        /// </summary>
        private IEnumerator ExecuteRequest(NetworkRequest request)
        {
            // 创建请求
            UnityWebRequest webRequest = CreateWebRequest(request);
            if (webRequest == null)
            {
                yield break;
            }

            // 设置请求头
            SetRequestHeaders(webRequest, request.headers);

            // 设置超时
            webRequest.timeout = (int)requestTimeout;

            LogMessage($"Sending {request.method} request to: {request.url}");

            // 发送请求
            yield return webRequest.SendWebRequest();

            // 处理响应
            yield return StartCoroutine(HandleResponse(webRequest, request));

            // 清理资源
            webRequest?.Dispose();
        }

        /// <summary>
        /// 创建Web请求
        /// </summary>
        private UnityWebRequest CreateWebRequest(NetworkRequest request)
        {
            try
            {
                switch (request.method.ToUpper())
                {
                    case "GET":
                        return UnityWebRequest.Get(request.url);
                    case "POST":
                        return UnityWebRequest.Post(request.url, request.data, "application/json");
                    case "PUT":
                        return UnityWebRequest.Put(request.url, request.data);
                    case "DELETE":
                        return UnityWebRequest.Delete(request.url);
                    default:
                        LogMessage($"Unsupported HTTP method: {request.method}");
                        request.callback?.Invoke(false, "", "Unsupported HTTP method");
                        return null;
                }
            }
            catch (Exception e)
            {
                string error = $"Failed to create request: {e.Message}";
                LogMessage(error);
                request.callback?.Invoke(false, "", error);
                return null;
            }
        }

        /// <summary>
        /// 设置请求头
        /// </summary>
        private void SetRequestHeaders(UnityWebRequest webRequest, Dictionary<string, string> headers)
        {
            if (headers == null) return;

            try
            {
                foreach (var header in headers)
                {
                    webRequest.SetRequestHeader(header.Key, header.Value);
                }
            }
            catch (Exception e)
            {
                LogMessage($"Failed to set headers: {e.Message}");
            }
        }

        /// <summary>
        /// 处理响应
        /// </summary>
        private IEnumerator HandleResponse(UnityWebRequest webRequest, NetworkRequest request)
        {
            // 检查响应结果
            bool isSuccess = false;
            string responseData = "";
            string errorMessage = "";

            try
            {
                isSuccess = webRequest.result == UnityWebRequest.Result.Success;
                if (isSuccess)
                {
                    responseData = webRequest.downloadHandler.text;
                    LogMessage($"Request successful: {responseData}");
                }
                else
                {
                    errorMessage = $"Request failed: {webRequest.error}";
                    LogMessage(errorMessage);
                }
            }
            catch (Exception e)
            {
                errorMessage = $"Response handling exception: {e.Message}";
                LogMessage(errorMessage);
                isSuccess = false;
            }

            // 处理成功响应
            if (isSuccess)
            {
                request.callback?.Invoke(true, responseData, "");
                yield break;
            }

            // 处理失败响应 - 重试逻辑
            if (request.retryCount < maxRetryCount)
            {
                request.retryCount++;
                LogMessage($"Retrying request ({request.retryCount}/{maxRetryCount})...");

                yield return new WaitForSeconds(retryDelay);
                yield return StartCoroutine(ExecuteRequest(request));
            }
            else
            {
                request.callback?.Invoke(false, "", errorMessage);
            }
        }

        /// <summary>
        /// 合并请求头
        /// </summary>
        private Dictionary<string, string> MergeHeaders(Dictionary<string, string> customHeaders)
        {
            var mergedHeaders = new Dictionary<string, string>(_defaultHeaders);

            if (customHeaders != null)
            {
                foreach (var header in customHeaders)
                {
                    mergedHeaders[header.Key] = header.Value;
                }
            }

            return mergedHeaders;
        }

        /// <summary>
        /// 获取User-Agent
        /// </summary>
        private string GetUserAgent()
        {
            string platform = PlatformDetector.GetPlatformName();
            string version = Application.version;
            return $"GameApp/{version} ({platform})";
        }

        /// <summary>
        /// 下载文件
        /// </summary>
        public void DownloadFile(string url, string savePath, NetworkCallback callback)
        {
            StartCoroutine(DownloadFileCoroutine(url, savePath, callback));
        }

        /// <summary>
        /// 下载文件协程
        /// </summary>
        private IEnumerator DownloadFileCoroutine(string url, string savePath, NetworkCallback callback)
        {
            using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
            {
                webRequest.timeout = (int)requestTimeout;

                yield return webRequest.SendWebRequest();

                if (webRequest.result == UnityWebRequest.Result.Success)
                {
                    try
                    {
                        System.IO.File.WriteAllBytes(savePath, webRequest.downloadHandler.data);
                        callback?.Invoke(true, savePath, "");
                    }
                    catch (Exception e)
                    {
                        callback?.Invoke(false, "", $"Failed to save file: {e.Message}");
                    }
                }
                else
                {
                    callback?.Invoke(false, "", webRequest.error);
                }
            }
        }

        /// <summary>
        /// 检查网络连接
        /// </summary>
        public bool IsNetworkAvailable()
        {
            return Application.internetReachability != NetworkReachability.NotReachable;
        }

        /// <summary>
        /// 获取网络类型
        /// </summary>
        public string GetNetworkType()
        {
            switch (Application.internetReachability)
            {
                case NetworkReachability.ReachableViaCarrierDataNetwork:
                    return "Mobile Data";
                case NetworkReachability.ReachableViaLocalAreaNetwork:
                    return "WiFi";
                default:
                    return "No Network";
            }
        }

        /// <summary>
        /// 设置默认请求头
        /// </summary>
        public void SetDefaultHeader(string key, string value)
        {
            _defaultHeaders[key] = value;
        }

        /// <summary>
        /// 移除默认请求头
        /// </summary>
        public void RemoveDefaultHeader(string key)
        {
            _defaultHeaders.Remove(key);
        }

        /// <summary>
        /// 清空请求队列
        /// </summary>
        public void ClearRequestQueue()
        {
            _requestQueue.Clear();
        }

        /// <summary>
        /// 记录日志
        /// </summary>
        private void LogMessage(string message)
        {
            if (enableLogging)
            {
                Debug.Log($"[NetworkManager] {message}");
            }
        }

        /// <summary>
        /// 获取网络状态信息
        /// </summary>
        public string GetNetworkInfo()
        {
            return $"Network Available: {IsNetworkAvailable()}, " +
                   $"Network Type: {GetNetworkType()}, " +
                   $"Queue Size: {_requestQueue.Count}, " +
                   $"Timeout: {requestTimeout}s";
        }
    }
}
