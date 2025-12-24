#!/bin/bash
# Setup development environment

echo "?? Setting up FirstAid API development environment..."

# Colors
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
NC='\033[0m' # No Color

# Check .NET installation
echo -e "${YELLOW}Checking .NET installation...${NC}"
if ! command -v dotnet &> /dev/null; then
    echo -e "${RED}? .NET CLI not found. Please install .NET 8 SDK${NC}"
    echo "Visit: https://dotnet.microsoft.com/download"
    exit 1
fi
echo -e "${GREEN}? .NET $(dotnet --version) found${NC}"

# Check PostgreSQL
echo -e "${YELLOW}Checking PostgreSQL...${NC}"
if ! command -v psql &> /dev/null; then
    echo -e "${RED}? PostgreSQL not found. Please install PostgreSQL 15${NC}"
    echo "Visit: https://www.postgresql.org/download"
    exit 1
else
    echo -e "${GREEN}? PostgreSQL found${NC}"
fi

# Setup git hooks
echo -e "${YELLOW}Setting up git hooks...${NC}"
if [ -d ".git" ]; then
    mkdir -p .git/hooks
    if [ -f ".githooks/pre-commit" ]; then
        cp .githooks/pre-commit .git/hooks/
        chmod +x .git/hooks/pre-commit
        echo -e "${GREEN}? Pre-commit hook installed${NC}"
    fi
else
    echo -e "${YELLOW}??  .git directory not found${NC}"
fi

# Create appsettings.Development.json if not exists
if [ ! -f "appsettings.Development.json" ]; then
    echo -e "${YELLOW}Creating appsettings.Development.json...${NC}"
    cat > appsettings.Development.json << 'EOF'
{
  "Logging": {
    "LogLevel": {
      "Default": "Information"
    }
  },
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=firstaid_dev;Username=postgres;Password=postgres;"
  },
  "JWT": {
    "Secret": "your-super-secret-key-change-this-in-production-at-least-32-chars",
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
EOF
    echo -e "${GREEN}? appsettings.Development.json created${NC}"
    echo -e "${YELLOW}??  Please update it with your actual values${NC}"
fi

# Restore NuGet packages
echo -e "${YELLOW}Restoring NuGet packages...${NC}"
dotnet restore
if [ $? -eq 0 ]; then
    echo -e "${GREEN}? Packages restored${NC}"
else
    echo -e "${RED}? Failed to restore packages${NC}"
    exit 1
fi

# Run database migrations
echo -e "${YELLOW}Checking database...${NC}"
echo "Make sure PostgreSQL is running!"
echo ""
echo "Then run: dotnet ef database update"

echo -e "${GREEN}? Development environment setup complete!${NC}"
echo ""
echo "Next steps:"
echo "1. Update appsettings.Development.json with your settings"
echo "2. Start PostgreSQL service"
echo "3. Run migrations (dotnet ef database update)"
echo "4. Start development server (dotnet run)"
echo ""
echo "Happy coding! ??"
