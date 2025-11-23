# OpenRA RA2 游戏开发知识库

## 项目结构

### 核心目录
- `OpenRA.Mods.RA2/` - C# 自定义代码（Traits, Projectiles, Warheads等）
- `mods/ra2/rules/` - 单位、建筑、玩家规则定义
- `mods/ra2/weapons/` - 武器定义
- `mods/ra2/sequences/` - 动画序列配置
- `mods/ra2/audio/` - 音频配置
- `mods/ra2/maps/` - 地图文件

## OpenRA 核心概念

### Trait 系统
Traits 是 OpenRA 中的组件化系统，每个 Trait 提供一个特定功能。

**常用 Traits：**
```yaml
# 基础 Traits
Buildable:              # 可建造
  Queue: Infantry       # 建造队列
  BuildPaletteOrder: 10 # 建造栏位置

Valued:                 # 价值
  Cost: 200            # 建造成本

Health:                 # 生命值
  HP: 100

Mobile:                 # 移动
  Speed: 56            # 移动速度
  Locomotor: foot      # 移动方式

Armament:               # 武器装备
  Weapon: M60          # 武器引用
  LocalOffset: 0,0,0   # 武器位置

AutoTarget:             # 自动攻击
AttackFrontal:          # 攻击行为
RevealsShroud:          # 视野
  Range: 4c0

Guard:                  # 警戒能力
Selectable:             # 可选择
RenderSprites:          # 渲染精灵
WithInfantryBody:       # 步兵身体渲染
Voiced:                 # 语音
```

**高级 Traits：**
```yaml
# 条件系统
GrantConditionOnDeploy:
  DeployedCondition: deployed

WithDecoration@PIPS:
  RequiresCondition: ammo > 0

# 特殊能力
Chronoshiftable:        # 可被超时空传送
Disguise:               # 伪装
Cloak:                  # 隐形
Capturable:             # 可占领
ProductionAirdrop:      # 空降
SupportPowerChargeBar:  # 超武充能条
```

### 武器系统

**武器结构：**
```yaml
WeaponName:
  Inherits: ^Bullet            # 继承基础模板
  ReloadDelay: 50              # 装填延迟（帧）
  Range: 5c0                   # 射程（5格）
  Report: iggatta.aud          # 开火音效
  Projectile: Bullet           # 弹道类型
    Speed: 170                 # 弹速
    Image: 120MM               # 弹体图像
  Warhead@Damage: SpreadDamage # 弹头类型
    Damage: 40                 # 伤害值
    Versus:                    # 对各护甲伤害百分比
      None: 100                # 对无护甲 100%
      Flak: 100                # 对轻装甲 100%
      Plate: 100
      Light: 60                # 对轻型载具 60%
      Medium: 40
      Heavy: 25                # 对重装甲 25%
      Concrete: 40
    Spread: 128                # 溅射范围
```

**弹道类型：**
- `Bullet` - 直线子弹
- `Missile` - 导弹（可追踪）
- `GravityBomb` - 抛物线炮弹
- `InstantHit` - 即时命中（激光）
- `ElectricBolt` - 电弧（RA2特有）
- `RadBeam` - 辐射光束（RA2特有）

**弹头类型：**
- `SpreadDamage` - 溅射伤害
- `TargetDamage` - 单体伤害
- `FireCluster` - 燃烧集束
- `CreateTintedCells` - 创建辐射区域（RA2特有）

### 护甲系统

**护甲类型（Versus 系统）：**
```yaml
None:      # 无护甲（步兵）
Flak:      # 防空装甲
Plate:     # 板甲
Light:     # 轻型载具
Medium:    # 中型载具
Heavy:     # 重型坦克
Wood:      # 木质建筑
Steel:     # 钢制建筑
Concrete:  # 混凝土建筑
Drone:     # 无人机/机器人
Rocket:    # 火箭/导弹
```

### 单位类型层次

