@echo off
REM RA2 Veterancy Testing Utility - Windows Version
REM Unit Veterancy Testing Tool - Windows Version

echo.
echo ==================================
echo RA2 Veterancy Testing Utility
echo ==================================
echo.
echo Choose test mode:
echo.
echo 1) Fast XP Mode (10 XP per level)
echo.
echo 2) Normal XP Mode (standard requirements)
echo.
echo 3) Developer Mode (with debug features)
echo.
set /p choice="Enter option (1-3): "

if "%choice%"=="1" goto fastmode
if "%choice%"=="2" goto normalmode
if "%choice%"=="3" goto devmode
echo Invalid choice
pause
exit /b 1

:fastmode
echo.
echo Starting fast veterancy test mode...
echo.
echo Tip: Units will level up very quickly after each kill
echo.
echo Make sure test units are using ^GainsExperienceFast
echo.
pause
launch-game.cmd Game.Mod=ra2 "Debug.EnableDebugCommandsInReplays=true"
goto end

:normalmode
echo.
echo Starting normal veterancy mode...
echo.
launch-game.cmd Game.Mod=ra2
goto end

:devmode
echo.
echo Starting developer mode...
echo.
echo In-game hotkeys:
echo   F9 - Show performance info
echo   CTRL+SHIFT+F9 - Show collision bounds
echo.
pause
launch-game.cmd Game.Mod=ra2 "Debug.EnableDebugCommandsInReplays=true"
goto end

:end
echo.
echo Testing complete
pause
