# Quick Reference Guide

## ?? Authentication & Authorization

### Get JWT Token
```bash
# Login as Admin
POST /api/account/login
{
  "email": "admin@demo.com",
  "password": "Admin123!"
}

# Response includes accessToken and refreshToken
```

### Use Token in Requests
```bash
Authorization: Bearer {accessToken}
```

---

## ?? Teams Endpoints

### Get All Teams
```bash
GET /api/teams
```

### Get Team by ID
```bash
GET /api/teams/{id}
```

### Create Team (Admin Only)
```bash
POST /api/teams
{
  "name": "Team Name",
  "description": "Team Description"
}
```

### Update Team (Admin Only)
```bash
PUT /api/teams/{id}
{
  "id": 1,
  "name": "Updated Name",
  "description": "Updated Description"
}
```

### Delete Team (Admin Only)
```bash
DELETE /api/teams/{id}
```

---

## ?? Tasks Endpoints

### Get All Tasks
```bash
GET /api/worktasks
```

### Get Task by ID
```bash
GET /api/worktasks/{id}
```

### Get Tasks by Team
```bash
GET /api/worktasks/team/{teamId}
```

### Create Task (Manager/Admin Only)
```bash
POST /api/worktasks
{
  "title": "Task Title",
  "description": "Task Description",
  "statusId": 1,
  "teamId": 1,
  "assignedToUserId": "user-id",
  "dueDate": "2025-02-28T00:00:00"
}
```

### Update Task (Manager/Admin Only)
```bash
PUT /api/worktasks/{id}
{
  "id": 1,
  "title": "Updated Title",
  "description": "Updated Description",
  "statusId": 2,
  "teamId": 1,
  "assignedToUserId": "user-id",
  "dueDate": "2025-03-15T00:00:00"
}
```

### Delete Task (Admin Only)
```bash
DELETE /api/worktasks/{id}
```

---

## ?? User Roles & Permissions

### Admin
- ? View teams, tasks
- ? Create teams
- ? Update teams
- ? Delete teams
- ? Create tasks
- ? Update tasks
- ? Delete tasks
- ? Manage users

### Manager
- ? View teams, tasks
- ? Create tasks
- ? Update tasks (any task)
- ? Delete tasks
- ? Create/Update/Delete teams

### Employee
- ? View teams, tasks
- ? Create tasks
- ? Update tasks
- ? Delete tasks
- ? Create/Update/Delete teams

---

## ?? Status IDs

```
1 = Todo
2 = In Progress
3 = Done
```

---

## ?? Test Scenarios

### Scenario 1: Admin Creates Team
1. Login as admin@demo.com / Admin123!
2. POST /api/teams with team data
3. Response: 201 Created

### Scenario 2: Manager Creates Task
1. Login as manager@demo.com / Manager123!
2. POST /api/worktasks with task data
3. Response: 201 Created

### Scenario 3: Employee Tries to Create Task
1. Login as employee@demo.com / Employee123!
2. POST /api/worktasks with task data
3. Response: 403 Forbidden

### Scenario 4: Anyone Views Teams
1. Login with any role
2. GET /api/teams
3. Response: 200 OK with list of teams

---

## ??? Common Issues & Solutions

### "Authorization Policy not found"
- Check policy name in controller matches Program.cs
- Policies: AdminOnly, AdminCanManageTeams, ManagerOrAdmin, EmployeeOrAbove

### "Unauthorized (401)"
- JWT token missing or expired
- Get new token by logging in again

### "Forbidden (403)"
- User role doesn't have permission
- Check role-based access control table above

### "Not Found (404)"
- Resource (team/task) doesn't exist
- Check ID is correct

---

## ?? Sample cURL Commands

### Login
```bash
curl -X POST http://localhost:5000/api/account/login \
  -H "Content-Type: application/json" \
  -d '{"email":"admin@demo.com","password":"Admin123!"}'
```

### Get All Teams
```bash
curl -X GET http://localhost:5000/api/teams \
  -H "Authorization: Bearer {token}"
```

### Create Team
```bash
curl -X POST http://localhost:5000/api/teams \
  -H "Authorization: Bearer {token}" \
  -H "Content-Type: application/json" \
  -d '{"name":"Dev Team","description":"Development"}'
```

### Create Task
```bash
curl -X POST http://localhost:5000/api/worktasks \
  -H "Authorization: Bearer {token}" \
  -H "Content-Type: application/json" \
  -d '{
    "title":"Build API",
    "description":"Create endpoints",
    "statusId":1,
    "teamId":1,
    "assignedToUserId":"user-123",
    "dueDate":"2025-02-28T00:00:00"
  }'
```

---

## ?? Related Files

- **ROLE_MANAGEMENT_GUIDE.md** - Role setup details
- **CQRS_CRUD_GUIDE.md** - Full CQRS documentation
- **IMPLEMENTATION_SUMMARY.md** - Complete implementation overview

---

## ? Checklist Before Going to Production

- [ ] Database migrations applied
- [ ] JWT TokenKey configured in appsettings.json
- [ ] Database connection string configured
- [ ] HTTPS enabled
- [ ] CORS configured if needed
- [ ] Logging configured
- [ ] Error handling tested
- [ ] Authorization policies verified
- [ ] Load testing completed
- [ ] Security audit passed

---

## ?? Quick Start

1. **Login**
   ```bash
   POST /api/account/login
   Email: admin@demo.com
   Password: Admin123!
   ```

2. **Copy Access Token from Response**

3. **Use Token in Authorization Header**
   ```
   Authorization: Bearer {accessToken}
   ```

4. **Start Making API Calls**
   ```bash
   GET /api/teams
   POST /api/teams
   GET /api/worktasks
   POST /api/worktasks
   ```

---

**Ready to use! ??**
