# OpenRA RA2 Multi-Agent Game Development System

Welcome to the Claude Code-based multi-agent architecture for game development!

## System Overview

This system uses specialized agents to collaboratively complete game development tasks:
- Game Design
- Code Implementation
- Configuration
- Balance Analysis
- Testing & Validation

## Architecture

```
Game Project Manager (/game:pm)
    ├─ Design Layer
    │   ├─ Game Designer (/game:design)
    │   └─ Game Balancer (/game:balance)
    ├─ Implementation Layer
    │   ├─ Config Engineer (/game:config)
    │   └─ Trait Developer (/game:trait)
    └─ Validation Layer
        ├─ Game Tester (/game:test)
        └─ Config Validator (/game:validate)
```

## Available Commands

### Main Coordinator
- **/game:pm** - Game Project Manager
  - Analyzes requirements and coordinates specialists
  - Use for complex multi-step tasks

### Design Commands
- **/game:design** - Game Designer
  - Designs units, weapons, mechanics
  - Outputs detailed design documents

- **/game:balance** - Balance Analyst
  - Analyzes unit balance
  - Calculates DPS, cost-efficiency metrics
  - Provides balance recommendations

### Implementation Commands
- **/game:config** - Config Engineer
  - Implements YAML configurations
  - Creates unit, weapon, sequence definitions

- **/game:trait** - Trait Developer
  - Develops custom C# code
  - Implements Traits, Projectiles, Warheads

### Validation Commands
- **/game:test** - Game Tester
  - Launches game for actual testing
  - Reports bugs and issues
  - Verifies functionality

- **/game:validate** - Config Validator
  - Checks YAML syntax
  - Verifies reference integrity
  - Detects configuration errors

## Quick Start

The simplest way to begin:

```bash
# Try adding a simple unit
/game:pm Add a simple test infantry unit and guide me through design to testing
```

The project manager will guide you through the entire process!

## Usage Examples

### Example 1: Add New Unit (Complete Flow)

**Option A: Using Project Manager (Recommended for beginners)**
```
/game:pm Add an Allied sniper infantry unit
```

**Option B: Manual Step-by-Step (Recommended for advanced users)**
```bash
# Step 1: Design
/game:design Design an Allied sniper - high damage, long range, fragile, effective vs infantry

# Step 2: Implement configuration
/game:config Implement the sniper unit based on the design above

# Step 3: Test
/game:test Test the new sniper unit

# Step 4: Validate
/game:validate Validate all new configuration files
```

### Example 2: Balance Adjustment

```bash
# Analyze current balance
/game:balance Analyze the balance of the Grizzly Tank

# Apply adjustments
/game:config Increase Grizzly Tank cost from $700 to $800

# Test changes
/game:test Test the Grizzly Tank balance adjustment
```

### Example 3: Implement New Mechanic

```bash
# Design mechanic
/game:design Design an "Overcharge" mechanic: unit can boost attack temporarily but loses HP

# Implement custom Trait
/game:trait Implement the Overcharge mechanic Trait

# Configure on unit
/game:config Add Overcharge ability to Apocalypse Tank

# Test
/game:test Test Apocalypse Tank's Overcharge ability
```

## Knowledge Base

### .claude/knowledge/openra-game-dev.md
Complete OpenRA development reference:
- Trait system
- Weapon configuration
- Armor and damage systems
- Numeric references
- Common patterns
- Balance principles

### .serena/project-index.md
Project structure index:
- Key file locations
- Unit and weapon definitions
- Common modification tasks

## Best Practices

1. **Start with Design** - Use `/game:design` for important features
2. **Test Incrementally** - Test after each step
3. **Validate Configs** - Run `/game:validate` before committing
4. **Iterate Balance** - Use data-driven balance adjustments
5. **Document Decisions** - Record important choices in `.serena/`

## File Structure

```
.claude/commands/game/     # Game development commands
.claude/knowledge/         # Development knowledge base
.serena/                   # Project index and context
mods/ra2/rules/           # Unit definitions
mods/ra2/weapons/         # Weapon definitions
OpenRA.Mods.RA2/Traits/   # Custom C# code
```

## Troubleshooting

**If validation fails:**
```bash
/game:validate Check recently modified files in detail
```

**If game won't launch:**
```bash
/game:test Launch game and report error logs
```

**If unit is unbalanced:**
```bash
/game:balance Analyze [unit name] in detail
/game:config Apply suggested balance adjustments
```

---

**Happy developing!** Ask any agent for help anytime.
