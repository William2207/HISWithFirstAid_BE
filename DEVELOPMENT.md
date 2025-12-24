# Development Setup Guide

## Prerequisites

- **.NET 8 SDK**: https://dotnet.microsoft.com/download
- **PostgreSQL 15**: https://www.postgresql.org/download
- **Git**: https://git-scm.com/download

## Installation Steps

### 1. Clone Repository
```bash
git clone https://github.com/William2207/FirstAid_BE.git
cd FirstAid_BE
git checkout apiv1
```

### 2. Setup Environment Variables

T?o file `appsettings.Development.json` trong th? m?c root:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=firstaid_dev;Username=postgres;Password=your_password;"
  },
  "JWT": {
    "Secret": "your-secret-key-here-at-least-32-characters-long",
    "ValidIssuer": "FirstAidAPI",
    "ValidAudience": "FirstAidClient",
    "ExpirationMinutes": 60
  },
  "EmailSettings": {
    "Host": "smtp.gmail.com",
    "Port": "587",
    "Username": "your-email@gmail.com",
    "Password": "your-app-password",
    "FromName": "FirstAid Support"
  },
  "CloudinarySettings": {
    "CloudName": "your-cloud-name",
    "ApiKey": "your-api-key",
    "ApiSecret": "your-api-secret"
  }
}
```

### 3. Database Setup

#### PostgreSQL Setup
```bash
# Windows
psql -U postgres -c "CREATE DATABASE firstaid_dev;"

# Mac/Linux
sudo -u postgres psql -c "CREATE DATABASE firstaid_dev;"
```

### 4. Restore Dependencies
```bash
dotnet restore
```

### 5. Apply Database Migrations
```bash
dotnet ef database update
```

### 6. Run Application
```bash
dotnet run
```

API s? ch?y t?i: `https://localhost:7000` ho?c `http://localhost:5000`

## Development Workflow

### Build Project
```bash
dotnet build
```

### Run Tests
```bash
dotnet test
```

### Run with Watch Mode
```bash
dotnet watch run
```

### Format Code
```bash
dotnet format
```

### Create Database Migration
```bash
dotnet ef migrations add MigrationName
```

### Update Database
```bash
dotnet ef database update
```

### Revert Last Migration
```bash
dotnet ef migrations remove
```

## Testing

### Run All Tests
```bash
dotnet test
```

### Run Specific Test Project
```bash
dotnet test Tests/FirstAidAPI.Tests.csproj
```

### Run with Coverage
```bash
dotnet test /p:CollectCoverage=true /p:CoverageFormat=opencover
```

## Git Workflow

### Create Feature Branch
```bash
git checkout -b feature/your-feature
```

### Commit Changes
```bash
git add .
git commit -m "feat: description of your feature"
```

### Push Branch
```bash
git push origin feature/your-feature
```

### Create Pull Request
1. Vào GitHub
2. Compare & pull request
3. Add description
4. Request reviewers
5. Wait for CI to pass

## Common Issues

### Issue: "Unable to connect to PostgreSQL"
**Solution:**
```bash
# Check PostgreSQL is running
psql -U postgres -c "SELECT 1;"

# Or check connection string in appsettings.Development.json
```

### Issue: "Migration error"
**Solution:**
```bash
dotnet ef database drop --force
dotnet ef database update
```

### Issue: "Port already in use"
**Solution:**
```bash
# Change port in appsettings.json or
lsof -i :5000  # Find process
kill -9 <PID>  # Kill process
```

## Debugging

### Enable Debug Logging
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Debug"
    }
  }
}
```

### Use VS Code Debugger
1. Install C# extension
2. Set breakpoints
3. Press F5 to start debugging
4. Use debug console to inspect variables

### Use Visual Studio
1. Set breakpoints
2. Press F5
3. Use Locals/Watch windows

## Code Standards

- **Language**: C# 12.0
- **.NET Target**: .NET 8
- **Naming**: PascalCase cho classes, methods; camelCase cho variables
- **Async**: Use `async/await` for I/O operations
- **Dependency Injection**: S? d?ng DI container
- **Logging**: Use ILogger interface

## Architecture

```
FirstAidAPI/
??? Controllers/          # API endpoints
??? Models/              # Data models
??? DTOs/                # Data transfer objects
??? Services/            # Business logic
??? Repository/          # Data access
??? Migrations/          # EF Core migrations
??? Extensions/          # Extension methods
??? Mappings/            # AutoMapper profiles
??? Data/                # DbContext
```

## Resources

- [.NET 8 Documentation](https://learn.microsoft.com/en-us/dotnet/core/whats-new/dotnet-8)
- [Entity Framework Core](https://learn.microsoft.com/en-us/ef/core/)
- [ASP.NET Core API](https://learn.microsoft.com/en-us/aspnet/core/web-api/)
- [PostgreSQL Docs](https://www.postgresql.org/docs/)

## Support

N?u g?p v?n ??, vui lòng:
1. Ki?m tra GitHub Issues
2. T?o Issue m?i v?i chi ti?t
3. Liên h? team
