# Employee Monitoring System

A secure, .NET Core-based employee monitoring solution with keystroke capture and screenshots. **This system requires explicit employee consent and is designed for legitimate workplace monitoring with proper disclosure.**

## Features

✅ **Keystroke Capture** - Records keyboard input (configurable)
✅ **Screenshot Capture** - Captures screen every hour (configurable)
✅ **Secure Storage** - Encrypted data storage with AES-256
✅ **Password Protected Access** - Role-based access with authentication
✅ **Configurable Settings** - Full configuration via JSON/YAML config files
✅ **Audit Logging** - Complete audit trail of all access and actions
✅ **Consent Management** - Tracks employee consent and opt-in status
✅ **Data Retention Policies** - Automatic cleanup of old data

## Legal Compliance

⚠️ **Important**: This system is designed for use **ONLY** with:
- **Explicit employee consent** in writing
- **Clear disclosure** of what is being monitored
- **Proper data protection** and privacy policies
- **Legal compliance** with applicable laws (GDPR, CCPA, etc.)
- **Transparent access controls** and audit trails

## Project Structure

```
task-evaluation/
├── src/
│   ├── EmployeeMonitoring.API/          # REST API
│   ├── EmployeeMonitoring.Core/         # Business logic
│   ├── EmployeeMonitoring.Infrastructure/ # Data access & capture
│   ├── EmployeeMonitoring.Services/     # Monitoring services
│   └── EmployeeMonitoring.Common/       # Shared utilities
├── config/
│   ├── monitoring.json                   # Monitoring configuration
│   ├── security.json                     # Security settings
│   └── keys/                             # Encryption keys (gitignored)
├── database/
│   └── migrations/                       # Database migrations
├── tests/
│   └── EmployeeMonitoring.Tests/        # Unit & integration tests
└── docker/
    └── docker-compose.yml                # Docker setup
```

## Quick Start

### Prerequisites
- .NET 6.0 or higher
- SQL Server or PostgreSQL
- Windows/Linux/Mac

### Installation

1. Clone the repository:
```bash
git clone https://github.com/technology-architect/task-evaluation.git
cd task-evaluation
```

2. Build the solution:
```bash
dotnet build
```

3. Configure settings:
```bash
cp config/monitoring.example.json config/monitoring.json
# Edit config/monitoring.json with your settings
```

4. Run migrations:
```bash
dotnet ef database update
```

5. Start the API:
```bash
cd src/EmployeeMonitoring.API
dotnet run
```

## Configuration

See [Configuration Guide](./docs/CONFIGURATION.md) for detailed setup.

## API Documentation

See [API Documentation](./docs/API.md) for endpoint details.

## Security

See [Security Guide](./docs/SECURITY.md) for encryption and authentication.

## Legal Compliance

See [Legal Compliance Guide](./docs/LEGAL_COMPLIANCE.md) for legal considerations.

## License

MIT License - See LICENSE file for details
