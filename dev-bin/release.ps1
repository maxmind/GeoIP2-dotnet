$ErrorActionPreference = 'Stop'
$DebugPreference = 'Continue'

$projectJsonFile=(Get-Item "GeoIP2/project.json").FullName
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

# Not using Powershell's built-in JSON support as that
# reformats the file.
(Get-Content -Encoding UTF8 $projectJsonFile) `
    -replace '(?<=version"\s*:\s*")[^"]+', $version ` |
  Out-File -Encoding UTF8 $projectJsonFile


& git diff

if ((Read-Host -Prompt 'Continue? (y/n)') -ne 'y') {
    Write-Error 'Aborting'
}

if (-Not(& git status --porcelain)) {
    & git add $projectJsonFile
    & git commit -m "Prepare for $version"
}

Push-Location GeoIP2

& dotnet restore
& dotnet build -c Release
& dotnet pack -c Release

Pop-Location

Push-Location GeoIP2.UnitTests

& dotnet restore
& dotnet test -c Release

Pop-Location

if ((Read-Host -Prompt 'Continue given tests? (y/n)') -ne 'y') {
    Write-Error 'Aborting'
}

if (Test-Path .gh-pages ) {
    Write-Debug "Updating .gh-pages"
    Push-Location .gh-pages
    & git pull
} else {
    Write-Debug "Checking out gh-pages in .gh-pages"
    & git clone -b gh-pages git@github.com:maxmind/GeoIP2-dotnet.git .gh-pages
    Push-Location .gh-pages
}

if (& git status --porcelain) {
    Write-Error '.gh-pages is not clean'
}
Pop-Location

$page = (Get-Item '.gh-pages\index.md').FullName


$pageHeader = @"
---
layout: default
title: MaxMind GeoIP2 .NET API
language: dotnet
version: $tag
---
"@

Remove-Item $page

# PowerShell write a BOM by default. GitHub Pages can't handle this.
$utf8NoBomEncoding = New-Object System.Text.UTF8Encoding($False)
[IO.File]::WriteAllLines($page, $pageHeader, $utf8NoBomEncoding)

Get-Content -Encoding UTF8 'README.md' | Out-File -Encoding UTF8 -Append $page
& MSBuild.exe .\geoip2.shfbproj /p:OutputPath=.gh-pages\doc\$tag

Push-Location .gh-pages

& git add doc/
& git commit -m "Updated for $tag" -a

if ((Read-Host -Prompt 'Should push? (y/n)') -ne 'y') {
    Write-Error 'Aborting'
}

& git push

Pop-Location
& git tag "$tag"
& git push
& git push --tags

& nuget push "GeoIP2/bin/Release/GeoIP2.$version.nupkg" -Source https://www.nuget.org/api/v2/package
