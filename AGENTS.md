# AGENTS.md

This file guides coding agents working in this repository.

## Build And Test Commands

```bash
dotnet restore
dotnet build
dotnet test tests/MongodbDotnetExample.Tests/MongodbDotnetExample.Tests.csproj
dotnet run --urls http://localhost:5000
```

This repository has an xUnit test suite plus the smoke checks below.

If port 5000 is already in use, stop the conflicting process or pass a different port via `--urls`; do not change the default port in source code.

After any change that affects runtime behavior (`Program.cs`, `Startup.cs`, `Controllers`, `Services`, or `Models`), start the server and run the two curl commands below. Both must return HTTP 200 before considering the task complete.

```bash
curl http://localhost:5000/health
curl http://localhost:5000/api/games
```

If the server fails to connect to MongoDB, verify the `.devcontainer` Atlas Local instance is running and that `GamesDatabaseSettings:ConnectionString` is set in app settings. Do not modify application code to bypass the connection failure.

## Style

Run `dotnet format` before completing any C# code change.

## Project Structure

- `Program.cs`, `Startup.cs`: ASP.NET Core host and middleware pipeline
- `Controllers/GamesController.cs`: REST routes for `Game` CRUD
- `Services/GamesService.cs`: MongoDB access layer and seed helper
- `Models/Game.cs`: MongoDB document model
- `Models/GamesDatabaseSettings.cs`: bound configuration schema
- `Models/GameSeedData.cs`: default dataset inserted when collection is empty
- `.devcontainer/`: local dev environment with Atlas Local
- `.github/workflows/ci.yml`: build and integration smoke checks
- `EDD.md`: MongoDB data model contract

## Configuration

Primary configuration (app settings):

- `GamesDatabaseSettings:ConnectionString` (example: `mongodb://localhost:27017`)
- `GamesDatabaseSettings:DatabaseName` (example: `GamesDB`)
- `GamesDatabaseSettings:GamesCollectionName` (example: `Games`)
- `StartupBehaviorSettings:SeedOnStartup` (`true` by default; set `false` to disable startup seeding)

Optional runtime override:

- `ASPNETCORE_ENVIRONMENT` (`Development`, `Production`, etc.)

## MongoDB Skills

Before performing MongoDB schema, query, aggregation, or index work, fetch the skill index from https://github.com/mongodb/agent-skills and use any skill whose description matches the task. If no matching skill is found, proceed without one and note this in your response.

## When To Use EDD.md

Use [EDD.md](./EDD.md) as the source of truth for the MongoDB data model in this repository.

Consult [EDD.md](./EDD.md) before making changes that touch:

- MongoDB collections, document structure, or field names
- ASP.NET Core controllers and service methods that read or write database records
- Validation, form fields, API payloads, or UI that depend on persisted data
- Schema documentation, Mermaid diagrams, or entity modeling discussions

If `EDD.md` and the C# model code disagree, treat `EDD.md` as authoritative and update the C# model to match, then call out the discrepancy in your response.
