# 英雄系统实现状态报告
## Hero System Implementation Status

**日期:** 2025-01-24
**版本:** 阶段1完成
**状态:** ✅ 基础YAML配置完成，待C#自定义Traits实现

---

## ✅ 已完成 (Phase 1: YAML Configuration)

### 1. 英雄祭坛建筑 (Hero Altars)
**文件:** `mods/ra2/rules/allied-structures.yaml`, `mods/ra2/rules/soviet-structures.yaml`

- ✅ **盟军英雄祭坛** (`gahero`) - Hero Monument
  - 成本: 1000
  - 前置: 兵营 + 科技中心
  - 生产类型: `Hero.Allied`

- ✅ **苏军英雄祭坛** (`nahero`) - Hero Command Center
  - 成本: 1000
  - 前置: 兵营 + 科技中心
  - 生产类型: `Hero.Soviet`

### 2. 英雄基类模板
**文件:** `mods/ra2/rules/defaults.yaml`

- ✅ **^Hero 基类** (Line 507-525)
  - 继承自 `^Infantry`
  - 无法被心灵控制
  - 无法被碾压
  - 高优先级选择
  - 被击杀时给予500经验值
  - 预留技能和装备系统接口

### 3. 6个初始英雄单位
**文件:** `mods/ra2/rules/hero-units.yaml`

#### 盟军英雄 (Allied Heroes)
1. ✅ **HERO_TANYA** - 谭雅 (突袭专家)
   - HP: 400 | 速度: 85 | 成本: $2000
   - 武器: 双枪 (高伤害步兵)
   - 特性: 可游泳, C4爆破, 高机动

2. ✅ **HERO_PRISM** - 光棱指挥官 (远程炮台)
   - HP: 600 | 速度: 60 | 成本: $2500
   - 武器: 光棱射线 (超长射程)
   - 特性: 10格射程, 高伤害

3. ✅ **HERO_CHRONO** - 超时空指挥官 (机动控制)
   - HP: 500 | 速度: 70 | 成本: $2200
   - 武器: 时空步枪
   - 特性: 高机动性

#### 苏军英雄 (Soviet Heroes)
4. ✅ **HERO_YURI** - 尤里 (心灵主宰)
   - HP: 450 | 速度: 55 | 成本: $2200
   - 武器: 心灵控制
   - 特性: 控制5单位, 反隐侦测

5. ✅ **HERO_BORIS** - 鲍里斯 (空袭专家)
   - HP: 550 | 速度: 65 | 成本: $2300
   - 武器: 突击步枪
   - 特性: 优秀视野 (8格)

6. ✅ **HERO_TESLA** - 磁暴督军 (前排坦克)
   - HP: 800 | 速度: 50 | 成本: $2400
   - 武器: 磁暴攻击
   - 特性: 最高生命值, AOE伤害, 重装甲

### 4. 英雄武器配置
**文件:** `mods/ra2/weapons/hero-weapons.yaml`

✅ **已实现武器:**
- `HeroTanyaPistols` / `HeroTanyaPistolsElite` - 谭雅双枪 (120/180伤害)
- `HeroPrismBeam` - 光棱射线 (200伤害, 10格射程)
- `HeroChronoRifle` - 时空步枪 (80伤害)
- `HeroMindControl` - 增强心灵控制
- `HeroBorisRifle` - 鲍里斯突击步枪 (100伤害)
- `HeroTeslaWeapon` - 磁暴武器 (150伤害, AOE)

### 5. 模组配置更新
**文件:** `mods/ra2/mod.yaml`

✅ 已添加到Rules列表:
- Line 77: `ra2|rules/hero-units.yaml`

✅ 已添加到Weapons列表:
- Line 178: `ra2|weapons/hero-weapons.yaml`

---

## 🔨 构建状态

```bash
✅ Build Status: SUCCESS
   - 0 Warnings
   - 0 Errors
   - Build time: ~13 seconds
```

---

## ⚠️ 当前限制

由于缺少自定义C# Traits，以下功能**暂时不可用**：

1. **❌ 英雄复活系统** - 英雄死亡后无法复活
   - 需要实现: `HeroDeathHandler` trait
   - 需要实现: `HeroRevivalManager` trait

2. **❌ 英雄数量限制** - 可以无限制招募英雄
   - 需要实现: `ProductionLimit` trait
   - 目标: 每个玩家最多3个英雄

3. **❌ 技能系统** - 预留接口但未实现
   - 需要实现: `HeroAbilityManager` trait
   - 需要实现: 4个技能槽位 (Q, W, E, R)

4. **❌ 装备系统** - 预留接口但未实现
   - 需要实现: `HeroInventory` trait
   - 需要实现: 6个物品槽位 (1-6)

**当前英雄行为:**
- ✅ 可以从英雄祭坛招募
- ✅ 拥有强化属性 (3-5x普通单位)
- ✅ 可以升级和获得经验
- ⚠️ 死亡后**永久失去** (无法复活)
- ⚠️ 可以**无限招募** (没有数量限制)

---

## 🎮 测试指南

### 如何测试当前实现:

