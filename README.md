# Task Management System

A comprehensive task management API built with .NET 10, ASP.NET Core, and Entity Framework Core. Features advanced search and filtering with pagination, structured logging, and enterprise-grade error handling.

## Prerequisites

- .NET 10 SDK
- SQL Server (LocalDB or Express)
- Visual Studio 2022 or VS Code

## Project Structure

```
Solution: Task&TeamManagementSystem/
??? Api/                    # ASP.NET Core API
??? Application/            # Business logic & MediatR handlers
??? Persistence/            # Entity Framework Core & DbContext
??? Domain/                 # DTOs and domain models
```

## Setup Instructions

### 1. Database Configuration

Update the connection string in `Api/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=TaskManagementDb;Trusted_Connection=true;"
  }
}
```

### 2. Restore Dependencies

```bash
dotnet restore
```

### 3. Create Database & Run Migrations

```bash
# Navigate to the solution directory
cd E:\Temp\Random\TaskManagement\Task&TeamManagementSystem

# Apply migrations
dotnet ef database update --project Persistence --startup-project Api
```

The database will be automatically seeded with:
- Admin, Manager, and Employee roles
- Test users
- Task statuses

### 4. Configure JWT Token

In `Api/appsettings.json`, ensure `TokenKey` is set:

```json
{
  "TokenKey": "your-super-secret-key-change-this-in-production"
}
```

## Running the Application

### Via Visual Studio

1. Set `Api` as the startup project
2. Press `F5` or click "Run"
3. API will start at `https://localhost:7208`

### Via Command Line

```bash
cd Api
dotnet run
```

API will be available at `https://localhost:7208`

## API Endpoints

### Authentication
- `POST /api/auth/login` - Login user
- `POST /api/auth/register` - Register new user
- `POST /api/auth/refresh-token` - Refresh JWT token

### Work Tasks
- `GET /api/worktasks` - Get all tasks
- `GET /api/worktasks/{id}` - Get task by ID
- `GET /api/worktasks/team/{teamId}` - Get tasks by team
- `POST /api/worktasks/search` - Search and filter tasks with pagination
- `POST /api/worktasks` - Create new task
- `PUT /api/worktasks/{id}` - Update task
- `DELETE /api/worktasks/{id}` - Delete task
- `GET /api/worktasks/my-tasks` - Get current user's tasks
- `PATCH /api/worktasks/{id}/status` - Update task status

### Teams
- `GET /api/teams` - Get all teams
- `GET /api/teams/{id}` - Get team by ID
- `POST /api/teams` - Create team
- `PUT /api/teams/{id}` - Update team
- `DELETE /api/teams/{id}` - Delete team

## Search & Filter Endpoint

### POST /api/worktasks/search

Search and filter tasks with pagination.

**Request Body:**
```json
{
  "statusId": 2,
  "assignedToUserId": "user-id",
  "teamId": 5,
  "dueDateFrom": "2024-01-01T00:00:00Z",
  "dueDateTo": "2024-12-31T23:59:59Z",
  "pageNumber": 1,
  "pageSize": 10
}
```

**Response:**
```json
{
  "items": [
    {
      "id": 1,
      "title": "Task Title",
      "description": "Task Description",
      "statusId": 2,
      "statusName": "In Progress",
      "teamId": 5,
      "teamName": "Backend Team",
      "assignedToUserId": "user-456",
      "assignedToUserName": "john.doe",
      "createdByUserId": "user-123",
      "createdByUserName": "admin",
      "dueDate": "2024-02-15T00:00:00Z"
    }
  ],
  "totalCount": 25,
  "pageNumber": 1,
  "pageSize": 10,
  "totalPages": 3,
  "hasPreviousPage": false,
  "hasNextPage": true
}
```

**Filter Parameters:**
- `statusId` (optional) - Filter by status ID
- `assignedToUserId` (optional) - Filter by assigned user
- `teamId` (optional) - Filter by team
- `dueDateFrom` (optional) - Start of due date range
- `dueDateTo` (optional) - End of due date range
- `pageNumber` (required, default: 1) - Page number
- `pageSize` (required, default: 10) - Items per page (1-100)

