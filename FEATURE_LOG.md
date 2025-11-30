# OpenRA RA2 Mod - 功能迭代日志

**最后更新**: 2025-11-23
**版本**: Development Build

---

## 📋 目录

- [核心玩法系统](#核心玩法系统)
- [开发工具](#开发工具)
- [项目结构优化](#项目结构优化)
- [已知问题与计划](#已知问题与计划)

---

## 核心玩法系统

### 🎖️ 1. 10级经验系统（2025-11-23）

**状态**: ✅ 已完成

**功能描述**:
将原有的 2 级老兵系统（Veteran/Elite）扩展为 10 级渐进式经验系统，提供更细腻的成长曲线和视觉反馈。

**技术实现**:
- 文件位置: `mods/ra2/rules/defaults.yaml:173-505`
- 基础 Trait: `^GainsExperience`
- 快速测试模式: `^GainsExperienceFast`

**经验需求曲线**:
```yaml
Level 1:    500 XP   (+500)
Level 2:   1000 XP   (+500)
Level 3:   1800 XP   (+800)
Level 4:   2800 XP   (+1000)
Level 5:   4000 XP   (+1200)
Level 6:   5500 XP   (+1500)
Level 7:   7200 XP   (+1700)
Level 8:   9200 XP   (+2000)
Level 9:  11500 XP   (+2300)
Level 10: 14000 XP   (+2500)
```

**属性加成系统**:

| 等级 | 火力 | 受伤减免 | 速度 | 装填速度 | 精度 | 回血 |
|------|------|----------|------|----------|------|------|
| 1 | +10% | 8% | +10% | 8% | - | - |
| 2 | +20% | 15% | +20% | 15% | 20% | - |
| 3 | +30% | 22% | +30% | 22% | 30% | - |
| 4 | +40% | 28% | +40% | 28% | 40% | +1 HP/150t |
| 5 | +50% | 33% | +50% | 33% | 50% | +2 HP/125t |
| 6 | +60% | 38% | +60% | 38% | 55% | +2 HP/100t |
| 7 | +70% | 42% | +70% | 42% | 60% | +3 HP/90t |
| 8 | +80% | 45% | +80% | 45% | 65% | +3 HP/80t |
| 9 | +90% | 48% | +90% | 48% | 70% | +4 HP/70t |
| 10 | +100% | 50% | +100% | 50% | 75% | +5 HP/60t |

**视觉显示**:
- **Level 1**: ⭐ 银色星章 (veteran)
- **Level 2**: ⭐ 金色星章 (elite)
- **Level 3-10**: ⭐ + 数字徽章 (星章 + 对应等级数字)
  - 使用 `WithTextDecoration` trait
  - 字体: TinyBold
  - 位置: 单位右下角，数字在星章旁边
  - 类似编组数字显示风格

**快速测试模式**:
```yaml
^GainsExperienceFast:
  每 10 XP 升 1 级 (Level 1-10: 10, 20, 30...100 XP)
  用于快速验证等级系统和视觉效果
```

**游戏设计影响**:
- ✅ 更长的单位成长线，增强玩家养成感
- ✅ 渐进式加成避免突然的实力跃升
- ✅ 清晰的视觉区分，便于识别精英单位
- ✅ 高等级单位具备自我恢复能力
- ⚠️ 可能影响游戏后期平衡，需长期测试

**相关文件**:
- `mods/ra2/rules/defaults.yaml` - 等级系统配置
- `mods/ra2/sequences/misc.yaml` - 星章图标序列

---

## 开发工具

### 🛠️ 2. 开发调试面板（2025-11-23）

**状态**: ✅ 已完成

**功能描述**:
游戏内开发者调试工具，用于快速测试单位属性、等级系统和健康状态。

**技术实现**:
- C# Logic: `OpenRA.Mods.RA2/Widgets/Logic/DevDebugPanelLogic.cs`
- UI 定义: `mods/ra2/chrome/dev-debug-panel.yaml`
- 测试脚本: `OpenRA.Mods.RA2/test-debug-panel.sh`

**主要功能**:

1. **单位信息显示**
   - 单位名称
   - 当前等级 / 最大等级
   - 当前生命值 / 最大生命值
   - 实时更新选中单位信息

2. **等级控制** (Level 0-10)
   - 11 个等级按钮 (0-10)
   - 一键设置单位到指定等级
   - 当前等级高亮显示
   - 禁用无效操作

3. **健康控制**
   - **Heal Full**: 立即满血
   - **-50% HP**: 造成 50% 最大生命值伤害
   - **Kill Unit**: 立即杀死单位

4. **属性修改器**
   - **+Firepower**: 增加火力加成
   - **+Speed**: 增加移动速度加成
   - **Reset All**: 重置所有临时属性

**UI 布局**:
```
┌─────────────────────────────────┐
│  Developer Debug Panel      [X] │
├─────────────────────────────────┤
│ Selected Unit Info              │
│  Name: GI                       │
│  Level: 5/10                    │
│  Health: 80/100                 │
│                                 │
│ Veterancy Control (0-10)        │
│  [0][1][2][3][4][5]            │
│  [6][7][8][9][10]              │
│                                 │
│ Health Control                  │
│  [Heal Full][-50%HP][Kill]     │
│                                 │
│ Attribute Modifiers             │
│  [+Firepower][+Speed]          │
│  [Reset All Attributes]         │
│                                 │
│ Instructions:                   │
│  1. Select a unit               │
│  2. Use buttons to modify       │
│  3. Highlighted = current level │
└─────────────────────────────────┘
```

**使用场景**:
- ✅ 快速测试 10 级经验系统
- ✅ 验证等级图标显示
- ✅ 测试单位平衡性
- ✅ 调试属性加成效果
- ✅ 模拟战斗场景

**启动方式**:
```bash
# 方式 1: 测试脚本
./OpenRA.Mods.RA2/test-debug-panel.sh

# 方式 2: 手动修改 chrome.yaml 添加面板引用
```

**相关文件**:
- `OpenRA.Mods.RA2/Widgets/Logic/DevDebugPanelLogic.cs`
- `mods/ra2/chrome/dev-debug-panel.yaml`
- `OpenRA.Mods.RA2/test-debug-panel.sh`

---

### 🧪 3. 自动化测试框架（2025-11-22）

**状态**: ✅ 已完成

**功能描述**:
完整的跨平台自动化测试框架，支持 WSL、Linux、Windows 多环境测试。

**测试脚本分类**:

**快速测试** (`scripts/test/`):
- `quick-test-windows.cmd` - Windows 快速测试
- `test-fast.cmd` - 快速模式 (^GainsExperienceFast)
- `test-normal.cmd` - 正常模式测试
- `test-game-simple.cmd` - 简单游戏测试
- `test-veterancy.sh/cmd` - 等级系统专项测试

**开发工具** (`scripts/dev/`):
- `autotest.sh` - Linux 自动化测试
- `autotest-windows.cmd` - Windows 自动化测试
- `watch-and-test.sh` - 文件监控自动测试
- `analyze-test.sh` - 测试结果分析
- `sync-to-windows.sh` - WSL 到 Windows 同步
- `wsl-to-windows.sh` - 路径转换工具

**测试场景**:
- ✅ 单位升级测试（快速/正常模式）
- ✅ 视觉效果验证
- ✅ 性能测试
- ✅ 回归测试

**相关文件**:
- `scripts/test/*` - 测试脚本
- `scripts/dev/*` - 开发工具
- `scripts/README.md` - 使用文档

---

## 项目结构优化

### 📁 4. 项目重组与文档整合（2025-11-23）

**状态**: ✅ 已完成

**改进内容**:

**清理统计**:
- 删除重复文档: 13 个
- 整合新文档: 5 个
- 重组脚本: 13 个
- 根目录文件减少: 77%

**新目录结构**:
```
ra2/
├── docs/                    # 📚 文档中心
│   ├── README.md           # 文档索引
│   ├── MAINTENANCE.md      # 维护指南
│   ├── development/        # 开发文档
│   │   ├── quick-start.md
│   │   └── workflow.md
│   ├── testing/            # 测试文档
│   │   └── overview.md
│   └── game-design/        # 游戏设计文档
│
├── scripts/                 # 🔧 脚本工具
│   ├── README.md           # 脚本使用指南
│   ├── test/               # 测试脚本
│   │   ├── quick-test-windows.cmd
│   │   ├── test-veterancy.sh
│   │   └── ...
│   └── dev/                # 开发工具
│       ├── autotest.sh
│       ├── watch-and-test.sh
│       └── ...
│
├── [核心启动脚本 8 个]      # 保留在根目录
│   ├── launch-game.sh/cmd
│   ├── make.ps1/cmd
│   └── ...
│
├── mods/                    # 游戏模组
├── OpenRA.Mods.RA2/        # C# 代码
└── engine/                  # OpenRA 引擎
```

**文档体系**:
```
README.md (项目主页)
    ↓
    ├─→ docs/README.md (文档中心)
    │       ├─→ development/quick-start.md
    │       ├─→ development/workflow.md
    │       └─→ testing/overview.md
    │
    └─→ scripts/README.md (脚本指南)
            ├─→ scripts/test/
            └─→ scripts/dev/
```

**优化效果**:
- ✅ 根目录整洁
- ✅ 文档易于查找
- ✅ 脚本按功能分类
- ✅ 新手友好
- ✅ 易于维护

**相关文件**:
- `CLEANUP_SUMMARY.md` - 清理报告
- `docs/MAINTENANCE.md` - 维护指南
- `scripts/README.md` - 脚本索引

---

## 已知问题与计划

### ⚠️ 当前问题

1. **等级系统平衡**
   - Level 10 单位可能过强
   - 需要长期测试数据
   - 计划: 收集玩家反馈后调整

2. **数字标签位置**
   - 当前位置可能在某些单位上重叠
   - 计划: 添加可配置的偏移参数

3. **构建问题**
   - 偶现文件锁定导致编译失败
   - 临时方案: `pkill OpenRA` 后重新构建

### 🔮 未来计划

**短期 (1周内)**:
- [ ] 测试所有单位的等级显示效果
- [ ] 验证快速模式经验值曲线
- [ ] 补充游戏平衡测试数据

**中期 (1个月内)**:
- [ ] 实现等级系统音效反馈
- [ ] 添加等级提升特效
- [ ] 优化数字标签显示位置
- [ ] 完善调试面板功能

**长期**:
- [ ] 设计专属等级图标（替代数字）
- [ ] 实现等级特殊技能系统
- [ ] 添加等级成就系统
- [ ] 完整的游戏平衡迭代

---

## 📊 技术指标

### 代码变更统计

**最近提交**:
```
2f0a006 - 10级经验系统 (+329/-31 lines)
d13f78e - Game PM 文档增强
a10b869 - 多智能体开发架构
```

**文件修改统计**:
- 修改文件: 258 个
- 新增文件: 7 个
- 删除文件: 14 个
- 总计变更: +498/-329 lines

### 性能影响

- ✅ 等级计算: 无性能影响（基于条件触发）
- ✅ 视觉渲染: 轻微增加（数字文本渲染）
- ✅ 内存占用: 无明显变化

---

## 📝 变更日志格式

每次功能迭代请遵循以下格式更新本文档:

```markdown
### 🎮 [功能序号]. [功能名称]（YYYY-MM-DD）

**状态**: [✅ 已完成 / 🚧 进行中 / ⏸️ 暂停 / ❌ 已取消]

**功能描述**:
简要描述功能目的和玩法影响

**技术实现**:
- 关键文件位置
- 核心代码逻辑
- 技术架构说明

**游戏设计影响**:
对游戏平衡和玩家体验的影响分析

**相关文件**:
列出所有相关的文件路径
```

---

## 🔗 快速链接

- [项目主页](README.md)
- [开发文档](docs/development/quick-start.md)
- [测试指南](docs/testing/overview.md)
- [脚本工具](scripts/README.md)
- [维护指南](docs/MAINTENANCE.md)

---

**文档维护**: 每次重要功能迭代后更新
**最后审查**: 2025-11-23
**负责人**: Development Team
