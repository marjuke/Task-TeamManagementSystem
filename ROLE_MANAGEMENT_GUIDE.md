# Role Management Implementation Guide

## Overview
Role-based access control (RBAC) has been implemented for Admin, Manager, and Employee roles across the Task & Team Management System.

## Components Updated

### 1. **UserDTO** (`Api/DTOs/UserDTO.cs`)
- Added `Roles` property to include user roles in API responses
- Roles are returned in login, register, refresh-token, and currentUser endpoints

### 2. **TokenService** (`Api/Services/TokenService.cs`)
- Updated `CreateAccessToken()` to include role claims in JWT tokens
- Roles are automatically added to the token's claims using `ClaimTypes.Role`

### 3. **AccountController** (`Api/Controllers/AccountController.cs`)
- **RegisterUser**: New users are automatically assigned the "Employee" role
- **LoginUser**: Returns user roles in the response
- **RefreshToken**: Returns user roles when refreshing tokens
- **GetCurrentUser**: Returns user roles in the response

### 4. **DbInitializer** (`Persistence/DbInitializer.cs`)
- Creates three roles: Admin, Manager, and Employee
- Seeds default users with respective roles:
  - Admin@demo.com ? Admin role
  - Manager@demo.com ? Manager role
  - Employee@demo.com ? Employee role
- Credentials follow the pattern: `{DisplayName}123!`
  - Admin123!
  - Manager123!
  - Employee123!

### 5. **Program.cs** (`Api/Program.cs`)
- Added authorization policies for easy role-based access control:
  - `AdminOnly`: Only Admin users
  - `ManagerOrAdmin`: Manager and Admin users
  - `EmployeeOrAbove`: Employee, Manager, and Admin users

## Usage in Controllers

### Example: Restrict endpoint to Admin only
```csharp
[Authorize(Policy = "AdminOnly")]
[HttpPost("admin-only-endpoint")]
public IActionResult AdminOnlyEndpoint()
{
    // Only Admin users can access this
    return Ok();
}
```

### Example: Restrict endpoint to Manager or Admin
```csharp
[Authorize(Policy = "ManagerOrAdmin")]
[HttpPost("manager-level-endpoint")]
public IActionResult ManagerLevelEndpoint()
{
    // Only Manager and Admin users can access this
    return Ok();
}
```

### Example: Restrict by single role
```csharp
[Authorize(Roles = "Admin")]
[HttpDelete("delete-user/{id}")]
public IActionResult DeleteUser(string id)
{
    // Only Admin users can access this
    return Ok();
}
```

## JWT Token Structure

The JWT token now includes role claims:
```
{
  "nameid": "user-id",
  "email": "user@example.com",
  "unique_name": "user@example.com",
  "DisplayName": "User Display Name",
  "role": ["Admin", "Manager"],  // Role claims
  "exp": 1234567890
}
```

## Database Schema

Roles are managed using ASP.NET Core Identity tables:
- `AspNetRoles`: Stores role definitions
- `AspNetUserRoles`: Stores user-to-role mappings

## Default Test Credentials

| Email | Password | Role |
|-------|----------|------|
| admin@demo.com | Admin123! | Admin |
| manager@demo.com | Manager123! | Manager |
| employee@demo.com | Employee123! | Employee |

## Assignment of Roles

- **New users during registration**: Automatically assigned "Employee" role
- **Database seeding**: Three default users are created with their respective roles
- **Manual assignment**: Use `userManager.AddToRoleAsync(user, "RoleName")` in your code

## Authorization Policies Available

1. **AdminOnly**: `[Authorize(Policy = "AdminOnly")]`
2. **ManagerOrAdmin**: `[Authorize(Policy = "ManagerOrAdmin")]`
3. **EmployeeOrAbove**: `[Authorize(Policy = "EmployeeOrAbove")]`

You can also add custom policies in `Program.cs`:
```csharp
builder.Services.AddAuthorizationBuilder()
    .AddPolicy("CustomPolicy", policy => policy.RequireRole("CustomRole"));
```
