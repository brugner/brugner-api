#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app
COPY *.sln .
COPY src/Brugner.API/*.csproj ./src/Brugner.API/
COPY tests/Brugner.API.Tests.Unit/*.csproj ./tests/Brugner.API.Tests.Unit/
RUN dotnet restore

## copy full solution over
COPY . .
RUN dotnet build
FROM build AS testrunner
WORKDIR /app/tests/Brugner.API.Tests.Unit
CMD ["dotnet", "test"]

# run the unit tests
FROM build AS test
WORKDIR /app/tests/Brugner.API.Tests.Unit
RUN dotnet test -c Release

# publish the API
FROM test AS publish
WORKDIR /app/src/Brugner.API
RUN dotnet publish -c Release -o out

# run the api
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime
WORKDIR /app
COPY --from=publish /app/src/Brugner.API/out ./
EXPOSE 80
ENTRYPOINT ["dotnet", "Brugner.API.dll"]