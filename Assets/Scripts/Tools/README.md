# GK Tools 工具类集合

这个文件夹包含了所有的跨平台游戏工具类，使用 `GK` 命名空间，按功能分类组织。

## 📁 文件夹结构

### Platform/ - 平台检测 (GK.Platform)
- `PlatformDetector.cs`: 检测当前运行平台（移动设备、微信小游戏、抖音小游戏等）

### Adapters/ - 适配器 (GK.Adapters)
- `ScreenAdapter.cs`: 屏幕分辨率和安全区域适配
- `UIAdapter.cs`: UI元素在不同屏幕尺寸下的适配

### Managers/ - 管理器 (GK.Managers)
- `InputManager.cs`: 统一输入管理（触摸、鼠标、键盘）
- `AudioManager.cs`: 音频播放和管理
- `DataManager.cs`: 本地数据存储管理
- `NetworkManager.cs`: HTTP网络请求管理
- `PerformanceManager.cs`: 性能监控和优化

### Utils/ - 内部工具类 (GK.Utils)
- `GameUtils.cs`: 内部工具类集合（不建议直接使用）

### Examples/ - 示例代码 (GK.Examples)
- `QuickStartExample.cs`: 🚀 快速开始示例，适合新手入门
- `ComprehensiveExample.cs`: 📖 综合示例，展示所有功能的标准用法
- `GameUtilsExample.cs`: 🎮 原始完整示例，包含UI交互演示
- `NetworkManagerTest.cs`: 🌐 网络管理器专项测试脚本
- `DiagnosticsTest.cs`: 🔧 诊断工具测试脚本
- `NamespaceTest.cs`: 📦 GK命名空间测试脚本
- `ScreenAdapter/`: 📱 屏幕适配工具示例和说明
- `ScreenAdapterUI/`: 🖼️ UI适配工具示例和说明
- `readme.txt`: 📋 示例说明文档

### 文档
- `README.md`: 📖 本文档
- `USAGE_GUIDE.md`: 📚 详细使用指南 ⭐
- `NAMESPACE_GUIDE.md`: 📦 GK命名空间使用指南 ⭐
- `CODE_STANDARDS.md`: 📝 代码规范和最佳实践 ⭐
- `API_COMPATIBILITY.md`: 🔧 API兼容性说明和问题解决指南
- `API_UPDATE_GUIDE.md`: ⚠️ Unity API更新警告的详细处理指南

## 🎯 使用方式

**推荐使用方式**: 通过根目录的 `GameUtils.cs` 统一入口访问所有功能

```csharp
// 一行代码初始化
GameUtils.Initialize();

// 简单易用的API
GameUtils.Platform.IsMobile();           // 平台检测
GameUtils.Audio.PlayMusic("bgm");        // 音频播放
GameUtils.Data.Save("key", data);        // 数据存储
GameUtils.Network.Get(url, callback);    // 网络请求
GameUtils.Performance.GetCurrentFPS();   // 性能监控
```

**直接使用方式**: 如果需要更精细的控制，可以直接使用具体的工具类

```csharp
using GK.Platform;
using GK.Managers;

// 直接使用
var platform = PlatformDetector.CurrentPlatform;
AudioManager.Instance.PlayMusic("bgm");
```

## 🔧 命名空间

- `GK.Platform`: 平台检测相关
- `GK.Adapters`: 适配器相关
- `GK.Managers`: 管理器相关
- `GK.Utils`: 内部工具类
- `GK.Examples`: 示例代码

## 📦 依赖关系

```
GameUtils (根入口)
    ↓
Tools.Utils.GameUtils (内部集合)
    ↓
各个具体工具类 (Platform, Adapters, Managers)
```

## 🚀 扩展指南

### 添加新的工具类

1. 在相应的文件夹中创建新的工具类
2. 使用正确的命名空间 `GK.XXX`
3. 在根目录的 `GameUtils.cs` 中添加便捷访问方法
4. 更新文档

### 添加新的分类

1. 在Tools文件夹下创建新的子文件夹
2. 创建相应的命名空间 `GK.NewCategory`
3. 在根目录的 `GameUtils.cs` 中添加新的静态类
4. 更新文档

## 📋 注意事项

- 所有工具类都设计为跨平台兼容
- 管理器类通常使用单例模式
- 适配器类可以作为组件添加到GameObject上
- 平台检测会自动适配不同平台的特性和限制

## 🔄 版本兼容性

- Unity 2021.3 LTS 或更高版本
- 支持团结引擎 (Tuanjie Engine)
- 兼容 IL2CPP 和 Mono 后端
- 支持所有主流平台构建
