#!/bin/bash
# WSL 快速切换到 Windows 测试
# Quick switch from WSL to Windows testing

echo "=================================="
echo "WSL to Windows Quick Test"
echo "WSL 到 Windows 快速测试"
echo "=================================="
echo ""

# 获取 Windows 路径
WIN_PATH=$(wslpath -w "$(pwd)")

echo "当前目录的 Windows 路径："
echo "Windows path of current directory:"
echo "  $WIN_PATH"
echo ""

echo "选择操作 / Choose action:"
echo ""
echo "1) 在 Windows 资源管理器中打开项目目录"
echo "   Open project folder in Windows Explorer"
echo ""
echo "2) 在 Windows Terminal 中打开 (需要已安装)"
echo "   Open in Windows Terminal (requires installation)"
echo ""
echo "3) 在 PowerShell 中打开"
echo "   Open in PowerShell"
echo ""
echo "4) 在 CMD 中打开"
echo "   Open in CMD"
echo ""
echo "5) 直接启动 Windows 测试脚本"
echo "   Directly launch Windows test script"
echo ""

read -p "输入选项 (1-5): " choice

case $choice in
    1)
        echo ""
        echo "正在打开 Windows 资源管理器..."
        echo "Opening Windows Explorer..."
        explorer.exe "$(wslpath -w "$(pwd)")"
        ;;
    2)
        echo ""
        echo "正在打开 Windows Terminal..."
        echo "Opening Windows Terminal..."
        cmd.exe /c "wt.exe -d \"$WIN_PATH\""
        ;;
    3)
        echo ""
        echo "正在打开 PowerShell..."
        echo "Opening PowerShell..."
        powershell.exe -NoExit -Command "cd '$WIN_PATH'"
        ;;
    4)
        echo ""
        echo "正在打开 CMD..."
        echo "Opening CMD..."
        cmd.exe /k "cd /d \"$WIN_PATH\""
        ;;
    5)
        echo ""
        echo "正在启动 Windows 测试脚本..."
        echo "Launching Windows test script..."
        echo ""
        echo "提示：如果没有反应，请手动在 Windows 中运行 test-veterancy.cmd"
        echo "Tip: If nothing happens, manually run test-veterancy.cmd in Windows"
        echo ""
        cmd.exe /c "cd /d \"$WIN_PATH\" && test-veterancy.cmd"
        ;;
    *)
        echo "无效选项 / Invalid choice"
        exit 1
        ;;
esac

echo ""
echo "操作完成 / Operation complete"
