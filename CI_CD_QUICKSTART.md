# ?? CI Tích H?p - H??ng D?n Chi Ti?t

Tôi ?ã t?o m?t **CI (Continuous Integration) pipeline hoàn ch?nh** cho project FirstAid API. ?ây là h??ng d?n step-by-step ?? activate nó.

## ?? Files T?o Ra

```
.github/
??? workflows/
?   ??? ci.yml                    # Main CI pipeline
??? pull_request_template.md      # PR template
??? CODEOWNERS                    # Code ownership
??? BRANCH_PROTECTION.md          # Branch rules
??? ISSUE_TEMPLATE/
?   ??? bug_report.md
?   ??? feature_request.md

Root directory:
??? .editorconfig                 # Code style rules
??? sonarcloud.properties         # SonarCloud config
??? CI_CD_SETUP.md               # Detailed CI guide
??? DEVELOPMENT.md               # Development guide
??? setup-dev.sh                 # Dev environment setup
??? README.md                    # Project readme
??? .githooks/
    ??? pre-commit              # Local pre-commit hooks
```

## ?? Setup Steps

### Step 1: C?u hình GitHub Secrets (Optional)

1. Vào repository: https://github.com/William2207/FirstAid_BE
2. Ch?n **Settings** ? **Secrets and variables** ? **Actions**
3. Thêm secret (optional):

#### SonarCloud (Code Quality Analysis)
```
Name: SONAR_TOKEN
Value: <token t? sonarcloud.io>
```

?? l?y SonarCloud token:
- Vào https://sonarcloud.io
- ??ng nh?p b?ng GitHub
- Account ? Security ? Generate token
- Copy token vào GitHub Secrets

### Step 2: Enable GitHub Actions

1. Vào tab **Actions**
2. Ch?n "I understand my workflows, go ahead and enable them"

### Step 3: Configure Branch Protection (Optional)

1. Vào **Settings** ? **Branches**
2. Add rule v?i pattern: `main`
3. C?u hình theo `.github/BRANCH_PROTECTION.md`

### Step 4: Setup Local Development

```bash
# Clone repo
git clone https://github.com/William2207/FirstAid_BE.git
cd FirstAid_BE
git checkout apiv1

# Run setup script
chmod +x setup-dev.sh
./setup-dev.sh

# Or manually:
dotnet restore
# Make sure PostgreSQL is running
dotnet ef database update
dotnet run
```

## ?? Pipeline Overview

### Khi b?n push code:

```
Push to GitHub
    ?
GitHub Actions triggered
    ?
???????????????????????????????????????????????
? JOB 1: build-and-test (REQUIRED)           ?
? ?? Restore dependencies                     ?
? ?? Build project                           ?
? ?? Run unit tests                          ?
? ?? Upload test results                     ?
???????????????????????????????????????????????
    ?
???????????????????????????????????????????????
? JOB 2: code-quality (OPTIONAL)             ?
? ?? SonarCloud analysis                     ?
???????????????????????????????????????????????
    ?
???????????????????????????????????????????????
? JOB 3: security-scan (OPTIONAL)            ?
? ?? Vulnerability check                     ?
???????????????????????????????????????????????
    ?
???????????????????????????????????????????????
? JOB 4: lint (OPTIONAL)                     ?
? ?? Code formatting check                   ?
???????????????????????????????????????????????
    ?
? All complete - Ready to merge!
```

## ? Features

### 1. **Build & Test** ?
- T? ??ng build project m?i l?n push
- Ch?y t?t c? unit tests
- Upload test results
- PostgreSQL test database ?i kèm

### 2. **Code Quality** (Optional)
- SonarCloud analysis
- Detect code smells
- Security hotspots
- Test coverage

### 3. **Security Scan** (Optional)
- Check vulnerable packages
- Dependency analysis
- Security assessment

### 4. **Code Formatting** (Optional)
- Check code style
- Validate formatting
- Ensure consistency

## ?? Workflow

### Normal Development

```bash
# 1. Create feature branch
git checkout -b feature/my-feature

# 2. Make changes
# ... code ...

# 3. Format code
dotnet format

# 4. Commit
git add .
git commit -m "feat: add my feature"

# 5. Push
git push origin feature/my-feature

# 6. Create Pull Request on GitHub
# - CI runs automatically
# - Wait for all checks to pass ?
# - Get code review
# - Merge when ready
```

### Pull Request Checklist

- [ ] Code builds successfully
- [ ] All tests pass
- [ ] No code quality issues (SonarCloud)
- [ ] No security vulnerabilities
- [ ] Code is properly formatted
- [ ] Commit messages are clear
- [ ] Documentation updated

## ?? Monitoring CI

### View Pipeline Status

1. **GitHub Actions**: https://github.com/William2207/FirstAid_BE/actions
2. **SonarCloud**: https://sonarcloud.io/organizations/william2207/projects
3. **Pull Requests**: https://github.com/William2207/FirstAid_BE/pulls

### View Details

- Click on workflow run
- See all jobs and their status
- Check logs for errors
- Download artifacts (test results, coverage)

## ?? Notifications

GitHub s? notify b?n:
- ? Khi workflow pass
- ? Khi workflow fail
- ?? Khi need review
- ? Khi ready to merge

## ?? Pre-commit Hooks (Local)

```bash
# Auto setup when running setup-dev.sh
# Or manual setup:
chmod +x .githooks/pre-commit
cp .githooks/pre-commit .git/hooks/

# Now every commit automatically:
# ? Checks if code builds
# ? Checks code formatting
# ? Prevents commits if build fails
```

## ?? Troubleshooting

### Build fails on GitHub but works locally?

1. Check .NET version: `dotnet --version`
2. Check connection string for tests
3. Ensure migrations are up to date
4. Clean and rebuild: `dotnet clean && dotnet build`

### Tests timeout?

1. Increase timeout in `.github/workflows/ci.yml`
2. Optimize slow tests
3. Run tests in parallel

### PostgreSQL connection issue?

1. Ensure PostgreSQL is running
2. Check connection string in appsettings.Development.json
3. Verify database exists

## ?? Best Practices

1. **Keep CI fast** - Optimize test speed
2. **Write tests** - Maintain high coverage
3. **Clear commits** - Use conventional commits
4. **Code review** - Always review before merge
5. **Monitor** - Check Actions regularly
6. **Document** - Keep docs updated

## ?? Additional Resources

- [GitHub Actions Docs](https://docs.github.com/en/actions)
- [SonarCloud Docs](https://docs.sonarcloud.io/)
- [.NET Testing](https://learn.microsoft.com/en-us/dotnet/core/testing/)
- [PostgreSQL Docs](https://www.postgresql.org/docs/)

## ? Checklist for Full Setup

- [ ] Repository cloned
- [ ] GitHub Secrets configured (SONAR_TOKEN - optional)
- [ ] GitHub Actions enabled
- [ ] Branch protection rules configured
- [ ] Local development environment setup (PostgreSQL running)
- [ ] First commit and PR created
- [ ] CI/CD pipeline verified
- [ ] Team notified of new CI

## ?? Success!

Khi m?i th? ho?t ??ng:
- ? Code auto-builds on push
- ? Tests run automatically
- ? Code quality checked
- ? Security scanned
- ? Team can see status in PR

---

**Questions?** Check CI_CD_SETUP.md or contact team!
