FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

COPY ["Tasker.Api/Tasker.Api.csproj", "Tasker.Api/"]
COPY ["Tasker.Application/Tasker.Application.csproj", "Tasker.Application/"]
COPY ["Tasker.Domain/Tasker.Domain.csproj", "Tasker.Domain/"]
COPY ["Tasker.Infrastructure/Tasker.Infrastructure.csproj", "Tasker.Infrastructure/"]

RUN dotnet restore "Tasker.Api/Tasker.Api.csproj"

COPY . .
WORKDIR "/src/Tasker.Api"
RUN dotnet build "Tasker.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Tasker.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Tasker.Api.dll"]
