# 🛡️ 分支保护配置指南

本文档说明如何为GK Unity工具包项目配置GitHub分支保护规则。

## 📋 目录

- [main分支保护配置](#main分支保护配置)
- [develop分支保护配置](#develop分支保护配置)
- [配置步骤](#配置步骤)
- [验证配置](#验证配置)

## 🏷️ main分支保护配置

### 基础保护设置

```
☑️ Require a pull request before merging
  ☑️ Require approvals (1)
  ☑️ Dismiss stale PR approvals when new commits are pushed
  ☑️ Require review from code owners (如果有CODEOWNERS文件)

☑️ Require status checks to pass before merging
  ☑️ Require branches to be up to date before merging
  ☑️ Status checks:
    - Validate Package
    - Test Compatibility
    - Build Package
```

### 高级保护设置

```
☑️ Require conversation resolution before merging
☐ Require signed commits (可选)
☐ Require linear history (可选)
☐ Require deployments to succeed before merging
☐ Lock branch
☑️ Do not allow bypassing the above settings
```

### 管理员规则

```
☐ Allow force pushes
☐ Allow deletions
```

## 🚧 develop分支保护配置

### 基础保护设置

```
☑️ Require a pull request before merging
  ☑️ Require approvals (1)
  ☑️ Dismiss stale PR approvals when new commits are pushed
  ☐ Require review from code owners (相对宽松)

☑️ Require status checks to pass before merging
  ☑️ Require branches to be up to date before merging
  ☑️ Status checks:
    - validate-structure / Validate Package Structure
    - validate-namespace / Validate Namespace Usage
    - validate-documentation / Validate Documentation
    - validate-examples / Validate Example Code
```

### 高级保护设置

```
☑️ Require conversation resolution before merging
☐ Require signed commits
☐ Require linear history
☐ Require deployments to succeed before merging
☐ Lock branch
☐ Do not allow bypassing the above settings (允许管理员绕过)
```

### 管理员规则

```
☐ Allow force pushes
☐ Allow deletions
```

## ⚙️ 配置步骤

### 1. 访问仓库设置

1. 打开GitHub仓库页面
2. 点击 **Settings** 标签
3. 在左侧菜单中选择 **Branches**

### 2. 配置main分支保护

1. 点击 **Add rule** 按钮
2. 在 **Branch name pattern** 中输入 `main`
3. 按照上述配置勾选相应选项
4. 点击 **Create** 保存规则

### 3. 配置develop分支保护

1. 再次点击 **Add rule** 按钮
2. 在 **Branch name pattern** 中输入 `develop`
3. 按照上述配置勾选相应选项
4. 点击 **Create** 保存规则

### 4. 配置状态检查

确保以下GitHub Actions工作流正常运行：

- **CI/CD Pipeline** (`.github/workflows/ci.yml`)
- **Package Validation** (`.github/workflows/package-validation.yml`)

## ✅ 验证配置

### 测试main分支保护

1. 尝试直接推送到main分支（应该被拒绝）
2. 创建PR到main分支，验证需要审查
3. 确认CI检查必须通过

### 测试develop分支保护

1. 尝试直接推送到develop分支（应该被拒绝）
2. 创建PR到develop分支，验证需要审查
3. 确认包验证检查必须通过

## 🔧 故障排除

### 常见问题

**Q: 状态检查不显示？**
A: 确保GitHub Actions工作流已经运行过至少一次

**Q: 无法合并PR？**
A: 检查所有必需的状态检查是否通过

**Q: 管理员无法推送？**
A: 检查是否启用了"Do not allow bypassing"选项

### 调试步骤

1. 检查Actions标签页的工作流运行状态
2. 查看PR页面的检查详情
3. 确认分支保护规则配置正确

## 📝 配置模板

### CODEOWNERS文件（可选）

创建 `.github/CODEOWNERS` 文件：

```
# GK Unity工具包代码所有者

# 全局代码所有者
* @your-username

# 核心工具类
Assets/Scripts/Tools/ @your-username @co-maintainer

# 文档
*.md @your-username
docs/ @your-username

# CI/CD配置
.github/ @your-username
```

### 分支保护JSON配置

如果需要通过API配置，可以使用以下JSON模板：

```json
{
  "required_status_checks": {
    "strict": true,
    "contexts": [
      "validate-package / Validate Unity Package",
      "test-compatibility / Test Platform Compatibility"
    ]
  },
  "enforce_admins": true,
  "required_pull_request_reviews": {
    "required_approving_review_count": 1,
    "dismiss_stale_reviews": true
  },
  "restrictions": null,
  "required_conversation_resolution": true
}
```

## 🎯 最佳实践

1. **渐进式保护** - 先配置基础保护，再逐步加强
2. **团队协作** - 确保团队成员了解分支保护规则
3. **定期审查** - 定期检查和更新保护规则
4. **文档同步** - 保持文档与实际配置同步

---

配置完成后，您的仓库将拥有企业级的分支保护策略！🛡️
