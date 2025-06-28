using UnityEngine;
using UnityEngine.UI;
using GK.Platform;

namespace GK.Adapters
{
    /// <summary>
    /// UI适配工具类
    /// 处理UI元素在不同屏幕尺寸下的适配
    /// </summary>
    public class UIAdapter : MonoBehaviour
    {
        [Header("适配类型")]
        [SerializeField] private AdaptType adaptType = AdaptType.ScaleWithScreenSize;
        [SerializeField] private bool adaptOnStart = true;
        [SerializeField] private bool adaptOnScreenChange = true;

        [Header("缩放设置")]
        [SerializeField] private Vector2 referenceResolution = new Vector2(1920, 1080);
        [SerializeField] private float minScale = 0.5f;
        [SerializeField] private float maxScale = 2.0f;

        [Header("位置适配")]
        [SerializeField] private bool adaptPosition = false;
        [SerializeField] private Vector2 positionOffset = Vector2.zero;

        [Header("安全区域")]
        [SerializeField] private bool respectSafeArea = true;

        private RectTransform _rectTransform;
        private Vector2 _originalSize;
        private Vector2 _originalPosition;
        private Vector2 _lastScreenSize;

        /// <summary>
        /// 适配类型
        /// </summary>
        public enum AdaptType
        {
            None,
            ScaleWithScreenSize,
            MatchWidth,
            MatchHeight,
            MatchWidthOrHeight,
            FixedSize
        }

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            if (_rectTransform != null)
            {
                _originalSize = _rectTransform.sizeDelta;
                _originalPosition = _rectTransform.anchoredPosition;
            }
            _lastScreenSize = new Vector2(Screen.width, Screen.height);
        }

        private void Start()
        {
            if (adaptOnStart)
            {
                AdaptUI();
            }
        }

        private void Update()
        {
            if (adaptOnScreenChange)
            {
                Vector2 currentScreenSize = new Vector2(Screen.width, Screen.height);
                if (currentScreenSize != _lastScreenSize)
                {
                    _lastScreenSize = currentScreenSize;
                    AdaptUI();
                }
            }
        }

        /// <summary>
        /// 执行UI适配
        /// </summary>
        public void AdaptUI()
        {
            if (_rectTransform == null) return;

            switch (adaptType)
            {
                case AdaptType.ScaleWithScreenSize:
                    AdaptScaleWithScreenSize();
                    break;
                case AdaptType.MatchWidth:
                    AdaptMatchWidth();
                    break;
                case AdaptType.MatchHeight:
                    AdaptMatchHeight();
                    break;
                case AdaptType.MatchWidthOrHeight:
                    AdaptMatchWidthOrHeight();
                    break;
                case AdaptType.FixedSize:
                    AdaptFixedSize();
                    break;
            }

            if (adaptPosition)
            {
                AdaptPosition();
            }

            if (respectSafeArea)
            {
                AdaptToSafeArea();
            }
        }

        /// <summary>
        /// 按屏幕尺寸缩放适配
        /// </summary>
        private void AdaptScaleWithScreenSize()
        {
            float scaleX = Screen.width / referenceResolution.x;
            float scaleY = Screen.height / referenceResolution.y;
            float scale = Mathf.Min(scaleX, scaleY);

            scale = Mathf.Clamp(scale, minScale, maxScale);

            _rectTransform.localScale = Vector3.one * scale;
        }

        /// <summary>
        /// 匹配宽度适配
        /// </summary>
        private void AdaptMatchWidth()
        {
            float scale = Screen.width / referenceResolution.x;
            scale = Mathf.Clamp(scale, minScale, maxScale);

            _rectTransform.localScale = Vector3.one * scale;
        }

        /// <summary>
        /// 匹配高度适配
        /// </summary>
        private void AdaptMatchHeight()
        {
            float scale = Screen.height / referenceResolution.y;
            scale = Mathf.Clamp(scale, minScale, maxScale);

            _rectTransform.localScale = Vector3.one * scale;
        }

        /// <summary>
        /// 匹配宽度或高度适配
        /// </summary>
        private void AdaptMatchWidthOrHeight()
        {
            float scaleX = Screen.width / referenceResolution.x;
            float scaleY = Screen.height / referenceResolution.y;

            // 根据平台选择适配策略
            float scale;
            if (PlatformDetector.IsMobile)
            {
                // 移动设备优先适配较小的比例
                scale = Mathf.Min(scaleX, scaleY);
            }
            else
            {
                // 其他平台使用平均值
                scale = (scaleX + scaleY) * 0.5f;
            }

            scale = Mathf.Clamp(scale, minScale, maxScale);
            _rectTransform.localScale = Vector3.one * scale;
        }

