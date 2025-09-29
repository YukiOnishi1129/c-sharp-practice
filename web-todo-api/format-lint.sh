#!/bin/bash

# Colors for output
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
RED='\033[0;31m'
NC='\033[0m' # No Color

echo -e "${GREEN}Starting C# formatting and linting...${NC}\n"

# Navigate to the app directory
cd app || exit

# Restore packages
echo -e "${YELLOW}Restoring packages...${NC}"
dotnet restore

# Build the project to trigger analyzers
echo -e "\n${YELLOW}Building project with analyzers...${NC}"
dotnet build --no-restore

# Run dotnet format
echo -e "\n${YELLOW}Running dotnet format...${NC}"

# Format whitespace
echo "Formatting whitespace..."
dotnet format whitespace --no-restore --verbosity diagnostic

# Format code style
echo -e "\nFormatting code style..."
dotnet format style --no-restore --verbosity diagnostic

# Run analyzers and apply fixes
echo -e "\n${YELLOW}Running code analyzers...${NC}"
dotnet format analyzers --no-restore --verbosity diagnostic

# Final build to check for any remaining issues
echo -e "\n${YELLOW}Final build check...${NC}"
dotnet build --no-restore --warnaserror

if [ $? -eq 0 ]; then
    echo -e "\n${GREEN}✓ Formatting and linting completed successfully!${NC}"
else
    echo -e "\n${RED}✗ There are still some issues that need manual fixing.${NC}"
    exit 1
fi