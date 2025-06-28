# GK工具类使用指南

## 🚀 快速开始

### 1. 基本初始化
```csharp
using UnityEngine;

public class GameManager : MonoBehaviour
{
    void Start()
    {
        // 一行代码初始化所有工具
        GameUtils.Initialize();
    }
}
```

### 2. 平台检测
```csharp
// 检测当前平台
var platform = GameUtils.Platform.GetCurrentPlatform();

// 平台特定逻辑
if (GameUtils.Platform.IsMobile())
{
    // 移动设备逻辑
    GameUtils.Performance.SetTargetFrameRate(30);
}
else if (GameUtils.Platform.IsMiniGame())
{
    // 小游戏逻辑
    GameUtils.Performance.SetTargetFrameRate(60);
}
```

### 3. 屏幕适配
```csharp
// 执行屏幕适配
GameUtils.Screen.Adapt();

// 检查屏幕信息
float aspectRatio = GameUtils.Screen.GetAspectRatio();
bool hasSafeArea = GameUtils.Screen.HasSafeArea();
```

## 📱 功能模块详解

### 平台检测 (Platform Detection)
```csharp
// 基本检测
GameUtils.Platform.GetCurrentPlatform();  // 获取平台类型
GameUtils.Platform.IsMobile();            // 是否移动设备
GameUtils.Platform.IsMiniGame();          // 是否小游戏
GameUtils.Platform.GetDeviceInfo();       // 获取设备信息

// 高级用法
using GK.Platform;
var platform = PlatformDetector.CurrentPlatform;
string deviceInfo = PlatformDetector.GetDeviceInfo();
```

### 音频管理 (Audio Management)
```csharp
// 基本音频操作
GameUtils.Audio.PlayMusic("background_music");     // 播放背景音乐
GameUtils.Audio.PlaySfx("button_click");          // 播放音效
GameUtils.Audio.StopMusic();                      // 停止音乐

// 音量控制
GameUtils.Audio.SetMasterVolume(0.8f);            // 主音量
GameUtils.Audio.SetMusicVolume(0.7f);             // 音乐音量
GameUtils.Audio.SetSfxVolume(1.0f);               // 音效音量

// 高级用法
using GK.Managers;
AudioManager.Instance.PlayMusic("bgm", true, 2f); // 带淡入效果
```

### 数据存储 (Data Storage)
```csharp
// 基本数据操作
GameUtils.Data.Save("player_data", playerData);   // 保存数据
var data = GameUtils.Data.Load<PlayerData>("player_data"); // 加载数据
GameUtils.Data.Delete("old_data");                // 删除数据
bool exists = GameUtils.Data.Exists("save_file"); // 检查存在

// 启用加密
GameUtils.Data.EnableEncryption("your_secret_key");

// 数据结构示例
[System.Serializable]
public class PlayerData
{
    public string name;
    public int level;
    public float[] position;
}
```

### 网络请求 (Network Requests)
```csharp
// 检查网络状态
bool available = GameUtils.Network.IsNetworkAvailable();
string networkType = GameUtils.Network.GetNetworkType();

// GET请求
GameUtils.Network.Get("https://api.example.com/data", (success, data, error) =>
{
    if (success)
    {
        Debug.Log("请求成功: " + data);
    }
    else
    {
        Debug.LogError("请求失败: " + error);
    }
});

// POST请求
string jsonData = JsonUtility.ToJson(requestData);
GameUtils.Network.Post("https://api.example.com/submit", jsonData, callback);
```

### 性能监控 (Performance Monitoring)
```csharp
// 获取性能信息
float fps = GameUtils.Performance.GetCurrentFPS();        // 当前FPS
float avgFPS = GameUtils.Performance.GetAverageFPS();     // 平均FPS
float memory = GameUtils.Performance.GetMemoryUsage();    // 内存使用

// 性能优化
GameUtils.Performance.SetTargetFrameRate(30);             // 设置目标帧率
GameUtils.Performance.ForceGC();                          // 强制垃圾回收
```

### 输入管理 (Input Management)
```csharp
// 注册输入事件
InputManager.OnTap += OnTap;
InputManager.OnSwipe += OnSwipe;
InputManager.OnInputDown += OnInputDown;

// 事件处理
private void OnTap(Vector2 position)
{
    Debug.Log("点击位置: " + position);
}

private void OnSwipe(Vector2 startPos, Vector2 endPos)
{
    Vector2 direction = InputManager.GetSwipeDirection(startPos, endPos);
    Debug.Log("滑动方向: " + direction);
}

// 获取输入状态
Vector2 currentPos = GameUtils.Input.GetCurrentPosition();
bool isActive = GameUtils.Input.IsInputActive();
```

