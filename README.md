# Preparing

## Project Settings

- Add below params at Player > Other Settings > Scripting Define Symbols.
  - `UNITASK_DOTWEEN_SUPPORT` at Player > Other Settings > Scripting Define Symbols.
  - `DOTWEEN` at Player > Other Settings > Scripting Define Symbols.
  - `STEAMWORKS_NET` at Player > Other Settings > Scripting Define Symbols.

## manifest.json

- Copy `manifest.json`.

## Package Manager

- Install below packages
  - DOTween Pro
  - Rainbow Folders 2
  - Text Animator for Unity

## TansanMilMilUtil

import from below git URL in Unity Package Manager

`https://github.com/TansanMilMil/template-unity-project.git?path=Packages/TansanMilMilUtil`

---

# Release Process (TansanMilMilUtil Package)

This section describes how to release a new version of the TansanMilMilUtil package.

## Automated Release with Shell Script

The package uses an automated release script for version management.

### Prerequisites

- `jq` command-line JSON processor must be installed
  ```bash
  # Ubuntu/Debian
  sudo apt-get install jq

  # macOS
  brew install jq
  ```
- Clean working directory (no uncommitted changes)
- Push access to the repository

### Release Steps

1. Run the release script with the new version number:
   ```bash
   ./scripts/release.sh <version>
   ```

   Example:
   ```bash
   ./scripts/release.sh 1.6.0
   ```

2. The script will:
   - Validate the version format
   - Check for uncommitted changes
   - Update `package.json` (version and changelogUrl)
   - Create a commit with message `chore: release v{version}`
   - Create tag `util_v{version}`
   - Move `util_latest` tag to the latest commit
   - Ask for confirmation before pushing

3. After pushing, GitHub Actions will automatically:
   - Create a GitHub Release
   - Generate changelog from commit history
   - Publish the release

### Version Naming Convention

This package follows Semantic Versioning (SemVer):
- **MAJOR** version for incompatible API changes
- **MINOR** version for backward-compatible new features
- **PATCH** version for backward-compatible bug fixes

Format: `X.Y.Z` (e.g., `1.6.0`)

Pre-release versions are also supported: `X.Y.Z-prerelease` (e.g., `1.6.0-beta.1`)

### Troubleshooting

**Error: "Working directory is not clean"**
- Commit or stash your changes before running the release script

**Error: "Tag already exists"**
- The version you're trying to release already exists
- Check existing tags: `git tag -l "util_v*"`

**Error: "jq: command not found"**
- Install jq (see Prerequisites section above)

**Release workflow not triggered**
- Ensure the tag matches pattern `util_v*`
- Check GitHub Actions tab for workflow status

### Manual Release (Legacy)

If you need to release manually for any reason:

1. Edit `Packages/TansanMilMilUtil/package.json`:
   - Update `version` field
   - Update `changelogUrl` to new release tag URL

2. Commit changes:
   ```bash
   git add Packages/TansanMilMilUtil/package.json
   git commit -m "chore: release v{version}"
   ```

3. Create and push tags:
   ```bash
   git tag util_v{version}
   git tag -f util_latest
   git push origin main
   git push origin util_v{version}
   git push origin util_latest --force
   ```

4. Create GitHub Release manually from the web interface
