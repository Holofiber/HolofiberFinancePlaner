# Holofiber Finance Planner

Enterprise-style фінансовий планувальник на базі **ASP.NET Core Web API** з архітектурою **Clean Architecture + CQRS**.

## Структура рішення

- `FinancialPlanner.Domain` — сутності та бізнес-інваріанти.
- `FinancialPlanner.Application` — CQRS (MediatR), валідація (FluentValidation), абстракції доступу до даних.
- `FinancialPlanner.Infrastructure` — EF Core/PostgreSQL, JWT, BCrypt, репозиторії, DI-реєстрація.
- `FinancialPlanner.Api` — контролери, middleware, ProblemDetails, Serilog.
- `FinancialPlanner.Contracts` — контракти запитів/відповідей для клієнтів.

## Ключові можливості

- JWT автентифікація та refresh-токени.
- Реєстрація/логін користувача.
- Створення та читання витрат.
- Централізована обробка помилок (ProblemDetails).
- Структуроване логування через Serilog з CorrelationId.
- Dockerized запуск із PostgreSQL.

## Локальний запуск (Docker)

```bash
docker compose up --build
```

API буде доступний на `http://localhost:8080`.

## Налаштування

Конфігурація в `FinancialPlanner.Api/appsettings.json`:

- `ConnectionStrings:DefaultConnection`
- `Jwt:Secret`
- `Jwt:Issuer`
- `Jwt:Audience`

> В production замініть `Jwt:Secret` на довгий випадковий ключ і передавайте через environment variables.

## Наступні кроки

- Додати EF Core migrations.
- Додати refresh-token rotation та revoke.
- Покрити Application unit-тестами й API integration-тестами.
