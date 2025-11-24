#!/bin/bash

set -eu -o pipefail

# Check that we're not on the main branch
current_branch=$(git branch --show-current)
if [ "$current_branch" = "main" ]; then
    echo "Error: Releases should not be done directly on the main branch."
    echo "Please create a release branch and run this script from there."
    exit 1
fi

# Fetch latest changes and check that we're not behind origin/main
echo "Fetching from origin..."
git fetch origin

if ! git merge-base --is-ancestor origin/main HEAD; then
    echo "Error: Current branch is behind origin/main."
    echo "Please merge or rebase with origin/main before releasing."
    exit 1
fi

changelog=$(cat releasenotes.md)

# GeoIP2-dotnet format: "5.4.0 (2025-11-20)" followed by "---"
regex='([0-9]+\.[0-9]+\.[0-9]+(-[a-zA-Z0-9]+)?) \(([0-9]{4}-[0-9]{2}-[0-9]{2})\)'

if [[ ! $changelog =~ $regex ]]; then
    echo "Could not find version/date in releasenotes.md!"
    exit 1
fi

version="${BASH_REMATCH[1]}"
date="${BASH_REMATCH[3]}"

# Extract release notes: everything after "---" line until next version header
notes=$(awk -v ver="$version" '
    $0 ~ "^" ver " \\(" { found=1; next }
    found && /^-+$/ { in_notes=1; next }
    in_notes && /^[0-9]+\.[0-9]+\.[0-9]+.* \([0-9]{4}-[0-9]{2}-[0-9]{2}\)/ { exit }
    in_notes { print }
' releasenotes.md | sed -e :a -e '/^\n*$/{$d;N;ba' -e '}')

if [[ "$date" != "$(date +"%Y-%m-%d")" ]]; then
    echo "$date is not today!"
    exit 1
fi

tag="v$version"

if [ -n "$(git status --porcelain)" ]; then
    echo ". is not clean." >&2
    exit 1
fi

# Update version in csproj
sed -i "s|<VersionPrefix>[^<]*</VersionPrefix>|<VersionPrefix>$version</VersionPrefix>|" MaxMind.GeoIP2/MaxMind.GeoIP2.csproj

# Build and test
dotnet build -c Release
dotnet test -c Release

echo $'\nDiff:'
git diff

echo $'\nRelease notes:'
echo "$notes"

read -e -p "Commit changes and create release? (y/n) " should_continue

if [ "$should_continue" != "y" ]; then
    echo "Aborting"
    exit 1
fi

git commit -m "Prepare for $version" -a

git push

gh release create --target "$(git branch --show-current)" -t "$version" -n "$notes" "$tag"
