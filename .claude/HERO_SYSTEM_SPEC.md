# 英雄系统实现规格书
## Warcraft 3 Style Hero System Specification

---

## 📋 系统概述

本文档定义了一个魔兽争霸3风格的英雄系统，适用于OpenRA RA2 Mod。

**核心设计目标：**
- 少量标志性英雄（每个阵营3-4个）
- 英雄死亡后可复活（有成本和时间）
- 每个玩家最多同时拥有2-3个英雄
- 英雄显著强于普通单位（3-5x）
- 通过科技/建筑解锁
- 为未来技能和装备系统预留接口

---

## 🏗️ 架构设计

### 组件关系图

```
Player
  ├── Hero Altar (建筑)
  │     ├── Production Queue (英雄招募)
  │     ├── Revival Queue (英雄复活)
  │     └── Hero Slot Manager (槽位管理)
  │
  ├── Hero Units (1-3个)
  │     ├── Hero Stats (强化属性)
  │     ├── Experience System (经验成长)
  │     ├── Death Handler (死亡处理)
  │     └── Future Extensions
  │           ├── Ability System (技能系统)
  │           └── Inventory System (装备系统)
  │
  └── Resources
        ├── Credits (招募/复活成本)
        └── Hero Slots (英雄槽位限制)
```

---

## 🏛️ 英雄祭坛 (Hero Altar)

### 基础配置

**盟军祭坛 (Allied Hero Monument):**
```yaml
GAHERO:  # Allied Hero Monument
  Inherits: ^Building
  Inherits@shape: ^2x2Shape
  Buildable:
    Queue: Defense
    BuildPaletteOrder: 100
    Prerequisites: ~gapile, ~gatech  # 需要兵营和科技中心
    Description: Recruits and revives heroes.\n  Maximum 3 heroes per player.\n  Strong vs Everything\n  Special: Can revive fallen heroes
  Valued:
    Cost: 1000
  Tooltip:
    Name: Hero Monument
  BuildingInfo:
    BuildDuration: 1500  # 60秒 (1500/25fps)
    Footprint: xx xx
    Dimensions: 2,2
  Health:
    HP: 1500
  Armor:
    Type: Concrete
  RevealsShroud:
    Range: 4c0
  Power:
    Amount: -50
  # 英雄生产队列
  Production:
    Produces: Hero.Allied
  ProductionQueue@Hero:
    Type: Hero.Allied
    DisplayOrder: 10
    Group: Hero
    LowPowerModifier: 300
    QueuedAudio: Training
    ReadyAudio: UnitReady
    BlockedAudio: NoDeploy
  # 复活机制 (自定义Trait，稍后实现)
  HeroRevivalManager:
    BaseRevivalCost: 500
    CostPerLevel: 100
    BaseRevivalTime: 1500  # 60秒
    RevivalSound: herorevive.aud
  # 限制英雄数量
  ProductionLimit@HeroSlots:
    Type: Hero.Allied
    Maximum: 3
  Exit@1:
    SpawnOffset: -256,256,0
  Exit@2:
    SpawnOffset: 256,256,0
```

**苏军祭坛 (Soviet Hero Barracks):**
```yaml
NAHERO:  # Soviet Hero Barracks
  Inherits: GAHERO
  Buildable:
    Prerequisites: ~nahand, ~natech
  Tooltip:
    Name: Hero Command Center
  Production:
    Produces: Hero.Soviet
  ProductionQueue@Hero:
    Type: Hero.Soviet
  ProductionLimit@HeroSlots:
    Type: Hero.Soviet
    Maximum: 3
```

---

## 🦸 英雄单位基类

### ^Hero 模板

所有英雄继承此基类，提供通用英雄特性：

