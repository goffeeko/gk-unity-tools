using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GK.Platform;

// 兼容性说明：
// 某些QualitySettings API在不同Unity版本中可能有变化
// 如果遇到API更新警告，可以选择让Unity自动更新
//
// 已知的API变化：
// - QualitySettings.masterTextureLimit -> QualitySettings.globalTextureMipmapLimit
// - 某些QualitySettings在URP/HDRP中不适用

namespace GK.Managers
{
    /// <summary>
    /// 性能管理器
    /// 针对移动设备和小游戏平台进行性能优化
    /// </summary>
    public class PerformanceManager : MonoBehaviour
    {
        [Header("性能设置")]
        [SerializeField] private bool autoOptimizeOnStart = true;
        [SerializeField] private bool enableFPSMonitor = true;
        [SerializeField] private bool enableMemoryMonitor = true;
        [SerializeField] private float monitorUpdateInterval = 1f;

        [Header("帧率设置")]
        [SerializeField] private int targetFrameRate = 60;
        [SerializeField] private bool adaptiveFrameRate = true;
        [SerializeField] private int lowEndFrameRate = 30;

        [Header("质量设置")]
        [SerializeField] private bool autoAdjustQuality = true;
        [SerializeField] private int mobileQualityLevel = 2;
        [SerializeField] private int miniGameQualityLevel = 1;

        private static PerformanceManager _instance;
        private float _currentFPS;
        private float _averageFPS;
        private long _currentMemory;
        private List<float> _fpsHistory;
        private int _fpsHistorySize = 60;
        private bool _isLowEndDevice;

        public static PerformanceManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<PerformanceManager>();
                    if (_instance == null)
                    {
                        GameObject go = new GameObject("PerformanceManager");
                        _instance = go.AddComponent<PerformanceManager>();
                        DontDestroyOnLoad(go);
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// 当前FPS
        /// </summary>
        public float CurrentFPS => _currentFPS;

        /// <summary>
        /// 平均FPS
        /// </summary>
        public float AverageFPS => _averageFPS;

        /// <summary>
        /// 当前内存使用量（MB）
        /// </summary>
        public float CurrentMemoryMB => _currentMemory / (1024f * 1024f);

        /// <summary>
        /// 是否为低端设备
        /// </summary>
        public bool IsLowEndDevice => _isLowEndDevice;

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);

                _fpsHistory = new List<float>();
                DetectDevicePerformance();

