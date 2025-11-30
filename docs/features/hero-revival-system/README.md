# Hero Revival System - Warcraft 3 Style

## Overview

Complete implementation of a Warcraft 3-style hero system for OpenRA Red Alert 2 mod, featuring:
- 6 unique hero units (3 Allied, 3 Soviet)
- Hero death and revival mechanics
- Production limit enforcement (3 heroes max per player)
- Visual hero identification with gold "HERO" badge
- Dynamic revival costs based on hero level
- Full integration with Red Alert 2 production interface

## Features Implemented

### 1. Hero Units

#### Allied Heroes
- **Tanya** - Elite Commando (Assassin role)
  - Cost: $2000 | HP: 400 | Speed: 85
  - Strong vs Infantry, Buildings
  - Special: C4 Demolition, Swimming

- **Prism Commander** - Light Architect (Artillery role)
  - Cost: $2500 | HP: 600 | Speed: 60
  - Strong vs Everything
  - Special: Long range beam weapon

- **Chrono Commander** - Time Weaver (Mobility role)
  - Cost: $2200 | HP: 500 | Speed: 70
  - Strong vs All
  - Special: High mobility

#### Soviet Heroes
- **Yuri** - Psychic Master (Controller role)
  - Cost: $2200 | HP: 450 | Speed: 55
  - Strong vs Infantry, Light Vehicles
  - Special: Mind control, Psychic detection

- **Boris** - Airstrike Commander (Artillery role)
  - Cost: $2300 | HP: 550 | Speed: 65
  - Strong vs Vehicles, Buildings
  - Special: Great vision for targeting

- **Tesla Commander** - Iron Warlord (Tank role)
  - Cost: $2400 | HP: 800 | Speed: 50
  - Strong vs Infantry, Vehicles
  - Special: AOE damage, High durability

### 2. Hero Altars

- **Allied Hero Monument** (gahero) - $1000
  - Prerequisites: Barracks + Battle Lab
  - Produces and revives Allied heroes

- **Soviet Hero Command Center** (nahero) - $1000
  - Prerequisites: Barracks + Battle Lab
  - Produces and revives Soviet heroes

### 3. Revival System

**Death Mechanics:**
- Hero "falls" instead of dying permanently
- Becomes invisible/inactive (hero_fallen condition)
- Automatically added to altar revival queue

**Revival Process:**
- Base cost: $500
- Additional cost per level: $100
  - Level 1 hero: $600
  - Level 5 hero: $1000
  - Level 10 hero: $1500
- Revival time: 60 seconds (1500 ticks)
- Hero respawns at altar with full health

**Queue System:**
- FIFO (First In, First Out)
- Multiple heroes revive sequentially
- Altar destruction clears queue

### 4. Production Limits

- Maximum 3 heroes per player
- Enforced via BuildLimit on hero units
- Notification when limit reached
- Applies across both factions

### 5. Visual Identification

**HERO Badge:**
- Gold text "HERO" above unit
- Appears at top position
- Font: TinyBold
- Color: FFD700 (gold)
- Disappears when hero is fallen

## Technical Implementation

### C# Traits (OpenRA.Mods.RA2/Traits/)

1. **HeroDeathHandler.cs** (156 lines)
   - Intercepts hero death via INotifyKilled
   - Grants hero_fallen condition
   - Adds hero to altar revival queue
   - Plays death notifications

2. **HeroRevivalManager.cs** (354 lines)
   - Manages revival queue on altars
   - Calculates dynamic revival costs
   - Processes revival timing
   - Handles spawning and restoration

3. **ProductionLimit.cs** (153 lines)
   - Monitors production limits
   - Provides player feedback
   - Works with BuildLimit enforcement

4. **HeroInfo.cs** (existing, 58 lines)
   - Stores hero metadata
   - Provides role and difficulty info

### YAML Configuration

