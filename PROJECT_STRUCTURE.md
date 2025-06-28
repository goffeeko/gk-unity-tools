# 📁 GK Unity Tools 项目结构

## 🎯 项目概述

GK Unity Tools 是一个专为Unity 2D游戏开发的跨平台工具类集合，使用 `GK` 命名空间，支持移动设备、微信小游戏、抖音小游戏等多个平台的适配。

## 📂 目录结构

```
gk-unity-tools/
├── 📁 Assets/
│   ├── 📁 Scenes/                    # Unity场景文件
│   │   └── SampleScene.scene         # 示例场景
│   └── 📁 Scripts/                   # 核心代码目录
│       ├── GameUtils.cs              # 🎯 主入口类 (GK.GameUtils)
│       ├── GameUtils.asmdef          # Assembly Definition (根命名空间: GK)
│       └── 📁 Tools/                 # 工具类集合
│           ├── 📁 Platform/          # 平台检测 (GK.Platform)
│           │   └── PlatformDetector.cs
│           ├── 📁 Adapters/          # 适配器 (GK.Adapters)
│           │   ├── ScreenAdapter.cs
│           │   └── UIAdapter.cs
│           ├── 📁 Managers/          # 管理器 (GK.Managers)
│           │   ├── InputManager.cs
│           │   ├── AudioManager.cs
│           │   ├── DataManager.cs
│           │   ├── NetworkManager.cs
│           │   └── PerformanceManager.cs
│           ├── 📁 Utils/             # 内部工具类 (GK.Utils)
│           │   └── GameUtils.cs      # 内部实现
│           ├── 📁 Examples/          # 示例代码 (GK.Examples)
│           │   ├── QuickStartExample.cs
│           │   ├── ComprehensiveExample.cs
│           │   ├── GameUtilsExample.cs
│           │   ├── NetworkManagerTest.cs
│           │   ├── DiagnosticsTest.cs
│           │   └── NamespaceTest.cs
│           └── 📁 Documentation/     # 工具类文档
│               ├── README.md         # 工具类总览
│               ├── USAGE_GUIDE.md    # 使用指南
│               ├── NAMESPACE_GUIDE.md # 命名空间指南
│               └── CODE_STANDARDS.md # 代码规范
├── 📁 .github/                      # GitHub配置
│   ├── 📁 workflows/                # GitHub Actions
│   │   ├── ci.yml                   # CI/CD流程
│   │   └── package-validation.yml   # 包验证
│   └── BRANCH_PROTECTION_GUIDE.md   # 分支保护指南
├── 📁 ProjectSettings/              # Unity项目设置
├── 📁 Packages/                     # Unity包管理
├── README.md                        # 项目主文档
├── CONTRIBUTING.md                  # 贡献指南
└── MERGE_CHECK_TEST.md              # 合并检查测试文档
```

## 🎯 核心组件

### 1. 主入口类 (GameUtils.cs)
- **位置**: `Assets/Scripts/GameUtils.cs`
- **命名空间**: `GK.GameUtils`
- **作用**: 提供统一的API入口，简化使用方式

### 2. 平台检测 (Platform/)
- **PlatformDetector.cs**: 检测运行平台（移动设备、小游戏等）

### 3. 适配器 (Adapters/)
- **ScreenAdapter.cs**: 屏幕分辨率和安全区域适配
- **UIAdapter.cs**: UI元素自动适配

### 4. 管理器 (Managers/)
- **InputManager.cs**: 统一输入管理
- **AudioManager.cs**: 音频播放和管理
- **DataManager.cs**: 本地数据存储
- **NetworkManager.cs**: HTTP网络请求
- **PerformanceManager.cs**: 性能监控和优化

### 5. 示例代码 (Examples/)
- **QuickStartExample.cs**: 🚀 快速开始示例
- **ComprehensiveExample.cs**: 📖 综合功能示例
- **GameUtilsExample.cs**: 🎮 完整UI交互示例
- **NetworkManagerTest.cs**: 🌐 网络功能测试
- **DiagnosticsTest.cs**: 🔧 诊断工具测试
- **NamespaceTest.cs**: 📦 命名空间测试

## 🔧 技术架构

### 命名空间设计
```
GK                          # 根命名空间
├── Platform                # 平台检测
├── Adapters                # 适配器
├── Managers                # 管理器
├── Utils                   # 内部工具类
├── Examples                # 示例代码
└── GameUtils (主入口)      # 统一入口类
```

### Assembly Definition
- **名称**: GK
- **根命名空间**: GK
- **依赖**: Unity.TextMeshPro, Unity.Timeline, Unity.ugui
- **自动引用**: 启用

## 🚀 使用方式

### 基本使用
```csharp
using UnityEngine;

public class GameManager : MonoBehaviour
{
    void Start()
    {
        // 一行代码初始化
        GameUtils.Initialize();
        
        // 使用各种功能
        GameUtils.Platform.IsMobile();
        GameUtils.Audio.PlayMusic("bgm");
        GameUtils.Data.Save("key", data);
    }
}
```

### 高级使用
```csharp
using UnityEngine;
using GK.Platform;
using GK.Managers;

public class AdvancedUsage : MonoBehaviour
{
    void Start()
    {
        // 直接使用具体类
        var platform = PlatformDetector.CurrentPlatform;
        AudioManager.Instance.PlayMusic("bgm");
    }
}
```

## 📋 项目特点

### ✅ 优势
- 🎯 **统一API** - 简单易用的接口设计
- 🔧 **模块化** - 清晰的功能分离
- 📦 **命名空间隔离** - 避免命名冲突
- 🚀 **跨平台** - 支持多个游戏平台
- 📚 **完整文档** - 详细的使用指南
- 🧪 **示例丰富** - 多种使用场景演示
- ⚙️ **CI/CD** - 自动化测试和构建

### 🛡️ 质量保证
- **分支保护** - master/develop分支保护策略
- **自动化测试** - GitHub Actions CI/CD
- **代码规范** - 统一的编码标准
- **文档完整** - 详细的API文档和使用指南

---

**📝 注意**: 这是一个经过优化的项目结构，移除了重复代码和无用文档，保留了核心功能和必要文档。