1. **启动游戏:**
   ```bash
   ./launch-game.sh
   ```

2. **进入地图编辑器或游戏:**
   - 选择盟军或苏军阵营
   - 建造英雄祭坛:
     - 盟军: "Hero Monument" (需要兵营+科技中心)
     - 苏军: "Hero Command Center" (需要兵营+科技中心)

3. **招募英雄:**
   - 从英雄祭坛生产队列中选择英雄
   - 等待建造完成 (约30-40秒)
   - 英雄将从祭坛出来

4. **测试英雄:**
   - 测试战斗能力 (应该明显强于普通单位)
   - 测试升级系统 (击杀敌人获得经验)
   - 测试特殊能力 (如Tanya的游泳, Yuri的心灵控制)

5. **验证问题:**
   - [ ] 英雄是否可以正常建造？
   - [ ] 英雄属性是否正确？
   - [ ] 武器伤害是否符合预期？
   - [ ] 是否无法被心灵控制？
   - [ ] 是否无法被碾压？

---

## 📋 下一步: 阶段2 - C# Traits实现

### 需要实现的C# Traits

#### 1. ProductionLimit Trait (英雄数量限制)
**优先级:** 🔴 HIGH - 防止英雄海

**功能:** 限制特定生产类型的单位数量

**文件位置:** `OpenRA.Mods.RA2/Traits/ProductionLimit.cs`

**实现要点:**
```csharp
[Desc("Limits the number of units that can be produced of a specific type.")]
public class ProductionLimitInfo : TraitInfo
{
    [Desc("Production type to limit.")]
    public readonly string Type = null;

    [Desc("Maximum number of units allowed.")]
    public readonly int Maximum = 3;

    [Desc("Error message when limit reached.")]
    public readonly string LimitReachedMessage = "Hero limit reached (Maximum: {0})";
}

public class ProductionLimit : INotifyProduction, INotifyOwnerChanged, INotifyKilled
{
    // Track active heroes per player
    Dictionary<string, int> activeUnits = new Dictionary<string, int>();

    // Block production if limit reached
    bool INotifyProduction.CanStartProduction(ActorInfo ai)
    {
        if (activeUnits[Type] >= info.Maximum)
        {
            // Display error message to player
            return false;
        }
        return true;
    }

    // Increment counter when hero produced
    void INotifyProduction.UnitProduced(Actor self, Actor other, CPos exit)

    // Decrement counter when hero dies
    void INotifyKilled.Killed(Actor self, AttackInfo e)
}
```

**测试方法:**
- 尝试建造第4个英雄，应该被阻止
- 英雄死亡后，应该可以再次招募

---

#### 2. HeroDeathHandler Trait (英雄死亡处理)
**优先级:** 🔴 HIGH - 核心复活机制

**功能:** 拦截英雄死亡事件，不移除单位，而是标记为"fallen"状态

**文件位置:** `OpenRA.Mods.RA2/Traits/HeroDeathHandler.cs`

**实现要点:**
```csharp
[Desc("Handles hero death, preventing removal and enabling revival.")]
public class HeroDeathHandlerInfo : ConditionalTraitInfo
{
    [Desc("Condition to grant when hero falls.")]
    public readonly string FallenCondition = "hero-fallen";

    [Desc("Notification to play when hero falls.")]
    public readonly string FallenNotification = "HeroFallen";

    [Desc("Speech to play when hero falls.")]
    public readonly string FallenSpeech = null;

    [Desc("Hero becomes invisible when fallen.")]
    public readonly bool HideWhenFallen = true;
}

public class HeroDeathHandler : ConditionalTrait<HeroDeathHandlerInfo>,
    INotifyKilled, INotifyDamage
{
    ConditionManager conditionManager;
    int fallenToken = ConditionManager.InvalidConditionToken;

    // Intercept death
    void INotifyKilled.Killed(Actor self, AttackInfo e)
    {
        if (IsTraitDisabled)
            return;

        // Don't actually kill the hero
        e.Damage.Value = 0;

        // Grant fallen condition
        if (fallenToken == ConditionManager.InvalidConditionToken)
            fallenToken = conditionManager.GrantCondition(self, info.FallenCondition);

        // Play notification
        Game.Sound.PlayNotification(..., info.FallenNotification);

        // Notify revival manager at altar
        NotifyRevivalManager(self);

        // Hide hero
        if (info.HideWhenFallen)
            self.World.AddFrameEndTask(w => self.SetVisibility(false));
    }

    private void NotifyRevivalManager(Actor hero)
    {
        // Find player's hero altar
        var altar = self.Owner.World.ActorsHavingTrait<HeroRevivalManager>()
            .FirstOrDefault(a => a.Owner == self.Owner);

        if (altar != null)
        {
            var revival = altar.Trait<HeroRevivalManager>();
            revival.AddToRevivalQueue(hero);
        }
    }
}
```

---

#### 3. HeroRevivalManager Trait (复活管理器)
**优先级:** 🔴 HIGH - 配合HeroDeathHandler

**功能:** 管理英雄复活队列，在祭坛复活英雄

**文件位置:** `OpenRA.Mods.RA2/Traits/HeroRevivalManager.cs`