```yaml
^Hero:
  Inherits: ^Infantry

  # 英雄标识
  Tooltip:
    GenericName: Hero

  # 无法被心灵控制
  -MindControllable:

  # 不可被碾压
  -Crushable:

  # 给予击杀者大量经验
  GivesExperience:
    Experience: 500
    PlayerExperienceMultiplier: 2

  # 英雄专属经验成长
  GainsExperience:
    Conditions:
      300: hero-level1
      800: hero-level2
      1500: hero-level3
      2400: hero-level4
      3500: hero-level5
      4800: hero-level6
      6300: hero-level7
      8000: hero-level8
      10000: hero-level9
      12500: hero-level10
    LevelUpImage: crate-effects
    LevelUpSequence: levelup
    LevelUpNotification: HeroLevelUp
    LevelUpSpeechNotification: HeroLevelUp

  # 英雄属性加成 (基于等级)
  # Level 1
  FirepowerMultiplier@HERO_L1:
    RequiresCondition: hero-level1 && !hero-level2
    Modifier: 115
  DamageMultiplier@HERO_L1:
    RequiresCondition: hero-level1 && !hero-level2
    Modifier: 90
  SpeedMultiplier@HERO_L1:
    RequiresCondition: hero-level1 && !hero-level2
    Modifier: 105

  # Level 2-10 (类似递进，每级增加5%攻击，减少2%受伤，增加2%速度)
  # ... (省略详细配置，实际需要完整实现)

  # 死亡处理 - 标记为可复活而非永久移除
  HeroDeathHandler:
    RevivalNotification: HeroFallen
    RevivalSpeech: HeroDown
    DeathSequence: die
    # 英雄"尸体"会返回祭坛复活队列

  # 未来技能系统接口
  HeroAbilityManager:
    MaxAbilitySlots: 4
    AbilityHotkeys: Q, W, E, R

  # 未来装备系统接口
  HeroInventory:
    MaxInventorySlots: 6
    InventoryHotkeys: 1, 2, 3, 4, 5, 6

  # 英雄光环效果（视觉）
  WithDecoration@HERO_AURA:
    Image: pips
    Sequence: hero-glow
    Position: Bottom
    Offset: 0,0,0

  # 等级显示
  WithLevelDecoration:
    Image: pips
    Sequences:
      hero-level1: level1
      hero-level2: level2
      hero-level3: level3
      hero-level4: level4
      hero-level5: level5
      hero-level6: level6
      hero-level7: level7
      hero-level8: level8
      hero-level9: level9
      hero-level10: level10
    Position: TopLeft

  # 英雄血条始终可见
  SelectionDecorations:
    VisualBounds: 32,32
  AlwaysVisible:
    ValidRelationships: Ally, Neutral, Enemy

  # 特殊选中音效
  Selectable:
    Priority: 100  # 优先选择英雄
    Voice: HeroSelect
```

---

## 👤 具体英雄实现

### 盟军英雄

#### 1. 谭雅 (Tanya) - 突袭专家

```yaml
HERO_TANYA:
  Inherits: ^Hero
  Buildable:
    Queue: Hero.Allied
    BuildPaletteOrder: 10
    Prerequisites: ~!gahero
    Description: Elite commando hero.\n  Strong vs Infantry, Buildings\n  Weak vs Vehicles\n  Special: C4 Demolition, Swimming
  Valued:
    Cost: 2000
  UpdatesPlayerStatistics:
    AddToArmyValue: true
  Tooltip:
    Name: Tanya
    GenericName: Hero
  Selectable:
    Bounds: 482, 1448, 0, -530
    Voice: HeroSelect
  Health:
    HP: 400  # 3x elite infantry
  Mobile:
    Speed: 85  # Very fast
    Locomotor: swimsuit
  RevealsShroud:
    Range: 7c0  # Increased vision
  Demolition:
    Voice: Attack
    DetonationDelay: 0
    Flashes: 3
  Armor:
    Type: Flak
  Passenger:
    CustomPipType: red

  # 武器 - 双枪
  Armament@PRIMARY:
    Weapon: HeroTanyaPistols
    RequiresCondition: !hero-level5
  Armament@ELITE:
    Weapon: HeroTanyaPistolsElite
    RequiresCondition: hero-level5

  AttackFrontal:
    Voice: Attack
    FacingTolerance: 0

  VoiceAnnouncement:
    Voice: Build
  Voiced:
    VoiceSet: TanyaHeroVoice

  QuantizeFacingsFromSequence:
    Sequence: stand
  WithInfantryBody:
    DefaultAttackSequence: shoot

  -TakeCover:

  # 英雄特性
  HeroInfo:
    ShortName: Tanya
    Title: Elite Commando
    Description: Legendary Allied special forces operative. Expert in infiltration and demolition.
    Role: Assassin
    Difficulty: Medium

  # 技能槽位（未来实现）
  HeroAbilitySlot@Q:
    Ability: TimeBomb
    Hotkey: Q
    Placeholder: true
  HeroAbilitySlot@W:
    Ability: Evasion
    Hotkey: W
    Placeholder: true
  HeroAbilitySlot@E:
    Ability: CriticalStrike
    Hotkey: E
    Placeholder: true
```