**Modified Files:**
- `mods/ra2/rules/defaults.yaml` - ^Hero template with revival and badge
- `mods/ra2/rules/hero-units.yaml` - All 6 heroes with BuildLimit and Infantry queue
- `mods/ra2/rules/allied-structures.yaml` - gahero with HeroRevivalManager
- `mods/ra2/rules/soviet-structures.yaml` - nahero with HeroRevivalManager
- `mods/ra2/audio/notifications.yaml` - Hero notifications
- `mods/ra2/sequences/allied-infantry.yaml` - Allied hero sequences
- `mods/ra2/sequences/soviet-infantry.yaml` - Soviet hero sequences

### Integration with RA2 Production System

Heroes use the standard Infantry production queue:
- Appear in Infantry sidebar tab
- BuildPaletteOrder: 100+ (bottom of list)
- Prerequisites control visibility
- Produced at altars, not barracks

## Usage

### In-Game

1. Build Barracks + Battle Lab
2. Build Hero Altar ($1000)
3. Click Infantry tab in sidebar
4. Heroes appear at bottom of list
5. Click hero to queue production
6. Hero spawns at altar with gold "HERO" badge

### Prerequisites

| Hero | Required Buildings |
|------|-------------------|
| Tanya / Yuri | Altar only |
| Prism Commander / Boris | Altar + War Factory |
| Chrono Commander / Tesla Commander | Altar + Battle Lab |

## Files

### Documentation
- `README.md` - This file
- `HERO_REVIVAL_IMPLEMENTATION.md` - Technical implementation details
- `hero-revival-examples.yaml` - Configuration examples
- `hero-revival-demo.png` - Screenshot demonstration

### Code
- `OpenRA.Mods.RA2/Traits/HeroDeathHandler.cs`
- `OpenRA.Mods.RA2/Traits/HeroRevivalManager.cs`
- `OpenRA.Mods.RA2/Traits/ProductionLimit.cs`
- `OpenRA.Mods.RA2/Traits/HeroInfo.cs`

### Configuration
- `mods/ra2/rules/defaults.yaml` (^Hero template)
- `mods/ra2/rules/hero-units.yaml` (6 heroes)
- `mods/ra2/rules/allied-structures.yaml` (gahero)
- `mods/ra2/rules/soviet-structures.yaml` (nahero)
- `mods/ra2/audio/notifications.yaml` (notifications)
- `mods/ra2/sequences/allied-infantry.yaml` (graphics)
- `mods/ra2/sequences/soviet-infantry.yaml` (graphics)

## Known Limitations

1. **Placeholder Graphics**: Heroes use base unit sprites temporarily
2. **No Revival Queue UI**: Revival progress not visible in sidebar
3. **Automatic Revival**: No manual trigger option
4. **Altar Destruction**: Heroes in queue are lost if altar destroyed

## Future Enhancements

1. Custom hero sprite graphics
2. Revival queue UI widget
3. Hero ability system (Q, W, E, R slots)
4. Hero inventory system (6 item slots)
5. Special visual effects for revival
6. Altar transfer on capture

## Testing

See `HERO_REVIVAL_IMPLEMENTATION.md` for comprehensive testing checklist.

Quick test:
```bash
./launch-game.sh
# Start skirmish → Build Barracks → Build Battle Lab → Build Hero Altar
# Click Infantry tab → Select hero → Verify HERO badge appears
```

## Version History

- **v1.0** (2025-11-30) - Initial implementation
  - 6 heroes with full stats
  - Revival system functional
  - Production limit enforced
  - HERO visual badge
  - RA2 production interface integration
  - Voice configuration fixes

## Credits

Developed using multi-agent system coordination:
- TraitDeveloper: C# trait implementation
- ConfigEngineer: YAML configuration
- ConfigValidator: Configuration validation
- GameTester: System testing
- Game PM: Project coordination

🤖 Generated with [Claude Code](https://claude.com/claude-code)
