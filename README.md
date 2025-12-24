# FirstAid API - Backend

Complete REST API for FirstAid application - Learning platform for first aid techniques and emergency response scenarios.

![.NET](https://img.shields.io/badge/.NET-8.0-blue)
![C#](https://img.shields.io/badge/C%23-12.0-green)
![PostgreSQL](https://img.shields.io/badge/PostgreSQL-15-336791)
![License](https://img.shields.io/badge/License-MIT-yellow)

## ?? Features

- ? User Authentication & Authorization (JWT)
- ? Email Verification & Password Reset with OTP
- ? First Aid Techniques Management
- ? Quiz System with Answer Validation
- ? Scenario-based Learning
- ? User Progress Tracking
- ? Course Management & Enrollment
- ? Shopping Cart & Orders
- ? User Achievements System
- ? Continuous Integration/Deployment

## ?? Quick Start

### Prerequisites
- .NET 8 SDK
- PostgreSQL 15

### Installation

```bash
# Clone repository
git clone https://github.com/William2207/FirstAid_BE.git
cd FirstAid_BE

# Setup development environment
chmod +x setup-dev.sh
./setup-dev.sh

# Or manually:
dotnet restore
dotnet ef database update
dotnet run
```

API will be available at: `https://localhost:7000`

## ?? API Documentation

### Authentication
```
POST /api/account/register      - Register new user
POST /api/account/login         - Login user
POST /api/account/forgot-password - Request password reset
POST /api/account/verify-otp    - Verify OTP and reset password
```

### Users
```
GET  /api/users              - Get all users
GET  /api/users/{id}         - Get user by ID
PUT  /api/users/{id}         - Update user
DELETE /api/users/{id}       - Delete user
```

### Techniques
```
GET  /api/techniques              - Get all techniques
GET  /api/techniques/{id}         - Get technique by ID
POST /api/techniques              - Create technique
PUT  /api/techniques/{id}         - Update technique
DELETE /api/techniques/{id}       - Delete technique
GET  /api/techniques/{id}/steps   - Get technique steps
```

### Quizzes
```
GET  /api/quiz                    - Get all quiz questions
GET  /api/quiz/{id}               - Get quiz by ID
POST /api/quiz                    - Create quiz
PUT  /api/quiz/{id}               - Update quiz
DELETE /api/quiz/{id}             - Delete quiz
```

### Scenarios
```
GET  /api/scenarios              - Get all scenarios
GET  /api/scenarios/{id}         - Get scenario by ID
POST /api/scenarios              - Create scenario
PUT  /api/scenarios/{id}         - Update scenario
DELETE /api/scenarios/{id}       - Delete scenario
```

## ??? Architecture

```
FirstAidAPI/
??? Controllers/          # API endpoints (REST)
??? Models/              # Domain models
??? DTOs/                # Data Transfer Objects
??? Services/            # Business logic layer
?   ??? IService.cs
?   ??? Implement/
??? Repository/          # Data access layer
?   ??? IRepository.cs
?   ??? Implement/
??? Mappings/            # AutoMapper profiles
??? Migrations/          # EF Core migrations
??? Extensions/          # Extension methods
??? Data/                # DbContext configuration
```

## ?? Authentication

Uses JWT (JSON Web Tokens) for API authentication:

```csharp
// Login
POST /api/account/login
{
  "email": "user@example.com",
  "password": "password123"
}

Response:
{
  "message": "Success",
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "user": {
    "email": "user@example.com",
    "roles": ["User"]
  }
}

// Use token in requests
Authorization: Bearer <token>
```

## ?? Password Reset Flow

1. User requests password reset with email
```
POST /api/account/forgot-password
{ "email": "user@example.com" }
```

2. System sends OTP (6-digit code) via email
3. User verifies OTP and receives new temporary password
```
POST /api/account/verify-otp
{
  "email": "user@example.com",
  "otp": "123456"
}
```

4. User receives new password via email
5. User can login with new password

## ?? Testing

```bash
# Run all tests
dotnet test

# Run specific test project
dotnet test Tests/FirstAidAPI.Tests.csproj

# Run with code coverage
dotnet test /p:CollectCoverage=true /p:CoverageFormat=opencover
```

## ?? CI/CD Pipeline

GitHub Actions automatically:
- ? Builds on every push
- ? Runs unit tests
- ? Performs code quality analysis (SonarCloud)
- ? Scans for security vulnerabilities
- ? Checks code formatting

### Monitoring CI
- [GitHub Actions](https://github.com/William2207/FirstAid_BE/actions)
- [SonarCloud](https://sonarcloud.io/organizations/william2207/projects)

## ?? Database Schema

### Key Tables
- **Users** - User accounts and profiles
- **Techniques** - First aid techniques
- **TechniqueSteps** - Steps in each technique
- **QuizQuestions** - Quiz questions
- **AnswerOptions** - Quiz answer options
- **Scenarios** - Emergency scenarios
- **ScenarioSteps** - Steps in scenarios
- **UserScenarioProgress** - User progress in scenarios
- **UserTechniqueProgress** - User progress in techniques
- **SavedTechniques** - Bookmarked techniques
- **Orders** - Purchase orders
- **CourseEnrollments** - Course registrations
- **PasswordResetTokens** - OTP tokens for password reset

## ??? Technologies

- **Framework**: ASP.NET Core 8
- **Database**: PostgreSQL 15
- **Authentication**: JWT (IdentityUser)
- **ORM**: Entity Framework Core
- **Mapping**: AutoMapper
- **Email**: MailKit
- **Logging**: Serilog (future)
- **Cache**: Redis (future)

## ?? Code Standards

- **Language**: C# 12.0
- **Naming Convention**: PascalCase for classes/methods, camelCase for variables
- **Async Pattern**: Use `async/await` for I/O operations
- **Dependency Injection**: Constructor injection
- **Logging**: ILogger interface
- **Error Handling**: Custom exceptions

## ?? Contributing

1. Create feature branch: `git checkout -b feature/feature-name`
2. Commit changes: `git commit -m "feat: description"`
3. Push branch: `git push origin feature/feature-name`
4. Create Pull Request

### Commit Convention
```
feat:    new feature
fix:     bug fix
docs:    documentation
style:   code style (formatting, semicolons)
refactor: code refactoring
test:    adding tests
chore:   build/dependency updates
```

## ?? Documentation

- [Development Setup](DEVELOPMENT.md)
- [CI/CD Configuration](CI_CD_SETUP.md)

## ?? Known Issues

None at the moment.

## ?? Support

For issues and questions:
1. Check [GitHub Issues](https://github.com/William2207/FirstAid_BE/issues)
2. Create new issue with details
3. Contact team members

## ?? License

MIT License - See LICENSE file for details

## ?? Contributors

- [@William2207](https://github.com/William2207) - Project Lead

## ?? Acknowledgments

- ASP.NET Core Team
- Entity Framework Team
- PostgreSQL Community

---

**Happy coding! ??**

For more information, visit the [GitHub Repository](https://github.com/William2207/FirstAid_BE)
