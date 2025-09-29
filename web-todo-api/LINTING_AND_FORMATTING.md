# C# Linting and Formatting Setup

This project is configured with comprehensive C# code analysis and formatting tools to maintain code quality and consistency.

## Tools Installed

1. **EditorConfig** - Maintains consistent coding styles across different editors and IDEs
2. **dotnet-format** - Built-in .NET formatting tool
3. **StyleCop.Analyzers** - Enforces StyleCop C# coding standards
4. **Microsoft.CodeAnalysis.NetAnalyzers** - Official .NET code quality analyzers
5. **Roslynator.Analyzers** - Additional code analysis and refactoring suggestions
6. **SonarAnalyzer.CSharp** - Detects bugs, vulnerabilities, and code smells

## Configuration Files

- `.editorconfig` - Defines coding style rules and formatting preferences
- `stylecop.json` - StyleCop specific configuration
- `.globalconfig` - Global analyzer configuration and rule severities

## Usage

### Using Make commands (Recommended)

```bash
# Run all formatters in sequence
make format-all

# Format whitespace only
make format-whitespace

# Format code style
make format-style

# Run code analyzers
make format-analyzers

# Run basic linting (build with analyzers)
make lint

# Run strict linting (warnings as errors)
make lint-strict

# Check formatting without making changes
make format-check

# Clean build artifacts
make clean

# Restore packages
make restore
```

### Using shell scripts

**On macOS/Linux:**
```bash
./format-lint.sh
```

**On Windows (PowerShell):**
```powershell
./format-lint.ps1
```

### Using dotnet CLI directly

```bash
cd app

# Format whitespace
dotnet format whitespace

# Format code style  
dotnet format style

# Run analyzers
dotnet format analyzers

# Check formatting without changes
dotnet format --verify-no-changes

# Build with analyzers
dotnet build
```

## Analyzer Rules

The project enforces several categories of rules:

- **Code Style**: Naming conventions, spacing, indentation
- **Code Quality**: Potential bugs, performance issues, security vulnerabilities
- **Maintainability**: Code complexity, documentation requirements
- **Reliability**: Null reference handling, exception handling

### Key Rules Enforced

- Nullable reference types are enabled
- Certain null-related warnings are treated as errors (CS8600, CS8602, CS8603)
- Interface names must begin with 'I'
- Private fields should be prefixed with underscore
- Async methods should end with 'Async' suffix
- Proper indentation (4 spaces, no tabs)
- Consistent brace placement and spacing

### Suppressed Rules

Some rules are intentionally suppressed in `.globalconfig`:
- SA1101: Prefix local calls with 'this'
- SA1600: Elements should be documented (XML comments)
- CA1303: Do not pass literals as localized parameters
- CS1591: Missing XML comment for publicly visible members

## Integration with CI/CD

To integrate with your CI/CD pipeline, add these steps:

```yaml
# Example for GitHub Actions
- name: Restore dependencies
  run: dotnet restore ./app
  
- name: Check formatting
  run: dotnet format --verify-no-changes ./app
  
- name: Build with analyzers
  run: dotnet build --warnaserror ./app
```

## Customization

To modify analyzer rules:

1. Edit `.globalconfig` to change rule severities
2. Update `.editorconfig` for style preferences
3. Modify `stylecop.json` for StyleCop-specific settings

Rule severity levels:
- `error` - Violation fails the build
- `warning` - Shows as warning
- `suggestion` - Shows as suggestion in IDE
- `none` - Rule is disabled

## Troubleshooting

**Build fails due to analyzer violations:**
- Run `make format-all` to auto-fix most issues
- Check build output for specific violations that need manual fixing

**Format command doesn't fix all issues:**
- Some issues require manual intervention
- Check the diagnostic output for specific file locations

**Analyzer rules conflict:**
- Check `.globalconfig` for conflicting rule configurations
- Ensure all config files are properly referenced in the .csproj file