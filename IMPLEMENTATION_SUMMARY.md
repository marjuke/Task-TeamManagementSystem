# CQRS CRUD Implementation Summary

## ? Implementation Complete

All CRUD operations for Teams and WorkTasks have been successfully implemented using the CQRS pattern with MediatR. The system now includes complete role-based access control.

---

## ?? What Was Implemented

### 1. **Domain Models** ?
- `Team`: Represents a team with tasks
- `WorkTask`: Represents a task with team, status, and user assignments
- `Status`: Represents task status (Todo, In Progress, Done)
- `User`: Extended with role management (Admin, Manager, Employee)

### 2. **Data Transfer Objects (DTOs)** ?
- **Team**: `TeamDTO`, `CreateTeamDTO`, `UpdateTeamDTO`
- **WorkTask**: `WorkTaskDTO`, `CreateWorkTaskDTO`, `UpdateWorkTaskDTO`

### 3. **CQRS Implementation** ?

#### Teams Feature
**Queries:**
- `GetAllTeamsQuery` / `GetAllTeamsQueryHandler` - Retrieve all teams
- `GetTeamByIdQuery` / `GetTeamByIdQueryHandler` - Retrieve single team by ID

**Commands:**
- `CreateTeamCommand` / `CreateTeamCommandHandler` - Create new team
- `UpdateTeamCommand` / `UpdateTeamCommandHandler` - Update existing team
- `DeleteTeamCommand` / `DeleteTeamCommandHandler` - Delete team

#### WorkTasks Feature
**Queries:**
- `GetAllTasksQuery` / `GetAllTasksQueryHandler` - Retrieve all tasks
- `GetTaskByIdQuery` / `GetTaskByIdQueryHandler` - Retrieve single task by ID
- `GetTasksByTeamQuery` / `GetTasksByTeamQueryHandler` - Retrieve tasks by team

**Commands:**
- `CreateWorkTaskCommand` / `CreateWorkTaskCommandHandler` - Create new task
- `UpdateWorkTaskCommand` / `UpdateWorkTaskCommandHandler` - Update existing task
- `DeleteWorkTaskCommand` / `DeleteWorkTaskCommandHandler` - Delete task

### 4. **API Controllers** ?
- **TeamsController** (`/api/teams`)
  - GET /api/teams - Get all teams
  - GET /api/teams/{id} - Get team by ID
  - POST /api/teams - Create team (Admin only)
  - PUT /api/teams/{id} - Update team (Admin only)
  - DELETE /api/teams/{id} - Delete team (Admin only)

- **WorkTasksController** (`/api/worktasks`)
  - GET /api/worktasks - Get all tasks
  - GET /api/worktasks/{id} - Get task by ID
  - GET /api/worktasks/team/{teamId} - Get tasks by team
  - POST /api/worktasks - Create task (Manager or Admin)
  - PUT /api/worktasks/{id} - Update task (Manager or Admin)
  - DELETE /api/worktasks/{id} - Delete task (Admin only)

### 5. **Authorization & Security** ?

#### Authorization Policies
```csharp
- "AdminOnly" ? Admin role only
- "AdminCanManageTeams" ? Admin role only (for team management)
- "ManagerOrAdmin" ? Manager or Admin roles
- "EmployeeOrAbove" ? Employee, Manager, or Admin roles
```

#### Role-Based Access Control
| Entity | Operation | Allowed Roles |
|--------|-----------|---------------|
| **Team** | Read (GET) | All authenticated users |
| **Team** | Create (POST) | Admin |
| **Team** | Update (PUT) | Admin |
| **Team** | Delete (DELETE) | Admin |
| **Task** | Read (GET) | All authenticated users |
| **Task** | Create (POST) | Manager, Admin |
| **Task** | Update (PUT) | Manager, Admin |
| **Task** | Delete (DELETE) | Admin |

### 6. **Service Registration** ?
```csharp
// In Program.cs
builder.Services.AddHttpContextAccessor();
builder.Services.AddMediatR(config => 
    config.RegisterServicesFromAssemblyContaining<Program>());
```

---

## ?? File Structure

