include .env


empty:
	echo "empty"

db-sh:
	docker exec -it ${POSTGRES_CONTAINER_NAME} psql -U ${POSTGRES_USER} -d ${POSTGRES_DB}

# Formatting and linting commands
format:
	@echo "Running C# formatter and linter..."
	cd app && dotnet format --verbosity diagnostic

format-whitespace:
	@echo "Formatting whitespace..."
	cd app && dotnet format whitespace --verbosity diagnostic

format-style:
	@echo "Formatting code style..."
	cd app && dotnet format style --verbosity diagnostic

format-analyzers:
	@echo "Running code analyzers..."
	cd app && dotnet format analyzers --verbosity diagnostic

format-all: format-whitespace format-style format-analyzers
	@echo "All formatting completed!"

lint:
	@echo "Running code analysis..."
	cd app && dotnet build --no-incremental

lint-strict:
	@echo "Running strict code analysis (warnings as errors)..."
	cd app && dotnet build --no-incremental --warnaserror

format-check:
	@echo "Checking code formatting (no changes)..."
	cd app && dotnet format --verify-no-changes --verbosity diagnostic

clean:
	cd app && dotnet clean
	cd app && rm -rf bin obj

restore:
	cd app && dotnet restore