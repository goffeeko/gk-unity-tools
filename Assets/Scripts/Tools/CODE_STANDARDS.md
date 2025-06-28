# GK工具类代码规范

## 📋 代码规范检查清单

### ✅ 命名规范
- [x] 使用 `GK` 作为根命名空间
- [x] 类名使用 PascalCase（如：`PlatformDetector`）
- [x] 方法名使用 PascalCase（如：`GetCurrentPlatform`）
- [x] 私有字段使用 _camelCase（如：`_isInitialized`）
- [x] 公共属性使用 PascalCase（如：`CurrentPlatform`）
- [x] 常量使用 UPPER_CASE（如：`UPDATE_INTERVAL`）

### ✅ 注释规范
- [x] 所有公共类都有 XML 文档注释
- [x] 所有公共方法都有 `<summary>` 注释
- [x] 复杂逻辑有行内注释说明
- [x] 参数和返回值有详细说明
- [x] 示例代码包含使用说明

### ✅ 代码结构
- [x] 使用 `#region` 组织代码块
- [x] 字段、属性、方法按逻辑分组
- [x] 公共成员在前，私有成员在后
- [x] 构造函数在属性之后，方法之前
- [x] 事件处理方法集中放置

### ✅ 异常处理
- [x] 关键操作使用 try-catch 包装
- [x] 异常信息包含上下文
- [x] 使用 Debug.LogError 记录错误
- [x] 提供降级处理方案
- [x] 避免空引用异常

### ✅ 性能优化
- [x] 避免在 Update 中频繁分配内存
- [x] 使用对象池模式（如音频源池）
- [x] 缓存频繁访问的组件引用
- [x] 合理使用协程避免阻塞
- [x] 及时释放不需要的资源

## 🎯 具体规范示例

### 1. 类结构规范
```csharp
using UnityEngine;
using System.Collections.Generic;
using GK.Platform;

namespace GK.Managers
{
    /// <summary>
    /// 音频管理器
    /// 处理不同平台的音频播放和管理
    /// </summary>
    public class AudioManager : MonoBehaviour
    {
        #region 序列化字段
        [Header("音频设置")]
        [SerializeField] private bool initializeOnStart = true;
        [SerializeField] private float masterVolume = 1.0f;
        #endregion

        #region 私有字段
        private static AudioManager _instance;
        private AudioSource _musicSource;
        private List<AudioSource> _sfxSources;
        #endregion

        #region 公共属性
        public static AudioManager Instance => _instance;
        public float MasterVolume { get; set; }
        #endregion

        #region Unity生命周期
        private void Awake() { }
        private void Start() { }
        #endregion

        #region 公共方法
        public void PlayMusic(string clipName) { }
        public void PlaySfx(string clipName) { }
        #endregion

        #region 私有方法
        private void InitializeAudioSources() { }
        private AudioSource GetAvailableSfxSource() { }
        #endregion
    }
}
```

### 2. 方法注释规范
```csharp
/// <summary>
/// 播放背景音乐
/// </summary>
/// <param name="clipName">音频剪辑名称</param>
/// <param name="loop">是否循环播放</param>
/// <param name="fadeInTime">淡入时间（秒）</param>
/// <example>
/// <code>
/// AudioManager.Instance.PlayMusic("background", true, 2f);
/// </code>
/// </example>
public void PlayMusic(string clipName, bool loop = true, float fadeInTime = 0f)
{
    // 实现代码
}
```

### 3. 异常处理规范
```csharp
/// <summary>
/// 安全地保存数据
/// </summary>
public void SaveData<T>(string key, T data)
{
    try
    {
        string jsonData = JsonUtility.ToJson(data);
        
        if (PlatformDetector.IsMiniGame)
        {
            SaveToPlayerPrefs(key, jsonData);
        }
        else
        {
            SaveToFile(key, jsonData);
        }
        
        Debug.Log($"[GK] Data saved successfully: {key}");
    }
    catch (System.Exception e)
    {
        Debug.LogError($"[GK] Failed to save data '{key}': {e.Message}");
        // 提供降级方案或用户提示
    }
}
```

