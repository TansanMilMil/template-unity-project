#!/bin/bash
# release.sh - Unity Package Release Automation Script
#
# Usage: ./scripts/release.sh <version>
# Example: ./scripts/release.sh 1.6.0

set -e  # Exit immediately if a command exits with a non-zero status

# ===== Configuration =====
SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
REPO_ROOT="$(cd "$SCRIPT_DIR/.." && pwd)"
PACKAGE_DIR="$REPO_ROOT/Packages/TansanMilMilUtil"
PACKAGE_JSON="$PACKAGE_DIR/package.json"
TAG_PREFIX="util_v"
LATEST_TAG="util_latest"
GITHUB_REPO="TansanMilMil/template-unity-project"

# ===== Function Definitions =====

# Validate version format (X.Y.Z)
validate_version() {
    local version=$1
    if ! [[ $version =~ ^[0-9]+\.[0-9]+\.[0-9]+(-[a-zA-Z0-9.]+)?$ ]]; then
        echo "Error: Invalid version format. Expected: X.Y.Z (e.g., 1.6.0) or X.Y.Z-prerelease (e.g., 1.6.0-beta.1)"
        exit 1
    fi
}

# Check if working directory is clean
check_working_directory() {
    cd "$REPO_ROOT"
    if ! git diff-index --quiet HEAD --; then
        echo "Error: Working directory is not clean. Please commit or stash your changes."
        echo ""
        git status --short
        exit 1
    fi
}

# Check if tag already exists
check_tag_exists() {
    local tag=$1
    if git rev-parse "$tag" >/dev/null 2>&1; then
        echo "Error: Tag '$tag' already exists."
        echo "To view existing tags: git tag -l '${TAG_PREFIX}*'"
        exit 1
    fi
}

# Check if jq is installed
check_jq_installed() {
    if ! command -v jq &> /dev/null; then
        echo "Error: 'jq' command not found."
        echo "Please install jq:"
        echo "  Ubuntu/Debian: sudo apt-get install jq"
        echo "  macOS: brew install jq"
        exit 1
    fi
}

# Update package.json (using jq)
update_package_json() {
    local version=$1
    local changelog_url="https://github.com/$GITHUB_REPO/releases/tag/${TAG_PREFIX}${version}"

    # Create backup
    cp "$PACKAGE_JSON" "$PACKAGE_JSON.backup"

    # Update using jq
    jq --arg ver "$version" --arg url "$changelog_url" \
       '.version = $ver | .changelogUrl = $url' \
       "$PACKAGE_JSON" > "$PACKAGE_JSON.tmp"

    mv "$PACKAGE_JSON.tmp" "$PACKAGE_JSON"

    echo "Updated package.json:"
    echo "  version: $version"
    echo "  changelogUrl: $changelog_url"
}

# Rollback function
rollback() {
    echo ""
    echo "Rolling back changes..."
    if [ -f "$PACKAGE_JSON.backup" ]; then
        mv "$PACKAGE_JSON.backup" "$PACKAGE_JSON"
        echo "Restored package.json from backup"
    fi
    cd "$REPO_ROOT"
    git reset --hard HEAD >/dev/null 2>&1 || true
    echo "Rollback complete"
    exit 1
}

# Set error handler
trap rollback ERR

# Display help
show_help() {
    cat <<EOF
Unity Package Release Script

USAGE:
    $0 <version>

ARGUMENTS:
    <version>    Semantic version number (X.Y.Z format)
                 Example: 1.6.0, 2.0.0, 1.6.0-beta.1

DESCRIPTION:
    This script automates the release process for TansanMilMilUtil package:
    1. Updates package.json (version and changelogUrl)
    2. Creates a commit
    3. Creates version tag (util_v{version})
    4. Moves util_latest tag to HEAD
    5. Pushes to origin (with confirmation)

PREREQUISITES:
    - jq must be installed
    - Clean working directory
    - Push access to repository

EXAMPLES:
    $0 1.6.0
    $0 2.0.0
    $0 1.6.0-beta.1

For more information, see README.md
EOF
    exit 0
}

# ===== Main Process =====

main() {
    # Check for help flag
    if [[ "$1" == "--help" ]] || [[ "$1" == "-h" ]]; then
        show_help
    fi

    # Check arguments
    if [ $# -ne 1 ]; then
        echo "Usage: $0 <version>"
        echo "Example: $0 1.6.0"
        echo ""
        echo "Run '$0 --help' for more information."
        exit 1
    fi

    VERSION=$1
    TAG_NAME="${TAG_PREFIX}${VERSION}"

    echo "========================================="
    echo "Unity Package Release Script"
    echo "========================================="
    echo "Package: TansanMilMilUtil"
    echo "New Version: $VERSION"
    echo "Tag: $TAG_NAME"
    echo "========================================="
    echo ""

    # Validation phase
    echo "[1/8] Validating version format..."
    validate_version "$VERSION"

    echo "[2/8] Checking if jq is installed..."
    check_jq_installed

    echo "[3/8] Checking working directory..."
    check_working_directory

    echo "[4/8] Checking if tag exists..."
    check_tag_exists "$TAG_NAME"

    # Update phase
    echo "[5/8] Updating package.json..."
    update_package_json "$VERSION"

    # Git operations phase
    echo "[6/8] Creating commit..."
    cd "$REPO_ROOT"
    git add "$PACKAGE_JSON"
    git commit -m "chore: release v${VERSION}"

    echo "[7/8] Creating tag '$TAG_NAME'..."
    git tag "$TAG_NAME"

    echo "[8/8] Moving '$LATEST_TAG' tag to HEAD..."
    # If util_latest tag exists, delete and recreate
    if git rev-parse "$LATEST_TAG" >/dev/null 2>&1; then
        git tag -d "$LATEST_TAG"
    fi
    git tag "$LATEST_TAG"

    # Confirmation prompt
    echo ""
    echo "========================================="
    echo "Ready to push to origin"
    echo "========================================="
    echo "The following will be pushed:"
    echo "  - 1 commit: 'chore: release v${VERSION}'"
    echo "  - Tag: $TAG_NAME"
    echo "  - Tag: $LATEST_TAG (force)"
    echo ""
    read -p "Push to origin? (y/N): " -n 1 -r
    echo

    if [[ $REPLY =~ ^[Yy]$ ]]; then
        # Remove backup (assuming success)
        rm -f "$PACKAGE_JSON.backup"

        # Push
        echo ""
        echo "Pushing to origin..."
        git push origin main
        git push origin "$TAG_NAME"
        git push origin "$LATEST_TAG" --force

        echo ""
        echo "========================================="
        echo "✓ Release completed successfully!"
        echo "========================================="
        echo "Version: $VERSION"
        echo "Tag: $TAG_NAME"
        echo ""
        echo "GitHub Release will be created automatically by GitHub Actions."
        echo "Check: https://github.com/$GITHUB_REPO/releases"
        echo "========================================="
    else
        echo ""
        echo "Push cancelled. Rolling back local changes..."
        git reset --hard HEAD~1
        git tag -d "$TAG_NAME"
        git tag -d "$LATEST_TAG" 2>/dev/null || true

        # Restore util_latest if it existed before
        if git rev-parse "refs/tags/${LATEST_TAG}@{1}" >/dev/null 2>&1; then
            git tag "$LATEST_TAG" "refs/tags/${LATEST_TAG}@{1}"
        fi

        rm -f "$PACKAGE_JSON.backup"
        echo "Rollback complete. No changes were pushed."
    fi
}

# Execute script
main "$@"
