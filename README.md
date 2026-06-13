# MamaFit Server

MamaFit Server is the backend API for the MamaFit maternity fashion platform. It manages authentication, user roles, maternity dress catalogs, custom design requests, measurements, appointments, carts, orders, payments, shipping, warranty flows, feedback, chat, notifications, analytics, and scheduled background jobs.

## Table of Contents

- [Tech Stack](#tech-stack)
- [Architecture](#architecture)
- [Features](#features)
- [Repository Structure](#repository-structure)
- [Prerequisites](#prerequisites)
- [Configuration](#configuration)
- [Getting Started](#getting-started)
- [Database Migrations](#database-migrations)
- [API Documentation](#api-documentation)
- [Real-time Hubs](#real-time-hubs)
- [Background Jobs](#background-jobs)
- [Docker](#docker)
- [CI/CD](#cicd)
- [Security Notes](#security-notes)
- [Development Guidelines](#development-guidelines)

## Tech Stack

- **Runtime:** .NET 8 / ASP.NET Core Web API
- **Database:** PostgreSQL with Entity Framework Core
- **Caching:** Redis
- **Background jobs:** Hangfire with PostgreSQL storage
- **Realtime:** SignalR
- **Authentication:** JWT bearer tokens
- **Validation:** FluentValidation
- **Mapping:** AutoMapper
- **Logging:** NLog
- **API docs:** Swagger / OpenAPI
- **File storage:** Cloudinary
- **Email:** Mailgun
- **Shipping:** Giao Hang Tiet Kiem (GHTK)
- **Payments:** Sepay QR and webhook integration
- **CMS integration:** Contentful
- **AI providers:** Groq and optional Ollama
- **Containerization:** Docker and Docker Compose

## Architecture

The solution follows a layered architecture:

```text
MamaFit.API
  Controllers, middleware, startup configuration, Swagger, auth, CORS, health checks

MamaFit.Services
  Business logic, validators, external integrations, SignalR hubs, background jobs

MamaFit.Repositories
  Generic repository, unit of work, query helpers, repository contracts and implementations

MamaFit.BusinessObjects
  EF Core DbContext, entities, DTOs, enums, seed data, migrations
```

Main runtime flow:

1. Requests enter `MamaFit.API` through REST controllers or SignalR hubs.
2. Controllers call service interfaces from `MamaFit.Services`.
3. Services coordinate validation, business rules, external services, cache, and repositories.
4. Repositories persist data through `ApplicationDbContext`.
5. Cross-cutting behavior is handled by middleware, JWT authentication, NLog, Redis, Hangfire, and Swagger.

## Features

- User registration, sign-in, OTP verification, Google login, refresh tokens, and logout
- Role-based system users for users, branch managers, designers, managers, staff, and admins
- Catalog management for categories, styles, components, component options, dresses, dress details, presets, add-ons, and sizes
- Customer measurements, pregnancy measurement diary, AI-assisted measurement calculation, and body-growth calculation
- Appointments with slot lookup, check-in, check-out, and cancellation
- Design requests, ready-to-buy orders, preset orders, order items, production tasks, milestones, and tickets
- Cart, voucher batches, voucher discounts, feedback, and warranty management
- Sepay payment QR generation, payment status lookup, and payment webhooks
- GHTK order submission, cancellation, fee calculation, tracking, labels, and address lookup
- Real-time chat and notification delivery through SignalR
- Transaction dashboard and revenue analytics endpoints
- Recurring jobs for appointment reminders and measurement generation
- Health endpoint for container monitoring

## Repository Structure

```text
.
|-- MamaFit.API/                 # ASP.NET Core API host
|   |-- Controllers/             # REST API controllers
|   |-- DependencyInjection/     # Service registration extensions
|   |-- Middlewares/             # Exception and permission middleware
|   |-- Properties/              # Launch profiles
|   |-- Program.cs               # Application entrypoint
|   `-- appsettings.json         # Base configuration template
|-- MamaFit.BusinessObjects/     # Entities, DTOs, enums, DbContext, migrations
|-- MamaFit.Repositories/        # Repository and unit-of-work layer
|-- MamaFit.Services/            # Business services, validators, hubs, integrations
|-- Dockerfile                   # Production image build
|-- docker-compose.yaml          # Deployment compose file
|-- docker-compose.build.yaml    # Compose build override
|-- filebeat.yml                 # Log shipping configuration
`-- MamaFit.sln                  # Visual Studio solution
```

## Prerequisites

- .NET SDK 8.x
- PostgreSQL 13 or newer
- Redis
- Docker and Docker Compose, if running with containers
- Optional external accounts:
  - Cloudinary
  - Mailgun
  - Sepay
  - GHTK
  - Contentful
  - Groq
  - Ollama, only when local AI provider is enabled

## Configuration

Configuration is read from `MamaFit.API/appsettings.json`, environment variables, user secrets, or `appsettings.Development.json`.

Do not commit real credentials. Prefer environment variables or .NET user secrets for local development.

### Required Settings

| Section | Key | Purpose |
| --- | --- | --- |
| `ConnectionStrings` | `DefaultConnection` | PostgreSQL connection used by EF Core |
| `ConnectionStrings` | `HangfireConnection` | PostgreSQL connection used by Hangfire |
| `JWT` | `Issuer`, `Audience`, `SecretKey` | JWT token validation |
| `RedisSettings` | `ConnectionString` | Redis cache and connection multiplexer |
| `Cloudinary` | `CloudName`, `ApiKey`, `ApiSecret` | Image upload and storage |
| `EmailSettings` | `ApiKey`, `ApiBaseUri`, `Domain`, `FromEmail`, `FromName` | Mailgun email sending |
| `SepaySettings` | `ApiKey`, `ApiBaseUri`, `AccountNumber`, `AccountName`, `BankCode` | QR payment integration |
| `GhtkSettings` | `ApiToken`, `BaseUri`, pickup fields | Shipping integration |
| `Contentful` | `SpaceId`, `ContentDeliveryKey`, `EntryId`, `SecretKey`, `ManagementToken` | CMS synchronization |
| `AI:Providers:Groq` | `ApiKey`, `Enabled`, `Model`, `MaxTokens` | Groq AI provider |
| `AI:Providers:Ollama` | `Enabled`, `BaseUrl`, `Model`, `FallbackModel` | Optional Ollama AI provider |

### Environment Variable Format

ASP.NET Core supports nested configuration with double underscores:

```bash
ConnectionStrings__DefaultConnection="Host=localhost;Database=mamafitdb;Username=postgres;Password=postgres"
ConnectionStrings__HangfireConnection="Host=localhost;Database=mamafitdb;Username=postgres;Password=postgres"
JWT__SecretKey="replace-with-a-long-random-secret"
RedisSettings__ConnectionString="localhost:6379"
Cloudinary__ApiKey="..."
Cloudinary__ApiSecret="..."
EmailSettings__ApiKey="..."
SepaySettings__ApiKey="..."
GhtkSettings__ApiToken="..."
Contentful__ManagementToken="..."
AI__Providers__Groq__ApiKey="..."
```

### User Secrets Example

From the repository root:

```bash
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=localhost;Database=mamafitdb;Username=postgres;Password=postgres" --project MamaFit.API
dotnet user-secrets set "ConnectionStrings:HangfireConnection" "Host=localhost;Database=mamafitdb;Username=postgres;Password=postgres" --project MamaFit.API
dotnet user-secrets set "JWT:SecretKey" "replace-with-a-long-random-secret" --project MamaFit.API
dotnet user-secrets set "RedisSettings:ConnectionString" "localhost:6379" --project MamaFit.API
```

## Getting Started

Restore dependencies:

```bash
dotnet restore
```

Build the solution:

```bash
dotnet build
```

Run the API locally:

```bash
dotnet run --project MamaFit.API
```

Default local URLs from `launchSettings.json`:

- HTTP: `http://localhost:5136`
- HTTPS: `https://localhost:7173`
- Swagger: `http://localhost:5136/swagger`
- Health check: `http://localhost:5136/health`
- Hangfire dashboard: `http://localhost:5136/hangfire`

Run tests:

```bash
dotnet test
```

> Note: no dedicated test project is currently present in the solution. `dotnet test` still validates buildable test targets if they are added later.

## Database Migrations

EF Core migrations live in `MamaFit.BusinessObjects/Migrations`.

Install the EF Core CLI if needed:

```bash
dotnet tool install --global dotnet-ef
```

Apply migrations:

```bash
dotnet ef database update \
  --project MamaFit.BusinessObjects \
  --startup-project MamaFit.API
```

Add a new migration:

```bash
dotnet ef migrations add <MigrationName> \
  --project MamaFit.BusinessObjects \
  --startup-project MamaFit.API \
  --output-dir Migrations
```

The design-time context factory expects `MamaFit.API/appsettings.Development.json` to provide `ConnectionStrings:DefaultConnection`.

Seed data creates baseline roles and sample system accounts during migration.

## API Documentation

Swagger is enabled for all environments.

- Local Swagger UI: `http://localhost:5136/swagger`
- Container Swagger UI: `http://localhost:8080/swagger`
- Production compose exposes the API on port `8080`

Core API groups include:

- `api/auth`
- `api/user`
- `api/role`
- `api/category`
- `api/style`
- `api/component`
- `api/component-option`
- `api/maternity-dress`
- `api/maternity-dress-detail`
- `api/preset`
- `api/measurement`
- `api/measurement-diary`
- `api/appointment`
- `api/cart-item`
- `api/order`
- `api/order-items`
- `api/order-item-tasks`
- `api/task`
- `api/milestone`
- `api/ticket`
- `api/feedback`
- `api/notification`
- `api/voucher-batch`
- `api/voucher-discount`
- `api/warranty-request`
- `api/warranty-request-item`
- `api/warranty-history`
- `api/transaction`
- `api/sepay-auth`
- `api/ai-test`

## Real-time Hubs

SignalR hubs are mapped at:

- `/chatHub`
- `/notificationHub`

JWT tokens can be supplied by SignalR clients through the `access_token` query string.

## Background Jobs

Hangfire is configured with PostgreSQL storage and starts a server with 5 workers. The dashboard is available at `/hangfire`.

Recurring jobs are registered at startup through `IRecurringJobScheduler`, including:

- appointment reminders
- measurement generation

## Docker

Build the image:

```bash
docker build -t mamafit-api .
```

Run the image:

```bash
docker run --rm -p 8080:8080 \
  -e ASPNETCORE_ENVIRONMENT=Production \
  -e ConnectionStrings__DefaultConnection="Host=host.docker.internal;Database=mamafitdb;Username=postgres;Password=postgres" \
  -e ConnectionStrings__HangfireConnection="Host=host.docker.internal;Database=mamafitdb;Username=postgres;Password=postgres" \
  -e JWT__SecretKey="replace-with-a-long-random-secret" \
  -e RedisSettings__ConnectionString="host.docker.internal:6379" \
  mamafit-api
```

Using Compose for deployment:

```bash
docker compose -f docker-compose.yaml -f docker-compose.build.yaml up -d --build
```

The production compose file expects a `.env` file with values such as:

```env
REGISTRY_NAME=
VPS_HOST=
DB_NAME=
DB_USER=
DB_PASSWORD=
JWT_SECRET=
CLOUDINARY_API_KEY=
CLOUDINARY_API_SECRET=
MAILGUN_API_KEY=
MAILGUN_DOMAIN=
REDIS_CONN=
SEPAY_API_KEY=
SEPAY_API_BASE_URI=
SEPAY_ACCOUNT_NUMBER=
SEPAY_ACCOUNT_NAME=
SEPAY_BANK_CODE=
GHTK_API_TOKEN=
GHTK_BASE_URI=
CONTENTFUL_SPACE_ID=
CONTENT_DELIVERY_KEY=
CONTENTFUL_ENTRY_ID=
CONTENTFUL_SECRET_KEY=
CONTENTFUL_MANAGEMENT_TOKEN=
GROQ_API_KEY=
```

Container details:

- API listens on `http://+:8080`
- Health check calls `GET /health`
- Logs are mounted to `/data/compose/app-logs`
- `nginx-proxy-manager` is included in `docker-compose.yaml`

## CI/CD

GitHub Actions workflow: `.github/workflows/main.yml`

Pipeline on `main`:

1. Restore dependencies
2. Build solution in Release mode
3. Run `dotnet test`
4. Build and push Docker image
5. Copy compose files to VPS
6. Generate deployment `.env` from GitHub secrets
7. Pull and recreate the `mamafit-api` container

## Security Notes

- Never commit real API keys, database passwords, JWT secrets, or provider tokens.
- Keep `appsettings.Development.json` local only.
- Rotate any secret that was committed, shared, logged, or exposed outside a trusted local environment.
- Use a long random value for `JWT:SecretKey`.
- Protect `/hangfire` before public production exposure. The current dashboard authorization filter allows all access.
- Review CORS before production exposure. The current policy allows any origin.
- Store production settings in GitHub Actions secrets, Docker secrets, cloud secret managers, or secured environment variables.

## Development Guidelines

- Keep controllers thin; place business behavior in `MamaFit.Services`.
- Add validation with FluentValidation in `MamaFit.Services/Validator`.
- Access persistence through repositories and `IUnitOfWork`.
- Add new DTOs under `MamaFit.BusinessObjects/DTO`.
- Add new entities under `MamaFit.BusinessObjects/Entity` and configure relationships in `ApplicationDbContext`.
- Add EF Core migrations after schema changes.
- Register new services and repositories in `ApplicationServiceExtension`.
- Prefer strongly typed options classes for external service settings.
- Keep Swagger annotations and response contracts current when adding endpoints.
