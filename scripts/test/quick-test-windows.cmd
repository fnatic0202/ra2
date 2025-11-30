@echo off
REM Quick launch Windows test - Minimal version

echo.
echo [Quick Test] Launching game...
echo.
echo Tip: Press Ctrl+C to force quit
echo.

REM Launch with developer mode - Fixed parameter format
launch-game.cmd Game.Mod=ra2 "Debug.EnableDebugCommandsInReplays=true"

echo.
echo Game closed
pause