## Authentication

### Getting a Token

```bash
curl -X POST https://localhost:7208/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{
    "email": "admin@example.com",
    "password": "password123"
  }'
```

### Using the Token

Add to request headers:
```
Authorization: Bearer YOUR_JWT_TOKEN
```

## Features

### ? Search & Filter
- Filter by status, assigned user, team, and due date
- Combine multiple filters with AND logic
- All filters are optional

### ? Pagination
- Configurable page size (1-100)
- Auto-correcting validation
- Navigation helpers (hasPreviousPage, hasNextPage)
- Total page count calculation

### ? Structured Logging
- Serilog integration
- Console and file output (rolling daily)
- 30-day log retention
- Context enrichment (user, request ID, machine name)

### ? Error Handling
- Global exception middleware
- Structured error responses
- Automatic exception logging
- Proper HTTP status codes

### ? Authorization
- JWT-based authentication
- Role-based access control (Admin, Manager, Employee)
- Policy-based authorization

### ? Fluent Validation
- Automatic input validation
- MediatR pipeline integration
- Comprehensive validation rules
- User-friendly error messages

---

## Input Validation

The application uses **FluentValidation** for comprehensive input validation on all commands and queries.

### Validation Rules

#### CreateWorkTaskCommand
- `Title`: Required, 3-200 characters
- `Description`: Optional, max 1000 characters
- `StatusID`: Required, must be > 0
- `TeamId`: Required, must be > 0
- `AssignedToUserID`: Required, cannot be null
- `DueDate`: Required, cannot be in the past

#### UpdateWorkTaskCommand
- Same rules as CreateWorkTaskCommand
- `Id`: Required, must be > 0

#### DeleteWorkTaskCommand
- `Id`: Required, must be > 0

#### GetFilteredTasksQuery
- `StatusID`: Optional, must be > 0 if provided
- `TeamId`: Optional, must be > 0 if provided
- `PageNumber`: Required, must be > 0
- `PageSize`: Required, 1-100 items per page
- `DueDateFrom` and `DueDateTo`: DueDateFrom cannot be greater than DueDateTo

#### UpdateTaskStatusCommand
- `Id`: Required, must be > 0
- `StatusID`: Required, must be > 0
- `UserId`: Required, cannot be null

### Error Response

When validation fails, the API returns a structured error response:

```json
{
  "statusCode": 400,
  "message": "Validation failed",
  "errors": {
    "Title": ["Title is required.", "Title must be at least 3 characters long."],
    "StatusID": ["Status ID must be greater than 0."]
  }
}
```

---

## Testing

### Quick Test with cURL

```bash
# 1. Get login token
curl -X POST https://localhost:7208/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{
    "email": "admin@example.com",
    "password": "password123"
  }'

# 2. Search tasks (use token from step 1)
curl -X POST https://localhost:7208/api/worktasks/search \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer YOUR_TOKEN" \
  -d '{
    "pageNumber": 1,
    "pageSize": 10
  }'
```

### Using Postman

1. Import the API endpoints into Postman
2. Set up environment variables for `base_url` and `token`
3. Create a login request to get JWT token
4. Use token in Authorization header for authenticated requests

## Logging

Logs are stored in `logs/` directory with rolling daily files.

**Log Levels:**
- Information: Normal operations
- Warning: Unexpected but recoverable events
- Error: Operation failures
- Debug: Detailed diagnostic info (development only)

**Example Log Output:**
```
[14:30:45 INF] HTTP Request started: POST /api/worktasks/search
[14:30:45 INF] Executing task search with filters: {@FilterSummary}, Page: 1, PageSize: 10
[14:30:45 INF] Total tasks matching filters: 25
[14:30:45 INF] Search completed: Found 10 tasks on page 1 of 3
[14:30:45 INF] HTTP Request completed: POST /api/worktasks/search - Status 200 - Duration 125ms
```

