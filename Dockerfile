FROM microsoft/dotnet:2.1-sdk

ENV PATH="${PATH}:~/.dotnet/tools"

RUN dotnet tool install --global dotnet-outdated

WORKDIR /project