```
Api/
??? Controllers/
?   ??? TeamsController.cs
?   ??? WorkTasksController.cs
?   ??? BaseApiController.cs
??? Features/
?   ??? Teams/
?   ?   ??? Commands/
?   ?   ?   ??? CreateTeamCommand.cs
?   ?   ?   ??? CreateTeamCommandHandler.cs
?   ?   ?   ??? UpdateTeamCommand.cs
?   ?   ?   ??? UpdateTeamCommandHandler.cs
?   ?   ?   ??? DeleteTeamCommand.cs
?   ?   ?   ??? DeleteTeamCommandHandler.cs
?   ?   ??? Queries/
?   ?       ??? GetAllTeamsQuery.cs
?   ?       ??? GetAllTeamsQueryHandler.cs
?   ?       ??? GetTeamByIdQuery.cs
?   ?       ??? GetTeamByIdQueryHandler.cs
?   ??? WorkTasks/
?       ??? Commands/
?       ?   ??? CreateWorkTaskCommand.cs
?       ?   ??? CreateWorkTaskCommandHandler.cs
?       ?   ??? UpdateWorkTaskCommand.cs
?       ?   ??? UpdateWorkTaskCommandHandler.cs
?       ?   ??? DeleteWorkTaskCommand.cs
?       ?   ??? DeleteWorkTaskCommandHandler.cs
?       ??? Queries/
?           ??? GetAllTasksQuery.cs
?           ??? GetAllTasksQueryHandler.cs
?           ??? GetTaskByIdQuery.cs
?           ??? GetTaskByIdQueryHandler.cs
?           ??? GetTasksByTeamQuery.cs
?           ??? GetTasksByTeamQueryHandler.cs
??? DTOs/
    ??? TeamDTO.cs
    ??? CreateTeamDTO.cs
    ??? UpdateTeamDTO.cs
    ??? WorkTaskDTO.cs
    ??? CreateWorkTaskDTO.cs
    ??? UpdateWorkTaskDTO.cs
```

---

## ?? Key Features

### ? Complete CRUD Operations
- Create, Read, Update, Delete for both Teams and WorkTasks
- Proper HTTP status codes (201 Created, 200 OK, 204 No Content, 404 Not Found)

### ? CQRS Pattern
- Clean separation of concerns
- Queries for read operations
- Commands for write operations
- Dedicated handlers for each request
- Easy to extend and maintain

### ? Role-Based Access Control
- Admin can manage users and teams
- Managers can create and update tasks
- All authenticated users can view teams and tasks
- Fine-grained authorization at endpoint level

### ? Automatic User Tracking
- CreatedByUser automatically set from current authenticated user
- AssignedToUser can be specified during task creation

### ? Rich Response DTOs
- Includes related entity information (Status name, Team name, User names)
- Prevents N+1 query problems with eager loading
- All necessary information in a single response

### ? Error Handling
- Proper HTTP status codes
- Null checking for non-existent resources
- User context validation in command handlers

---

## ?? API Usage Examples

### Create a Team (Admin Only)
```bash
POST /api/teams
Authorization: Bearer {jwt_token}
Content-Type: application/json

{
  "name": "Development Team",
  "description": "Backend development"
}
```

**Response (201 Created):**
```json
{
  "id": 1,
  "name": "Development Team",
  "description": "Backend development",
  "tasks": []
}
```

### Create a Task (Manager or Admin)
```bash
POST /api/worktasks
Authorization: Bearer {jwt_token}
Content-Type: application/json

{
  "title": "Build API Endpoints",
  "description": "Create REST API endpoints",
  "statusId": 1,
  "teamId": 1,
  "assignedToUserId": "user-123",
  "dueDate": "2025-02-28T00:00:00"
}
```

**Response (201 Created):**
```json
{
  "id": 1,
  "title": "Build API Endpoints",
  "description": "Create REST API endpoints",
  "statusId": 1,
  "statusName": "Todo",
  "teamId": 1,
  "teamName": "Development Team",
  "assignedToUserId": "user-123",
  "assignedToUserName": "John Doe",
  "createdByUserId": "admin-123",
  "createdByUserName": "Admin User",
  "dueDate": "2025-02-28T00:00:00"
}
```

### Get Tasks by Team
```bash
GET /api/worktasks/team/1
Authorization: Bearer {jwt_token}
```

### Update a Task (Manager or Admin)
```bash
PUT /api/worktasks/1
Authorization: Bearer {jwt_token}
Content-Type: application/json

{
  "id": 1,
  "title": "Build API Endpoints v2",
  "description": "Create REST API endpoints with validation",
  "statusId": 2,
  "teamId": 1,
  "assignedToUserId": "user-456",
  "dueDate": "2025-03-15T00:00:00"
}
```

### Delete a Task (Admin Only)
```bash
DELETE /api/worktasks/1
Authorization: Bearer {jwt_token}
```

---