**Tanya专属武器：**
```yaml
# weapons/hero-weapons.yaml
HeroTanyaPistols:
  Inherits: ^Bullet
  ReloadDelay: 30  # Fast fire rate
  Range: 5c512
  Report: tanyaatk1.aud
  ValidTargets: Ground, Infantry
  Projectile: Bullet
    Speed: 1c682
    Image: 120MM
    ContrailLength: 5
    ContrailColor: FFFF0040
  Warhead@1Dam: SpreadDamage
    Damage: 120  # 3x normal pistol
    Versus:
      None: 100
      Flak: 90
      Plate: 90
      Light: 25
      Medium: 15
      Heavy: 10
      Concrete: 25
    Spread: 128
  Warhead@2Smu: LeaveSmudge
    SmudgeType: Crater
    InvalidTargets: Vehicle, Structure, Wall, Husk, Trees

HeroTanyaPistolsElite:
  Inherits: HeroTanyaPistols
  ReloadDelay: 20  # Even faster
  Warhead@1Dam: SpreadDamage
    Damage: 180  # 5x normal
```

---

#### 2. 光棱指挥官 (Prism Commander)

```yaml
HERO_PRISM:
  Inherits: ^Hero
  Buildable:
    Queue: Hero.Allied
    BuildPaletteOrder: 20
    Prerequisites: ~!gahero, ~gaweap  # 需要战车工厂
    Description: Prism technology expert hero.\n  Strong vs Everything\n  Weak vs Fast Units\n  Special: Beam refraction, Long range
  Valued:
    Cost: 2500
  Tooltip:
    Name: Prism Commander
    GenericName: Hero
  Health:
    HP: 600
  Mobile:
    Speed: 60  # Slower
  RevealsShroud:
    Range: 9c0  # Excellent vision
  Armor:
    Type: Plate

  # 光棱武器 - 可折射
  Armament:
    Weapon: HeroPrismBeam
    LocalOffset: 0,0,256
    PauseOnCondition: empdisable || chronodisable

  AttackFrontal:
    Voice: Attack

  Voiced:
    VoiceSet: PrismHeroVoice

  WithInfantryBody:
    DefaultAttackSequence: shoot

  HeroInfo:
    ShortName: Prism
    Title: Light Architect
    Description: Master of prism technology and advanced optics.
    Role: Artillery
    Difficulty: Hard
```

**光棱武器（可折射）：**
```yaml
HeroPrismBeam:
  Inherits: ^Cannon
  ReloadDelay: 80
  Range: 10c0  # Long range
  Report: prismtwr.aud
  MinRange: 2c0
  Projectile: LaserZap
    Width: 128
    Duration: 15
    Color: FF0000FF
    SecondaryColor: 800080FF
    HitAnim: pulsefx1
    HitAnimSequence: hit
  Warhead@1Dam: SpreadDamage
    Damage: 200  # High damage
    Versus:
      None: 100
      Flak: 100
      Plate: 100
      Light: 100
      Medium: 100
      Heavy: 100
      Concrete: 80
    Spread: 256
  # 折射机制 (需要自定义Trait)
  Warhead@2Refract: PrismRefraction
    MaxTargets: 3
    Range: 5c0
    DamageReduction: 25  # 每次折射减少25%伤害
```

---

#### 3. 超时空指挥官 (Chrono Commander)