                if (autoOptimizeOnStart)
                {
                    OptimizeForPlatform();
                }
            }
            else if (_instance != this)
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            if (enableFPSMonitor || enableMemoryMonitor)
            {
                StartCoroutine(MonitorPerformance());
            }
        }

        /// <summary>
        /// 检测设备性能
        /// </summary>
        private void DetectDevicePerformance()
        {
            // 基于系统信息判断设备性能
            int memoryMB = SystemInfo.systemMemorySize;
            int processorCount = SystemInfo.processorCount;
            string gpuName = SystemInfo.graphicsDeviceName.ToLower();

            _isLowEndDevice = memoryMB < 3000 || processorCount < 4 ||
                             gpuName.Contains("adreno 5") || gpuName.Contains("mali-t");

            Debug.Log($"Device Performance - Memory: {memoryMB}MB, CPU Cores: {processorCount}, " +
                     $"GPU: {SystemInfo.graphicsDeviceName}, Low End: {_isLowEndDevice}");
        }

        /// <summary>
        /// 根据平台优化性能
        /// </summary>
        public void OptimizeForPlatform()
        {
            // 设置目标帧率
            SetTargetFrameRate();

            // 调整质量设置
            AdjustQualitySettings();

            // 优化渲染设置
            OptimizeRenderingSettings();

            // 优化物理设置
            OptimizePhysicsSettings();

            // 平台特定优化
            ApplyPlatformSpecificOptimizations();

            Debug.Log($"Performance optimized for platform: {PlatformDetector.GetPlatformName()}");
        }

        /// <summary>
        /// 设置目标帧率
        /// </summary>
        private void SetTargetFrameRate()
        {
            int frameRate = targetFrameRate;

            if (adaptiveFrameRate)
            {
                if (_isLowEndDevice || PlatformDetector.IsMiniGame)
                {
                    frameRate = lowEndFrameRate;
                }
            }

            Application.targetFrameRate = frameRate;
            Debug.Log($"Target frame rate set to: {frameRate}");
        }

        /// <summary>
        /// 调整质量设置
        /// </summary>
        private void AdjustQualitySettings()
        {
            int qualityLevel = QualitySettings.GetQualityLevel();

            if (PlatformDetector.IsMobile)
            {
                qualityLevel = _isLowEndDevice ? 0 : mobileQualityLevel;
            }
            else if (PlatformDetector.IsMiniGame)
            {
                qualityLevel = miniGameQualityLevel;
            }

            QualitySettings.SetQualityLevel(qualityLevel, true);
            Debug.Log($"Quality level set to: {qualityLevel}");
        }

        /// <summary>
        /// 优化渲染设置
        /// </summary>
        private void OptimizeRenderingSettings()
        {
            if (PlatformDetector.IsMobile || PlatformDetector.IsMiniGame)
            {
                // 禁用不必要的渲染特性
                QualitySettings.shadows = ShadowQuality.Disable;

                // 检查渲染管线类型
                bool isUsingURP = IsUsingURP();
                bool isUsingHDRP = IsUsingHDRP();

                // 只在Built-in渲染管线中设置这些选项
                if (!isUsingURP && !isUsingHDRP)
                {
                    // 使用反射安全地设置可能过时的API，避免直接引用
                    SetLegacyRenderingSetting("softParticles", false);
                    SetLegacyRenderingSetting("realtimeReflectionProbes", false);
                }
                else
                {
                    Debug.Log("Using modern render pipeline (URP/HDRP), skipping legacy settings");
                }

                if (_isLowEndDevice)
                {
                    QualitySettings.antiAliasing = 0;
                    QualitySettings.anisotropicFiltering = AnisotropicFiltering.Disable;
                }
            }
        }

        /// <summary>
        /// 安全地设置渲染选项
        /// </summary>
        private void SetRenderingSetting(System.Action action, string settingName = "Unknown")
        {
            try
            {
                action?.Invoke();
            }
            catch (System.Exception ex)
            {
                Debug.LogWarning($"Failed to set rendering setting '{settingName}': {ex.Message}. This may be due to using a different render pipeline.");
            }
        }

        /// <summary>
        /// 检查是否使用URP渲染管线
        /// </summary>
        private bool IsUsingURP()
        {
            var renderPipelineAsset = UnityEngine.Rendering.GraphicsSettings.renderPipelineAsset;
            return renderPipelineAsset != null && renderPipelineAsset.GetType().Name.Contains("Universal");
        }

        /// <summary>
        /// 检查是否使用HDRP渲染管线
        /// </summary>
        private bool IsUsingHDRP()
        {
            var renderPipelineAsset = UnityEngine.Rendering.GraphicsSettings.renderPipelineAsset;
            return renderPipelineAsset != null && renderPipelineAsset.GetType().Name.Contains("HDRenderPipeline");
        }

        /// <summary>
        /// 使用反射安全地设置可能过时的渲染API
        /// </summary>
        private void SetLegacyRenderingSetting(string propertyName, object value)
        {
            try
            {
                var qualitySettingsType = typeof(QualitySettings);
                var property = qualitySettingsType.GetProperty(propertyName,
                    System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);

                if (property != null && property.CanWrite)
                {
                    property.SetValue(null, value);
                    Debug.Log($"Successfully set {propertyName} = {value}");
                }
                else
                {
                    Debug.LogWarning($"Property {propertyName} not found or not writable in QualitySettings. This may be due to API changes.");
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogWarning($"Failed to set legacy rendering setting '{propertyName}': {ex.Message}. This is likely due to API changes in newer Unity versions.");
            }
        }

        /// <summary>
        /// 优化物理设置
        /// </summary>
        private void OptimizePhysicsSettings()
        {
            if (PlatformDetector.IsMobile || _isLowEndDevice)
            {
                // 降低物理更新频率
                Time.fixedDeltaTime = 1f / 30f; // 30Hz instead of 50Hz

                // 减少物理迭代次数
                Physics2D.velocityIterations = 6;
                Physics2D.positionIterations = 2;
            }
        }

        /// <summary>
        /// 应用平台特定优化
        /// </summary>
        private void ApplyPlatformSpecificOptimizations()
        {
            if (PlatformDetector.IsMiniGame)
            {
                // 小游戏平台优化
                QualitySettings.vSyncCount = 0; // 禁用垂直同步
                Screen.sleepTimeout = SleepTimeout.NeverSleep;
            }
            else if (PlatformDetector.IsMobile)
            {
                // 移动设备优化
                Screen.sleepTimeout = SleepTimeout.SystemSetting;

                if (_isLowEndDevice)
                {
                    // 低端设备额外优化
                    // pixelLightCount 只在Built-in渲染管线中有效
                    if (!IsUsingURP() && !IsUsingHDRP())
                    {
                        SetLegacyRenderingSetting("pixelLightCount", 1);
                    }

                    // 使用新的API替代过时的masterTextureLimit
                    QualitySettings.globalTextureMipmapLimit = 1;
                }
            }
        }

        /// <summary>
        /// 性能监控协程
        /// </summary>
        private IEnumerator MonitorPerformance()
        {
            while (true)
            {
                if (enableFPSMonitor)
                {
                    UpdateFPSMonitor();
                }

                if (enableMemoryMonitor)
                {
                    UpdateMemoryMonitor();
                }

                if (autoAdjustQuality)
                {
                    AutoAdjustQuality();
                }

                yield return new WaitForSeconds(monitorUpdateInterval);
            }
        }

        /// <summary>
        /// 更新FPS监控
        /// </summary>
        private void UpdateFPSMonitor()
        {
            _currentFPS = 1f / Time.unscaledDeltaTime;

            _fpsHistory.Add(_currentFPS);
            if (_fpsHistory.Count > _fpsHistorySize)
            {
                _fpsHistory.RemoveAt(0);
            }

            // 计算平均FPS
            float sum = 0f;
            foreach (float fps in _fpsHistory)
            {
                sum += fps;
            }
            _averageFPS = sum / _fpsHistory.Count;
        }

        /// <summary>
        /// 更新内存监控
        /// </summary>
        private void UpdateMemoryMonitor()
        {
            _currentMemory = System.GC.GetTotalMemory(false);

            // 如果内存使用过高，触发垃圾回收
            if (CurrentMemoryMB > 100f) // 100MB threshold
            {
                System.GC.Collect();
            }
        }

        /// <summary>
        /// 自动调整质量
        /// </summary>
        private void AutoAdjustQuality()
        {
            if (_averageFPS < targetFrameRate * 0.8f) // 如果FPS低于目标的80%
            {
                int currentQuality = QualitySettings.GetQualityLevel();
                if (currentQuality > 0)
                {
                    QualitySettings.SetQualityLevel(currentQuality - 1, true);
                    Debug.Log($"Quality decreased to level: {currentQuality - 1}");
                }
            }
            else if (_averageFPS > targetFrameRate * 0.95f) // 如果FPS接近目标
            {
                int currentQuality = QualitySettings.GetQualityLevel();
                int maxQuality = PlatformDetector.IsMobile ? mobileQualityLevel : QualitySettings.names.Length - 1;

                if (currentQuality < maxQuality)
                {
                    QualitySettings.SetQualityLevel(currentQuality + 1, true);
                    Debug.Log($"Quality increased to level: {currentQuality + 1}");
                }
            }
        }

        /// <summary>
        /// 强制垃圾回收
        /// </summary>
        public void ForceGarbageCollection()
        {
            System.GC.Collect();
            Resources.UnloadUnusedAssets();
        }

        /// <summary>
        /// 设置目标帧率
        /// </summary>
        public void SetTargetFrameRate(int frameRate)
        {
            targetFrameRate = frameRate;
            Application.targetFrameRate = frameRate;
        }

        /// <summary>
        /// 启用/禁用性能监控
        /// </summary>
        public void SetMonitoringEnabled(bool fps, bool memory)
        {
            enableFPSMonitor = fps;
            enableMemoryMonitor = memory;
        }

        /// <summary>
        /// 获取性能信息
        /// </summary>
        public string GetPerformanceInfo()
        {
            return $"FPS: {_currentFPS:F1} (Avg: {_averageFPS:F1}), " +
                   $"Memory: {CurrentMemoryMB:F1}MB, " +
                   $"Quality: {QualitySettings.GetQualityLevel()}, " +
                   $"Low End Device: {_isLowEndDevice}";
        }

        /// <summary>
        /// 获取设备信息
        /// </summary>
        public string GetDeviceInfo()
        {
            return $"Platform: {PlatformDetector.GetPlatformName()}, " +
                   $"Memory: {SystemInfo.systemMemorySize}MB, " +
                   $"CPU: {SystemInfo.processorType} ({SystemInfo.processorCount} cores), " +
                   $"GPU: {SystemInfo.graphicsDeviceName}";
        }
    }
}
