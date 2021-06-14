FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build-env
WORKDIR /app

COPY *.csproj ./
RUN dotnet restore

COPY . .
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:3.1-buster-slim AS runtime

WORKDIR /app

COPY --from=build-env /app/out .

EXPOSE 80
EXPOSE 443

ENTRYPOINT ["dotnet", "KingdomApi.dll"]
