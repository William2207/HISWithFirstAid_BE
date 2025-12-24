# CI Configuration Guide

## GitHub Actions Secrets c?n thi?t

Hãy thêm các secrets sau vào GitHub repository settings (optional):

### SonarCloud (Code Quality Analysis - Optional)
- **SONAR_TOKEN**: Token t? https://sonarcloud.io
  - ??ng nh?p vào SonarCloud
  - Ch?n Organization > Security
  - Generate token m?i
  - Copy token vào GitHub Secrets

## Cách c?u hình

### Step 1: C?u hình SonarCloud (Optional)
1. Truy c?p https://sonarcloud.io
2. ??ng nh?p v?i GitHub account
3. Import organization "william2207"
4. Ch?n project "FirstAid_BE"
5. L?y Project Key và Token
6. Thêm vào GitHub Secrets

### Step 2: C?u hình GitHub Repository
1. Vào Settings > Secrets and variables > Actions
2. Thêm secrets (optional):
   - SONAR_TOKEN

### Step 3: Enable GitHub Actions
1. Vào Actions tab
2. Ch?n "I understand my workflows, go ahead and enable them"

## Branches ???c CI monitor
- `apiv1` - Development branch
- `main` - Production branch
- `develop` - Staging branch

## Pipeline Jobs

### 1. **build-and-test** (B?t bu?c) ?
- ? Restore dependencies
- ? Build project
- ? Run unit tests
- ? Generate test reports
- ? PostgreSQL test database

### 2. **code-quality** (Tu? ch?n)
- ? SonarCloud analysis
- ? Code coverage
- ? Code smell detection

### 3. **security-scan** (Tu? ch?n)
- ? Vulnerability scan
- ? Dependency check
- ? Security analysis

### 4. **lint** (Tu? ch?n)
- ? Code formatting check
- ? Style validation

## Local Development Setup

### Install Prerequisites
```bash
# .NET 8 SDK
https://dotnet.microsoft.com/download

# PostgreSQL 15
https://www.postgresql.org/download

# Git
https://git-scm.com/download
```

### Setup Environment

```bash
# Clone repo
git clone https://github.com/William2207/FirstAid_BE.git
cd FirstAid_BE
git checkout apiv1

# Run setup script
chmod +x setup-dev.sh
./setup-dev.sh

# Or manually
dotnet restore

# Make sure PostgreSQL is running
dotnet ef database update
dotnet run
```

## Troubleshooting

### Build fails
- Check .NET version: `dotnet --version`
- Restore packages: `dotnet restore`
- Clean build: `dotnet clean && dotnet build`

### Tests fail
- Run locally: `dotnet test`
- Check database connection string
- Ensure PostgreSQL is running

### SonarCloud issues
- Verify SONAR_TOKEN is correct
- Check project key matches
- Review SonarCloud project settings

## Best Practices

1. **Commit messages**: S? d?ng conventional commits
   ```
   feat: add new feature
   fix: fix bug
   docs: update documentation
   ```

2. **PR reviews**: ??i CI pass tr??c khi merge

3. **Testing**: Vi?t unit tests cho new features
   ```bash
   dotnet test
   ```

## Monitoring

- GitHub Actions: https://github.com/William2207/FirstAid_BE/actions
- SonarCloud: https://sonarcloud.io/organizations/william2207/projects
- Test Results: Artifact uploads trong Actions

## CI Status Checks

Required checks for pull requests:
- ? build-and-test (MUST PASS)
- ?? code-quality (should improve or maintain)
- ?? security-scan (no critical issues)
- ?? lint (minor issues acceptable)
