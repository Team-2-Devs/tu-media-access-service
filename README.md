# Trackunit Media Access Service
![CI](https://github.com/Team-2-Devs/tu-media-access-service/actions/workflows/ci.yml/badge.svg)

Media access microservice for Trackunit.

## Purpose
- Authorize internal access to media and retrieve pre-signed GET URLs via Storage.  
- Provide single and batch URL issuance for downstream services.  

## Endpoints (v1)
*Note: endpoints are defined here as part of the design. They are not yet implemented unless otherwise stated.*

- POST /internal/v1/media/get-url – fetch pre-signed GET URL for a single object  
- POST /internal/v1/media/get-url-batch – (planned) fetch pre-signed GET URLs for multiple objects
- GET  /health  

## Tech
- .NET 8, ASP.NET Core Web API  
- Clean/hexagonal layering: Api, Application, Domain, Infrastructure  
- Typed HttpClient to Storage (configurable BaseUrl)  
- CI via reusable org workflow (see [Team-2-Devs/.github](https://github.com/Team-2-Devs/.github))

## Related services
- [tu-ingestion-service](https://github.com/Team-2-Devs/tu-ingestion-service) – handles upload initiation and confirmation, publishes events
- [tu-storage-service](https://github.com/Team-2-Devs/tu-storage-service) – issues pre-signed PUT/GET URLs

## Local dev
```bash
dotnet restore
dotnet build
dotnet run --project src/MediaAccess.Api
```