**实现要点:**
```csharp
[Desc("Manages hero revival queue at hero altar.")]
public class HeroRevivalManagerInfo : TraitInfo
{
    [Desc("Base cost to revive a hero.")]
    public readonly int BaseRevivalCost = 500;

    [Desc("Additional cost per hero level.")]
    public readonly int CostPerLevel = 100;

    [Desc("Base time (ticks) to revive a hero.")]
    public readonly int BaseRevivalTime = 1500;  // 60 seconds

    [Desc("Time reduction per level (ticks).")]
    public readonly int TimeReductionPerLevel = 50;

    [Desc("Sound to play when revival starts.")]
    public readonly string RevivalSound = null;

    [Desc("Notification when hero revives.")]
    public readonly string RevivedNotification = "HeroRevived";
}

public class HeroRevivalManager : INotifyProduction, ITick, INotifyOwnerChanged
{
    Queue<Actor> revivalQueue = new Queue<Actor>();
    Actor currentRevival = null;
    int revivalTicks = 0;
    int revivalCost = 0;

    public void AddToRevivalQueue(Actor hero)
    {
        revivalQueue.Enqueue(hero);
    }

    void ITick.Tick(Actor self)
    {
        // Process revival queue
        if (currentRevival == null && revivalQueue.Count > 0)
        {
            StartRevival(revivalQueue.Dequeue());
        }

        if (currentRevival != null)
        {
            revivalTicks--;

            if (revivalTicks <= 0)
            {
                CompleteRevival(self);
            }
        }
    }

    private void StartRevival(Actor hero)
    {
        currentRevival = hero;

        // Calculate cost based on level
        int heroLevel = GetHeroLevel(hero);
        revivalCost = info.BaseRevivalCost + (heroLevel * info.CostPerLevel);

        // Calculate time
        revivalTicks = info.BaseRevivalTime - (heroLevel * info.TimeReductionPerLevel);

        // Deduct cost from player
        var playerResources = self.Owner.PlayerActor.Trait<PlayerResources>();
        if (playerResources.Cash < revivalCost)
        {
            // Can't afford revival, put back in queue
            revivalQueue.Enqueue(hero);
            currentRevival = null;
            return;
        }

        playerResources.TakeCash(revivalCost, true);

        // Show revival progress bar
        // TODO: Implement visual indicator
    }

    private void CompleteRevival(Actor altar)
    {
        // Respawn hero at altar
        var exit = altar.TraitOrDefault<Exit>();
        if (exit != null)
        {
            // Restore hero
            currentRevival.SetVisibility(true);
            currentRevival.Health.HP = currentRevival.Health.MaxHP;

            // Remove fallen condition
            var deathHandler = currentRevival.TraitOrDefault<HeroDeathHandler>();
            if (deathHandler != null)
                deathHandler.RemoveFallenCondition();

            // Move to exit
            exit.SendOut(altar, currentRevival);

            // Play notification
            Game.Sound.PlayNotification(..., info.RevivedNotification);
        }

        currentRevival = null;
    }

    private int GetHeroLevel(Actor hero)
    {
        var experience = hero.TraitOrDefault<GainsExperience>();
        return experience != null ? experience.Level : 1;
    }
}
```

---

#### 4. HeroInfo Trait (英雄元数据)
**优先级:** 🟡 MEDIUM - 提供英雄信息

**功能:** 存储英雄的元数据（名称、角色、描述等）

**文件位置:** `OpenRA.Mods.RA2/Traits/HeroInfo.cs`

**实现要点:**
```csharp
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

public class HeroInfo
{
    readonly HeroInfoInfo info;

    public HeroInfo(HeroInfoInfo info)
    {
        this.info = info;
    }

    public string GetShortName() => info.ShortName;
    public string GetTitle() => info.Title;
    public string GetDescription() => info.Description;
    public string GetRole() => info.Role;
    public string GetDifficulty() => info.Difficulty;
}
```

---

### 实现顺序建议

1. **第一步:** `ProductionLimit` (最简单，防止英雄海)
2. **第二步:** `HeroInfo` (简单，提供元数据)
3. **第三步:** `HeroDeathHandler` + `HeroRevivalManager` (复杂，核心系统)

---

## 🎯 阶段3预览: 未来扩展

### 技能系统 (Ability System)
- 4个技能槽位 (Q, W, E, R)
- 主动/被动/光环/引导技能类型
- 冷却管理
- 能量系统（可选）

### 装备系统 (Inventory System)
- 6个物品槽位 (1-6)
- 可拾取物品
- 装备提升属性
- 合成系统（可选）

### 视觉效果
- 英雄光环特效
- 等级显示徽章
- 技能施放动画
- 复活特效

---

## 📞 需要帮助？

如果在实现过程中遇到问题：

1. **参考规格书:** `.claude/HERO_SYSTEM_SPEC.md`
2. **参考知识库:** `.claude/knowledge/openra-game-dev.md`
3. **使用游戏代理:** `/game:pm`, `/game:config`, `/game:trait`

---

**最后更新:** 2025-01-24
**下次更新:** 阶段2完成后
