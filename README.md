# Trackunit Media Access Service

![CI](https://github.com/Team-2-Devs/tu-media-access-service/actions/workflows/ci.yml/badge.svg)

Media Access microservice for Trackunit.

## Status

* Active Development

## Purpose

* Request short-lived presigned **GET** URLs from the Storage service
* Expose a stable internal API so downstream services never call Storage directly
* Forward requests to Storage using a typed HttpClient
* Require Storage’s internal access token for service-to-service calls

## Endpoints (v0)

*Note: endpoints describe the intended interface. Only implemented endpoints are noted.*

* **POST** `/internal/v0/media/get-url` – return a presigned GET URL for one object
* **POST** `/internal/v0/media/get-url-batch` – return presigned GET URLs for multiple objects
* **GET** `/health` – service health check

## Tech

* .NET 8, ASP.NET Core Web API
* Clean/hexagonal layering (Api, Application, Ports, Infrastructure)
* Typed HttpClient for Storage integration
* CI via reusable workflow (Team-2-Devs/.github)

## Related services

* [tu-ingestion-service](https://github.com/Team-2-Devs/tu-ingestion-service)
* [tu-storage-service](https://github.com/Team-2-Devs/tu-storage-service)

## Local development

See [DEV.md](./docs/DEV.md) for full setup instructions.

Two modes:

* Docker Compose (recommended)
* dotnet run (debugging)

## API Contracts

Formal versioned specifications are in:

[`v0-mediaaccess.md`](./docs/api-contracts/v0-mediaaccess.md)

Frozen contract for `/internal/v0/media`:

* `POST /get-url` *(implemented)*
* `POST /get-url-batch` *(planned)*
* `GET /health
