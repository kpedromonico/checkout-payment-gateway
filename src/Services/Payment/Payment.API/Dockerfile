FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src

COPY ["Services/Payment/Payment.API/Payment.API.csproj", "Services/Payment/Payment.API/"]
COPY ["Services/Payment/Payment.Infrastructure/Payment.Infrastructure.csproj", "Services/Payment/Payment.Infrastructure/"]
COPY ["Services/Payment/Payment.Domain/Payment.Domain.csproj", "Services/Payment/Payment.Domain/"]
RUN dotnet restore "Services/Payment/Payment.API/Payment.API.csproj"

COPY . .
WORKDIR "/src/Services/Payment/Payment.API"
RUN dotnet build "Payment.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Payment.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Payment.API.dll"]