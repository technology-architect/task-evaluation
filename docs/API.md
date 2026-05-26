# API Documentation

## Base URL

```
https://localhost:5001/api/v1
```

## Authentication

All requests require JWT token in Authorization header:

```
Authorization: Bearer <jwt_token>
```

## Endpoints

### Authentication

#### Login

```http
POST /auth/login
Content-Type: application/json

{
  "username": "admin@company.com",
  "password": "secure_password"
}

Response:
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "refreshToken": "...",
  "expiresIn": 3600,
  "user": {
    "id": "user123",
    "username": "admin@company.com",
    "email": "admin@company.com",
    "roles": ["Admin"]
  }
}
```

#### Logout

```http
POST /auth/logout
Authorization: Bearer <jwt_token>

Response: 204 No Content
```

### Consent Management

#### Get Employee Consent

```http
GET /employees/{employeeId}/consent
Authorization: Bearer <jwt_token>

Response:
{
  "employeeId": "emp123",
  "keystrokeConsentGiven": true,
  "screenshotConsentGiven": true,
  "consentDate": "2026-01-15T10:30:00Z",
  "consentVersion": "1.0"
}
```

#### Request Consent

```http
POST /employees/{employeeId}/request-consent
Authorization: Bearer <jwt_token>
Content-Type: application/json

{
  "keystrokeCapture": true,
  "screenshotCapture": true,
  "reason": "Performance monitoring and security"
}

Response:
{
  "requestId": "consent_req_123",
  "status": "Pending",
  "createdAt": "2026-05-26T10:30:00Z"
}
```

#### Grant Consent

```http
POST /employees/{employeeId}/grant-consent
Authorization: Bearer <jwt_token>
Content-Type: application/json

{
  "keystrokeConsent": true,
  "screenshotConsent": true
}

Response: 200 OK
{
  "message": "Consent granted successfully",
  "consentId": "consent_123"
}
```

#### Revoke Consent

```http
POST /employees/{employeeId}/revoke-consent
Authorization: Bearer <jwt_token>

Response: 200 OK
{
  "message": "Consent revoked successfully"
}
```

### Monitoring Data

#### Get Employee Screenshots

```http
GET /monitoring/screenshots?employeeId={employeeId}&from={date}&to={date}&page=1&pageSize=10
Authorization: Bearer <jwt_token>

Response:
{
  "total": 240,
  "page": 1,
  "pageSize": 10,
  "screenshots": [
    {
      "id": "screenshot_123",
      "employeeId": "emp456",
      "capturedAt": "2026-05-26T09:00:00Z",
      "filename": "screenshot_20260526_090000.png",
      "fileSize": 245632,
      "encrypted": true,
      "url": "/api/v1/monitoring/screenshots/screenshot_123/download"
    }
  ]
}
```

#### Download Screenshot

```http
GET /monitoring/screenshots/{screenshotId}/download
Authorization: Bearer <jwt_token>

Response: 200 OK
Content-Type: image/png
(Binary image data)
```

#### Get Employee Keylogs

```http
GET /monitoring/keylogs?employeeId={employeeId}&from={date}&to={date}&page=1&pageSize=50
Authorization: Bearer <jwt_token>

Response:
{
  "total": 5000,
  "page": 1,
  "pageSize": 50,
  "keylogs": [
    {
      "id": "keylog_123",
      "employeeId": "emp456",
      "capturedAt": "2026-05-26T10:30:15Z",
      "application": "Microsoft Word",
      "window": "Document1.docx - Microsoft Word",
      "keyData": "<encrypted>",
      "decrypted": false
    }
  ]
}
```

#### Decrypt Keylog

```http
POST /monitoring/keylogs/{keylogId}/decrypt
Authorization: Bearer <jwt_token>

Response: 200 OK
{
  "id": "keylog_123",
  "keyData": "The quick brown fox",
  "decrypted": true,
  "decryptedAt": "2026-05-26T10:35:00Z"
}
```

### Audit Logs

#### Get Audit Logs

```http
GET /audit-logs?eventType={type}&userId={userId}&from={date}&to={date}&page=1&pageSize=50
Authorization: Bearer <jwt_token>

Response:
{
  "total": 1250,
  "page": 1,
  "pageSize": 50,
  "auditLogs": [
    {
      "id": "audit_123",
      "timestamp": "2026-05-26T10:30:00Z",
      "eventType": "DataAccess",
      "userId": "manager123",
      "targetEmployeeId": "emp456",
      "action": "ViewScreenshot",
      "status": "Success",
      "ipAddress": "192.168.1.100"
    }
  ]
}
```

### Configuration Management

#### Get Current Configuration

```http
GET /admin/configuration
Authorization: Bearer <jwt_token>
Roles: Admin

Response:
{
  "monitoring": {
    "keystrokeCapture": {
      "enabled": true,
      "captureInterval": 5000
    },
    "screenshotCapture": {
      "enabled": true,
      "intervalMinutes": 60
    }
  }
}
```

#### Update Configuration

```http
POST /admin/configuration
Authorization: Bearer <jwt_token>
Roles: Admin
Content-Type: application/json

{
  "monitoring": {
    "keystrokeCapture": {
      "enabled": false
    },
    "screenshotCapture": {
      "enabled": true,
      "intervalMinutes": 120
    }
  }
}

Response: 200 OK
{
  "message": "Configuration updated successfully"
}
```

## Error Responses

### 401 Unauthorized

```json
{
  "error": "Unauthorized",
  "message": "Invalid or expired token"
}
```

### 403 Forbidden

```json
{
  "error": "Forbidden",
  "message": "You don't have permission to access this resource"
}
```

### 404 Not Found

```json
{
  "error": "NotFound",
  "message": "Resource not found"
}
```

### 500 Internal Server Error

```json
{
  "error": "InternalServerError",
  "message": "An unexpected error occurred",
  "traceId": "0HN1GHBG7C5MR:00000001"
}
```

## Rate Limiting

API implements rate limiting:
- **100 requests per minute** per user
- **Throttle response headers**:
  ```
  X-RateLimit-Limit: 100
  X-RateLimit-Remaining: 95
  X-RateLimit-Reset: 1664214060
  ```

## Pagination

All list endpoints support pagination:
- **page**: Page number (default: 1)
- **pageSize**: Items per page (default: 30, max: 100)

## Filtering

List endpoints support filtering:
- **from**: Start date (ISO 8601)
- **to**: End date (ISO 8601)
- **status**: Filter by status
- **search**: Full-text search
