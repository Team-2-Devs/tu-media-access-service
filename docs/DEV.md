# Developer Guide – Media Access Service

This document explains how to run the **Trackunit Media Access Service** locally, connect it to the Storage service and MinIO, and perform a simple smoke test verifying presigned GET URL generation.

---

## 1. Prerequisites

- **Docker Desktop** (WSL2 + Virtual Machine Platform enabled)
- **.NET 8 SDK**
- Optional: `curl` (included with Git Bash / PowerShell 7+)

Check installation:

```bash
docker version
dotnet --version
```

---

## 2. Local infrastructure (MinIO + Storage)

> Ensure Docker Desktop is running before proceeding.

1. **Start MinIO**

   ```bash
   docker compose up -d
   ```

   Verify:

   ```bash
   docker ps
   ```

   Expected output includes:

   ```
   minio/minio:latest   0.0.0.0:9000-9001->9000-9001/tcp
   ```

2. **Access MinIO Console**

   - URL: [http://localhost:9001](http://localhost:9001)
   - Username: `minioadmin`
   - Password: `minioadmin`

3. **Default bucket**
   `trackunit-images`
   (Create it once if missing.)

4. **Run the Storage service**

   ```bash
   dotnet run --project ../tu-storage-service/src/Storage.Api
   ```

   Confirm it listens on:
   `http://localhost:5136`

---

## 3. Run the Media Access API

Start the service:

```bash
dotnet run --project src/MediaAccess.Api
```

Health check:

```bash
curl -s http://localhost:5290/health
```

Expected output:

```
Healthy
```

---

## 4. Configuration

Edit `src/MediaAccess.Api/appsettings.Development.json` if needed:

```json
{
  "Storage": {
    "BaseUrl": "http://localhost:5136",
    "InternalToken": "dev-secret-only"
  }
}
```

---

## 5. Smoke test (UC2 – Media Access Flow)

> **Precondition:** The object to be accessed must already exist in MinIO.  
> To upload a sample object, follow the Storage service guide (see [Storage DEV.md](https://github.com/Team-2-Devs/tu-storage-service/blob/develop/docs/DEV.md), section 5.1 Presign PUT → Upload).

### Step 1 – Presign GET URL

Request a presigned GET URL for an existing object:

```bash
curl -s -X POST http://localhost:5290/internal/v0/media/get-url -H "Content-Type: application/json" -d '{"objectKey":"images/2025/11/06/sample.jpg"}'
```

Example response:

```json
{
  "url": "http://localhost:9000/trackunit-images/images/2025/11/06/sample.jpg?...",
  "expiresAt": "2025-11-06T19:32:12Z"
}
```

### Step 2 – Download from MinIO

Use the returned `url` to download the file:

```bash
curl -o ~/Desktop/downloaded.jpg "<paste-url-here>"
```

Expected: silent success (`HTTP 200`).

### Step 3 – Verify file integrity

If you also have the original file (`sample.jpg`):

```bash
sha256sum ~/Desktop/sample.jpg ~/Desktop/downloaded.jpg
```

Expected: identical hashes.

---

## 6. Troubleshooting

| Problem | Cause | Fix |
|----------|--------|-----|
| `422 UnprocessableEntity` | Missing or invalid `objectKey` | Provide a valid key path |
| `Request has expired` | The presigned URL TTL expired before use | Re-request URL |
| `NoSuchKey` | Object doesn’t exist in MinIO | Verify object key in MinIO Console |
| `Storage service not reachable` | Storage not running | Start Storage service on `http://localhost:5136` |

---

## 7. Tear down

Stop MinIO:

```bash
docker compose down
```

Remove volumes (optional):

```bash
docker compose down -v
```

---

## 8. Developer checklist

- [ ] MinIO running (`docker compose up -d`)
- [ ] Storage service running on `http://localhost:5136`
- [ ] Media Access service running on `http://localhost:5290`
- [ ] `/health` returns Healthy
- [ ] `/internal/v0/media/get-url` returns presigned URL
- [ ] Object exists in MinIO Console
- [ ] Download via presigned URL succeeds

---

## Reference

For service overview and related services, see [README.md](../README.md).

---

**Updated:** November 2025