```yaml
HERO_CHRONO:
  Inherits: ^Hero
  Buildable:
    Queue: Hero.Allied
    BuildPaletteOrder: 30
    Prerequisites: ~!gahero, ~gatech, ~gaorep  # 需要科技中心和矿厂
    Description: Temporal manipulation hero.\n  Strong vs All\n  Weak vs Massed Units\n  Special: Teleportation, Time freeze
  Valued:
    Cost: 2200
  Tooltip:
    Name: Chrono Commander
    GenericName: Hero
  Health:
    HP: 500
  Mobile:
    Speed: 70
  RevealsShroud:
    Range: 6c0
  Armor:
    Type: Light

  # 时空武器
  Armament:
    Weapon: HeroChronoRifle

  AttackFrontal:
    Voice: Attack

  Voiced:
    VoiceSet: ChronoHeroVoice

  WithInfantryBody:
    DefaultAttackSequence: shoot

  # 自身可传送（未来技能实现）
  PortableChrono:
    ChargeDelay: 750  # 30秒冷却
    MaxDistance: 12c0
    Hotkey: D  # 临时占用D键

  HeroInfo:
    ShortName: Chrono
    Title: Time Weaver
    Description: Master of temporal mechanics and spacetime manipulation.
    Role: Mobility
    Difficulty: Very Hard
```

---

### 苏军英雄

#### 1. 尤里 (Yuri) - 心灵主宰

```yaml
HERO_YURI:
  Inherits: ^Hero
  Buildable:
    Queue: Hero.Soviet
    BuildPaletteOrder: 10
    Prerequisites: ~!nahero
    Description: Psychic commander hero.\n  Strong vs Infantry, Light Vehicles\n  Weak vs Snipers, Buildings\n  Special: Mind control, Psychic detection
  Valued:
    Cost: 2200
  Tooltip:
    Name: Yuri
    GenericName: Hero
  Health:
    HP: 450
  Mobile:
    Speed: 55  # Slow
  RevealsShroud:
    Range: 6c0
  Armor:
    Type: None

  # 心灵控制
  Armament:
    Weapon: HeroMindControl

  AttackFrontal:
    Voice: Attack

  # 心灵控制容量
  MindController:
    Capacity: 5  # Can control 5 units
    ControllingCondition: mindcontrolling
    RevealControllerOnCondition: mindcontrolling

  # 心灵探测（反隐）
  DetectCloaked:
    Range: 8c0
    DetectionTypes: Cloak

  Voiced:
    VoiceSet: YuriHeroVoice

  WithInfantryBody:
    DefaultAttackSequence: shoot

  HeroInfo:
    ShortName: Yuri
    Title: Psychic Master
    Description: Supreme psychic commander with unmatched mental powers.
    Role: Controller
    Difficulty: Hard
```

---

#### 2. 鲍里斯 (Boris) - 空袭专家

```yaml
HERO_BORIS:
  Inherits: ^Hero
  Buildable:
    Queue: Hero.Soviet
    BuildPaletteOrder: 20
    Prerequisites: ~!nahero, ~naweap  # 需要战车工厂
    Description: Airstrike coordinator hero.\n  Strong vs Vehicles, Buildings\n  Weak vs AA, Fast Units\n  Special: Laser designator, MiG support
  Valued:
    Cost: 2300
  Tooltip:
    Name: Boris
    GenericName: Hero
  Health:
    HP: 550
  Mobile:
    Speed: 65
  RevealsShroud:
    Range: 8c0  # Great vision for designation
  Armor:
    Type: None

  # 突击步枪
  Armament@PRIMARY:
    Weapon: HeroBorisRifle
    RequiresCondition: !designating

  # 激光指引（主动技能）
  Armament@DESIGNATOR:
    Weapon: HeroLaserDesignator
    RequiresCondition: designating
    Cursor: ability

  AttackFrontal:
    Voice: Attack

  # 激光指引机制（未来技能实现）
  GrantConditionOnDeploy:
    DeployedCondition: designating
    Facing: 0
    AllowedTerrainTypes: Clear, Rough, Road, Ore, Gems
    Voice: Deploy

  Voiced:
    VoiceSet: BorisHeroVoice

  WithInfantryBody:
    DefaultAttackSequence: shoot
    DeployingSequence: deploy

  HeroInfo:
    ShortName: Boris
    Title: Airstrike Commander
    Description: Elite Soviet officer with direct air support access.
    Role: Artillery
    Difficulty: Medium
```

---

#### 3. 磁暴督军 (Tesla Commander)