### UI适配 (UI Adaptation)
```csharp
// 为UI元素添加适配
GameUtils.UI.AddAdapter(uiElement, UIAdapter.AdaptType.ScaleWithScreenSize);

// 批量适配
GameUtils.UI.AdaptElements(uiElements);

// 获取推荐适配类型
var recommendedType = GameUtils.UI.GetRecommendedAdaptType();

// 手动适配
using GK.Adapters;
var adapter = gameObject.AddComponent<UIAdapter>();
adapter.SetAdaptType(UIAdapter.AdaptType.MatchWidthOrHeight);
```

### 系统诊断 (System Diagnostics)
```csharp
// 获取系统信息
string systemInfo = GameUtils.Diagnostics.GetSystemInfo();

// 输出所有状态
GameUtils.Diagnostics.LogAllStatus();
```

## 🎯 最佳实践

### 1. 初始化顺序
```csharp
public class GameManager : MonoBehaviour
{
    void Start()
    {
        // 1. 初始化工具类
        GameUtils.Initialize();
        
        // 2. 检测平台并应用设置
        ApplyPlatformSettings();
        
        // 3. 适配屏幕
        GameUtils.Screen.Adapt();
        
        // 4. 加载游戏数据
        LoadGameData();
        
        // 5. 初始化音频
        InitializeAudio();
    }
}
```

### 2. 错误处理
```csharp
try
{
    var data = GameUtils.Data.Load<PlayerData>("player");
    // 处理数据
}
catch (System.Exception e)
{
    Debug.LogError("数据加载失败: " + e.Message);
    // 使用默认数据
}
```

### 3. 平台特定优化
```csharp
private void ApplyPlatformSettings()
{
    if (GameUtils.Platform.IsMobile())
    {
        // 移动设备优化
        GameUtils.Performance.SetTargetFrameRate(30);
        QualitySettings.SetQualityLevel(1);
    }
    else if (GameUtils.Platform.IsMiniGame())
    {
        // 小游戏优化
        GameUtils.Performance.SetTargetFrameRate(60);
        QualitySettings.SetQualityLevel(2);
    }
}
```

### 4. 资源管理
```csharp
private void OnDestroy()
{
    // 取消事件注册
    InputManager.OnTap -= OnTap;
    InputManager.OnSwipe -= OnSwipe;
    
    // 清理资源
    CancelInvoke();
}
```

## 🔧 高级用法

### 1. 直接使用具体类
```csharp
using GK.Platform;
using GK.Managers;

// 直接访问具体功能
var platform = PlatformDetector.CurrentPlatform;
AudioManager.Instance.PlayMusic("bgm");
```

### 2. 扩展功能
```csharp
namespace GK.Extensions
{
    public static class GameUtilsExtensions
    {
        public static void PlayRandomSfx(this AudioManager audio, string[] clips)
        {
            if (clips.Length > 0)
            {
                string randomClip = clips[Random.Range(0, clips.Length)];
                audio.PlaySfx(randomClip);
            }
        }
    }
}
```

### 3. 自定义配置
```csharp
// 自定义性能配置
PerformanceManager.Instance.SetMonitoringEnabled(true, true);
PerformanceManager.Instance.SetTargetFrameRate(45);

// 自定义网络配置
NetworkManager.Instance.SetDefaultHeader("Authorization", "Bearer token");
```

## 📋 常见问题

### Q: 如何在不同场景间保持数据？
A: 使用 `GameUtils.Data.Save()` 保存到本地，或使用 `DontDestroyOnLoad()` 保持对象。

### Q: 小游戏平台有什么特殊限制？
A: 网络请求需要域名白名单，音频需要用户交互激活，包体大小有限制。

### Q: 如何优化移动设备性能？
A: 使用 `GameUtils.Performance.SetTargetFrameRate(30)` 和适当的质量设置。

### Q: 数据加密安全吗？
A: 提供基本加密，适合一般游戏数据，敏感数据建议使用服务器验证。

## 📞 技术支持

- 查看示例代码：`Assets/Scripts/Tools/Examples/`
- 阅读API文档：`Assets/Scripts/Tools/README.md`
- 命名空间指南：`Assets/Scripts/Tools/NAMESPACE_GUIDE.md`
- API兼容性：`Assets/Scripts/Tools/API_COMPATIBILITY.md`