### 4. 日志规范
```csharp
// 使用统一的日志前缀
Debug.Log("[GK] Initializing GameUtils...");
Debug.LogWarning("[GK] Network not available, using cached data");
Debug.LogError("[GK] Failed to load audio clip: " + clipName);

// 包含上下文信息
Debug.Log($"[GK] Platform detected: {platform}, Mobile: {isMobile}");
```

### 5. 常量定义规范
```csharp
public class NetworkManager : MonoBehaviour
{
    #region 常量定义
    private const float DEFAULT_TIMEOUT = 30f;
    private const int MAX_RETRY_COUNT = 3;
    private const string LOG_PREFIX = "[GK.Network]";
    #endregion
}
```

## 🔍 代码审查要点

### 1. 可读性检查
- [ ] 变量名能清楚表达用途
- [ ] 方法长度适中（一般不超过50行）
- [ ] 嵌套层级不超过3层
- [ ] 避免魔法数字，使用命名常量
- [ ] 代码逻辑清晰，易于理解

### 2. 维护性检查
- [ ] 单一职责原则
- [ ] 避免重复代码
- [ ] 合理的抽象层次
- [ ] 松耦合设计
- [ ] 易于扩展和修改

### 3. 性能检查
- [ ] 避免不必要的内存分配
- [ ] 合理使用缓存
- [ ] 避免频繁的字符串拼接
- [ ] 适当的算法复杂度
- [ ] 资源及时释放

### 4. 兼容性检查
- [ ] 支持目标Unity版本
- [ ] 兼容不同平台
- [ ] 处理API版本差异
- [ ] 优雅降级处理
- [ ] 向后兼容性

## 📝 提交前检查清单

### 代码质量
- [ ] 编译无错误无警告
- [ ] 所有公共API有文档注释
- [ ] 异常处理完善
- [ ] 日志信息清晰
- [ ] 性能影响可接受

### 测试验证
- [ ] 基本功能测试通过
- [ ] 不同平台测试通过
- [ ] 边界条件测试
- [ ] 错误情况处理测试
- [ ] 性能测试通过

### 文档更新
- [ ] README.md 更新
- [ ] 示例代码更新
- [ ] API文档更新
- [ ] 变更日志更新
- [ ] 使用指南更新

## 🛠️ 开发工具推荐

### IDE设置
- 启用代码格式化
- 配置代码分析规则
- 使用一致的缩进（4个空格）
- 启用拼写检查
- 配置代码模板

### 推荐插件
- Unity Code Snippets
- C# Extensions
- XML Documentation Comments
- Code Spell Checker
- GitLens

## 📚 参考资源

- [C# 编码约定](https://docs.microsoft.com/zh-cn/dotnet/csharp/programming-guide/inside-a-program/coding-conventions)
- [Unity 脚本编写最佳实践](https://unity.com/how-to/naming-and-code-style-tips-c-scripting-unity)
- [.NET API 设计指南](https://docs.microsoft.com/zh-cn/dotnet/standard/design-guidelines/)

## ⚠️ 常见编译错误

### 字符字面量错误
**错误**: `CS1012: Too many characters in character literal`

**原因**: 字符字面量只能包含单个字符，不能包含多个字符。

**错误示例**:
```csharp
// ❌ 错误：字符字面量包含多个字符
string result = text.Replace('\n', ', ');  // 逗号和空格是两个字符

// ❌ 错误：语法错误
string result = text.Replace('\n', ' , ');
```

**正确写法**:
```csharp
// ✅ 正确：字符替换字符 (Replace(char, char))
string result = text.Replace('\n', ' ');   // 单字符替换单字符

// ✅ 正确：字符串替换字符串 (Replace(string, string))
string result = text.Replace("\n", ", ");  // 字符串替换字符串

// ❌ 错误：混用字符和字符串参数
// string result = text.Replace('\n', ", ");  // 编译错误：参数类型不匹配
```

**Replace方法重载说明**:
- `Replace(char oldChar, char newChar)`: 字符替换字符
- `Replace(string oldValue, string newValue)`: 字符串替换字符串
- 不能混用字符和字符串参数

## 🔄 持续改进

定期审查和更新代码规范：
1. 收集团队反馈
2. 分析常见问题
3. 更新规范文档
4. 培训团队成员
5. 工具和流程优化
