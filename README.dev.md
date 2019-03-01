## Running Unit Tests on Linux

1. Install dotnet core, cli, and sdk
2. `MAXMIND_TEST_BASE_DIR="$PWD/GeoIP2.UnitTests" CONFIGURATION=Debug dev-bin`

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
6. Verify the release on [NuGet](https://www.nuget.org/packages/MaxMind.GeoIP2/).

## Using Docker

A Dockerfile is included to build an image that can assist in updating dependencies, running benchmarks and running tests.

A `Makefile` is also included to act as a task runner for the tasks mentioned above. Below are the targets and their descriptions.

```Makefile
# Initialize the repo by installing git submodules and building the `geoip2-dotnet` Docker image
make init

# Run benchmarks
make benchmark

# Run tests
make test

# Update dependencies
make update_dependencies
```