```yaml
HERO_TESLA:
  Inherits: ^Hero
  Buildable:
    Queue: Hero.Soviet
    BuildPaletteOrder: 30
    Prerequisites: ~!nahero, ~natech
    Description: Tesla technology hero.\n  Strong vs Infantry, Vehicles\n  Weak vs Long Range\n  Special: AOE damage, High durability
  Valued:
    Cost: 2400
  Tooltip:
    Name: Tesla Commander
    GenericName: Hero
  Health:
    HP: 800  # Tankiest hero
  Mobile:
    Speed: 50  # Slowest hero
  RevealsShroud:
    Range: 5c0
  Armor:
    Type: Heavy  # Unique for infantry

  # 磁暴攻击
  Armament:
    Weapon: HeroTeslaWeapon

  AttackFrontal:
    Voice: Attack

  Voiced:
    VoiceSet: TeslaHeroVoice

  WithInfantryBody:
    DefaultAttackSequence: shoot

  # 磁暴护甲（被动）- 反伤
  DamageReflection:
    Percentage: 15  # 反弹15%伤害
    ValidRelationships: Enemy

  HeroInfo:
    ShortName: Tesla
    Title: Iron Warlord
    Description: Heavily armored commander wielding devastating Tesla technology.
    Role: Tank
    Difficulty: Easy
```

---

## 🔄 复活系统

### 死亡处理流程

```
英雄死亡 → 播放死亡动画 → 标记为"fallen" →
不从游戏移除 → 添加到祭坛复活队列 →
等待复活 → 支付复活成本 → 等待复活时间 →
英雄在祭坛重生
```

### 自定义Trait实现需求

需要实现以下新Traits（C#）：

#### 1. HeroDeathHandler
```csharp
// 处理英雄死亡，不移除单位，而是标记为fallen
[Desc("Handles hero death, marking hero as fallen instead of killed.")]
public class HeroDeathHandlerInfo : TraitInfo
{
    [Desc("Notification to play when hero falls.")]
    public readonly string RevivalNotification = null;

    [Desc("Speech notification for hero down.")]
    public readonly string RevivalSpeech = null;

    public override object Create(ActorInitializer init)
    {
        return new HeroDeathHandler(init.Self, this);
    }
}
```

#### 2. HeroRevivalManager
```csharp
// 管理英雄复活队列，附加在祭坛建筑上
[Desc("Manages hero revival queue for hero altar.")]
public class HeroRevivalManagerInfo : TraitInfo
{
    [Desc("Base cost to revive a hero.")]
    public readonly int BaseRevivalCost = 500;

    [Desc("Additional cost per hero level.")]
    public readonly int CostPerLevel = 100;

    [Desc("Base time (ticks) to revive a hero.")]
    public readonly int BaseRevivalTime = 1500;

    [Desc("Sound to play when revival starts.")]
    public readonly string RevivalSound = null;

    public override object Create(ActorInitializer init)
    {
        return new HeroRevivalManager(init.Self, this);
    }
}
```

#### 3. ProductionLimit
```csharp
// 限制特定类型单位的生产数量（英雄槽位）
[Desc("Limits the number of units of a specific type that can be produced.")]
public class ProductionLimitInfo : TraitInfo
{
    [Desc("Production type to limit.")]
    public readonly string Type = null;

    [Desc("Maximum number of units allowed.")]
    public readonly int Maximum = 3;

    public override object Create(ActorInitializer init)
    {
        return new ProductionLimit(init.Self, this);
    }
}
```

#### 4. HeroInfo
```csharp
// 存储英雄的元数据信息
[Desc("Provides hero metadata and information.")]
public class HeroInfoInfo : TraitInfo
{
    [Desc("Short hero name.")]
    public readonly string ShortName = "Hero";

    [Desc("Hero title.")]
    public readonly string Title = "Hero";

    [Desc("Hero description.")]
    public readonly string Description = "";

    [Desc("Hero role (Assassin, Tank, Support, etc).")]
    public readonly string Role = "DPS";

    [Desc("Difficulty rating.")]
    public readonly string Difficulty = "Medium";

    public override object Create(ActorInitializer init)
    {
        return new HeroInfo(this);
    }
}
```

---

## 🎯 未来扩展接口

### 技能系统预留接口

