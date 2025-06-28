# GK 命名空间使用指南

## 🎯 命名空间重构

我们已经将整个项目重构为使用 `GK` 命名空间，这带来了更好的代码组织和管理效果。

## 📁 新的命名空间结构

```
GK                          # 根命名空间
├── Platform                # 平台检测
│   └── PlatformDetector
├── Adapters                # 适配器
│   ├── ScreenAdapter
│   └── UIAdapter
├── Managers                # 管理器
│   ├── InputManager
│   ├── AudioManager
│   ├── DataManager
│   ├── NetworkManager
│   └── PerformanceManager
├── Utils                   # 内部工具类
│   └── GameUtils (内部)
├── Examples                # 示例代码
│   ├── GameUtilsExample
│   ├── NetworkManagerTest
│   └── DiagnosticsTest
└── GameUtils (主入口)      # 统一入口类
```

## 🚀 使用方式

### 方式1: 直接使用 (推荐)
```csharp
using UnityEngine;

public class MyScript : MonoBehaviour
{
    void Start()
    {
        // 直接使用，无需额外using语句
        GameUtils.Initialize();
        
        // 使用各种功能
        GameUtils.Platform.IsMobile();
        GameUtils.Audio.PlayMusic("bgm");
        GameUtils.Data.Save("key", data);
    }
}
```

### 方式2: 显式使用GK命名空间
```csharp
using UnityEngine;
using GK;

public class MyScript : MonoBehaviour
{
    void Start()
    {
        // 使用GK命名空间
        GameUtils.Initialize();
        
        // 或者直接使用具体的类
        var platform = PlatformDetector.CurrentPlatform;
        AudioManager.Instance.PlayMusic("bgm");
    }
}
```

### 方式3: 使用别名
```csharp
using UnityEngine;
using GameUtils = GK.GameUtils;

public class MyScript : MonoBehaviour
{
    void Start()
    {
        // 使用别名
        GameUtils.Initialize();
        GameUtils.Platform.IsMobile();
    }
}
```

## ✨ 优势

### 1. 避免命名冲突
```csharp
// 之前可能的冲突
Debug.Log("Unity日志");           // 可能与GameUtils.Debug冲突

// 现在完全隔离
Debug.Log("Unity日志");           // Unity的Debug
GameUtils.Diagnostics.LogAllStatus(); // GK的诊断工具
```

### 2. 清晰的层次结构
```csharp
// 一眼就能看出这是我们的工具类
GK.Platform.PlatformDetector
GK.Managers.AudioManager
GK.Adapters.ScreenAdapter
```

### 3. 便于移植
```csharp
// 只需要复制GK命名空间下的所有代码
// 在新项目中添加 using GK; 即可使用
```

### 4. 更好的IntelliSense支持
```csharp
// 输入 GK. 后会显示所有可用的子命名空间
GK.Platform.     // 平台相关
GK.Managers.     // 管理器相关
GK.Adapters.     // 适配器相关
```

## 🔄 迁移指南

### 从旧版本迁移
如果您之前使用的是 `GameUtils.Tools.*` 命名空间：

```csharp
// 旧版本
using GameUtils.Tools.Platform;
using GameUtils.Tools.Managers;

// 新版本
using GK.Platform;
using GK.Managers;
// 或者简单地使用
using GK;
```

### API调用保持不变
```csharp
// 这些调用方式完全不变
GameUtils.Initialize();
GameUtils.Platform.IsMobile();
GameUtils.Audio.PlayMusic("bgm");
GameUtils.Data.Save("key", data);
```

## 📚 示例代码

### 完整的使用示例
```csharp
using UnityEngine;
using GK;

namespace MyGame
{
    public class GameManager : MonoBehaviour
    {
        void Start()
        {
            // 初始化GK工具类
            GameUtils.Initialize();
            
            // 检测平台
            if (GameUtils.Platform.IsMobile())
            {
                Debug.Log("运行在移动设备上");
                GameUtils.Performance.SetTargetFrameRate(30);
            }
            else if (GameUtils.Platform.IsMiniGame())
            {
                Debug.Log("运行在小游戏平台");
                GameUtils.Performance.SetTargetFrameRate(60);
            }
            
            // 适配屏幕
            GameUtils.Screen.Adapt();
            
            // 播放背景音乐
            GameUtils.Audio.PlayMusic("background");
            
            // 加载游戏数据
            var gameData = GameUtils.Data.Load<GameData>("save");
            
            // 输出系统信息
            GameUtils.Diagnostics.LogAllStatus();
        }
    }
    
    [System.Serializable]
    public class GameData
    {
        public int level;
        public int score;
        public string playerName;
    }
}
```

## 🛠️ 开发建议

### 1. 推荐的项目结构
```
MyGame/
├── Scripts/
│   ├── GK/                 # GK工具类 (复制整个文件夹)
│   ├── Game/               # 游戏逻辑
│   ├── UI/                 # UI相关
│   └── Data/               # 数据模型
```

### 2. 命名约定
```csharp
// 使用GK命名空间的类
namespace GK.MyExtension
{
    public class MyCustomManager : MonoBehaviour
    {
        // 自定义扩展
    }
}

// 游戏逻辑使用自己的命名空间
namespace MyGame.Logic
{
    public class GameController : MonoBehaviour
    {
        // 游戏逻辑
    }
}
```

### 3. 扩展建议
```csharp
// 扩展GK工具类
namespace GK.Extensions
{
    public static class GameUtilsExtensions
    {
        public static void MyCustomFunction()
        {
            // 自定义功能
        }
    }
}
```

## 📞 技术支持

如果在使用新的命名空间时遇到问题：

1. 确保添加了正确的 `using GK;` 语句
2. 检查Assembly Definition是否正确引用
3. 确认所有文件都使用了新的命名空间
4. 参考示例代码进行调试

## 🔄 版本历史

- **v1.0-1.4**: 使用 `GameUtils.Tools.*` 命名空间
- **v2.0**: 重构为 `GK.*` 命名空间，提供更好的组织结构
