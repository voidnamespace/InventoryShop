
# InventoryShop API

API для управления товарами, заказами и пользователями магазина.

## Описание проекта
**InventoryShop** — учебный проект на ASP.NET Core с использованием EF Core и JWT аутентификации.  
Проект реализует CRUD для товаров, заказов и пользователей, поддерживает юнит-тесты и структуру для расширения функционала.

## Структура проекта

InventoryShop.sln          # Решение
src/                       # Основной проект API
tests/                     # Юнит-тесты
.gitignore                  # Игнорируемые файлы
README.md                  # Этот файл

### src/
- `InventoryShop.Api/` — проект Web API
  - `Program.cs` — точка входа
  - `appsettings.json` — настройки приложения
  - `Properties/launchSettings.json` — настройки запуска
  - `bin/` и `obj/` — автоматически создаются сборкой
  - Сущности, DTO, контроллеры и сервисы проекта

### tests/
- `InventoryShop.Tests/` — тесты
  - Unit-тесты для сервисов и контроллеров

## Стек технологий
- .NET 9
- ASP.NET Core Web API
- Entity Framework Core (SQLite)
- JWT аутентификация
- xUnit для тестирования
- Git + GitHub

## Как начать проект

1. Клонировать репозиторий:
```bash
git clone <URL_REPO>
cd InventoryShop
```

2. Сборка проекта:
```bash
dotnet build
```

3. Запуск API:
```bash
dotnet run --project src/InventoryShop.Api
```

4. Запуск тестов:
```bash
dotnet test tests/InventoryShop.Tests
```

## Пошаговая шпаргалка разработки

1. Создать solution:
```bash
dotnet new sln -n InventoryShop
```

2. Создать проекты:
```bash
dotnet new webapi -o src/InventoryShop.Api
dotnet new xunit -o tests/InventoryShop.Tests
```

3. Добавить проекты в solution:
```bash
dotnet sln add src/InventoryShop.Api/InventoryShop.Api.csproj
dotnet sln add tests/InventoryShop.Tests/InventoryShop.Tests.csproj
```

4. Подключить ссылку на API проект в тестах:
```bash
cd tests/InventoryShop.Tests
dotnet add reference ../../src/InventoryShop.Api/InventoryShop.Api.csproj
```

5. Настроить `.gitignore` для Visual Studio и C#

6. Создать пустой `Program.cs` и настроить DI, сервисы, логирование

7. Создавать сущности и таблицы через EF Core

8. Реализовать CRUD и HTTP методы

9. Параллельно добавлять юнит-тесты

10. Создавать ветки для каждой фичи и делать коммиты с осмысленными сообщениями

## Ветки и пул-реквесты

- `main` — основная ветка
- `feature/<название>` — новые фичи  

Делать пул-реквест, когда фича полностью рабочая (с сервисами, логированием и тестами)
