#!/usr/bin/env bash

set -euo pipefail

readonly native_aot_rid="${1:?usage: test-native-aot.sh <runtime-identifier>}"
# Use `pwd -W` under Git Bash so MAXMIND_TEST_BASE_DIR reaches the native test
# binary as a Windows path. MSYS converts command arguments, not environment
# variables. `pwd -W` fails on other shells, which then fall back to `pwd`.
repository_root="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && { pwd -W 2>/dev/null || pwd; })"
readonly repository_root
readonly library_project="$repository_root/MaxMind.GeoIP2/MaxMind.GeoIP2.csproj"
readonly app_project="$repository_root/MaxMind.GeoIP2.NativeAot/MaxMind.GeoIP2.NativeAot.csproj"

# Strip the CR that the native Windows dotnet writes, as $(...) removes only LF.
version_prefix="$(dotnet msbuild "$library_project" -getProperty:VersionPrefix -p:TargetFramework=net8.0 | tr -d '\r')"
readonly package_version="$version_prefix-aot-ci"

# Isolate both the feed and restore cache so repeated runs test the new package.
test_directory="$(mktemp -d)"
readonly test_directory
cleanup() {
    rm -rf "$test_directory"
}
trap cleanup EXIT

dotnet pack "$library_project" \
    --configuration Release \
    --output "$test_directory/packages" \
    -p:PackageVersion="$package_version"

dotnet publish "$app_project" \
    --configuration Release \
    --runtime "$native_aot_rid" \
    --output "$test_directory/publish" \
    -p:GeoIP2PackageVersion="$package_version" \
    -p:RestoreAdditionalProjectSources="$test_directory/packages" \
    -p:RestorePackagesPath="$test_directory/restore" \
    -p:RestoreNoCache=true

if [[ "$native_aot_rid" == win-* ]]; then
    "$test_directory/publish/MaxMind.GeoIP2.NativeAot.exe"
else
    "$test_directory/publish/MaxMind.GeoIP2.NativeAot"
fi

readonly tests_project="$repository_root/MaxMind.GeoIP2.UnitTests.NativeAot/MaxMind.GeoIP2.UnitTests.NativeAot.csproj"
export MAXMIND_TEST_BASE_DIR="$repository_root/MaxMind.GeoIP2.UnitTests"

for framework in net9.0 net10.0; do
    dotnet publish "$tests_project" \
        --configuration Release \
        --framework "$framework" \
        --runtime "$native_aot_rid" \
        --output "$test_directory/$framework" \
        -p:GeoIP2PackageVersion="$package_version" \
        -p:RestoreAdditionalProjectSources="$test_directory/packages" \
        -p:RestorePackagesPath="$test_directory/restore" \
        -p:RestoreNoCache=true

    if [[ "$native_aot_rid" == win-* ]]; then
        "$test_directory/$framework/MaxMind.GeoIP2.UnitTests.exe" --minimum-expected-tests 162
    else
        "$test_directory/$framework/MaxMind.GeoIP2.UnitTests" --minimum-expected-tests 162
    fi
done
