# 快速开始指南 | Quick Start Guide

## 🚀 5分钟上手 | 5-Minute Setup

### 环境说明 | Environment
- **WSL路径**: `/mnt/g/workspace/ra2`
- **Windows路径**: `G:\workspace\ra2`
- **文件共享**: 实时同步,无需手动操作

### 开发流程 | Workflow
1. **[WSL]** 编辑代码/配置文件
2. **[Windows]** 测试游戏
3. **[WSL]** 提交修改

---

## 📝 常用命令 | Common Commands

### WSL环境
```bash
# 进入项目
cd /mnt/g/workspace/ra2

# 编辑配置
vim mods/ra2/rules/allied-infantry.yaml

# 版本控制
git status
git add .
git commit -m "描述"

# 编译(修改C#代码后)
make all
```

### Windows测试
```cmd
# 方式1: 快速测试(推荐)
quick-test-windows.cmd

# 方式2: 完整测试菜单
test-veterancy.cmd

# 方式3: 直接启动游戏
launch-game.cmd
```

---

## 🎮 实战示例 | Examples

### 示例1: 修改单位属性
```bash
# [WSL] 编辑文件
vim mods/ra2/rules/allied-infantry.yaml
# 修改HP, Speed, Damage等
:wq

# [Windows] 测试
quick-test-windows.cmd
```

### 示例2: 测试单位升级
```bash
# [WSL] 启用快速升级模式
vim mods/ra2/rules/allied-infantry.yaml
# 将 ^GainsExperience 改为 ^GainsExperienceFast
:wq

# [Windows] 游戏中测试(击杀1-2个单位即可升满级)

# [WSL] 测试完成后恢复
# 改回 ^GainsExperience
```

---

## 🔧 故障排除 | Troubleshooting

### 文件修改不生效?
```bash
# 确认在正确位置
pwd  # 应显示 /mnt/g/workspace/ra2

# 确认文件已保存
ls -l mods/ra2/rules/allied-infantry.yaml
```

### 脚本无法执行?
```bash
# 添加执行权限
chmod +x *.sh
```

### 游戏启动错误?
```bash
# 查看日志
tail -50 ~/.config/openra/Logs/ra2.log
```

---

## 💡 最佳实践 | Best Practices

### ✅ 推荐
- 在WSL中编辑所有文件
- 在Windows中测试游戏
- 使用Git进行版本控制
- 经常提交小的更改

### ❌ 避免
- 不要在两边同时编辑同一文件
- 不要手动复制文件(已自动共享)
- 不要在WSL文件系统(`~/`)中工作

---

## 📖 更多文档 | More Documentation

- `workflow.md` - 详细开发工作流
- `../testing/overview.md` - 测试指南
- `debug-panel.md` - 调试面板使用

---

**Happy Coding! 开发愉快!** 🎉
