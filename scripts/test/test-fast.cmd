@echo off
REM Fast Test Mode - Launch game with debug enabled
echo.
echo Starting game in developer mode...
echo.
launch-game.cmd Game.Mod=ra2 "Debug.EnableDebugCommandsInReplays=true"
