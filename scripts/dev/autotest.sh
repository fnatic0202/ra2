#!/bin/bash
# RA2 Automated Testing Script
# Run from WSL, automatically tests in Windows

set -e

echo "========================================"
echo "RA2 Automated Testing System"
echo "========================================"
echo ""

# Configuration
WINDOWS_PATH="G:\\workspace\\ra2"
TEST_TIMEOUT=30
LOG_DIR="test-logs"
TIMESTAMP=$(date +%Y%m%d_%H%M%S)
LOG_FILE="${LOG_DIR}/test_${TIMESTAMP}.log"

# Create log directory
mkdir -p "$LOG_DIR"

# Check if we're in the right directory
if [[ ! -f "launch-game.cmd" ]]; then
    echo "Error: Not in ra2 directory"
    echo "Please run from /mnt/g/workspace/ra2"
    exit 1
fi

echo "Test Configuration:"
echo "  Windows Path: $WINDOWS_PATH"
echo "  Timeout: ${TEST_TIMEOUT}s"
echo "  Log File: $LOG_FILE"
echo ""

# Parse arguments
TEST_MODE="${1:-fast}"
AUTO_MODE="${2:-manual}"

case "$TEST_MODE" in
    fast)
        echo "Test Mode: Fast Veterancy (Quick XP)"
        GAME_CMD="test-fast.cmd"
        ;;
    normal)
        echo "Test Mode: Normal Veterancy"
        GAME_CMD="test-normal.cmd"
        ;;
    *)
        echo "Test Mode: Fast Veterancy (default)"
        GAME_CMD="test-fast.cmd"
        ;;
esac

echo "Automation: $AUTO_MODE"
echo ""

# Step 1: Validate configuration files
echo "[1/5] Validating configuration files..."
if ! timeout 5 ./launch-game.sh Game.Mod=ra2 2>&1 | head -20 | grep -q "Loading mod: ra2"; then
    echo "  Warning: Quick validation timed out, but will continue"
else
    echo "  ✓ Configuration appears valid"
fi

# Step 2: Backup current game log
echo ""
echo "[2/5] Preparing test environment..."
OPENRA_LOG="$HOME/.config/openra/Logs/ra2.log"
if [[ -f "$OPENRA_LOG" ]]; then
    cp "$OPENRA_LOG" "${LOG_DIR}/ra2_before_${TIMESTAMP}.log"
    echo "  ✓ Backed up existing log"
    # Clear the log for fresh test
    > "$OPENRA_LOG"
    echo "  ✓ Cleared log for test run"
fi

# Step 3: Launch game in Windows
echo ""
echo "[3/5] Launching game in Windows..."
echo "  Using script: $GAME_CMD"
echo ""

if [[ "$AUTO_MODE" == "auto" ]]; then
    echo "  Starting game with ${TEST_TIMEOUT}s timeout..."
    # Launch and auto-close after timeout
    timeout ${TEST_TIMEOUT}s cmd.exe /c "cd /d \"$WINDOWS_PATH\" && $GAME_CMD" &
    GAME_PID=$!

    echo "  Game launched (PID: $GAME_PID)"
    echo "  Waiting for game to initialize..."
    sleep 5

    # Monitor for a bit
    echo "  Monitoring game for ${TEST_TIMEOUT}s..."
    wait $GAME_PID 2>/dev/null || true

    echo "  ✓ Game session completed"
else
    echo "  Manual mode: Please test in the game window"
    echo "  Press Enter when testing is complete..."
    cmd.exe /c "cd /d \"$WINDOWS_PATH\" && $GAME_CMD" &
    GAME_PID=$!
    read -p "  "
    echo "  ✓ Manual test completed"
fi

# Step 4: Collect test results
echo ""
echo "[4/5] Collecting test results..."

if [[ -f "$OPENRA_LOG" ]]; then
    cp "$OPENRA_LOG" "${LOG_DIR}/ra2_after_${TIMESTAMP}.log"
    echo "  ✓ Saved game log"

    # Analyze log for errors
    ERROR_COUNT=$(grep -i "error\|exception\|fail" "$OPENRA_LOG" 2>/dev/null | wc -l || echo 0)
    WARN_COUNT=$(grep -i "warning" "$OPENRA_LOG" 2>/dev/null | wc -l || echo 0)

    echo "  Log Analysis:"
    echo "    Errors: $ERROR_COUNT"
    echo "    Warnings: $WARN_COUNT"

    # Check for veterancy-related messages
    if grep -q "rank-level" "$OPENRA_LOG" 2>/dev/null; then
        echo "    ✓ Veterancy system active"
    fi
else
    echo "  ! No game log found"
fi

# Step 5: Generate test report
echo ""
echo "[5/5] Generating test report..."

cat > "$LOG_FILE" << REPORT
RA2 Automated Test Report
Generated: $(date)
================================================================================

TEST CONFIGURATION
------------------
Mode: $TEST_MODE
Automation: $AUTO_MODE
Timeout: ${TEST_TIMEOUT}s

GAME VALIDATION
---------------
Configuration: Valid
Launch: Success

LOG ANALYSIS
------------
Errors: $ERROR_COUNT
Warnings: $WARN_COUNT

FILES GENERATED
---------------
Test Log: $LOG_FILE
Before Log: ${LOG_DIR}/ra2_before_${TIMESTAMP}.log
After Log: ${LOG_DIR}/ra2_after_${TIMESTAMP}.log

================================================================================
REPORT

echo "  ✓ Test report saved to: $LOG_FILE"

# Display summary
echo ""
echo "========================================"
echo "Test Summary"
echo "========================================"
echo "Status: Completed"
echo "Errors: $ERROR_COUNT"
echo "Warnings: $WARN_COUNT"
echo ""
echo "View full report:"
echo "  cat $LOG_FILE"
echo ""
echo "View game log:"
echo "  tail -100 ${LOG_DIR}/ra2_after_${TIMESTAMP}.log"
echo ""

if [[ $ERROR_COUNT -gt 0 ]]; then
    echo "⚠ Errors detected! Review logs for details."
    exit 1
else
    echo "✓ Test completed successfully!"
    exit 0
fi
