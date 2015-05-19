To publish the to NuGet:

1. Update release notes.
2. Bump AssemblyVersion and AssemblyFileVersion.
3. Commit/push any changes. Wait for CI.
4. Build solution.
5. From the GeoIP2 subdirectory:
   1. nuget pack GeoIP2.csproj
   2. nuget push GeoIP2.<version>.nupkg
6. Run dev-bin/release.sh <tag> from a unix machine. This will generate docs
   and create the tag. The tag should be prefixed with "v".
7. Update GitHub Release page for the release.
