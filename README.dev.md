To publish the to NuGet:

1. Update release notes.
2. Bump `AssemblyFileVersion` and `AssemblyInformationalVersion` if in use.
  * Do _not_ bump the `AssemblyVersion` unless there has been a breaking
    change. We previously did this incorrectly. See [this StackOverflow
    question](http://stackoverflow.com/questions/64602/what-are-differences-between-assemblyversion-assemblyfileversion-and-assemblyin)
    for more information.
  * The `AssemblyInformationalVersionAttribute` is used to specify Semantic
    Versions not supported by the `AssemblyFileVersion` field. See the
    [NuGet Versioning documentation](https://docs.nuget.org/create/versioning#creating-prerelease-packages)
    for more information.
  * Due to incorrectly increasing the `AssemblyVersion`, it is at 2.3.0.0
    rather than 2.0.0.0. Ignore the temptation to increase it unless there
    is a breaking changes, which should only happen at 3.0.0 (or a
    pre-release of it).
3. Commit/push any changes. Wait for CI.
4. Build solution.
5. From the GeoIP2 subdirectory:
   1. nuget pack GeoIP2.csproj
   2. nuget push GeoIP2.<version>.nupkg
6. Run dev-bin/release.sh. This will generate docs and create the tag.
7. Update GitHub Release page for the release.
