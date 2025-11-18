# DEV.md – Local Development Guide (Media Access Service)

This guide explains how to run the **Trackunit Media Access Service** locally and verify that it can communicate with the Storage service to generate presigned GET URLs.

Media Access does not talk directly to MinIO. It always calls **Storage.Api**, which in turn talks to MinIO.

Two modes are supported:

* **Option A (recommended): Docker Compose** – runs MediaAccess.Api in a container configured via `.env`.
* **Option B: `dotnet run`** – runs MediaAccess.Api from your IDE/CLI for debugging.

Use only one mode at a time.

---

## 1. Prerequisites

* Docker Desktop
* .NET 8 SDK
* curl (Git Bash or PowerShell 7+)

Verify installation:

```
docker version
dotnet --version
```

---

## 2. Dependencies

The Media Access service depends on:

* **Storage service** (`tu-storage-service`)

  * Exposes internal presign endpoints under `/internal/v1/storage`.
  * Talks to MinIO (bucket `trackunit-images`).

Before starting Media Access, ensure Storage is running as described in `tu-storage-service/docs/DEV.md`.

---

## 3. Configuration Overview

### 3.1 Docker Compose – `.env`

In this repository, Docker Compose reads configuration from a local `.env` file.

`.env` keys used by Media Access:

* `INTERNAL_AUTH_API_KEY` – internal token used when calling Storage.Api.

  * Must match `InternalAuth:ApiKey` used by `tu-storage-service`.
* `STORAGE_BASE_URL` – base URL for Storage.Api as seen from inside the Media Access container.

  * For local dev when Storage runs on the host, use:

    * `http://host.docker.internal:8080`.

`.env` is **not committed**. `.env.example` contains safe defaults.

### 3.2 `dotnet run` – appsettings

When you run the API via `dotnet run`, configuration is loaded from:

* `appsettings.json`
* `appsettings.Development.json`
* Environment variables
* (Optionally) user secrets

Typical development configuration:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "Storage": {
    "BaseUrl": "http://localhost:8080",
    "InternalAccess": "<your-internal-auth-key>"
  }
}
```

`InternalAccess` must equal the `InternalAuth:ApiKey` used by Storage.Api.

---

## 4. Option A – Run Media Access via Docker Compose (Recommended)

### 4.1 Start Storage + MinIO

From the storage repo:

```bash
cd ../tu-storage-service
docker compose up --build
```

Verify Storage health:

```bash
curl -v http://localhost:8080/health
```

You may see either `200 OK` or a `Missing internal token` message depending on configuration. Both confirm the service is running.

Ensure bucket `trackunit-images` exists in the MinIO console (`http://localhost:9001`).

### 4.2 Create `.env` from template

In the media access repo:

```bash
cd ../tu-media-access-service
cp .env.example .env
```

Edit `.env` and adjust if needed:

```env
INTERNAL_AUTH_API_KEY=<your-internal-auth-key>
STORAGE_BASE_URL=http://host.docker.internal:8080
```

### 4.3 Start Media Access

```bash
docker compose up --build
```

This will:

* Build the Media Access API image from `src/MediaAccess.Api/Dockerfile`.
* Start MediaAccess.Api on port `5136` (host) → `8080` (container).

### 4.4 Health check

```bash
curl -v http://localhost:5136/health
```

Expected:

```
HTTP/1.1 200 OK
Healthy
```

### 4.5 Stop the environment

```bash
docker compose down
```

(Optional)

```bash
docker compose down -v
```

---

## 5. Option B – Run Media Access via `dotnet run` (Debug Mode)

### 5.1 Start Storage + MinIO

```bash
cd ../tu-storage-service
docker compose up --build
```

### 5.2 Configure Media Access

Example development config:

```json
{
  "Storage": {
    "BaseUrl": "http://localhost:8080",
    "InternalAccess": "<your-internal-auth-key>"
  }
}
```

### 5.3 Run Media Access

```bash
dotnet run --project src/MediaAccess.Api
```

Health:

```bash
curl -v http://localhost:5136/health
```

---

## 6. Smoke Test – Presigned GET Flow (UC2)

> Precondition: You must first upload a file using the Storage service (Presign PUT → Upload).

### 6.1 Direct Storage presign-get (control)

```bash
curl -v -X POST "http://localhost:8080/internal/v1/storage/presign-get" \
  -H "Content-Type: application/json" \
  -H "X-Internal-Token: <your-internal-auth-key>" \
  -d '{"key":"<existing-object-key>","ttlSec":300}'
```

Expected:

```json
{
  "url": "http://localhost:9000/trackunit-images/...",
  "expiresAt": "..."
}
```

### 6.2 Media Access → Storage presign-get

```bash
curl -v -X POST "http://localhost:5136/internal/v0/media/get-url" \
  -H "Content-Type: application/json" \
  -d '{"objectKey":"<existing-object-key>"}'
```

Expected:

```json
{
  "url": "http://localhost:9000/trackunit-images/...",
  "expiresAt": "..."
}
```

### 6.3 Download the file from MinIO (Desktop download)

Git Bash / macOS / Linux:

```bash
curl -L "<presigned-url>" -o "$HOME/Desktop/downloaded.jpg"
```

PowerShell:

```powershell
curl.exe -L "<presigned-url>" -o "$HOME\Desktop\downloaded.jpg"
```

Expected:

* HTTP 200
* File appears on Desktop

This confirms: Media Access → Storage → MinIO.

---

## 7. Troubleshooting

| Problem                           | Cause            | Resolution                                             |
| --------------------------------- | ---------------- | ------------------------------------------------------ |
| 401 from Storage                  | Token mismatch   | Ensure Media Access sends the correct internal token   |
| 400 from Storage                  | Wrong JSON       | Must send `{ "key": "...", "ttlSec": 300 }` to Storage |
| Request expired                   | TTL passed       | Request a new presigned URL                            |
| NoSuchKey                         | Object missing   | Check key and MinIO bucket                             |
| Cannot reach host.docker.internal | Networking issue | Ensure Storage is running and URL is correct           |

---

## 8. Developer Checklist

* [ ] `.env` created from `.env.example` (Docker mode).
* [ ] Storage.Api running on `http://localhost:8080`.
* [ ] MinIO console reachable on `http://localhost:9001`.
* [ ] MediaAccess.Api reachable on `http://localhost:5136` (Docker) or local port (`dotnet run`).
* [ ] Known object exists in `trackunit-images` bucket.
* [ ] Direct Storage presign-get returns a URL.
* [ ] Media Access presign-get returns a URL.
* [ ] File download via presigned URL to Desktop succeeds.

## Reference

See the Storage and Ingestion DEV guides for related service behavior.

---

**Updated:** November 2025 (11/18)