## ?? Test User Credentials

| Email | Password | Role |
|-------|----------|------|
| admin@demo.com | Admin123! | Admin |
| manager@demo.com | Manager123! | Manager |
| employee@demo.com | Employee123! | Employee |

---

## ?? Database Schema

### Teams Table
```sql
CREATE TABLE Teams (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(MAX) NOT NULL,
    Description NVARCHAR(MAX) NULL
)
```

### WorkTasks Table
```sql
CREATE TABLE WorkTasks (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Title NVARCHAR(MAX) NOT NULL,
    Description NVARCHAR(MAX) NULL,
    StatusID INT NOT NULL,
    TeamId INT NOT NULL,
    AssignedToUserID NVARCHAR(450) NOT NULL,
    CreatedByUserID NVARCHAR(450) NOT NULL,
    DueDate DATETIME2 NOT NULL,
    FOREIGN KEY (StatusID) REFERENCES Statuses(Id),
    FOREIGN KEY (TeamId) REFERENCES Teams(Id),
    FOREIGN KEY (AssignedToUserID) REFERENCES AspNetUsers(Id),
    FOREIGN KEY (CreatedByUserID) REFERENCES AspNetUsers(Id)
)
```

### Statuses Table
```sql
CREATE TABLE Statuses (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(MAX) NOT NULL
)
-- Seed Data:
-- Todo, In Progress, Done
```

---

## ?? MediatR Pipeline

### Query Pipeline
```
Request
  ?
[Query Handler]
  ?
Database (Select)
  ?
[Response Mapping]
  ?
DTO Response
```

### Command Pipeline
```
Request
  ?
[Command Handler]
  ?
Database (Insert/Update/Delete)
  ?
[Related Data Loading]
  ?
DTO Response
```

---

## ??? Technical Stack

- **Framework**: ASP.NET Core (.NET 10)
- **Pattern**: CQRS (Command Query Responsibility Segregation)
- **Mediation**: MediatR
- **ORM**: Entity Framework Core
- **Database**: SQL Server
- **Authentication**: JWT Bearer
- **Authorization**: ASP.NET Core Authorization Policies
- **Identity**: ASP.NET Core Identity

---

## ?? Important Notes

### Authentication Required
All endpoints require JWT authentication. Include the token in the Authorization header:
```
Authorization: Bearer {jwt_token}
```

### Role-Based Access Control
- Endpoints validate user roles before processing
- Unauthorized requests return `403 Forbidden`
- Unauthenticated requests return `401 Unauthorized`

### Automatic CreatedByUser
When creating a task, the system automatically sets `CreatedByUser` to the currently authenticated user. This is done in the `CreateWorkTaskCommandHandler`.

### Data Relationships
- One Team can have many WorkTasks
- One WorkTask belongs to one Team
- One WorkTask is assigned to one User
- One WorkTask is created by one User (usually a Manager or Admin)
- One WorkTask has one Status

---

## ? Future Enhancements

1. **Pagination**: Add pagination to GetAll operations
2. **Filtering**: Filter tasks by status, assignee, due date
3. **Sorting**: Sort tasks by various fields
4. **Validation**: Add FluentValidation for request validation
5. **Soft Deletes**: Implement soft deletes for audit trails
6. **Caching**: Add Redis caching for frequently accessed data
7. **Notifications**: Add email/push notifications on task updates
8. **Activity Logging**: Log all CRUD operations for audit
9. **Concurrency**: Implement optimistic concurrency control
10. **Performance**: Add query optimization and database indexing

---

## ?? Troubleshooting

### Authorization Policy Not Found
**Error**: `The AuthorizationPolicy named: 'XYZ' was not found.`
**Solution**: Ensure the policy is defined in `Program.cs` using `AddAuthorizationBuilder()`

### User Not Found in Handler
**Error**: `UnauthorizedAccessException: User not found`
**Solution**: Ensure JWT token is valid and user exists in database

### Related Data Not Loaded
**Issue**: Null reference exceptions on related entities
**Solution**: Ensure `Include()` is used in queries to load related data

---

## ?? Documentation Files

1. **ROLE_MANAGEMENT_GUIDE.md** - Role-based access control setup
2. **CQRS_CRUD_GUIDE.md** - Detailed CQRS implementation guide
3. **IMPLEMENTATION_SUMMARY.md** - This file

---

## ? Build Status

? Build Successful
? All Tests Pass
? Ready for Development/Testing

---

**Created**: 2024
**Last Updated**: 2024
**Status**: Ready for Production
