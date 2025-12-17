# CQRS CRUD Operations for Teams and Tasks

## Overview
This document describes the CQRS (Command Query Responsibility Segregation) pattern implementation for Teams and WorkTasks management in the Task & Team Management System. The implementation uses MediatR for mediation and includes complete CRUD operations with role-based access control.

## Architecture

### Pattern: CQRS (Command Query Responsibility Segregation)
- **Queries**: Handle read operations (GetAll, GetById, GetByTeam)
- **Commands**: Handle write operations (Create, Update, Delete)
- **Handlers**: Implement the business logic for each query/command
- **DTOs**: Data Transfer Objects for API requests/responses

### Directory Structure
```
Api/
??? Features/
?   ??? Teams/
?   ?   ??? Queries/
?   ?   ?   ??? GetAllTeamsQuery.cs
?   ?   ?   ??? GetAllTeamsQueryHandler.cs
?   ?   ?   ??? GetTeamByIdQuery.cs
?   ?   ?   ??? GetTeamByIdQueryHandler.cs
?   ?   ??? Commands/
?   ?       ??? CreateTeamCommand.cs
?   ?       ??? CreateTeamCommandHandler.cs
?   ?       ??? UpdateTeamCommand.cs
?   ?       ??? UpdateTeamCommandHandler.cs
?   ?       ??? DeleteTeamCommand.cs
?   ?       ??? DeleteTeamCommandHandler.cs
?   ??? WorkTasks/
?       ??? Queries/
?       ?   ??? GetAllTasksQuery.cs
?       ?   ??? GetAllTasksQueryHandler.cs
?       ?   ??? GetTaskByIdQuery.cs
?       ?   ??? GetTaskByIdQueryHandler.cs
?       ?   ??? GetTasksByTeamQuery.cs
?       ?   ??? GetTasksByTeamQueryHandler.cs
?       ??? Commands/
?           ??? CreateWorkTaskCommand.cs
?           ??? CreateWorkTaskCommandHandler.cs
?           ??? UpdateWorkTaskCommand.cs
?           ??? UpdateWorkTaskCommandHandler.cs
?           ??? DeleteWorkTaskCommand.cs
?           ??? DeleteWorkTaskCommandHandler.cs
??? Controllers/
?   ??? TeamsController.cs
?   ??? WorkTasksController.cs
??? DTOs/
    ??? TeamDTO.cs
    ??? CreateTeamDTO.cs
    ??? UpdateTeamDTO.cs
    ??? WorkTaskDTO.cs
    ??? CreateWorkTaskDTO.cs
    ??? UpdateWorkTaskDTO.cs
```

## API Endpoints

### Teams Controller (`/api/teams`)

#### GET /api/teams
- **Description**: Get all teams
- **Authorization**: Required
- **Roles**: All authenticated users
- **Response**: `200 OK` with list of TeamDTO

```json
[
  {
    "id": 1,
    "name": "Development Team",
    "description": "Backend development",
    "tasks": []
  }
]
```

#### GET /api/teams/{id}
- **Description**: Get team by ID
- **Authorization**: Required
- **Roles**: All authenticated users
- **Parameters**: 
  - `id` (int): Team ID
- **Response**: `200 OK` or `404 Not Found`

#### POST /api/teams
- **Description**: Create a new team
- **Authorization**: Required
- **Roles**: Manager, Admin
- **Request Body**: CreateTeamDTO
- **Response**: `201 Created` with created TeamDTO

```json
{
  "name": "Design Team",
  "description": "UI/UX design"
}
```

#### PUT /api/teams/{id}
- **Description**: Update an existing team
- **Authorization**: Required
- **Roles**: Manager, Admin
- **Parameters**: 
  - `id` (int): Team ID
- **Request Body**: UpdateTeamDTO
- **Response**: `200 OK` or `404 Not Found`

```json
{
  "id": 1,
  "name": "Design Team Updated",
  "description": "UI/UX design and prototyping"
}
```

#### DELETE /api/teams/{id}
- **Description**: Delete a team
- **Authorization**: Required
- **Roles**: Admin only
- **Parameters**: 
  - `id` (int): Team ID
- **Response**: `204 No Content` or `404 Not Found`

---

### WorkTasks Controller (`/api/worktasks`)

#### GET /api/worktasks
- **Description**: Get all tasks
- **Authorization**: Required
- **Roles**: All authenticated users
- **Response**: `200 OK` with list of WorkTaskDTO

```json
[
  {
    "id": 1,
    "title": "Build API",
    "description": "Create REST API endpoints",
    "statusId": 2,
    "statusName": "In Progress",
    "teamId": 1,
    "teamName": "Development Team",
    "assignedToUserId": "user-id-123",
    "assignedToUserName": "John Doe",
    "createdByUserId": "user-id-456",
    "createdByUserName": "Admin User",
    "dueDate": "2025-01-31T00:00:00"
  }
]
```

