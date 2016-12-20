#!/usr/bin/env bash

set -exu

pushd "$(dirname "$0")/.."

if [ -n "${DOTNETCORE:-}" ]; then

  echo Using .NET CLI

  dotnet restore

  dotnet build -f netstandard1.4 -c "$CONFIGURATION" ./GeoIP2

  # Running Benchmark
  dotnet run -f netcoreapp1.0 -c "$CONFIGURATION"  -p ./GeoIP2.Benchmark

  # Running Unit Tests
  dotnet test -f netcoreapp1.0 -c "$CONFIGURATION" ./GeoIP2.UnitTests

else

  echo Using Mono

  pushd mono

  nuget restore

  xbuild /p:Configuration=$CONFIGURATION

  mono packages/NUnit.ConsoleRunner.3.4.1/tools/nunit3-console.exe --where "cat != BreaksMono" ./bin/$CONFIGURATION/MaxMind.GeoIP2.UnitTests.dll

  popd
fi

popd
