# Этап 1: Сборка Blazor WebAssembly приложения
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Копируем csproj и восстанавливаем зависимости
COPY blazor-client.csproj ./
RUN dotnet restore blazor-client.csproj 

# Копируем остальной код и собираем приложение
COPY . ./
RUN dotnet publish blazor-client.csproj -c Release -o /app/publish

# Этап 2: Настройка NGINX для обслуживания приложения
FROM nginx:alpine AS final
WORKDIR /usr/share/nginx/html

# Удаляем стандартный конфиг NGINX
RUN rm /etc/nginx/conf.d/default.conf

# Копируем собственный конфиг NGINX
COPY nginx.conf /etc/nginx/conf.d/

# Копируем собранное приложение в папку NGINX
COPY --from=build /app/publish/wwwroot .

# Открываем порт 80
EXPOSE 80

# Запуск NGINX
CMD ["nginx", "-g", "daemon off;"]