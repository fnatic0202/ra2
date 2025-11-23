#!/bin/bash
# Test Report Analyzer

LOG_DIR="test-logs"

echo "========================================"
echo "RA2 Test Report Analyzer"
echo "========================================"
echo ""

# Find latest test log
if [[ ! -d "$LOG_DIR" ]]; then
    echo "No test logs found. Run ./autotest.sh first."
    exit 1
fi

LATEST_AFTER=$(ls -t ${LOG_DIR}/ra2_after_*.log 2>/dev/null | head -1)
LATEST_REPORT=$(ls -t ${LOG_DIR}/test_*.log 2>/dev/null | head -1)

if [[ -z "$LATEST_AFTER" ]]; then
    echo "No test results found."
    exit 1
fi

echo "Analyzing: $LATEST_AFTER"
echo ""

# Extract statistics
echo "=== ERROR ANALYSIS ==="
ERRORS=$(grep -i "error\|exception" "$LATEST_AFTER" 2>/dev/null | wc -l)
WARNINGS=$(grep -i "warning" "$LATEST_AFTER" 2>/dev/null | wc -l)

echo "Total Errors: $ERRORS"
echo "Total Warnings: $WARNINGS"
echo ""

if [[ $ERRORS -gt 0 ]]; then
    echo "Error Details:"
    grep -i "error\|exception" "$LATEST_AFTER" | head -10
    echo ""
fi

# Check for specific features
echo "=== FEATURE CHECKS ==="

if grep -q "rank-level" "$LATEST_AFTER" 2>/dev/null; then
    LEVELS=$(grep -o "rank-level[0-9]" "$LATEST_AFTER" | sort -u | wc -l)
    echo "✓ Veterancy System: Active ($LEVELS levels detected)"
else
    echo "✗ Veterancy System: Not detected"
fi

if grep -q "GainsExperience" "$LATEST_AFTER" 2>/dev/null; then
    echo "✓ Experience System: Loaded"
else
    echo "! Experience System: Not confirmed"
fi

echo ""

# Performance metrics
echo "=== PERFORMANCE ==="
LOAD_TIME=$(grep "Loading mod:" "$LATEST_AFTER" | head -1)
if [[ -n "$LOAD_TIME" ]]; then
    echo "✓ Mod loaded successfully"
else
    echo "! Mod load not confirmed"
fi

# Count unit spawns (if any)
UNIT_COUNT=$(grep -i "actor.*created\|spawn" "$LATEST_AFTER" 2>/dev/null | wc -l)
echo "Units created: $UNIT_COUNT"

echo ""

# Show test report if exists
if [[ -f "$LATEST_REPORT" ]]; then
    echo "=== TEST REPORT ==="
    cat "$LATEST_REPORT"
fi

# Summary
echo ""
echo "========================================"
echo "SUMMARY"
echo "========================================"
if [[ $ERRORS -eq 0 ]]; then
    echo "Status: ✓ PASS"
    echo "No errors detected in test run"
else
    echo "Status: ✗ FAIL"
    echo "$ERRORS errors need attention"
fi

echo ""
echo "Full logs available in: $LOG_DIR"
echo ""
