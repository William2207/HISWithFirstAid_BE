# Branch Protection Rules for FirstAid API

## Configuration for GitHub Repository

### Step-by-step setup:

1. Go to: Settings ? Branches
2. Add Rule ? Pattern: `main`
3. Add Rule ? Pattern: `apiv1`

### Settings for `main` branch (Production):

- ? Require a pull request before merging
  - ? Require approval reviews: 2
  - ? Dismiss stale pull request approvals when new commits
  - ? Require review from code owners
  
- ? Require status checks to pass before merging
  - ? Require branches to be up to date
  - Required checks:
    - `build-and-test`
    - `code-quality`
    - `security-scan`

- ? Require conversation resolution before merging

- ? Include administrators: YES

- ? Restrict who can push to matching branches
  - Allow: Only administrators

- ? Allow force pushes: NO

- ? Allow deletions: NO

### Settings for `apiv1` branch (Development):

- ? Require a pull request before merging
  - ? Require approval reviews: 1
  - ? Dismiss stale pull request approvals when new commits
  
- ? Require status checks to pass before merging
  - ? Require branches to be up to date
  - Required checks:
    - `build-and-test`

- ? Require conversation resolution before merging

- ? Include administrators: NO

- ? Restrict who can push to matching branches: NO

- ? Allow force pushes: NO

- ? Allow deletions: NO

## Code Owners File (.github/CODEOWNERS)

```
# Default owners for everything
* @William2207

# API Controllers
/Controllers/ @William2207

# Business Logic
/Service/ @William2207

# Data Access
/Repository/ @William2207

# Database
/Migrations/ @William2207
/Data/ @William2207

# Configuration
/appsettings*.json @William2207
*.yml @William2207
Dockerfile @William2207
docker-compose.yml @William2207
```

## Pull Request Template (.github/pull_request_template.md)

```markdown
## Description
<!-- Brief description of the changes -->

## Type of Change
- [ ] Bug fix
- [ ] New feature
- [ ] Breaking change
- [ ] Documentation update

## Related Issues
Closes #(issue number)

## Changes Made
- Change 1
- Change 2

## Testing
- [ ] Unit tests added
- [ ] Integration tests added
- [ ] Manual testing completed

## Checklist
- [ ] Code follows style guidelines
- [ ] Self-review completed
- [ ] Comments added for complex logic
- [ ] Documentation updated
- [ ] No new warnings generated
- [ ] Tests pass locally

## Screenshots (if applicable)
<!-- Add screenshots for UI changes -->

## Reviewers
@William2207
```

## Issue Templates

### Bug Report (.github/ISSUE_TEMPLATE/bug_report.md)

```markdown
---
name: Bug Report
about: Report a bug to help us improve
---

## Describe the Bug
<!-- Clear description of what the bug is -->

## Steps to Reproduce
1. Step 1
2. Step 2

## Expected Behavior
<!-- What you expected to happen -->

## Actual Behavior
<!-- What actually happened -->

## Environment
- OS: [e.g., Windows 11]
- .NET Version: [e.g., 8.0]
- Browser: [if applicable]

## Logs/Screenshots
<!-- Attach error logs or screenshots -->

## Additional Context
<!-- Any other context -->
```

### Feature Request (.github/ISSUE_TEMPLATE/feature_request.md)

```markdown
---
name: Feature Request
about: Suggest a new feature
---

## Description
<!-- Clear description of the feature -->

## Motivation
<!-- Why is this feature needed? -->

## Proposed Solution
<!-- How should this feature work? -->

## Alternatives Considered
<!-- Other solutions considered -->

## Additional Context
<!-- Any other context -->
```

## Automated Checks Summary

The pipeline will automatically check:

| Check | Branch | Passing Requirement |
|-------|--------|-------------------|
| build-and-test | All | Must pass |
| code-quality | All | Should improve or maintain |
| security-scan | All | Should not have critical issues |
| lint | All | Warning level |
| build-docker | main/apiv1 | Should build successfully |

## Review Process

1. Create feature branch from `apiv1`
2. Make changes and push
3. Create PR to `apiv1`
4. Automated checks run
5. Code review (1+ approvals required)
6. Merge when all checks pass
7. For production (`main`), require 2 approvals + all checks

## Merge Strategies

- Use: **Squash and merge** for feature branches
- Use: **Create a merge commit** for release merges
- Use: **Rebase and merge** for documentation

## Protected Branches Summary

```
main (Production)
??? Requires 2 approvals
??? Requires all status checks
??? Requires code owners review
??? Admin only push allowed

apiv1 (Development)
??? Requires 1 approval
??? Requires status checks
??? Anyone can push
```

## Notes

- All commits should follow conventional commits format
- Squash commits before merging
- Keep commit history clean
- Delete branch after merging
