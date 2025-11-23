#!/bin/bash
# Watch for file changes and auto-test

echo "========================================"
echo "File Watcher - Auto Test on Change"
echo "========================================"
echo ""

WATCH_DIR="mods/ra2/rules"
WATCH_FILES="defaults.yaml allied-infantry.yaml soviet-infantry.yaml"

echo "Watching directory: $WATCH_DIR"
echo "Watching files: $WATCH_FILES"
echo ""
echo "Press Ctrl+C to stop watching"
echo ""

# Check if inotifywait is available
if ! command -v inotifywait &> /dev/null; then
    echo "Installing inotify-tools for file watching..."
    sudo apt-get update && sudo apt-get install -y inotify-tools
fi

# Function to run test
run_test() {
    echo ""
    echo "========================================"
    echo "Change detected! Running auto-test..."
    echo "========================================"
    ./autotest.sh fast auto
    echo ""
    echo "Waiting for next change..."
}

# Initial message
echo "Waiting for file changes..."

# Watch for modifications
while inotifywait -q -e modify,close_write "$WATCH_DIR"/*.yaml; do
    # Debounce - wait a bit for multiple rapid changes
    sleep 2
    run_test
done
