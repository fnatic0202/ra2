#!/bin/bash
# Test Debug Panel Launch Script

echo "[Debug Panel Test] Launching game..."
echo "Tip: Press Ctrl+C to force quit"
echo ""

cd "$(dirname "$0")"

# Launch with developer mode enabled
./launch-game.sh Game.Mod=ra2 Debug.EnableDebugCommandsInReplays=true
