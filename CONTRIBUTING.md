# 🤝 贡献指南

感谢您对GK Unity工具包项目的关注！我们欢迎所有形式的贡献，包括但不限于：

- 🐛 报告错误
- 💡 提出新功能建议
- 📝 改进文档
- 🔧 提交代码修复
- ✨ 开发新功能

## 📋 目录

- [开发环境设置](#-开发环境设置)
- [分支策略](#-分支策略)
- [贡献流程](#-贡献流程)
- [代码规范](#-代码规范)
- [提交规范](#-提交规范)
- [测试要求](#-测试要求)
- [文档要求](#-文档要求)

## 🛠️ 开发环境设置

### 必需软件

- **Unity 2021.3 LTS** 或更高版本
- **Git** 版本控制
- **Visual Studio Code** 或 **Visual Studio** (推荐)
- **Node.js** (用于包管理和CI/CD)

### 项目设置

1. **Fork项目**
   ```bash
   # 在GitHub上Fork项目到您的账户
   ```

2. **克隆项目**
   ```bash
   git clone https://github.com/goffeeko/gk-unity-tools.git
   cd gk-unity-tools
   ```

3. **设置上游仓库**
   ```bash
   git remote add upstream https://github.com/goffeeko/gk-unity-tools.git
   ```

4. **在Unity中打开项目**
   - 启动Unity Hub
   - 点击"打开"选择项目文件夹
   - 等待Unity导入项目

## 🌳 分支策略

我们使用 **Git Flow** 工作流：

### 主要分支

- **`main`** 🏷️
  - 生产分支，包含稳定的发布版本
  - 只接受来自 `release/*` 和 `hotfix/*` 的合并
  - 每次合并都会触发正式版本发布

- **`develop`** 🚧
  - 开发分支，包含最新的开发功能
  - 所有功能分支都合并到这里
  - 定期发布Alpha测试版本

### 支持分支

- **`feature/*`** ✨
  - 功能分支，用于开发新功能
  - 从 `develop` 分支创建
  - 完成后合并回 `develop`

- **`release/*`** 📦
  - 发布分支，用于准备新版本
  - 从 `develop` 分支创建
  - 完成后合并到 `main` 和 `develop`

- **`hotfix/*`** 🚨
  - 热修复分支，用于紧急修复
  - 从 `main` 分支创建
  - 完成后合并到 `main` 和 `develop`

## 🔄 贡献流程

### 1. 准备工作

```bash
# 切换到develop分支
git checkout develop

# 拉取最新代码
git pull upstream develop

# 创建功能分支
git checkout -b feature/your-feature-name
```

### 2. 开发功能

- 📝 编写代码，遵循项目代码规范
- 🧪 添加相应的测试和示例
- 📚 更新相关文档
- ✅ 确保代码编译通过

### 3. 提交更改

```bash
# 添加更改
git add .

# 提交更改（使用规范的提交信息）
git commit -m "feat: 添加新的平台检测功能"

# 推送到您的Fork
git push origin feature/your-feature-name
```

### 4. 创建Pull Request

1. 在GitHub上打开您的Fork
2. 点击"New Pull Request"
3. 选择 `develop` 作为目标分支
4. 填写详细的PR描述
5. 等待代码审查

## 📏 代码规范

### C# 代码规范

```csharp
// ✅ 正确的命名空间使用
namespace GK.Platform
{
    // ✅ 使用PascalCase命名类
    public class PlatformDetector
    {
        // ✅ 使用PascalCase命名公共属性
        public static PlatformType CurrentPlatform { get; private set; }
        
        // ✅ 使用camelCase命名私有字段
        private static bool isInitialized = false;
        
        // ✅ 使用PascalCase命名方法
        public static void Initialize()
        {
            // ✅ 使用camelCase命名局部变量
            var platformType = DetectPlatform();
            CurrentPlatform = platformType;
        }
    }
}
```

### 文件组织

```
Assets/Scripts/Tools/
├── Platform/           # 平台相关功能
├── Managers/          # 管理器类
├── Adapters/          # 适配器类
├── Utils/             # 工具类
└── Examples/          # 示例代码
```

### 注释规范

```csharp
/// <summary>
/// 检测当前运行平台
/// </summary>
/// <returns>平台类型枚举</returns>
public static PlatformType DetectPlatform()
{
    // 检测WebGL平台
    if (Application.platform == RuntimePlatform.WebGLPlayer)
    {
        return PlatformType.WebGL;
    }
    
    return PlatformType.Desktop;
}
```

## 📝 提交规范

使用 [Conventional Commits](https://www.conventionalcommits.org/) 格式：

### 提交类型

- **`feat:`** 新功能
- **`fix:`** 错误修复
- **`docs:`** 文档更新
- **`style:`** 代码格式调整（不影响功能）
- **`refactor:`** 代码重构
- **`test:`** 测试相关
- **`chore:`** 构建过程或辅助工具的变动

### 示例

```bash
feat: 添加微信小游戏平台检测功能
fix: 修复iOS设备屏幕适配问题
docs: 更新API使用文档
style: 统一代码缩进格式
refactor: 重构音频管理器架构
test: 添加平台检测单元测试
chore: 更新CI/CD配置
```

## 🧪 测试要求

### 必需测试

1. **编译测试** - 确保代码在Unity中正常编译
2. **功能测试** - 验证新功能按预期工作
3. **兼容性测试** - 确保跨平台兼容性
4. **示例测试** - 验证示例代码可以正常运行

### 测试文件位置

```
Assets/Scripts/Tools/Examples/
├── QuickStartExample.cs      # 快速开始示例
├── ComprehensiveExample.cs   # 综合功能示例
├── YourFeatureExample.cs     # 您的功能示例
└── Tests/                    # 单元测试（如果需要）
```

## 📚 文档要求

### 必需文档

1. **代码注释** - 所有公共API必须有XML文档注释
2. **README更新** - 如果添加新功能，更新主README
3. **示例代码** - 为新功能提供使用示例
4. **API文档** - 更新相关的API文档

### 文档模板

```csharp
/// <summary>
/// [功能简述]
/// </summary>
/// <param name="parameter">[参数说明]</param>
/// <returns>[返回值说明]</returns>
/// <example>
/// <code>
/// // 使用示例
/// var result = YourMethod(parameter);
/// </code>
/// </example>
public static ReturnType YourMethod(ParameterType parameter)
{
    // 实现代码
}
```

## ❓ 常见问题

### Q: 如何同步上游更改？

```bash
git fetch upstream
git checkout develop
git merge upstream/develop
```

### Q: 如何解决合并冲突？

1. 手动编辑冲突文件
2. 标记冲突已解决：`git add conflicted-file.cs`
3. 完成合并：`git commit`

### Q: 如何撤销提交？

```bash
# 撤销最后一次提交（保留更改）
git reset --soft HEAD~1

# 撤销最后一次提交（丢弃更改）
git reset --hard HEAD~1
```

## 📞 获取帮助

- 📋 [提交Issue](https://github.com/your-username/gk-unity-tools/issues)
- 💬 [讨论区](https://github.com/your-username/gk-unity-tools/discussions)
- 📧 邮件联系：gk-tools@example.com

---

再次感谢您的贡献！🎉
