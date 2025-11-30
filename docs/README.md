# 文档中心 | Documentation Center

欢迎查看 OpenRA RA2 Mod 项目文档。

---

## 📚 文档索引

### ⭐ 功能文档 | Features
- **[功能列表 (FEATURES.md)](../FEATURES.md)** - 所有功能的版本化清单
- **[英雄复活系统](features/hero-revival-system/README.md)** - Warcraft 3风格英雄系统 (v1.0.0)

### 开发文档 | Development
- **[快速开始](development/quick-start.md)** - 5分钟快速上手指南
- **[开发工作流](development/workflow.md)** - WSL + Windows 跨平台开发流程

### 测试文档 | Testing
- **[测试总览](testing/overview.md)** - 测试方法和工具总览
- **[自动化测试](testing/automated-testing.md)** - 自动化测试详细说明
- **[升级系统测试](testing/veterancy-testing.md)** - 单位升级系统测试指南

### 游戏设计 | Game Design
- **[多Agent架构](game-design/agents.md)** - Claude AI多Agent游戏开发系统说明

---

## 🚀 快速链接

### 我是新手
1. 阅读 [快速开始指南](development/quick-start.md)
2. 了解 [开发工作流](development/workflow.md)
3. 尝试修改配置并测试

### 我要开发功能
1. 参考 [开发工作流](development/workflow.md)
2. 使用 [测试工具](testing/overview.md) 验证
3. 查看项目根目录 `README.md` 了解项目结构

### 我要测试
1. 查看 [测试总览](testing/overview.md)
2. 使用自动化测试脚本
3. 参考 [升级系统测试](testing/veterancy-testing.md)

---

## 📂 文档结构

```
docs/
├── README.md                    # 本文件
├── features/                    # 功能文档 (NEW!)
│   └── hero-revival-system/    # 英雄复活系统
│       ├── README.md           # 功能概览
│       ├── CHANGELOG.md        # 版本历史
│       ├── HERO_REVIVAL_*.md   # 技术文档
│       └── *.yaml              # 配置示例
├── development/                 # 开发文档
│   ├── quick-start.md          # 快速开始
│   └── workflow.md             # 工作流程
├── testing/                     # 测试文档
│   ├── overview.md             # 测试总览
│   ├── automated-testing.md    # 自动化测试
│   └── veterancy-testing.md    # 升级测试
└── game-design/                # 游戏设计
    └── agents.md               # AI Agent系统
```

---

## 🔗 外部资源

- **OpenRA官网**: https://www.openra.net/
- **OpenRA Wiki**: https://github.com/OpenRA/OpenRA/wiki
- **项目仓库**: 查看根目录 `README.md`

---

## 💡 文档贡献

文档需要改进? 欢迎贡献:
1. 在 `docs/` 目录下编辑相应文档
2. 遵循现有格式和结构
3. 提交 Pull Request

---

**祝你开发愉快! Happy Coding!** 🎉
