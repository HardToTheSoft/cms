CMS Service

A .NET 10 service that ingests CMS events, stores them in SQLite, and exposes a REST API.

Prerequisites
.NET 10 SDK
SQLite (no separate install needed; included via EF Core provider)
Setup & Run
Clone the repo:
git clone https://github.com/hardtothesoft/cms.git
cd cms
Restore dependencies:
dotnet restore
Apply EF Core migrations:
dotnet ef database update
Run the service with a port of your choice and Development environment:
# Replace <PORT> with any available port
DOTNET_ENVIRONMENT=Development DOTNET_URLS=http://localhost:<PORT> dotnet run

Example: DOTNET_ENVIRONMENT=Development DOTNET_URLS=http://localhost:5050 dotnet run

Authentication

All endpoints require Basic Authentication. Usernames are prefixed with their role:

Role	Username format	Password format	Notes
CMS	cms_<string> (10–20 chars)	valid GUID	Used only for /cms/events
Admin	admin_<string> (10–20 chars)	valid GUID	Can access admin-only endpoint /disable.json
User	user_<string> (10–20 chars)	valid GUID	Can access /api/entities.json and /api/entities/{id}.json
API Endpoints (All return JSON)
CMS Webhook: POST /cms/events (Basic Auth: CMS only)
List all entities: GET /api/entities.json (Basic Auth: admin or user)
Get single entity: GET /api/entities/{id}.json (Basic Auth: admin or user)
Disable entity (Admin-only): POST /api/entities/{id}/disable.json (Basic Auth: admin only)

Swagger UI: http://localhost:<PORT>/swagger/index.html

Example: http://localhost:5050/swagger/index.html

Tests

Run all tests in the cms.tests project:

dotnet test cms.tests
Viewing the SQLite Database
DB Browser for SQLite
– free, open-source, cross-platform GUI
Open cms.db to browse tables and query entity versions

Other options:

SQLiteStudio – lightweight GUI
VS Code + SQLite extension – browse/query DB directly from VS Code