#### GET /api/worktasks/{id}
- **Description**: Get task by ID
- **Authorization**: Required
- **Roles**: All authenticated users
- **Parameters**: 
  - `id` (int): Task ID
- **Response**: `200 OK` or `404 Not Found`

#### GET /api/worktasks/team/{teamId}
- **Description**: Get all tasks for a specific team
- **Authorization**: Required
- **Roles**: All authenticated users
- **Parameters**: 
  - `teamId` (int): Team ID
- **Response**: `200 OK` with list of WorkTaskDTO

#### POST /api/worktasks
- **Description**: Create a new task
- **Authorization**: Required
- **Roles**: Manager, Admin
- **Request Body**: CreateWorkTaskDTO
- **Response**: `201 Created` with created WorkTaskDTO

```json
{
  "title": "Design Homepage",
  "description": "Create homepage mockups",
  "statusId": 1,
  "teamId": 2,
  "assignedToUserId": "user-id-789",
  "dueDate": "2025-02-15T00:00:00"
}
```

#### PUT /api/worktasks/{id}
- **Description**: Update an existing task
- **Authorization**: Required
- **Roles**: Manager, Admin
- **Parameters**: 
  - `id` (int): Task ID
- **Request Body**: UpdateWorkTaskDTO
- **Response**: `200 OK` or `404 Not Found`

```json
{
  "id": 1,
  "title": "Build API v2",
  "description": "Create REST API endpoints with validation",
  "statusId": 3,
  "teamId": 1,
  "assignedToUserId": "user-id-123",
  "dueDate": "2025-02-28T00:00:00"
}
```

#### DELETE /api/worktasks/{id}
- **Description**: Delete a task
- **Authorization**: Required
- **Roles**: Admin only
- **Parameters**: 
  - `id` (int): Task ID
- **Response**: `204 No Content` or `404 Not Found`

---

## Role-Based Access Control

### Authorization Policies

| Endpoint | Method | Policy | Allowed Roles |
|----------|--------|--------|---------------|
| /teams | GET | Required Auth | All Users |
| /teams/{id} | GET | Required Auth | All Users |
| /teams | POST | ManagerOrAdmin | Manager, Admin |
| /teams/{id} | PUT | ManagerOrAdmin | Manager, Admin |
| /teams/{id} | DELETE | AdminOnly | Admin |
| /worktasks | GET | Required Auth | All Users |
| /worktasks/{id} | GET | Required Auth | All Users |
| /worktasks/team/{teamId} | GET | Required Auth | All Users |
| /worktasks | POST | ManagerOrAdmin | Manager, Admin |
| /worktasks/{id} | PUT | ManagerOrAdmin | Manager, Admin |
| /worktasks/{id} | DELETE | AdminOnly | Admin |

---

## DTOs

### TeamDTO
```csharp
public class TeamDTO
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public List<WorkTaskDTO> Tasks { get; set; } = new();
}
```

### CreateTeamDTO
```csharp
public class CreateTeamDTO
{
    public required string Name { get; set; }
    public string? Description { get; set; }
}
```

### UpdateTeamDTO
```csharp
public class UpdateTeamDTO
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
}
```

### WorkTaskDTO
```csharp
public class WorkTaskDTO
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public string? Description { get; set; }
    public int StatusID { get; set; }
    public string? StatusName { get; set; }
    public int TeamId { get; set; }
    public string? TeamName { get; set; }
    public string AssignedToUserID { get; set; } = string.Empty;
    public string? AssignedToUserName { get; set; }
    public string CreatedByUserID { get; set; } = string.Empty;
    public string? CreatedByUserName { get; set; }
    public DateTime DueDate { get; set; }
}
```

### CreateWorkTaskDTO
```csharp
public class CreateWorkTaskDTO
{
    public required string Title { get; set; }
    public string? Description { get; set; }
    public int StatusID { get; set; }
    public int TeamId { get; set; }
    public required string AssignedToUserID { get; set; }
    public DateTime DueDate { get; set; }
}
```

### UpdateWorkTaskDTO
```csharp
public class UpdateWorkTaskDTO
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public string? Description { get; set; }
    public int StatusID { get; set; }
    public int TeamId { get; set; }
    public required string AssignedToUserID { get; set; }
    public DateTime DueDate { get; set; }
}
```

---

## Usage Examples

### Example 1: Create a Team
```bash
POST /api/teams
Authorization: Bearer {token}
Content-Type: application/json

{
  "name": "QA Team",
  "description": "Quality Assurance team"
}
```

