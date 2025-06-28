using UnityEngine;
using GK.Platform;

namespace GK.Adapters
{
    /// <summary>
    /// 屏幕适配工具类
    /// 处理不同设备的屏幕分辨率和安全区域适配
    /// </summary>
    public class ScreenAdapter : MonoBehaviour
    {
        [Header("适配设置")]
        [SerializeField] private bool autoAdaptOnStart = true;
        [SerializeField] private bool adaptToSafeArea = true;
        [SerializeField] private Vector2 referenceResolution = new Vector2(1920, 1080);
        [SerializeField] private float matchWidthOrHeight = 0.5f;

        [Header("调试信息")]
        [SerializeField] private bool showDebugInfo = false;

        private static ScreenAdapter _instance;
        private Camera _mainCamera;
        private Canvas _mainCanvas;
        private RectTransform _safeAreaRect;

        public static ScreenAdapter Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<ScreenAdapter>();
                    if (_instance == null)
                    {
                        GameObject go = new GameObject("ScreenAdapter");
                        _instance = go.AddComponent<ScreenAdapter>();
                        DontDestroyOnLoad(go);
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// 当前屏幕宽高比
        /// </summary>
        public static float AspectRatio => (float)Screen.width / Screen.height;

        /// <summary>
        /// 是否为刘海屏或有安全区域
        /// </summary>
        public static bool HasSafeArea => Screen.safeArea.size != new Vector2(Screen.width, Screen.height);

        /// <summary>
        /// 安全区域
        /// </summary>
        public static Rect SafeArea => Screen.safeArea;

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else if (_instance != this)
            {
                Destroy(gameObject);
                return;
            }

            InitializeComponents();
        }

        private void Start()
        {
            if (autoAdaptOnStart)
            {
                AdaptScreen();
            }
        }

        private void InitializeComponents()
        {
            _mainCamera = Camera.main;
            if (_mainCamera == null)
                _mainCamera = FindObjectOfType<Camera>();

            _mainCanvas = FindObjectOfType<Canvas>();
        }

        /// <summary>
        /// 执行屏幕适配
        /// </summary>
        public void AdaptScreen()
        {
            AdaptResolution();
            if (adaptToSafeArea)
            {
                AdaptSafeArea();
            }

            if (showDebugInfo)
            {
                LogScreenInfo();
            }
        }

        /// <summary>
        /// 适配分辨率
        /// </summary>
        private void AdaptResolution()
        {
            if (_mainCanvas == null) return;

            var canvasScaler = _mainCanvas.GetComponent<UnityEngine.UI.CanvasScaler>();
            if (canvasScaler == null)
            {
                canvasScaler = _mainCanvas.gameObject.AddComponent<UnityEngine.UI.CanvasScaler>();
            }

            canvasScaler.uiScaleMode = UnityEngine.UI.CanvasScaler.ScaleMode.ScaleWithScreenSize;
            canvasScaler.referenceResolution = referenceResolution;
            canvasScaler.matchWidthOrHeight = matchWidthOrHeight;

            // 根据平台调整适配策略
            if (PlatformDetector.IsMobile)
            {
                // 移动设备优先适配宽度
                canvasScaler.matchWidthOrHeight = AspectRatio > (referenceResolution.x / referenceResolution.y) ? 1f : 0f;
            }
            else if (PlatformDetector.IsMiniGame)
            {
                // 小游戏平台使用混合适配
                canvasScaler.matchWidthOrHeight = 0.5f;
            }
        }

        /// <summary>
        /// 适配安全区域
        /// </summary>
        private void AdaptSafeArea()
        {
            if (!HasSafeArea) return;

            if (_safeAreaRect == null)
            {
                CreateSafeAreaRect();
            }

            var safeArea = Screen.safeArea;
            var anchorMin = safeArea.position;
            var anchorMax = safeArea.position + safeArea.size;

            anchorMin.x /= Screen.width;
            anchorMin.y /= Screen.height;
            anchorMax.x /= Screen.width;
            anchorMax.y /= Screen.height;

            _safeAreaRect.anchorMin = anchorMin;
            _safeAreaRect.anchorMax = anchorMax;
        }

        /// <summary>
        /// 创建安全区域矩形
        /// </summary>
        private void CreateSafeAreaRect()
        {
            if (_mainCanvas == null) return;

            GameObject safeAreaGO = new GameObject("SafeArea");
            safeAreaGO.transform.SetParent(_mainCanvas.transform, false);

            _safeAreaRect = safeAreaGO.AddComponent<RectTransform>();
            _safeAreaRect.anchorMin = Vector2.zero;
            _safeAreaRect.anchorMax = Vector2.one;
            _safeAreaRect.offsetMin = Vector2.zero;
            _safeAreaRect.offsetMax = Vector2.zero;
        }

        /// <summary>
        /// 获取安全区域RectTransform
        /// </summary>
        public RectTransform GetSafeAreaRect()
        {
            if (_safeAreaRect == null && adaptToSafeArea)
            {
                AdaptSafeArea();
            }
            return _safeAreaRect;
        }

        /// <summary>
        /// 设置UI元素到安全区域内
        /// </summary>
        public void SetToSafeArea(RectTransform rectTransform)
        {
            if (rectTransform == null || !HasSafeArea) return;

            var safeAreaRect = GetSafeAreaRect();
            if (safeAreaRect != null)
            {
                rectTransform.SetParent(safeAreaRect, false);
            }
        }

        /// <summary>
        /// 获取屏幕适配比例
        /// </summary>
        public Vector2 GetScreenScale()
        {
            float scaleX = Screen.width / referenceResolution.x;
            float scaleY = Screen.height / referenceResolution.y;
            return new Vector2(scaleX, scaleY);
        }

        /// <summary>
        /// 输出屏幕信息
        /// </summary>
        private void LogScreenInfo()
        {
            Debug.Log($"Screen Info - Resolution: {Screen.width}x{Screen.height}, " +
                     $"Aspect Ratio: {AspectRatio:F2}, " +
                     $"Safe Area: {SafeArea}, " +
                     $"Has Safe Area: {HasSafeArea}, " +
                     $"Platform: {PlatformDetector.GetPlatformName()}");
        }

        /// <summary>
        /// 强制刷新适配
        /// </summary>
        public void RefreshAdaptation()
        {
            AdaptScreen();
        }

        private void OnValidate()
        {
            if (Application.isPlaying && autoAdaptOnStart)
            {
                AdaptScreen();
            }
        }
    }
}
