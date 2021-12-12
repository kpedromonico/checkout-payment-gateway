FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Services/Bank/Bank.API/Bank.API.csproj", "Services/Bank/Bank.API/"]
RUN dotnet restore "Services/Bank/Bank.API/Bank.API.csproj"
COPY . .
WORKDIR "/src/Services/Bank/Bank.API"
RUN dotnet build "Bank.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Bank.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Bank.API.dll"]