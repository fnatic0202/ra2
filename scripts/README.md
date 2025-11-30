# 脚本中心 | Scripts Center

本目录包含所有开发和测试脚本。

---

## 📋 脚本分类

### 测试脚本 (`test/`)
快速测试和验证游戏功能

| 脚本 | 平台 | 用途 |
|------|------|------|
| `quick-test-windows.cmd` | Windows | ⭐ 最简单,双击即启动 |
| `test-veterancy.cmd` | Windows | 交互式测试菜单 (CMD) |
| `test-veterancy.ps1` | Windows | 交互式测试菜单 (PowerShell) |
| `test-veterancy.sh` | WSL | Linux测试脚本 |
| `test-fast.cmd` | Windows | 快速开发者模式 |
| `test-normal.cmd` | Windows | 正常游戏模式 |

### 开发工具 (`dev/`)
自动化和开发辅助工具

| 脚本 | 平台 | 用途 |
|------|------|------|
| `autotest.sh` | WSL | ⭐ 自动化测试主脚本 |
| `watch-and-test.sh` | WSL | 文件监控自动测试 |
| `analyze-test.sh` | WSL | 测试日志分析 |
| `sync-to-windows.sh` | WSL | 检查文件同步状态 |
| `wsl-to-windows.sh` | WSL | WSL切换到Windows |
| `autotest-windows.cmd` | Windows | Windows自动化测试 |

---

## 🚀 快速开始

### Windows用户 (最简单)
```cmd
# 双击运行
scripts\test\quick-test-windows.cmd
```

### WSL用户 (开发推荐)
```bash
# 自动化测试
./scripts/dev/autotest.sh fast auto

# 文件监控测试
./scripts/dev/watch-and-test.sh

# 分析测试结果
./scripts/dev/analyze-test.sh
```

---

## 📖 详细使用说明

### 1. 自动化测试 (autotest.sh)

**最强大的测试工具,从WSL一键启动完整测试流程**

```bash
# 快速自动测试 (30秒)
./scripts/dev/autotest.sh fast auto

# 正常模式手动测试
./scripts/dev/autotest.sh normal manual

# 查看帮助
./scripts/dev/autotest.sh --help
```

**功能:**
- ✅ 自动启动Windows游戏
- ✅ 自动关闭并收集日志
- ✅ 生成测试报告
- ✅ 错误检测和分析

### 2. 文件监控 (watch-and-test.sh)

**持续开发最佳选择,保存即测试**

```bash
# 启动监控
./scripts/dev/watch-and-test.sh

# 修改配置文件后自动触发测试
vim mods/ra2/rules/allied-infantry.yaml
:wq  # 保存后自动测试
```

### 3. 快速测试 (quick-test-windows.cmd)

**Windows最简单的测试方式**

```cmd
# 方式1: 双击文件
scripts\test\quick-test-windows.cmd

# 方式2: 命令行运行
cd scripts\test
quick-test-windows.cmd
```

### 4. 测试分析 (analyze-test.sh)

**分析测试日志,查找问题**

```bash
# 分析最新测试
./scripts/dev/analyze-test.sh

# 查看统计信息
./scripts/dev/analyze-test.sh --stats
```

---

## 🔄 工作流示例

### 场景1: 快速验证修改
```bash
# 1. 修改配置
vim mods/ra2/rules/allied-infantry.yaml

# 2. 自动测试
./scripts/dev/autotest.sh fast auto

# 3. 查看结果
./scripts/dev/analyze-test.sh
```

### 场景2: 持续开发
```bash
# 终端1: 启动监控
./scripts/dev/watch-and-test.sh

# 终端2: 编辑文件
vim mods/ra2/rules/...
# 保存后自动测试
```

### 场景3: Windows直接测试
```cmd
# 双击运行
scripts\test\quick-test-windows.cmd

# 或使用菜单
scripts\test\test-veterancy.cmd
```

---

## 💡 脚本选择指南

| 需求 | 推荐脚本 | 原因 |
|------|---------|------|
| Windows快速测试 | `test/quick-test-windows.cmd` | 最简单,双击即用 |
| WSL自动化测试 | `dev/autotest.sh` | 全流程自动化 |
| 持续开发 | `dev/watch-and-test.sh` | 保存即测试 |
| 测试分析 | `dev/analyze-test.sh` | 详细的日志分析 |
| 跨平台切换 | `dev/wsl-to-windows.sh` | 便捷切换 |

---

## 🛠️ 脚本维护

### 添加新脚本
1. 根据用途放入 `test/` 或 `dev/`
2. 添加执行权限: `chmod +x script.sh`
3. 更新本文档的脚本列表

### 废弃脚本
1. 移动到 `archived/` 目录
2. 从本文档中移除

### 脚本规范
- **命名**: 使用描述性名称 (如 `autotest.sh`)
- **注释**: 脚本开头说明用途和用法
- **帮助**: 提供 `--help` 参数
- **错误处理**: 检查依赖和错误退出码

---

## 📂 目录结构

```
scripts/
├── README.md           # 本文件
├── test/              # 测试脚本
│   ├── quick-test-windows.cmd
│   ├── test-veterancy.cmd
│   ├── test-veterancy.ps1
│   ├── test-veterancy.sh
│   ├── test-fast.cmd
│   └── test-normal.cmd
├── dev/               # 开发工具
│   ├── autotest.sh
│   ├── watch-and-test.sh
│   ├── analyze-test.sh
│   ├── sync-to-windows.sh
│   ├── wsl-to-windows.sh
│   └── autotest-windows.cmd
└── archived/          # 废弃脚本
```

---

## 🐛 故障排除

### WSL脚本无法执行
```bash
# 添加执行权限
chmod +x scripts/dev/*.sh
chmod +x scripts/test/*.sh
```

### Windows脚本乱码
- 所有CMD脚本已使用ASCII编码
- 如遇问题,用记事本检查编码

### 路径问题
```bash
# WSL中运行脚本
cd /mnt/g/workspace/ra2
./scripts/dev/autotest.sh

# Windows中运行
cd G:\workspace\ra2\scripts\test
quick-test-windows.cmd
```

---

## 📖 相关文档

- [测试指南](../docs/testing/overview.md) - 完整测试文档
- [开发工作流](../docs/development/workflow.md) - 开发流程说明
- [快速开始](../docs/development/quick-start.md) - 新手入门

---

**高效脚本,快乐开发!** 🚀