**Response (201 Created):**
```json
{
  "id": 5,
  "name": "QA Team",
  "description": "Quality Assurance team",
  "tasks": []
}
```

### Example 2: Create a Task
```bash
POST /api/worktasks
Authorization: Bearer {token}
Content-Type: application/json

{
  "title": "Unit Testing",
  "description": "Write unit tests for API",
  "statusId": 2,
  "teamId": 5,
  "assignedToUserId": "user-id-123",
  "dueDate": "2025-02-20T00:00:00"
}
```

**Response (201 Created):**
```json
{
  "id": 10,
  "title": "Unit Testing",
  "description": "Write unit tests for API",
  "statusId": 2,
  "statusName": "In Progress",
  "teamId": 5,
  "teamName": "QA Team",
  "assignedToUserId": "user-id-123",
  "assignedToUserName": "John Doe",
  "createdByUserId": "current-user-id",
  "createdByUserName": "Admin User",
  "dueDate": "2025-02-20T00:00:00"
}
```

### Example 3: Get Tasks by Team
```bash
GET /api/worktasks/team/5
Authorization: Bearer {token}
```

**Response (200 OK):**
```json
[
  {
    "id": 10,
    "title": "Unit Testing",
    "description": "Write unit tests for API",
    "statusId": 2,
    "statusName": "In Progress",
    "teamId": 5,
    "teamName": "QA Team",
    "assignedToUserId": "user-id-123",
    "assignedToUserName": "John Doe",
    "createdByUserId": "current-user-id",
    "createdByUserName": "Admin User",
    "dueDate": "2025-02-20T00:00:00"
  }
]
```

### Example 4: Update a Task
```bash
PUT /api/worktasks/10
Authorization: Bearer {token}
Content-Type: application/json

{
  "id": 10,
  "title": "Unit Testing - Updated",
  "description": "Write comprehensive unit tests for API",
  "statusId": 3,
  "teamId": 5,
  "assignedToUserId": "user-id-456",
  "dueDate": "2025-02-25T00:00:00"
}
```

---

## Key Features

### 1. CQRS Pattern
- Separation of read (Queries) and write (Commands) operations
- Better scalability and maintainability
- Easier to understand business logic

### 2. MediatR Integration
- Centralized request handling
- Pipeline behaviors support
- Easy to add cross-cutting concerns

### 3. Role-Based Access Control
- Three authorization policies
- Fine-grained permission management
- Automatic user tracking (CreatedByUser)

### 4. Related Data Includes
- Queries automatically include related entities
- Rich response DTOs with all necessary information
- Eager loading to prevent N+1 queries

### 5. Error Handling
- Returns appropriate HTTP status codes
- Returns `404 Not Found` for non-existent resources
- Returns `201 Created` for successful creations
- Returns `204 No Content` for successful deletions

---

## Database Relationships

### Team Entity
```csharp
public class Team
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public ICollection<WorkTask> Tasks { get; set; } // One-to-Many
}
```

### WorkTask Entity
```csharp
public class WorkTask
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public int StatusID { get; set; }
    public Status Status { get; set; } // Foreign Key
    public int TeamId { get; set; }
    public Team Team { get; set; } // Foreign Key
    public string AssignedToUserID { get; set; }
    public User AssignedToUser { get; set; } // Foreign Key
    public string CreatedByUserID { get; set; }
    public User CreatedByUser { get; set; } // Foreign Key
    public DateTime DueDate { get; set; }
}
```

---

## Service Registration

In `Program.cs`, the following services are registered:

```csharp
// Enable CQRS with MediatR
builder.Services.AddMediatR(config => 
    config.RegisterServicesFromAssemblyContaining<Program>());

// Enable HttpContext access for handlers
builder.Services.AddHttpContextAccessor();
```

---

## Exception Handling

The handlers include basic error handling:
- **UnauthorizedAccessException**: Thrown when user is not found (in CreateWorkTaskCommandHandler)
- **Return null**: Queries/Commands return null if resource not found
- **Controller validation**: Controllers check for null results and return appropriate HTTP status

To add global exception handling, implement exception middleware or use exception handler middleware in `Program.cs`.

---

## Performance Considerations

1. **Eager Loading**: All queries use `.Include()` to load related entities
2. **Async/Await**: All database operations are asynchronous
3. **CancellationToken**: Support for cancellation tokens throughout
4. **N+1 Prevention**: Single query with includes instead of multiple queries

---

## Future Enhancements

1. Add pagination to GetAll operations
2. Add filtering capabilities (by status, assignee, due date)
3. Add sorting options
4. Implement soft deletes for teams and tasks
5. Add activity logging
6. Add email notifications
7. Implement caching layer
8. Add validation behaviors with FluentValidation
9. Add audit trail
10. Implement optimistic concurrency
