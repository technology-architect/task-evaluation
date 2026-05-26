# Configuration Guide

## Overview

The monitoring system is configured through JSON configuration files. All sensitive settings should be stored securely.

## monitoring.json

```json
{
  "monitoring": {
    "enabled": true,
    "keystrokeCapture": {
      "enabled": true,
      "captureInterval": 5000,
      "excludeApplications": [
        "Password Manager",
        "Banking App"
      ],
      "excludeFields": true
    },
    "screenshotCapture": {
      "enabled": true,
      "intervalMinutes": 60,
      "quality": 85,
      "maxResolution": "1920x1080",
      "includeSecondaryMonitors": false
    },
    "dataRetention": {
      "keylogsRetentionDays": 30,
      "screenshotsRetentionDays": 90,
      "auditLogsRetentionDays": 365,
      "autoDeleteEnabled": true
    }
  },
  "storage": {
    "type": "FileSystem",
    "path": "./captured_data",
    "encryption": {
      "enabled": true,
      "algorithm": "AES-256-GCM",
      "keyRotationDays": 90
    }
  },
  "database": {
    "provider": "PostgreSQL",
    "connectionString": "Server=localhost;Database=EmployeeMonitoring;User Id=admin;Password=secure_password"
  },
  "security": {
    "enableMFA": true,
    "sessionTimeout": 30,
    "passwordPolicy": {
      "minLength": 12,
      "requireUppercase": true,
      "requireNumbers": true,
      "requireSpecialChars": true
    }
  },
  "logging": {
    "level": "Information",
    "auditLog": {
      "enabled": true,
      "logAllAccess": true,
      "logDataExport": true
    }
  }
}
```

## Key Configuration Options

### Keystroke Capture

- **enabled**: Enable/disable keystroke capture
- **captureInterval**: Milliseconds between capture checks
- **excludeApplications**: List of apps to exclude from monitoring
- **excludeFields**: Auto-detect and exclude password fields

### Screenshot Capture

- **enabled**: Enable/disable screenshot capture
- **intervalMinutes**: How often to capture (recommended: 60 minutes)
- **quality**: JPEG quality (0-100)
- **maxResolution**: Max resolution to capture
- **includeSecondaryMonitors**: Include all monitors or primary only

### Data Retention

- **keylogsRetentionDays**: Days to keep keystroke logs
- **screenshotsRetentionDays**: Days to keep screenshots
- **auditLogsRetentionDays**: Days to keep audit logs
- **autoDeleteEnabled**: Automatically delete expired data

### Storage

- **type**: FileSystem or Cloud (S3, Azure Blob)
- **path**: Local storage path
- **encryption**: AES-256 encryption for stored data
- **keyRotation**: Automatic encryption key rotation

### Security

- **enableMFA**: Multi-factor authentication
- **sessionTimeout**: Session timeout in minutes
- **passwordPolicy**: Strong password requirements

## Environment Variables

```bash
# Database
DB_CONNECTION_STRING=Server=localhost;Database=EmployeeMonitoring;...

# Encryption
ENCRYPTION_KEY=your-base64-encoded-256-bit-key

# API
API_PORT=5000
API_HTTPS_PORT=5001

# Logging
LOG_LEVEL=Information
```

## Security Best Practices

1. **Never commit sensitive data** to version control
2. **Use secrets management** (Azure Key Vault, AWS Secrets Manager)
3. **Rotate encryption keys** regularly
4. **Use HTTPS only** in production
5. **Enable MFA** for all admin accounts
6. **Review audit logs** regularly
7. **Set appropriate retention policies** based on legal requirements

## Example: Minimal Configuration

```json
{
  "monitoring": {
    "enabled": true,
    "keystrokeCapture": {
      "enabled": false
    },
    "screenshotCapture": {
      "enabled": true,
      "intervalMinutes": 60
    }
  },
  "storage": {
    "type": "FileSystem",
    "path": "./captured_data",
    "encryption": {
      "enabled": true,
      "algorithm": "AES-256-GCM"
    }
  },
  "database": {
    "provider": "PostgreSQL",
    "connectionString": "Server=localhost;Database=EmployeeMonitoring;User Id=admin;Password=secure_password"
  }
}
```
