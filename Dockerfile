FROM mcr.microsoft.com/dotnet/core/sdk:2.1 AS build-env
WORKDIR /app

COPY . ./
RUN dotnet restore
RUN dotnet build

FROM build-env AS testrunner
WORKDIR /app
ENTRYPOINT ["dotnet", "test", "--logger:trx"]

FROM build-env as test
WORKDIR /app
RUN dotnet test

FROM build-env as publish
WORKDIR /app
RUN dotnet publish -c Release -o out



FROM mcr.microsoft.com/dotnet/core/aspnet:2.1
WORKDIR /app
COPY --from=publish /app/NServiceBusCore/out .
ENTRYPOINT ["dotnet", "NServiceBusCore.dll"]