**继承模板：**
```yaml
^Infantry:          # 步兵基类
  Mobile:
    Locomotor: foot
  Selectable:
    Bounds: 12,17,0,-9

^Vehicle:           # 载具基类
  Mobile:
    Locomotor: wheeled

^Tank:              # 坦克基类
  Inherits: ^Vehicle
  Mobile:
    Locomotor: tracked

^Building:          # 建筑基类
  Building:
  RequiresPower:
```

## 数值参考

### 测量单位
- **距离：** 1c0 = 1格 = 1024世界单位
- **时间：** 1秒 = 25帧（游戏默认25FPS逻辑）
- **速度：** 56 = 普通步兵速度, 80+ = 快速单位

### 常见数值
```yaml
步兵移动速度:
  - 慢速: 35-45
  - 正常: 50-60
  - 快速: 70-85

载具移动速度:
  - 重型坦克: 40-50
  - 中型坦克: 55-70
  - 轻型载具: 80-100
  - 快速载具: 100+

建造时间:
  - 廉价步兵: 100-150帧
  - 普通步兵: 200-300帧
  - 精英步兵: 400-600帧
  - 轻型载具: 400-600帧
  - 坦克: 600-1000帧
  - 建筑: 1000-3000帧

生命值:
  - 弱小步兵: 50-75
  - 普通步兵: 100-125
  - 精英步兵: 125-200
  - 轻型载具: 150-250
  - 中型坦克: 300-500
  - 重型坦克: 600-900
  - 建筑: 600-2000
```

### DPS 计算公式
```
DPS = Damage / (ReloadDelay / 25)
实际DPS = DPS × Versus[TargetArmor] / 100

例如：
武器伤害: 40
装填: 50帧
对Heavy护甲: 25%
DPS = 40 / (50/25) = 20
实际DPS vs 重甲 = 20 × 0.25 = 5
```

### 成本效益比
```
Cost Efficiency = HP / Cost
DPS Efficiency = DPS / Cost

参考值:
- 步兵: 0.5-0.7
- 载具: 0.4-0.6
- 坦克: 0.3-0.5
```

## RA2 特有系统

### 心灵控制系统
```yaml
# 控制者
MindController:
  Capacity: 3                    # 最多控制3个单位
  ControllingCondition: active

# 可被控制者
MindControllable:
  Condition: mindcontrolled
  RevealControlerOnCondition: mindcontrolled
```

### 超时空系统
```yaml
Chronoshiftable:
  ChronoshiftSound: chrotnk1.aud
  ExplodeInstead: false          # 建筑会爆炸

ChronoResourceDelivery:          # 矿车传送
  Image: overlay
  Sequence: chronoshift
```

### 航母系统
```yaml
# 航母
CarrierParent:
  Actors: orca,orca,orca
  SpawnRearmDelay: 150

# 载机
CarrierChild:
  ParentActors: carrier
```

### 幻影坦克系统
```yaml
Mirage:
  DisguisedCondition: disguised
  RequiresCondition: !disguised
```

## 配置文件规范

### YAML 语法要点
```yaml
# 继承
UnitID:
  Inherits: ^BaseTemplate

# 条件引用
-RequiresCondition: deployed
RequiresCondition: !deployed    # 非

# 多个同类型Trait
Armament@PRIMARY:
  Weapon: MainGun
Armament@SECONDARY:
  Weapon: MG

# 列表
BuildablePrerequisites:
  - barracks
  - radar
```

### 命名约定
```yaml
单位ID:
  - 全大写
  - 简短（2-8字符）
  - 例: E1, HTNK, APOC

武器ID:
  - 描述性
  - 例: M60, 120mm, TeslaBolt

Trait实例名:
  - @后大写
  - 例: Armament@PRIMARY, Warhead@1Dam
```

## 常见开发任务模板

### 添加新步兵
```yaml
# mods/ra2/rules/allied-infantry.yaml
NEWUNIT:
  Inherits: ^Infantry
  Buildable:
    Queue: Infantry
    BuildPaletteOrder: 100
    Prerequisites: barracks
  Valued:
    Cost: 200
  Health:
    HP: 100
  Mobile:
    Speed: 56
  Armament:
    Weapon: M60
  AutoTarget:
  AttackFrontal:
  WithInfantryBody:
    DefaultAttackSequence: shoot
```