```yaml
# 每个英雄有4个技能槽位 (Q, W, E, R)
HeroAbilitySlot@Q:
  Ability: AbilityName  # 技能ID
  Hotkey: Q
  Cooldown: 10000  # 10秒冷却（以ticks为单位）
  ManaCost: 50  # 预留能量系统
  Range: 5c0
  Description: "Ability description"

# 技能类型示例
Abilities:
  - Active: 主动释放技能
  - Passive: 被动效果
  - Toggle: 切换状态
  - Aura: 光环效果
  - Channel: 引导技能
```

### 装备系统预留接口

```yaml
# 每个英雄有6个物品槽 (1-6)
HeroInventory:
  MaxInventorySlots: 6
  InventoryHotkeys: 1, 2, 3, 4, 5, 6

# 物品类型示例
Items:
  - Consumable: 一次性使用（治疗药水）
  - Equipment: 装备提升属性（护甲、武器）
  - Artifact: 特殊效果（隐身斗篷）
  - Quest: 任务物品
```

---

## 📊 数值平衡参考

### 英雄成本与数值对比

| 英雄 | 成本 | HP | 速度 | 攻击 | 射程 | 难度 |
|------|------|-----|------|------|------|------|
| Tanya | 2000 | 400 | 85 | 120 | 5c0 | Medium |
| Prism | 2500 | 600 | 60 | 200 | 10c0 | Hard |
| Chrono | 2200 | 500 | 70 | 80 | 6c0 | V.Hard |
| Yuri | 2200 | 450 | 55 | MC | 5c0 | Hard |
| Boris | 2300 | 550 | 65 | 100 | 7c0 | Medium |
| Tesla | 2400 | 800 | 50 | 150 | 4c0 | Easy |

**普通精英单位参考：**
- 精英步兵：$600-1000, 125-200 HP
- 主战坦克：$900-1200, 400-600 HP

**英雄倍率：**
- 成本：2-3x 精英单位
- 生命：2-4x 精英单位
- 伤害：3-5x 精英单位

### 复活成本计算

```
复活成本 = BaseRevivalCost + (HeroLevel × CostPerLevel)

例如：
- Level 1 英雄: 500 + (1 × 100) = 600
- Level 5 英雄: 500 + (5 × 100) = 1000
- Level 10 英雄: 500 + (10 × 100) = 1500
```

### 复活时间

```
基础复活时间: 60秒
可通过研究/升级缩短至40秒
```

---

## 🚀 实现路线图

### Phase 1: 核心系统 (2-3天)
- [ ] 创建英雄祭坛建筑配置
- [ ] 实现 ^Hero 基类模板
- [ ] 配置6个初始英雄
- [ ] 实现基础武器配置

### Phase 2: 复活机制 (3-4天)
- [ ] 实现 HeroDeathHandler trait
- [ ] 实现 HeroRevivalManager trait
- [ ] 实现 ProductionLimit trait
- [ ] 测试死亡和复活流程

### Phase 3: 视觉和音效 (2-3天)
- [ ] 添加英雄光环特效
- [ ] 添加等级显示装饰
- [ ] 配置英雄语音
- [ ] 添加复活特效

### Phase 4: 平衡测试 (持续)
- [ ] 单位对抗测试
- [ ] 成本效益分析
- [ ] 调整数值
- [ ] 多人游戏测试

### Future Phases:
- Phase 5: 技能系统 (4-6周)
- Phase 6: 装备系统 (4-6周)
- Phase 7: 英雄任务和剧情 (可选)

---

## 📝 注意事项

### 技术限制
1. OpenRA不原生支持英雄复活机制，需要自定义Trait
2. 物品/装备系统需要大量自定义代码
3. 技能系统需要UI扩展

### 平衡考虑
1. 英雄不应过强导致普通单位无价值
2. 英雄应有明确的克制关系
3. 复活成本应足够高以避免无脑送死
4. 英雄上限（3个）防止英雄海

### 用户体验
1. 英雄死亡应有明显提示
2. 复活进度应清晰可见
3. 英雄等级应易于识别
4. 快捷键应符合直觉

---

## 📚 参考资料

- OpenRA Trait系统文档
- 魔兽争霸3英雄平衡数据
- Red Alert 2原版commando单位
- 现有veterancy系统实现

---

**文档版本:** 1.0
**创建日期:** 2025-01-24
**作者:** Claude Game Dev System
**状态:** 设计完成，待实现
