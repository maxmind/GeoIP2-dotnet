$ErrorActionPreference = 'Stop'
$DebugPreference = 'Continue'

$projectFile=(Get-Item "MaxMind.GeoIP2\MaxMind.GeoIP2.csproj").FullName
$matches = (Get-Content -Encoding UTF8 releasenotes.md) ` |
            Select-String '(\d+\.\d+\.\d+(?:-\w+)?) \((\d{4}-\d{2}-\d{2})\)' `

$version = $matches.Matches.Groups[1].Value
$date = $matches.Matches.Groups[2].Value

if((Get-Date -format 'yyyy-MM-dd')  -ne $date ) {
    Write-Error "$date is not today!"
}

$tag = "v$version"

if (& git status --porcelain) {
    Write-Error '. is not clean'
}

(Get-Content -Encoding UTF8 $projectFile) `
    -replace '(?<=<VersionPrefix>)[^<]+', $version ` |
  Out-File -Encoding UTF8 $projectFile

& git diff

if ((Read-Host -Prompt 'Continue? (y/n)') -ne 'y') {
    Write-Error 'Aborting'
}

& git commit -m "$version" -a

Push-Location MaxMind.GeoIP2

& dotnet restore
& dotnet build -c Release
& dotnet pack -c Release

Pop-Location

Push-Location MaxMind.GeoIP2.UnitTests

& dotnet restore
& dotnet test -c Release

Pop-Location

if ((Read-Host -Prompt 'Continue given tests? (y/n)') -ne 'y') {
    Write-Error 'Aborting'
}

& git push -u origin HEAD

if ((Read-Host -Prompt 'Should release? (y/n)') -ne 'y') {
    Write-Error 'Aborting'
}

& gh release create --target "$(git branch --show-current)" -t "$version" "$tag"

& nuget push "MaxMind.GeoIP2/bin/Release/MaxMind.GeoIP2.$version.nupkg" -Source https://www.nuget.org/api/v2/package