### 添加新武器
```yaml
# mods/ra2/weapons/bullets.yaml
NewWeapon:
  Inherits: ^Bullet
  ReloadDelay: 50
  Range: 5c0
  Report: gunfire.aud
  Projectile: Bullet
    Speed: 170
  Warhead@1Dam: SpreadDamage
    Damage: 30
    Versus:
      None: 100
      Flak: 100
      Light: 60
      Medium: 40
      Heavy: 25
```

### 添加新 Trait (C#)
```csharp
// OpenRA.Mods.RA2/Traits/CustomTrait.cs
using OpenRA.Traits;

namespace OpenRA.Mods.RA2.Traits
{
    [Desc("描述这个Trait的功能")]
    public class CustomTraitInfo : TraitInfo
    {
        [Desc("参数说明")]
        public readonly int Value = 100;

        public override object Create(ActorInitializer init)
        {
            return new CustomTrait(this);
        }
    }

    public class CustomTrait : ITick
    {
        readonly CustomTraitInfo info;

        public CustomTrait(CustomTraitInfo info)
        {
            this.info = info;
        }

        void ITick.Tick(Actor self)
        {
            // 每帧执行的逻辑
        }
    }
}
```

## 平衡性指导原则

### 单位定位
- **基础步兵（$100-200）**：弱，大量生产，炮灰
- **精英步兵（$600-1000）**：强力，但昂贵，易被碾压
- **轻型载具（$600-800）**：快速，侦查，骚扰
- **主战坦克（$900-1200）**：主力，平衡
- **重型单位（$1500-2000）**：强大但慢，后期

### 平衡检查点
1. **成本收益比**：HP/Cost 应在合理区间
2. **射程平衡**：贵的单位通常射程更远
3. **速度权衡**：强力单位通常更慢
4. **克制关系**：应有明确的克制链
5. **时间窗口**：建造时间应与价值匹配

### 阵营平衡
- **盟军**：科技、远程、精确
- **苏联**：力量、近战、群殴
- 确保两阵营在不同阶段都有竞争力

## 调试技巧

### 启用调试模式
```bash
./launch-game.sh Game.Mod=ra2 Debug.EnableDebugCommandsInReplays=true
```

### 常用调试命令
- F9 - 显示性能信息
- CTRL+SHIFT+F9 - 显示碰撞边界
- /lua - 打开Lua控制台

### 日志位置
```
~/.config/openra/Logs/
```

## 资源参考

### 已存在的单位（可供参考）
```yaml
步兵:
  - E1 (GI) - 基础步兵
  - ENGINEER - 工程师
  - SPY - 间谍
  - GHOST - 狙击手

载具:
  - MTNK - 灰熊坦克
  - HTNK - 犀牛坦克
  - APOC - 天启坦克
  - MGTK - 幻影坦克

航空:
  - ORCA - 虎鲸战机
  - SHAD - 夜鹰直升机
```

### 已存在的武器模板
```yaml
^Bullet - 子弹基类
^MG - 机枪基类
^Missile - 导弹基类
^Cannon - 火炮基类
^Flak - 防空炮基类
```

## 工作流程建议

### 新单位开发流程
1. **设计阶段**
   - 定义单位定位（角色、阵营）
   - 确定数值范围（成本、HP、DPS）
   - 设计特殊能力

2. **实现阶段**
   - 如需新 Trait，先实现 C# 代码
   - 编写 rules/*.yaml 配置
   - 编写武器配置（如需要）
   - 配置动画序列
   - 配置音效

3. **测试阶段**
   - 启动游戏验证
   - 检查平衡性
   - 对比参考单位

4. **验证阶段**
   - YAML 语法检查
   - 引用完整性验证
   - 文档更新

---

**最后更新：** 2025-01-22
**维护者：** Claude Game Dev Agents
