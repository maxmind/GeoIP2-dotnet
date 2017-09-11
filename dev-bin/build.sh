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

  dotnet restore ./MaxMind.GeoIP2.sln

  # Running Benchmark
  dotnet run -f $CONSOLE_FRAMEWORK -c "$CONFIGURATION"  -p ./MaxMind.GeoIP2.Benchmark/MaxMind.GeoIP2.Benchmark.csproj

  # Running Unit Tests
  dotnet test -f $CONSOLE_FRAMEWORK -c "$CONFIGURATION" ./MaxMind.GeoIP2.UnitTests/MaxMind.GeoIP2.UnitTests.csproj

else

  echo Using Mono

  msbuild /t:restore ./MaxMind.GeoIP2.sln

  msbuild /t:build /p:Configuration=$CONFIGURATION /p:TargetFramework=net452 ./MaxMind.GeoIP2.UnitTests/MaxMind.GeoIP2.UnitTests.csproj

  nuget install xunit.runner.console -ExcludeVersion -Version 2.2.0 -OutputDirectory .
  mono ./xunit.runner.console/tools/xunit.console.exe ./MaxMind.GeoIP2.UnitTests/bin/$CONFIGURATION/net452/MaxMind.GeoIP2.UnitTests.dll

fi

popd
