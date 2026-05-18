# CMS Service

A .NET 10 service that ingests CMS events, stores them in SQLite, and exposes a REST API.

## Prerequisites

- .NET 10 SDK
- SQLite (no separate install needed; included via EF Core provider)

## Setup & Run

1. Clone the repo:

```bash
git clone https://github.com/hardtothesoft/cms.git
cd cms
```

2. Restore dependencies:

```bash
dotnet restore
```

3. Apply EF Core migrations:

```bash
dotnet ef database update
```

4. Run the service with a port of your choice and Development environment:

```bash
# Replace <PORT> with any available port
DOTNET_ENVIRONMENT=Development DOTNET_URLS=http://localhost:<PORT> dotnet run
```

Example:

```bash
DOTNET_ENVIRONMENT=Development DOTNET_URLS=http://localhost:5050 dotnet run
```

## Authentication

All endpoints require **Basic Authentication**.

Usernames are prefixed with their role:

| Role  | Username format              | Password format | Notes                                                         |
| ----- | ---------------------------- | --------------- | ------------------------------------------------------------- |
| CMS   | cms_<string> (10–20 chars)   | valid GUID      | Used only for `/cms/events`                                   |
| Admin | admin_<string> (10–20 chars) | valid GUID      | Can access admin-only endpoint `/disable.json`                |
| User  | user_<string> (10–20 chars)  | valid GUID      | Can access `/api/entities.json` and `/api/entities/{id}.json` |

## API Endpoints (All return JSON)

| Endpoint                          | Method | Auth          | Notes                       |
| --------------------------------- | ------ | ------------- | --------------------------- |
| `/cms/events`                     | POST   | CMS only      | CMS Webhook                 |
| `/api/entities.json`              | GET    | admin or user | List all entities           |
| `/api/entities/{id}.json`         | GET    | admin or user | Get single entity           |
| `/api/entities/{id}/disable.json` | POST   | admin only    | Disable entity (Admin-only) |
| `/swagger/index.html`             | GET    | None          | Swagger UI                  |


## Swagger UI

```bash
http://localhost:<PORT>/swagger/index.html
```

Example:

```bash
http://localhost:5050/swagger/index.html
```

## Tests

Run all tests in the cms.tests project:

```bash
dotnet test cms.tests
```

## Viewing the SQLite Database

DB Browser for SQLite
– free, open-source, cross-platform GUI
Open cms.db to browse tables and query entity versions