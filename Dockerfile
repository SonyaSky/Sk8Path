# Этап сборки
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# 1. Логируем начальную структуру
RUN echo "=== INITIAL STRUCTURE ===" && ls -la

# 2. Копируем solution и проект
COPY ["Sk8Path.sln", "./"]
COPY ["api/api.csproj", "./api/"]
RUN echo "=== AFTER COPYING SOLUTION/PROJECT ===" && ls -laR

# 3. Восстанавливаем зависимости
RUN dotnet restore "Sk8Path.sln"

# 4. Копируем весь исходный код
COPY . .
RUN echo "=== AFTER COPYING ALL SOURCES ===" && ls -laR

# 5. Публикуем проект
WORKDIR "/src/api"
RUN dotnet publish "api.csproj" -c Release -o /app/publish \
    && echo "=== PUBLISH OUTPUT ===" \
    && ls -la /app/publish

# Финальный этап
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# 6. Копируем опубликованные файлы
COPY --from=build /app/publish .
RUN echo "=== FINAL APP FOLDER ===" && ls -la

ENTRYPOINT ["dotnet", "api.dll"]