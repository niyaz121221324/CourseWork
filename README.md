# FreightFlow

**FreightFlow** - это приложение для управления грузоперевозками, построенное на платформе ASP.NET Core. Оно использует Docker Compose для контейнеризации, а также PostgreSQL для хранения данных.

## Описание

Проект **FreightFlow** предоставляет решения для отслеживания и управления процессами грузоперевозок. Система включает в себя такие функции, как управление заказами, планирование маршрутов, отслеживание статусов и многое другое.

## Требования

Перед началом работы убедитесь, что на вашем компьютере установлены следующие компоненты:

- [Docker](https://www.docker.com/get-started)
- [Docker Compose](https://docs.docker.com/compose/)
- .NET SDK 6.0 и выше
- PostgreSQL (при необходимости)

## Установка

1. Клонируйте репозиторий:

    ```bash
    git clone https://github.com/your-username/FreightFlow.git
    cd FreightFlow
    ```

2. Построение и запуск контейнеров с помощью Docker Compose:

    ```bash
    docker-compose up --build
    ```

    Эта команда создаст и запустит необходимые контейнеры, включая:

    - **FreightFlow API** (ASP.NET Core)
    - **PostgreSQL** (для хранения данных)
    - **FreightFlow Client** (Blazor WebAssembly)
    - **Ngrok** (для доступа к сервису из сети интернет)

3. Приложение будет доступно по адресам:

    ```text
    http://localhost:3000
    ```
    ```text
    https://glider-dear-hog.ngrok-free.app
    ```

## Структура проекта

- `FreightFlow.Api/` - Основной код API, написанный на ASP.NET Core.
- `Dockerfile` - Файл для сборки контейнера с приложением.
- `docker-compose.yml` - Конфигурация для Docker Compose, описывающая сервисы и их настройки.
- `blazor-client` - Клиентская часть приложения

## Конфигурация базы запуска проекта
```yml
services:
  postgres:
    image: postgres:15  
    container_name: postgres_db
    restart: always
    environment:
      POSTGRES_USER: admin          
      POSTGRES_PASSWORD: admin  
      POSTGRES_DB: cargo_transportation_system 
    ports:
      - "5433:5432"                     
    volumes:
      - postgres_data:/var/lib/postgresql/data
    networks:
      - app_network
 
  freightflow.api:
    image: ${DOCKER_REGISTRY-}freightflowapi
    container_name: freightflow.api
    build:
      context: .
      dockerfile: FreightFlow.API/Dockerfile
    depends_on:
      - postgres
    networks:
      - app_network

  blazor-client:
    image: ${DOCKER_REGISTRY-}freightflowclient
    container_name: freightflowclient
    build:
      context: ./blazor-client
      dockerfile: Dockerfile
    ports:
      - "3000:80"
    depends_on:
      - freightflow.api
    networks:
      - app_network
    
  ngrok:
    image: ngrok/ngrok:latest
    container_name: ngrok
    restart: unless-stopped
    command:
      - "start"
      - "--all"
      - "--config"
      - "/etc/ngrok.yml"
    volumes:
      - ./ngrok.yml:/etc/ngrok.yml:ro
    depends_on:
      - blazor-client
    ports:
      - 4040:4040
    networks:
      - app_network

volumes:
  postgres_data:
    driver: local  

networks:
  app_network:
    driver: bridge
```
Также необходимо после запуска compose нужно подключится к контейнеру бд и запустить скрипт
```sql
-- Таблица пользователей
CREATE TABLE Users (
    UserID SERIAL PRIMARY KEY,
    FirstName VARCHAR(50) NOT NULL,
    LastName VARCHAR(50) NOT NULL,
    Role VARCHAR(20) CHECK (Role IN ('Client', 'Driver', 'Admin', 'Dispatcher')) NOT NULL,
    Login VARCHAR(50) UNIQUE NOT NULL,
    PasswordHash VARCHAR(255) NOT NULL,
    Phone VARCHAR(15),
    Email VARCHAR(100)
);

-- Таблица клиентов
CREATE TABLE Clients (
    ClientID SERIAL PRIMARY KEY,
    CompanyName VARCHAR(100),
    INN VARCHAR(12),
    ContactPerson VARCHAR(100),
    Phone VARCHAR(15),
    Email VARCHAR(100),
    Address TEXT
);

-- Таблица грузов
CREATE TABLE Cargo (
    CargoID SERIAL PRIMARY KEY,
    Name VARCHAR(100) NOT NULL,
    Type VARCHAR(20) CHECK (Type IN ('Dangerous', 'Perishable', 'Oversized', 'Standard')) NOT NULL,
    Weight NUMERIC(10, 2) NOT NULL, -- Вес в кг
    Volume NUMERIC(10, 2), -- Объем в м³
    SpecialRequirements TEXT
);

-- Таблица транспортных средств
CREATE TABLE Vehicles (
    VehicleID SERIAL PRIMARY KEY,
    VehicleNumber VARCHAR(20) NOT NULL,
    VehicleType VARCHAR(20) CHECK (VehicleType IN ('Truck', 'Van', 'Tank', 'Other')) NOT NULL,
    LoadCapacity NUMERIC(10, 2) NOT NULL, -- Грузоподъемность в тоннах
    Volume NUMERIC(10, 2), -- Объем кузова в м³
    Status VARCHAR(20) CHECK (Status IN ('Available', 'In Repair', 'On Trip')) DEFAULT 'Available'
);

-- Таблица маршрутов
CREATE TABLE Routes (
    RouteID SERIAL PRIMARY KEY,
    StartPoint VARCHAR(100) NOT NULL,
    EndPoint VARCHAR(100) NOT NULL,
    IntermediatePoints TEXT, -- Промежуточные точки, например, через запятую
    Distance NUMERIC(10, 2) NOT NULL, -- Расстояние в км
    Description TEXT
);

-- Таблица заказов
CREATE TABLE Orders (
    OrderID SERIAL PRIMARY KEY,
    ClientID INT NOT NULL REFERENCES Clients(ClientID) ON DELETE CASCADE,
    CargoID INT NOT NULL REFERENCES Cargo(CargoID) ON DELETE CASCADE,
    RouteID INT NOT NULL REFERENCES Routes(RouteID) ON DELETE CASCADE,
    VehicleID INT REFERENCES Vehicles(VehicleID) ON DELETE SET NULL,
    DriverID INT REFERENCES Users(UserID) ON DELETE SET NULL,
    OrderDate DATE NOT NULL,
    DeliveryDate DATE,
    Status VARCHAR(20) CHECK (Status IN ('New', 'In Progress', 'Completed', 'Cancelled')) DEFAULT 'New'
);

-- Таблица водителей
CREATE TABLE Drivers (
    DriverID SERIAL PRIMARY KEY,
    FirstName VARCHAR(50) NOT NULL,
    LastName VARCHAR(50) NOT NULL,
    Phone VARCHAR(15),
    LicenseNumber VARCHAR(50) NOT NULL,
    LicenseExpiry DATE NOT NULL,
    Status VARCHAR(20) CHECK (Status IN ('Available', 'On Trip', 'On Break')) DEFAULT 'Available'
);

-- Таблица рейсов
CREATE TABLE Trips (
    TripID SERIAL PRIMARY KEY,
    OrderID INT NOT NULL REFERENCES Orders(OrderID) ON DELETE CASCADE,
    VehicleID INT NOT NULL REFERENCES Vehicles(VehicleID) ON DELETE CASCADE,
    DriverID INT NOT NULL REFERENCES Users(UserID) ON DELETE CASCADE,
    StartDate DATE NOT NULL,
    EndDate DATE,
    Status VARCHAR(20) CHECK (Status IN ('In Progress', 'Completed')) DEFAULT 'In Progress'
);

-- Добавляем администратора в таблицу Users
INSERT INTO Users (FirstName, LastName, Role, Login, PasswordHash)
VALUES ('Admin', 'System', 'Admin', 'admin', 'admin123hash');
```