        /// <summary>
        /// 固定尺寸适配
        /// </summary>
        private void AdaptFixedSize()
        {
            _rectTransform.sizeDelta = _originalSize;
            _rectTransform.localScale = Vector3.one;
        }

        /// <summary>
        /// 位置适配
        /// </summary>
        private void AdaptPosition()
        {
            Vector2 screenScale = new Vector2(
                Screen.width / referenceResolution.x,
                Screen.height / referenceResolution.y
            );

            Vector2 adaptedPosition = new Vector2(
                _originalPosition.x * screenScale.x + positionOffset.x,
                _originalPosition.y * screenScale.y + positionOffset.y
            );

            _rectTransform.anchoredPosition = adaptedPosition;
        }

        /// <summary>
        /// 安全区域适配
        /// </summary>
        private void AdaptToSafeArea()
        {
            if (!ScreenAdapter.HasSafeArea) return;

            Rect safeArea = Screen.safeArea;
            Vector2 anchorMin = safeArea.position;
            Vector2 anchorMax = safeArea.position + safeArea.size;

            anchorMin.x /= Screen.width;
            anchorMin.y /= Screen.height;
            anchorMax.x /= Screen.width;
            anchorMax.y /= Screen.height;

            // 只在需要时调整锚点
            if (Vector2.Distance(_rectTransform.anchorMin, anchorMin) > 0.01f ||
                Vector2.Distance(_rectTransform.anchorMax, anchorMax) > 0.01f)
            {
                _rectTransform.anchorMin = anchorMin;
                _rectTransform.anchorMax = anchorMax;
            }
        }

        /// <summary>
        /// 设置适配类型
        /// </summary>
        public void SetAdaptType(AdaptType type)
        {
            adaptType = type;
            AdaptUI();
        }

        /// <summary>
        /// 设置参考分辨率
        /// </summary>
        public void SetReferenceResolution(Vector2 resolution)
        {
            referenceResolution = resolution;
            AdaptUI();
        }

        /// <summary>
        /// 设置缩放范围
        /// </summary>
        public void SetScaleRange(float min, float max)
        {
            minScale = min;
            maxScale = max;
            AdaptUI();
        }

        /// <summary>
        /// 重置到原始状态
        /// </summary>
        public void ResetToOriginal()
        {
            if (_rectTransform != null)
            {
                _rectTransform.sizeDelta = _originalSize;
                _rectTransform.anchoredPosition = _originalPosition;
                _rectTransform.localScale = Vector3.one;
            }
        }

        /// <summary>
        /// 获取当前缩放比例
        /// </summary>
        public float GetCurrentScale()
        {
            return _rectTransform != null ? _rectTransform.localScale.x : 1f;
        }

        /// <summary>
        /// 获取适配信息
        /// </summary>
        public string GetAdaptInfo()
        {
            return $"Adapt Type: {adaptType}, " +
                   $"Reference Resolution: {referenceResolution}, " +
                   $"Current Scale: {GetCurrentScale():F2}, " +
                   $"Screen Size: {Screen.width}x{Screen.height}, " +
                   $"Safe Area: {respectSafeArea}";
        }

        /// <summary>
        /// 静态方法：为GameObject添加UI适配
        /// </summary>
        public static UIAdapter AddUIAdapter(GameObject target, AdaptType type = AdaptType.ScaleWithScreenSize)
        {
            UIAdapter adapter = target.GetComponent<UIAdapter>();
            if (adapter == null)
            {
                adapter = target.AddComponent<UIAdapter>();
            }

            adapter.adaptType = type;
            adapter.AdaptUI();

            return adapter;
        }

        /// <summary>
        /// 静态方法：批量适配UI元素
        /// </summary>
        public static void AdaptUIElements(GameObject[] targets, AdaptType type = AdaptType.ScaleWithScreenSize)
        {
            foreach (var target in targets)
            {
                if (target != null)
                {
                    AddUIAdapter(target, type);
                }
            }
        }

        /// <summary>
        /// 静态方法：获取推荐的适配类型
        /// </summary>
        public static AdaptType GetRecommendedAdaptType()
        {
            if (PlatformDetector.IsMobile)
            {
                return AdaptType.MatchWidthOrHeight;
            }
            else if (PlatformDetector.IsMiniGame)
            {
                return AdaptType.ScaleWithScreenSize;
            }
            else
            {
                return AdaptType.ScaleWithScreenSize;
            }
        }
    }
}
