#!/usr/bin/env bash

set -exu

pushd "$(dirname "$0")/.."

if [ -n "${DOTNETCORE:-}" ]; then

  echo Using .NET CLI

  if [[ "$TRAVIS_OS_NAME" == "osx" ]]; then
    # This is due to: https://github.com/NuGet/Home/issues/2163#issue-135917905
    echo "current ulimit is: `ulimit -n`..."
    ulimit -n 1024
    echo "new limit: `ulimit -n`"
  fi

  dotnet restore

  dotnet build -f netstandard1.4 -c "$CONFIGURATION" ./GeoIP2

  # Running Benchmark
  dotnet run -f netcoreapp1.0 -c "$CONFIGURATION"  -p ./GeoIP2.Benchmark/GeoIP2.Benchmark.csproj

  # Running Unit Tests
  dotnet test -f netcoreapp1.0 -c "$CONFIGURATION" ./GeoIP2.UnitTests/GeoIP2.UnitTests.csproj

else

  echo Using Mono

  nuget restore

  xbuild /p:Configuration=$CONFIGURATION mono/GeoIP2.sln

  mono ./packages/xunit.runner.console.2.2.0/tools/xunit.console.exe ./mono/bin/$CONFIGURATION/MaxMind.GeoIP2.UnitTests.dll

fi

popd
