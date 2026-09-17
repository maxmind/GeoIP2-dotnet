## Running Unit Tests on Linux

1. Install dotnet core, cli, and sdk
2. `MAXMIND_TEST_BASE_DIR="$PWD/GeoIP2.UnitTests" CONFIGURATION=Debug dev-bin`

Web-service tests use injected HTTP handlers on modern .NET. The `net481` tests
keep a local WireMock server because the legacy synchronous transport uses
`HttpWebRequest` and bypasses those handlers.

## Running NativeAOT Integration Tests

Run the integration test on the platform matching the runtime identifier:

```sh
bash dev-bin/test-native-aot.sh linux-x64
```

Use `win-x64` on Windows or `osx-arm64` on Apple Silicon. The script packs the
library and restores each test application from that package in an isolated
cache. It publishes and runs:

- A small .NET 8 smoke test for database decoding, web-service JSON, and
  locales.
- The shared xUnit suite as NativeAOT executables for .NET 9 and .NET 10.

The xUnit AOT runner requires .NET 9 or later. The shared suite uses the normal
database, model, and web-service tests. Only the two tests of reflection-based
JSON deserialization are excluded. HTTP responses come from an injected handler,
so no service credentials or external requests are needed. A runtime test checks
that dynamic code and default JSON reflection are disabled. These switches alone
do not prove native execution. The script publishes and runs the native binaries
and requires at least 162 tests per native run to catch discovery failures.

The script requires the .NET 10 SDK and the platform's
[NativeAOT prerequisites](https://learn.microsoft.com/en-us/dotnet/core/deploying/native-aot/#prerequisites).

Both native test projects stay outside the solution. Ordinary restore and build
use a reference to the library project. Publishing requires the package version
supplied by the script so the native tests validate the packed library.

## JSON defaults workaround

The null-coalescing `init` accessors in the models and responses preserve
non-null defaults when JSON omits a property. System.Text.Json source generation
in versions 8-10 assigns `default(T)` to omitted `init` properties, overwriting
their initializers.
[dotnet/runtime#124650](https://github.com/dotnet/runtime/pull/124650) fixes
this by assigning those properties only when they appear in JSON.

As of September 15, 2026, the fix is in the .NET 11 release branch but absent
from System.Text.Json 10.0.12, the latest stable release. Version 11 is still a
prerelease, so we retain the workaround rather than introduce a prerelease
dependency. Our .NET 8-10 targets use the framework's serializer, and our .NET
Standard targets reference System.Text.Json 10.0.11. Updating that package
reference alone would not update the generator for all targets.

Revisit the workaround when a stable serializer containing the fix can be used
for every target. Validate omitted properties with generated metadata and
NativeAOT before removing guards. The guards also normalize explicit JSON `null`
to empty defaults, which the upstream fix does not do. Preserve that documented
behavior or make a separate compatibility decision. The six named backing fields
used by `WithLocales` also support the separate .NET 8 record cloning
workaround.

## Publishing to NuGet

1. Review open issues and PRs to see if any can easily be fixed, closed, or
   merged.
2. Bump copyright year in `README.md` and the `AssemblyInfo.cs` files, if
   necessary.
3. Review `releasenotes.md` for completeness and correctness. Update its release
   date.
4. Run dev-bin/release.sh. This will build the project, generate docs, upload to
   NuGet, and make a GitHub release.
5. Update GitHub Release page for the release.
6. Verify the release on
   [NuGet](https://www.nuget.org/packages/MaxMind.GeoIP2/).