## Database Optimization

### Recommended Indexes

Create these indexes for optimal performance:

```sql
CREATE INDEX IX_WorkTasks_StatusID ON WorkTasks(StatusID);
CREATE INDEX IX_WorkTasks_AssignedToUserID ON WorkTasks(AssignedToUserID);
CREATE INDEX IX_WorkTasks_TeamId ON WorkTasks(TeamId);
CREATE INDEX IX_WorkTasks_DueDate ON WorkTasks(DueDate);
CREATE INDEX IX_WorkTasks_StatusTeamDueDate ON WorkTasks(StatusID, TeamId, DueDate);
```

## Performance

Expected response times (with optimized indexes):

| Scenario | Time |
|----------|------|
| No filters | ~50ms |
| Single filter | ~60ms |
| Multiple filters | ~80ms |
| Large page (100 items) | ~120ms |

## Troubleshooting

### Database Connection Error
- Verify SQL Server is running
- Check connection string in appsettings.json
- Ensure database user has appropriate permissions

### JWT Token Expired
- Get a new token using the login endpoint
- Verify TokenKey is configured in appsettings.json

### Slow Query Performance
- Create recommended database indexes
- Check database query execution plan
- Monitor logs for slow queries

### Migration Issues
```bash
# Remove last migration
dotnet ef migrations remove --project Persistence --startup-project Api

# Re-apply migrations
dotnet ef database update --project Persistence --startup-project Api
```

## Configuration

### JWT Token Settings

In `appsettings.json`:
```json
{
  "TokenSettings": {
    "AccessTokenExpirationMinutes": 15,
    "RefreshTokenExpirationDays": 7
  }
}
```

### Serilog Configuration

In `appsettings.json`:
```json
{
  "Serilog": {
    "MinimumLevel": {
      "Default": "Information",
      "Override": {
        "Microsoft.EntityFrameworkCore": "Warning"
      }
    },
    "WriteTo": [
      {
        "Name": "Console"
      },
      {
        "Name": "File",
        "Args": {
          "path": "logs/log-.txt",
          "rollingInterval": "Day",
          "retainedFileCountLimit": 30
        }
      }
    ]
  }
}
```

## Security Considerations

1. **Change Default TokenKey**: Update in production environment
2. **Use HTTPS**: Always use HTTPS in production
3. **Validate Input**: All inputs are validated on the server
4. **SQL Injection Prevention**: Using parameterized queries via EF Core
5. **Rate Limiting**: Recommended to implement rate limiting in production

## Architecture

### Design Patterns Used
- **MediatR Pattern**: Command/Query separation
- **Specification Pattern**: Reusable filter logic
- **Repository Pattern**: Data access abstraction
- **Dependency Injection**: ASP.NET Core built-in DI
- **DTO Pattern**: Data transfer objects

### Technology Stack
- **.NET 10** - Runtime
- **ASP.NET Core 10** - Web framework
- **Entity Framework Core 10** - ORM
- **MediatR 14** - Command/Query handling
- **Serilog 8** - Structured logging
- **JWT** - Authentication
- **SQL Server** - Database

## Development

### Build Solution
```bash
dotnet build
```

### Run Tests
```bash
dotnet test
```

### Watch Mode (Development)
```bash
cd Api
dotnet watch run
```

## Production Deployment

1. **Update Configuration**: Set production-specific values in appsettings.Production.json
2. **Enable HTTPS**: Configure SSL certificates
3. **Database**: Use SQL Server in production (not LocalDB)
4. **Logging**: Configure log aggregation service
5. **Monitoring**: Set up application performance monitoring
6. **Security**: Update JWT token key and configure CORS

## License

This project is proprietary and confidential.

## Support

For issues or questions:
1. Check the application logs in `logs/` directory
2. Verify database connectivity
3. Ensure JWT token is valid and not expired
4. Review error response messages from API

## Version

- **.NET**: 10.0
- **C#**: 14.0
- **ASP.NET Core**: 10.0
