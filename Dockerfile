FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

RUN echo "=== INITIAL STRUCTURE ===" && ls -la

COPY ["Sk8Path.sln", "./"]
COPY ["api/api.csproj", "./api/"]
RUN echo "=== AFTER COPYING SOLUTION/PROJECT ===" && ls -laR

RUN dotnet restore "Sk8Path.sln"

COPY . .
RUN echo "=== AFTER COPYING ALL SOURCES ===" && ls -laR

WORKDIR "/src/api"
RUN dotnet publish "api.csproj" -c Release -o /app/publish \
    && echo "=== PUBLISH OUTPUT ===" \
    && ls -la /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

COPY --from=build /app/publish .
RUN echo "=== FINAL APP FOLDER ===" && ls -la

ENTRYPOINT ["dotnet", "api.dll"]