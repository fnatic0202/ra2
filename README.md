# Red Alert 2 mod for OpenRA

[![Continuous Integration](https://github.com/OpenRA/ra2/workflows/Continuous%20Integration/badge.svg)](https://github.com/OpenRA/ra2/actions/workflows/ci.yml)

A mod for OpenRA that recreates the gameplay of Command & Conquer: Red Alert 2.

EA has not endorsed and does not support this product.

---

## 🚀 Quick Start

### Installation
Consult the [wiki](https://github.com/OpenRA/ra2/wiki) for installation instructions.

### Development
See [docs/development/quick-start.md](docs/development/quick-start.md) for a quick setup guide.

---

## 📚 Documentation

### For Developers
- **[Quick Start Guide](docs/development/quick-start.md)** - Get started in 5 minutes
- **[Development Workflow](docs/development/workflow.md)** - WSL + Windows development setup
- **[Documentation Center](docs/README.md)** - Complete documentation index

### For Testers
- **[Testing Overview](docs/testing/overview.md)** - Testing tools and methods
- **[Scripts Guide](scripts/README.md)** - Available test scripts

---

## 📂 Project Structure

```
ra2/
├── docs/                      # Documentation
│   ├── development/          # Development guides
│   ├── testing/             # Testing guides
│   ├── game-design/         # Game design docs
│   └── features/            # Feature documentation
│       └── hero-revival-system/  # Hero system docs & examples
│
├── scripts/                   # Development and test scripts
│   ├── test/                # Quick test scripts
│   └── dev/                 # Development tools
│
├── mods/ra2/                 # Game mod files
│   ├── rules/               # Unit and game rules (YAML)
│   ├── sequences/           # Sprite definitions
│   ├── weapons/             # Weapon configurations
│   └── chrome/              # UI definitions
│
├── OpenRA.Mods.RA2/         # C# custom code
│   └── Traits/              # Custom game traits
│       ├── HeroDeathHandler.cs       # Hero death & revival
│       ├── HeroRevivalManager.cs     # Revival queue management
│       └── ProductionLimit.cs        # Production monitoring
│
├── .claude/                  # Claude Code configuration
│   ├── commands/            # Slash commands
│   └── knowledge/           # Knowledge base
│
└── [Build files]            # launch-game.*, make.*, etc.
```

---

## 🛠️ Development

### Prerequisites
- .NET SDK 8.0+
- OpenRA engine
- WSL2 (for cross-platform development)

### Building
```bash
# Compile the mod
make all

# Run the game
./launch-game.sh Game.Mod=ra2
```

### Testing
```bash
# Quick test (Windows)
scripts/test/quick-test-windows.cmd

# Automated test (WSL)
scripts/dev/autotest.sh fast auto

# File monitoring (WSL)
scripts/dev/watch-and-test.sh
```

See [scripts/README.md](scripts/README.md) for all available scripts.

---

## 🎮 Features

### ⭐ Hero Revival System (NEW!)
Complete Warcraft 3-style hero system with revival mechanics:
- **6 Unique Heroes**: 3 Allied (Tanya, Prism Commander, Chrono Commander) + 3 Soviet (Yuri, Boris, Tesla Commander)
- **Hero Altars**: Dedicated buildings for hero production and revival
- **Revival Mechanics**: Heroes respawn after death for a cost ($500 + level×$100) and 60-second wait
- **Production Limits**: Maximum 3 heroes per player
- **Visual Identification**: Gold "HERO" badge above hero units
- **Full RA2 Integration**: Uses standard Infantry production sidebar

📖 **[Read More](docs/features/hero-revival-system/README.md)** | 📋 **[Changelog](docs/features/hero-revival-system/CHANGELOG.md)**

### 10-Level Veterancy System
- Extends original 3-level system to 10 levels
- Progressive stat bonuses (firepower, armor, speed)
- Health regeneration at higher levels
- Fast-test mode for development

### Custom Mechanics
- Mind Control system
- Chronoshift abilities
- Weather control
- Carrier parent/child units
- And more...

---

## 🤝 Contributing

We welcome contributions! Please:
1. Read [CONTRIBUTING.md](CONTRIBUTING.md)
2. Follow [CODE_OF_CONDUCT.md](CODE_OF_CONDUCT.md)
3. Check the [development workflow](docs/development/workflow.md)
4. Test your changes thoroughly

---

## 📖 Resources

- **Official Wiki**: https://github.com/OpenRA/ra2/wiki
- **OpenRA Website**: https://www.openra.net/
- **OpenRA Documentation**: https://github.com/OpenRA/OpenRA/wiki
- **Issue Tracker**: https://github.com/OpenRA/ra2/issues

---

## 📜 License

See [LICENSE](LICENSE) for license information.

---

**Happy modding!** 🎉
