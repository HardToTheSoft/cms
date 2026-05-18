# CMS Service

A .NET 10 service that ingests CMS events, stores them in SQLite, and exposes a REST API.

## Prerequisites

- .NET 10 SDK
- SQLite (no separate install needed; included via EF Core provider)

## Setup & Run

Clone the repo:

```bash
git clone https://github.com/hardtothesoft/cms.git
cd cms

Restore dependencies:

```bash
dotnet restore

Apply EF Core migrations:

```bash
dotnet ef database update

Run the service with a port of your choice and Development environment:

```bash
# Replace <PORT> with any available port
DOTNET_ENVIRONMENT=Development DOTNET_URLS=http://localhost:<PORT> dotnet run

Example:

```bash
DOTNET_ENVIRONMENT=Development DOTNET_URLS=http://localhost:5050 dotnet run