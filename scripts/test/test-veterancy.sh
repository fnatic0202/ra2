#!/bin/bash
# RA2 Veterancy Testing Utility
# 快速测试单位升级系统的脚本

echo "=================================="
echo "RA2 Veterancy Testing Utility"
echo "单位升级测试工具"
echo "=================================="
echo ""
echo "请选择测试模式 / Choose test mode:"
echo ""
echo "1) 快速升级模式 (Fast XP - 每10点经验升1级)"
echo "   Fast veterancy mode (10 XP per level)"
echo ""
echo "2) 正常升级模式 (Normal XP - 标准经验需求)"
echo "   Normal veterancy mode (standard XP requirements)"
echo ""
echo "3) 开发者模式 + 控制台 (Developer mode with console)"
echo "   Developer mode with Lua console for manual testing"
echo ""
read -p "输入选项 (1-3): " choice

case $choice in
    1)
        echo ""
        echo "启动快速升级测试模式..."
        echo "Starting fast veterancy test mode..."
        echo ""
        echo "提示：在游戏中，单位每击杀一个敌人就能快速升级"
        echo "Tip: Units will level up very quickly after each kill"
        echo ""
        ./launch-game.sh Game.Mod=ra2 Debug.EnableDebugCommandsInReplays=true
        ;;
    2)
        echo ""
        echo "启动正常升级模式..."
        echo "Starting normal veterancy mode..."
        echo ""
        ./launch-game.sh Game.Mod=ra2
        ;;
    3)
        echo ""
        echo "启动开发者模式 + Lua 控制台..."
        echo "Starting developer mode with Lua console..."
        echo ""
        echo "游戏内按键："
        echo "  F9 - 显示性能/调试信息"
        echo "  CTRL+SHIFT+F9 - 显示碰撞边界"
        echo ""
        echo "In-game hotkeys:"
        echo "  F9 - Show performance info"
        echo "  CTRL+SHIFT+F9 - Show collision bounds"
        echo ""
        ./launch-game.sh Game.Mod=ra2 Debug.EnableDebugCommandsInReplays=true
        ;;
    *)
        echo "无效选项 / Invalid choice"
        exit 1
        ;;
esac
