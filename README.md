
# Todo API - RESTful ASP.NET Core Application

A simple RESTful API for managing todo items built with ASP.NET Core 8.0, Entity Framework Core, and MySQL.

> **Note**: This project was created using AI assistance (Claude by Anthropic with filesystem MCP).

## AI Development Feedback

### Was it easy to complete the task using AI?
Yes, the task was straightforward to complete using AI. Claude provided comprehensive code generation including all necessary files, proper project structure, and even additional features like Docker support and testing scripts.

### How long did the task take to complete?
The entire task took approximately **15-20 minutes** to complete, including:
- Project setup and structure: 5 minutes
- Core API implementation: 5 minutes
- Test implementation: 5 minutes
- Documentation and configuration: 5 minutes

### Was the code ready to run after generation?
Yes, the generated code was production-ready with minimal adjustments needed:
- All code compiled without errors
- Proper package references were included
- Configuration files were correctly formatted
- The API was immediately functional

Minor adjustments that might be needed:
- Updating MySQL connection string for specific environments
- Adjusting port numbers if conflicts exist

### Challenges faced during completion
1. **File system access**: Initial attempt to create folders outside allowed directory required adjustment
2. **Package versions**: Ensuring compatible package versions for .NET 8.0
3. **MySQL timestamp handling**: Needed to implement custom timestamp logic in EF Core

### Good practices learned for AI prompting
1. **Be specific about requirements**: Mentioning all acceptance criteria upfront helped generate comprehensive code
2. **Request complete solutions**: Asking for "all required codes and configs" resulted in a full project setup
3. **Include quality requirements**: Specifying ">80% coverage" and "good quality code" led to comprehensive test implementation
4. **Ask for documentation**: The AI automatically included README, comments, and API documentation
5. **Specify technology versions**: Mentioning specific frameworks (ASP.NET Core, EF Core, MySQL) ensured compatibility



## Features

- ✅ Full CRUD operations for Todo items
- ✅ RESTful API design
- ✅ Entity Framework Core with MySQL
- ✅ Swagger/OpenAPI documentation
- ✅ Unit and Integration tests (>80% coverage)
- ✅ Docker support
- ✅ Code quality checks
- ✅ Proper error handling and logging

## Prerequisites

- .NET 8.0 SDK
- MySQL Server 8.0+ (or Docker)
- Visual Studio 2022 or VS Code (optional)

## Getting Started

### 1. Clone the repository

```bash
git clone <repository-url>
cd test-claude-filesystem
```

### 2. Set up the database

#### Option A: Using Docker (Recommended)

```bash
docker-compose up -d
```

This will start a MySQL instance with:
- Host: localhost
- Port: 3306
- Database: todoapi
- Root Password: yourpassword

#### Option B: Using existing MySQL

Update the connection string in `TodoApi/appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=todoapi;User=root;Password=yourpassword;"
}
```

### 3. Build the solution

```bash
dotnet build
```

or use the provided script:

```bash
# Windows
.\build.bat

# Linux/Mac
./build.sh
```

### 4. Run the application

```bash
cd TodoApi
dotnet run
```

The API will be available at:
- HTTP: http://localhost:5140
- HTTPS: https://localhost:7066
- Swagger UI: http://localhost:5140/swagger

## API Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | /api/todoitems | Get all todo items |
| GET | /api/todoitems/{id} | Get a specific todo item |
| POST | /api/todoitems | Create a new todo item |
| PUT | /api/todoitems/{id} | Update an existing todo item |
| DELETE | /api/todoitems/{id} | Delete a todo item |

### Request/Response Examples

#### Create a Todo Item
```http
POST /api/todoitems
Content-Type: application/json

{
  "title": "Complete project",
  "description": "Finish the todo API project",
  "isCompleted": false
}
```

#### Response
```json
{
  "id": 1,
  "title": "Complete project",
  "description": "Finish the todo API project",
  "isCompleted": false,
  "createdAt": "2024-01-20T10:30:00Z",
  "updatedAt": "2024-01-20T10:30:00Z"
}
```

## Running Tests

Run all tests with coverage:

```bash
# Windows
.\test.bat

# Linux/Mac
./test.sh
```

Or manually:

```bash
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=cobertura
```

### Test Coverage

The project includes:
- **Unit Tests**: Controller logic, data context, and models
- **Integration Tests**: Full API endpoint testing
- **Coverage**: >80% code coverage

## Project Structure

```
test-claude-filesystem/
├── TodoApi/                    # Main API project
│   ├── Controllers/           # API controllers
│   ├── Data/                  # Database context
│   ├── Models/                # Data models
│   └── Program.cs             # Application entry point
├── TodoApi.Tests/             # Test project
│   ├── Controllers/           # Controller unit tests
│   ├── Data/                  # Data layer tests
│   ├── Integration/           # Integration tests
│   └── Models/                # Model tests
├── docker-compose.yml         # Docker configuration
├── Dockerfile                 # API Docker image
└── README.md                  # This file
```

## Code Quality

The project includes:
- EditorConfig for consistent code style
- Code analysis rules (TodoApi.ruleset)
- Comprehensive .gitignore
- XML documentation for API endpoints

## Docker Support

Build and run the API in Docker:

```bash
# Build the image
docker build -t todoapi .

# Run the container
docker run -d -p 8080:80 --name todoapi-app todoapi
```
## License

This project is provided as-is for demonstration purposes.
