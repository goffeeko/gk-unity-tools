# GK - Unity 2D 跨平台游戏工具类

[![Unity Version](https://img.shields.io/badge/Unity-2021.3%2B-blue.svg)](https://unity3d.com/get-unity/download)
[![License](https://img.shields.io/badge/License-MIT-green.svg)](LICENSE)
[![Platform](https://img.shields.io/badge/Platform-Mobile%20%7C%20WebGL%20%7C%20Desktop-lightgrey.svg)]()

这是一个专为Unity 2D游戏开发的跨平台工具类集合，使用 `GK` 命名空间，支持移动设备、微信小游戏、抖音小游戏等多个平台的适配。

## 🎯 项目状态 / Project Status

- 🔄 **当前版本**: 0.1.0-alpha
- 🌟 **主分支**: master
- 🚧 **开发分支**: develop
- 🚀 **Git Flow**: 准备实施标准分支策略
- ⚙️ **CI/CD**: GitHub Actions配置中
- 📅 **创建时间**: 2024-06-28
- 🔄 **测试状态**: 正在进行合并检查测试

## ✨ 核心特性

- 🎮 **平台检测**: 自动识别运行平台（Android、iOS、微信小游戏、抖音小游戏等）
- 📱 **屏幕适配**: 处理不同设备的屏幕分辨率和安全区域适配
- 🎯 **输入管理**: 统一处理触摸、鼠标、键盘等不同平台的输入方式
- 🔊 **音频管理**: 跨平台音频播放和管理，适配小游戏平台限制
- 💾 **数据存储**: 统一的本地数据存储，支持加密，适配不同平台存储机制
- 🌐 **网络请求**: HTTP请求管理，适配小游戏平台网络限制
- 🖼️ **UI适配**: UI元素在不同屏幕尺寸下的自动适配
- ⚡ **性能优化**: 针对移动设备和小游戏平台的性能优化
- 🔧 **系统诊断**: 完整的系统信息和性能监控工具

## 📁 项目结构

```
Assets/Scripts/
├── Tools/             # GK工具类集合
│   ├── Platform/      # GK.Platform - 平台检测
│   │   └── PlatformDetector.cs
│   ├── Adapters/      # GK.Adapters - 适配器类
│   │   ├── ScreenAdapter.cs
│   │   └── UIAdapter.cs
│   ├── Managers/      # GK.Managers - 管理器类
│   │   ├── InputManager.cs
│   │   ├── AudioManager.cs
│   │   ├── DataManager.cs
│   │   ├── NetworkManager.cs
│   │   └── PerformanceManager.cs
│   ├── Utils/         # GK.Utils - 内部工具类
│   │   └── GameUtils.cs
│   ├── Examples/      # GK.Examples - 示例代码
│   │   ├── GameUtilsExample.cs
│   │   ├── NetworkManagerTest.cs
│   │   ├── DiagnosticsTest.cs
│   │   ├── ScreenAdapter/
│   │   ├── ScreenAdapterUI/
│   │   └── readme.txt
│   ├── README.md      # Tools说明文档
│   ├── NAMESPACE_GUIDE.md  # 命名空间使用指南
│   ├── API_COMPATIBILITY.md
│   └── API_UPDATE_GUIDE.md
├── GameUtils.cs       # GK.GameUtils - 统一入口类
└── GameUtils.asmdef   # Assembly Definition (根命名空间: GK)
```

## 🚀 快速开始

### 1️⃣ 一键初始化

```csharp
using UnityEngine;

public class GameManager : MonoBehaviour
{
    void Start()
    {
        // 一行代码初始化所有工具类
        GameUtils.Initialize();

        // 立即可用的功能
        if (GameUtils.Platform.IsMobile())
        {
            GameUtils.Performance.SetTargetFrameRate(30);
        }

        GameUtils.Screen.Adapt();
        GameUtils.Audio.SetMasterVolume(0.8f);
    }
}
```

### 2️⃣ 查看示例

- 📖 **新手入门**: [QuickStartExample.cs](Assets/Scripts/Tools/Examples/QuickStartExample.cs)
- 🎯 **完整演示**: [ComprehensiveExample.cs](Assets/Scripts/Tools/Examples/ComprehensiveExample.cs)
- 📚 **使用指南**: [USAGE_GUIDE.md](Assets/Scripts/Tools/USAGE_GUIDE.md)

> **💡 提示**: 项目使用 `GK` 命名空间，但保持 `GameUtils` 的简单使用方式。详见 [命名空间指南](Assets/Scripts/Tools/NAMESPACE_GUIDE.md)。

## 📖 核心功能示例

### 🎮 平台检测

```csharp
// 智能平台检测
var platform = GameUtils.Platform.GetCurrentPlatform();

if (GameUtils.Platform.IsMobile())
{
    // 移动设备优化
    GameUtils.Performance.SetTargetFrameRate(30);
    QualitySettings.SetQualityLevel(1);
}
else if (GameUtils.Platform.IsMiniGame())
{
    // 小游戏平台优化
    GameUtils.Performance.SetTargetFrameRate(60);
    // 注意：需要配置域名白名单
}
```

### 📱 屏幕适配

```csharp
// 自动屏幕适配
GameUtils.Screen.Adapt();

// 安全区域处理（刘海屏等）
if (GameUtils.Screen.HasSafeArea())
{
    var safeArea = GameUtils.Screen.GetSafeArea();
    Debug.Log($"安全区域: {safeArea}");
}

// UI元素适配
GameUtils.UI.AddAdapter(uiElement, UIAdapter.AdaptType.ScaleWithScreenSize);
```

### 🔊 音频管理

```csharp
// 音频播放
GameUtils.Audio.PlayMusic("background_music", true);  // 循环播放
GameUtils.Audio.PlaySfx("button_click", 0.8f);        // 指定音量

// 音量控制
GameUtils.Audio.SetMasterVolume(0.8f);   // 主音量
GameUtils.Audio.SetMusicVolume(0.7f);    // 音乐音量
GameUtils.Audio.SetSfxVolume(1.0f);      // 音效音量

// 停止播放
GameUtils.Audio.StopMusic();
```

### 💾 数据存储

```csharp
// 数据结构定义
[System.Serializable]
public class PlayerData
{
    public string name;
    public int level;
    public float[] position;
}

// 保存和加载
var playerData = new PlayerData { name = "Player", level = 10 };
GameUtils.Data.Save("player", playerData);
var loadedData = GameUtils.Data.Load<PlayerData>("player");

// 数据加密（可选）
GameUtils.Data.EnableEncryption("your_secret_key");

// 数据管理
bool exists = GameUtils.Data.Exists("save_file");
GameUtils.Data.Delete("old_data");
```

### 🌐 网络请求

```csharp
// 网络状态检查
if (GameUtils.Network.IsNetworkAvailable())
{
    string networkType = GameUtils.Network.GetNetworkType(); // "WiFi" 或 "Mobile Data"

    // GET请求
    GameUtils.Network.Get("https://api.example.com/data", (success, data, error) =>
    {
        if (success)
        {
            Debug.Log($"请求成功，数据长度: {data.Length}");
            // 处理返回的JSON数据
        }
        else
        {
            Debug.LogError($"请求失败: {error}");
        }
    });

    // POST请求
    var requestData = new { userId = 123, action = "login" };
    string jsonData = JsonUtility.ToJson(requestData);
    GameUtils.Network.Post("https://api.example.com/submit", jsonData, callback);
}
```

### 7. UI适配

```csharp
// 为GameObject添加UI适配
GameUtils.UI.AddAdapter(uiElement, UIAdapter.AdaptType.ScaleWithScreenSize);

// 批量适配UI元素
GameUtils.UI.AdaptElements(uiElements);

// 获取推荐的适配类型
var recommendedType = GameUtils.UI.GetRecommendedAdaptType();
```

### 8. 性能监控

```csharp
// 获取性能信息
float fps = GameUtils.Performance.GetCurrentFPS();
float memory = GameUtils.Performance.GetMemoryUsage();

// 强制垃圾回收
GameUtils.Performance.ForceGC();

// 设置目标帧率
GameUtils.Performance.SetTargetFrameRate(30);
```

### 9. 系统诊断

```csharp
// 获取完整的系统信息
string systemInfo = GameUtils.Diagnostics.GetSystemInfo();

// 输出所有工具类状态到控制台
GameUtils.Diagnostics.LogAllStatus();
```

## 🔧 详细配置

### 平台适配设置

各个工具类会根据检测到的平台自动调整设置：

- **移动设备**: 优化触摸输入、降低质量设置、启用性能监控
- **微信小游戏**: 限制网络请求频率、使用PlayerPrefs存储、音频需要用户交互激活
- **抖音小游戏**: 类似微信小游戏的限制和优化
- **桌面平台**: 启用鼠标和键盘输入、使用文件系统存储

### 输入事件

```csharp
// 注册输入事件
InputManager.OnTap += OnTap;
InputManager.OnSwipe += OnSwipe;
InputManager.OnInputDown += OnInputDown;
InputManager.OnInputUp += OnInputUp;

private void OnTap(Vector2 position)
{
    Debug.Log("点击位置: " + position);
}

private void OnSwipe(Vector2 startPos, Vector2 endPos)
{
    Vector2 direction = InputManager.GetSwipeDirection(startPos, endPos);
    Debug.Log("滑动方向: " + direction);
}
```

### 屏幕适配组件

在需要适配的UI元素上添加`ScreenAdapter`或`UIAdapter`组件：

```csharp
// 添加屏幕适配器
var screenAdapter = gameObject.AddComponent<ScreenAdapter>();
screenAdapter.adaptToSafeArea = true;

// 添加UI适配器
var uiAdapter = gameObject.AddComponent<UIAdapter>();
uiAdapter.SetAdaptType(UIAdapter.AdaptType.MatchWidthOrHeight);
```

## 📱 平台特定注意事项

### 微信小游戏

1. 需要在微信开发者工具中配置域名白名单
2. 音频播放需要用户交互后才能激活
3. 网络请求有频率限制
4. 使用PlayerPrefs进行数据存储

### 抖音小游戏

1. 类似微信小游戏的限制
2. 需要配置相应的域名白名单
3. 音频和网络限制类似

### 移动设备

1. 自动检测设备性能等级
2. 根据设备性能调整质量设置
3. 优化触摸输入处理
4. 启用性能监控和内存管理

## 🛠️ 扩展和自定义

### 添加新平台支持

1. 在`PlatformDetector.cs`中添加新的平台类型
2. 在各个管理器中添加平台特定的适配逻辑
3. 更新`GameUtils.cs`中的便捷方法

### 自定义适配策略

```csharp
// 自定义屏幕适配
public class CustomScreenAdapter : ScreenAdapter
{
    protected override void AdaptResolution()
    {
        // 自定义适配逻辑
    }
}
```

## 📄 许可证

MIT License

## 🤝 贡献

我们欢迎社区贡献！请遵循以下流程来参与项目开发：

### 🌳 分支策略

我们使用 **Git Flow** 分支模型：

- **`main`** - 生产分支，包含稳定的发布版本
- **`develop`** - 开发分支，包含最新的开发功能
- **`feature/*`** - 功能分支，用于开发新功能
- **`release/*`** - 发布分支，用于准备新版本发布
- **`hotfix/*`** - 热修复分支，用于紧急修复生产问题

### 📋 贡献流程

1. **Fork 项目** 到您的GitHub账户
2. **克隆您的Fork** 到本地开发环境
3. **创建功能分支** 从 `develop` 分支创建：
   ```bash
   git checkout develop
   git pull origin develop
   git checkout -b feature/your-feature-name
   ```
4. **开发和测试** 您的功能
5. **提交更改** 使用清晰的提交信息：
   ```bash
   git commit -m "feat: 添加新的平台检测功能"
   ```
6. **推送分支** 到您的Fork：
   ```bash
   git push origin feature/your-feature-name
   ```
7. **创建Pull Request** 从您的功能分支到 `develop` 分支

### 🔍 代码审查

所有Pull Request都需要：
- ✅ 通过自动化测试
- ✅ 代码审查批准
- ✅ 解决所有讨论
- ✅ 符合代码规范

### 📝 提交规范

请使用 [Conventional Commits](https://www.conventionalcommits.org/) 格式：

- `feat:` 新功能
- `fix:` 错误修复
- `docs:` 文档更新
- `style:` 代码格式调整
- `refactor:` 代码重构
- `test:` 测试相关
- `chore:` 构建过程或辅助工具的变动

### 🧪 测试要求

- 新功能必须包含相应的示例代码
- 确保所有平台兼容性测试通过
- 更新相关文档

### 📞 获取帮助

- 📋 [提交Issue](https://github.com/your-username/gk-unity-tools/issues) 报告问题或建议
- 💬 [讨论区](https://github.com/your-username/gk-unity-tools/discussions) 技术讨论
- 📖 [Wiki](https://github.com/your-username/gk-unity-tools/wiki) 详细文档

## ⚙️ 项目配置

### Unity版本要求
- Unity 2021.3 LTS 或更高版本
- 支持团结引擎 (Tuanjie Engine)

### 构建设置

#### 移动平台 (Android/iOS)
```
Player Settings:
- Target API Level: 最新
- Scripting Backend: IL2CPP
- Api Compatibility Level: .NET Standard 2.1
- Strip Engine Code: 启用
```

#### 微信小游戏
```
Player Settings:
- WebGL Memory Size: 32MB
- WebGL Exception Support: 启用
- Compression Format: Gzip
- Code Optimization: 启用
```

#### 抖音小游戏
```
类似微信小游戏设置
需要额外配置抖音开发者工具
```

### 依赖包
项目使用以下Unity包：
- com.unity.ugui (UI系统)
- com.unity.textmeshpro (文本渲染)
- com.unity.timeline (时间轴)

## 📞 支持

如果您在使用过程中遇到问题，请查看示例代码或提交Issue。


