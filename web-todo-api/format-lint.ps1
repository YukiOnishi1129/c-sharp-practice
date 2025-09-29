# PowerShell script for formatting and linting C# code

Write-Host "Starting C# formatting and linting..." -ForegroundColor Green
Write-Host ""

# Navigate to the app directory
Set-Location -Path "app"

# Restore packages
Write-Host "Restoring packages..." -ForegroundColor Yellow
dotnet restore

# Build the project to trigger analyzers
Write-Host ""
Write-Host "Building project with analyzers..." -ForegroundColor Yellow
dotnet build --no-restore

# Run dotnet format
Write-Host ""
Write-Host "Running dotnet format..." -ForegroundColor Yellow

# Format whitespace
Write-Host "Formatting whitespace..."
dotnet format whitespace --no-restore --verbosity diagnostic

# Format code style
Write-Host ""
Write-Host "Formatting code style..."
dotnet format style --no-restore --verbosity diagnostic

# Run analyzers and apply fixes
Write-Host ""
Write-Host "Running code analyzers..." -ForegroundColor Yellow
dotnet format analyzers --no-restore --verbosity diagnostic

# Final build to check for any remaining issues
Write-Host ""
Write-Host "Final build check..." -ForegroundColor Yellow
$buildResult = dotnet build --no-restore --warnaserror

if ($LASTEXITCODE -eq 0) {
    Write-Host ""
    Write-Host "✓ Formatting and linting completed successfully!" -ForegroundColor Green
} else {
    Write-Host ""
    Write-Host "✗ There are still some issues that need manual fixing." -ForegroundColor Red
    exit 1
}