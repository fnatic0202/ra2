#!/bin/bash
# WSL to Windows 同步脚本
# 在 WSL 中开发后，确保 Windows 可以访问最新文件

echo "=================================="
echo "WSL to Windows Sync Check"
echo "WSL 到 Windows 同步检查"
echo "=================================="
echo ""

# 检查当前路径
CURRENT_DIR=$(pwd)
echo "当前目录 / Current directory:"
echo "  WSL:     $CURRENT_DIR"
echo "  Windows: $(wslpath -w "$CURRENT_DIR")"
echo ""

# 检查是否在挂载的 Windows 盘符下
if [[ $CURRENT_DIR == /mnt/* ]]; then
    echo "✓ 你已经在 Windows 挂载盘符下工作"
    echo "✓ You are already working on a Windows mounted drive"
    echo ""
    echo "文件实时共享 - 无需同步！"
    echo "Files are shared in real-time - no sync needed!"
    echo ""
    echo "在 Windows 中的路径："
    echo "Windows path:"
    WIN_PATH=$(wslpath -w "$CURRENT_DIR")
    echo "  $WIN_PATH"
    echo ""
    echo "你可以直接在 Windows 中运行："
    echo "You can run directly in Windows:"
    echo "  cd $WIN_PATH"
    echo "  test-veterancy.cmd"
else
    echo "⚠ 警告：你在 WSL 独立文件系统中工作"
    echo "⚠ Warning: You are in WSL native filesystem"
    echo ""
    echo "建议将项目移动到 /mnt/c/ 或 /mnt/d/ 等 Windows 盘符下"
    echo "Recommend moving project to /mnt/c/ or /mnt/d/ for better performance"
    echo ""
    echo "如果需要同步，请使用："
    echo "If you need to sync, use:"
    echo "  rsync -av --progress $CURRENT_DIR/ /mnt/c/workspace/ra2/"
fi

echo ""
echo "=================================="
echo "测试脚本位置 / Test Scripts:"
echo "=================================="
echo "WSL:     ./test-veterancy.sh"
echo "Windows: test-veterancy.cmd"
echo ""
