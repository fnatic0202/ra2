# RA2 Veterancy Testing Utility - PowerShell Version
# 单位升级测试工具 - PowerShell 版本

Write-Host "==================================" -ForegroundColor Cyan
Write-Host "RA2 Veterancy Testing Utility" -ForegroundColor Green
Write-Host "单位升级测试工具" -ForegroundColor Green
Write-Host "==================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "请选择测试模式 / Choose test mode:" -ForegroundColor Yellow
Write-Host ""
Write-Host "1) 快速升级模式 (Fast XP - 每10点经验升1级)" -ForegroundColor White
Write-Host "   Fast veterancy mode (10 XP per level)" -ForegroundColor Gray
Write-Host ""
Write-Host "2) 正常升级模式 (Normal XP - 标准经验需求)" -ForegroundColor White
Write-Host "   Normal veterancy mode (standard XP requirements)" -ForegroundColor Gray
Write-Host ""
Write-Host "3) 开发者模式 + 控制台 (Developer mode with console)" -ForegroundColor White
Write-Host "   Developer mode with debug features" -ForegroundColor Gray
Write-Host ""

$choice = Read-Host "输入选项 (1-3)"

switch ($choice) {
    "1" {
        Write-Host ""
        Write-Host "启动快速升级测试模式..." -ForegroundColor Green
        Write-Host "Starting fast veterancy test mode..." -ForegroundColor Gray
        Write-Host ""
        Write-Host "提示：在游戏中，单位每击杀一个敌人就能快速升级" -ForegroundColor Yellow
        Write-Host "Tip: Units will level up very quickly after each kill" -ForegroundColor Gray
        Write-Host ""
        Write-Host "请确保已将测试单位修改为使用 ^GainsExperienceFast" -ForegroundColor Yellow
        Write-Host "Make sure test units are using ^GainsExperienceFast" -ForegroundColor Gray
        Write-Host ""
        Pause
        & .\launch-game.cmd Game.Mod=ra2 Debug.EnableDebugCommandsInReplays=true
    }
    "2" {
        Write-Host ""
        Write-Host "启动正常升级模式..." -ForegroundColor Green
        Write-Host "Starting normal veterancy mode..." -ForegroundColor Gray
        Write-Host ""
        & .\launch-game.cmd Game.Mod=ra2
    }
    "3" {
        Write-Host ""
        Write-Host "启动开发者模式..." -ForegroundColor Green
        Write-Host "Starting developer mode..." -ForegroundColor Gray
        Write-Host ""
        Write-Host "游戏内按键：" -ForegroundColor Yellow
        Write-Host "  F9 - 显示性能/调试信息"
        Write-Host "  CTRL+SHIFT+F9 - 显示碰撞边界"
        Write-Host ""
        Write-Host "In-game hotkeys:" -ForegroundColor Yellow
        Write-Host "  F9 - Show performance info"
        Write-Host "  CTRL+SHIFT+F9 - Show collision bounds"
        Write-Host ""
        Pause
        & .\launch-game.cmd Game.Mod=ra2 Debug.EnableDebugCommandsInReplays=true
    }
    default {
        Write-Host "无效选项 / Invalid choice" -ForegroundColor Red
        Pause
        exit 1
    }
}

Write-Host ""
Write-Host "测试完成 / Testing complete" -ForegroundColor Green
Pause
