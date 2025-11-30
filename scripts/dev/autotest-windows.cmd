@echo off
REM Automated Test Helper for Windows
REM Launches game and auto-closes after timeout

set TIMEOUT_SECONDS=%1
if "%TIMEOUT_SECONDS%"=="" set TIMEOUT_SECONDS=30

echo.
echo ========================================
echo RA2 Automated Test (Windows)
echo ========================================
echo.
echo Timeout: %TIMEOUT_SECONDS% seconds
echo.
echo Starting game...
echo.

REM Start game in background - Fixed parameter format
start "" /B launch-game.cmd Game.Mod=ra2 "Debug.EnableDebugCommandsInReplays=true"

REM Wait for timeout
echo Waiting %TIMEOUT_SECONDS% seconds...
timeout /t %TIMEOUT_SECONDS% /nobreak >nul

REM Try to close game gracefully
echo.
echo Closing game...
taskkill /IM OpenRA.exe /F >nul 2>&1
taskkill /IM OpenRA.Launcher.exe /F >nul 2>&1

echo.
echo Test session completed
echo.
