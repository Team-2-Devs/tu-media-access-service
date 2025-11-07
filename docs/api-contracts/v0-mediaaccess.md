# API Contract – Media Access Service (v0)

**Version:** v0 (draft)  
**Last updated:** November 2025  
**Owner:** Trackunit Media Access Service  
**Scope:** Internal service-to-service contract for issuing short-lived presigned GET URLs to retrieve media.  
**Status:** Draft – breaking changes may occur until stabilized under /v1/.

---

## Overview

Media Access exposes an internal endpoint that returns **short-lived presigned GET URLs** for existing objects in object storage.  
It delegates URL generation to the Storage service and does not proxy the object bytes.

- No persistent state is kept.  
- TTL is server-controlled (currently 300 seconds).  
- Authorization is not enforced yet; internal authentication may be added later.

---

## Base URL

```bash
http://localhost:5290/internal/v0/media
```

> Replace host and port as appropriate for your environment (e.g., `https://media.<env>.trackunit.internal/internal/v0/media`).

---

## Endpoints

### 1. POST /internal/v0/media/get-url

Return a **presigned GET URL** for an existing object key.

#### Description
Issues a temporary, signed URL suitable for direct HTTP GET from the object store (MinIO/S3 compatible).  
Intended for internal services (e.g., AI) that need read access to media.

#### Request headers
- Content-Type: application/json  
- X-Internal-Token: `<token>` (optional; reserved for future enforcement)

#### Request
```json
{
  "objectKey": "images/2025/11/06/sample.jpg"
}
```

#### Parameters

| Field | Type | Required | Description |
|-------|------|-----------|--------------|
| `objectKey` | string | yes | S3/MinIO object key (validated for shape/charset; existence not verified). |

#### Response 200 OK
```json
{
  "url": "http://localhost:9000/trackunit-images/images/2025/11/06/sample.jpg?...",
  "expiresAt": "2025-11-06T19:32:12Z"
}
```

#### Response 422 Unprocessable Entity
```json
{
  "errors": {
    "objectKey": ["Required"]
  }
}
```

#### Response  500 Internal Server Error
```json
{
  "type": "about:blank",
  "title": "Internal Server Error",
  "status": 500,
  "detail": "Unexpected failure while generating presigned URL."
}
```

#### Notes
- **Existence check:** presigning does not confirm the object exists; object stores can presign non-existent keys. Consumers should handle 404 Not Found when using the returned URL if the object is missing.  
- **TTL:** controlled by the service; clients cannot specify or override it in v0.

---

### 2. GET /health

Simple health probe used by orchestrators or load balancers.

#### Response 200 OK
Plain text:
```text
Healthy
```

---

## Example

```bash
curl -s -X POST -H "Content-Type: application/json" -d '{"objectKey":"images/2025/11/06/sample.jpg"}' http://localhost:5290/internal/v0/media/get-url
```

Response:  
```json
{
  "url": "http://localhost:9000/trackunit-images/images/2025/11/06/sample.jpg?...",
  "expiresAt": "2025-11-06T19:32:12Z"
}
```

---

## Changelog

| Date | Version | Changes |
|------|----------|----------|
| 2025-11-07 | v0 | Initial draft for /get-url endpoint |

---

## Reference
For service overview and related services, see [README.md](../../README.md).

---

**End of document**