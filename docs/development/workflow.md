# 开发工作流 | Development Workflow

## 🔄 WSL + Windows 跨平台开发

### 环境配置
- **WSL (开发)**: `/mnt/g/workspace/ra2`
- **Windows (测试)**: `G:\workspace\ra2`
- **文件共享**: 实时同步

### 推荐工作流
```
[WSL] 编辑代码/配置 → [Windows] 测试游戏 → [WSL] 提交修改
```

---

## 📝 典型开发流程

### 1. 修改配置文件
```bash
# [WSL] 编辑
cd /mnt/g/workspace/ra2
vim mods/ra2/rules/allied-infantry.yaml

# 修改后保存 (:wq)
# 文件自动同步到Windows
```

### 2. 测试验证
```bash
# 方式A: WSL中启动Windows测试
./wsl-to-windows.sh

# 方式B: Windows中直接运行
# 双击 quick-test-windows.cmd
```

### 3. 版本控制
```bash
# [WSL] 查看修改
git status
git diff

# [WSL] 提交
git add mods/ra2/rules/
git commit -m "Update infantry stats"
git push
```

---

## 🛠️ 开发工具

### WSL工具
| 命令 | 用途 |
|------|------|
| `vim / code` | 代码编辑 |
| `git` | 版本控制 |
| `make all` | 编译C#代码 |
| `./autotest.sh` | 自动化测试 |
| `./watch-and-test.sh` | 文件监控测试 |

### Windows工具
| 文件 | 用途 |
|------|------|
| `quick-test-windows.cmd` | 快速测试 |
| `test-veterancy.cmd` | 完整测试菜单 |
| `launch-game.cmd` | 直接启动游戏 |

---

## 🎯 使用场景

### 场景1: 快速验证配置
```bash
# 1. [WSL] 修改配置
vim mods/ra2/rules/allied-infantry.yaml

# 2. [WSL] 自动测试
./autotest.sh fast auto

# 3. [WSL] 查看结果
./analyze-test.sh
```

### 场景2: 开发新功能
```bash
# 1. [WSL] 修改C#代码
code OpenRA.Mods.RA2/Traits/

# 2. [WSL] 编译
make all

# 3. [Windows] 测试
quick-test-windows.cmd

# 4. [WSL] 查看日志
tail -f ~/.config/openra/Logs/ra2.log
```

### 场景3: 持续迭代
```bash
# 终端1: 启动文件监控
./watch-and-test.sh

# 终端2: 持续编辑
vim mods/ra2/rules/...
# 每次保存自动触发测试
```

---

## 📊 最佳实践

### ✅ 推荐做法
- 所有文件编辑在WSL中进行
- 游戏测试在Windows中运行
- 使用Git管理版本
- 经常提交小的更改
- 使用自动化测试验证

### ❌ 避免做法
- 不要同时在两边编辑同一文件
- 不要手动复制文件(已自动共享)
- 不要在WSL文件系统(`~/`)工作
- 不要跳过测试就提交

---

## 🐛 故障排除

### 文件同步问题
```bash
# 确认工作目录
pwd  # 应显示 /mnt/g/workspace/ra2

# 检查文件
ls -l mods/ra2/rules/
```

### 脚本权限问题
```bash
# 添加执行权限
chmod +x *.sh
```

### 游戏启动问题
```bash
# 查看日志
tail -50 ~/.config/openra/Logs/ra2.log

# 验证路径
ls -l launch-game.cmd
```

---

## 💡 进阶技巧

### VS Code集成
```bash
# 在WSL中打开项目
code .

# VS Code会自动检测WSL环境
# 可使用集成终端
```

### 别名设置
```bash
# 添加到 ~/.bashrc
alias cdra2='cd /mnt/g/workspace/ra2'
alias testgame='./autotest.sh fast auto'
alias watchgame='./watch-and-test.sh'

# 使用
cdra2
testgame
```

### 日志分析
```bash
# 实时查看日志
tail -f ~/.config/openra/Logs/ra2.log

# 搜索错误
grep -i error ~/.config/openra/Logs/ra2.log

# 查看最近50行
tail -50 ~/.config/openra/Logs/ra2.log
```

---

## 🔗 相关文档

- `quick-start.md` - 快速入门
- `../testing/overview.md` - 测试指南
- `debug-panel.md` - 调试工具

---

**高效工作流,快乐开发!** 🚀
