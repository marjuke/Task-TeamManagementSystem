# Testing Guide - CQRS CRUD Operations

## ?? Pre-Testing Setup

### 1. Ensure Database is Migrated
```bash
# Run migrations (automatic on app startup)
# Or manually:
dotnet ef database update
```

### 2. Default Test Credentials
```
Admin:
  Email: admin@demo.com
  Password: Admin123!
  Role: Admin

Manager:
  Email: manager@demo.com
  Password: Manager123!
  Role: Manager

Employee:
  Email: employee@demo.com
  Password: Employee123!
  Role: Employee
```

### 3. Postman Collection Setup
- Create new collection: "Task Management API"
- Create new environment with variables:
  - `base_url`: http://localhost:5000
  - `token`: (auto-set after login)
  - `team_id`: (auto-set after team creation)
  - `task_id`: (auto-set after task creation)

---

## ?? Test Cases

### Test Suite 1: Authentication

#### Test 1.1: Login as Admin
```http
POST {{base_url}}/api/account/login
Content-Type: application/json

{
  "email": "admin@demo.com",
  "password": "Admin123!"
}
```
**Expected Response:**
- Status: 200 OK
- Response includes: accessToken, refreshToken, displayName, roles
- Store `accessToken` in `token` variable

**? Pass if:**
- Returns 200 OK
- accessToken is a valid JWT
- roles array contains "Admin"

---

#### Test 1.2: Login as Manager
```http
POST {{base_url}}/api/account/login
Content-Type: application/json

{
  "email": "manager@demo.com",
  "password": "Manager123!"
}
```
**Expected Response:**
- Status: 200 OK
- roles array contains "Manager"

---

#### Test 1.3: Invalid Credentials
```http
POST {{base_url}}/api/account/login
Content-Type: application/json

{
  "email": "admin@demo.com",
  "password": "WrongPassword123!"
}
```
**Expected Response:**
- Status: 401 Unauthorized

**? Pass if:**
- Returns 401 Unauthorized

---

### Test Suite 2: Teams - Read Operations

#### Test 2.1: Get All Teams (Authenticated)
```http
GET {{base_url}}/api/teams
Authorization: Bearer {{token}}
```
**Expected Response:**
- Status: 200 OK
- Returns array of TeamDTO objects

**? Pass if:**
- Returns 200 OK
- Response is a valid JSON array
- Each team has id, name, description, tasks

---

#### Test 2.2: Get Team by ID
```http
GET {{base_url}}/api/teams/1
Authorization: Bearer {{token}}
```
**Expected Response:**
- Status: 200 OK if team exists
- Status: 404 Not Found if team doesn't exist

**? Pass if:**
- Valid team ID returns 200 OK with TeamDTO
- Invalid team ID returns 404 Not Found

---

#### Test 2.3: Get Teams Without Authentication
```http
GET {{base_url}}/api/teams
```
**Expected Response:**
- Status: 401 Unauthorized

**? Pass if:**
- Returns 401 Unauthorized

---

### Test Suite 3: Teams - Write Operations

#### Test 3.1: Create Team as Admin ?
```http
POST {{base_url}}/api/teams
Authorization: Bearer {{admin_token}}
Content-Type: application/json

{
  "name": "Test Team 1",
  "description": "Testing CQRS implementation"
}
```
**Expected Response:**
- Status: 201 Created
- Response includes id, name, description, empty tasks array
- Store `id` in `team_id` variable

**? Pass if:**
- Returns 201 Created
- Response includes new team ID
- Team exists in database

---

#### Test 3.2: Create Team as Manager ?
```http
POST {{base_url}}/api/teams
Authorization: Bearer {{manager_token}}
Content-Type: application/json

{
  "name": "Test Team 2",
  "description": "Should fail"
}
```
**Expected Response:**
- Status: 403 Forbidden

**? Pass if:**
- Returns 403 Forbidden
- Team is NOT created

---

#### Test 3.3: Create Team as Employee ?
```http
POST {{base_url}}/api/teams
Authorization: Bearer {{employee_token}}
Content-Type: application/json

{
  "name": "Test Team 3",
  "description": "Should fail"
}
```
**Expected Response:**
- Status: 403 Forbidden

**? Pass if:**
- Returns 403 Forbidden
- Team is NOT created

---

#### Test 3.4: Update Team as Admin ?
```http
PUT {{base_url}}/api/teams/{{team_id}}
Authorization: Bearer {{admin_token}}
Content-Type: application/json

{
  "id": {{team_id}},
  "name": "Updated Team Name",
  "description": "Updated description"
}
```
**Expected Response:**
- Status: 200 OK
- Response includes updated team data

**? Pass if:**
- Returns 200 OK
- Changes reflect in database

---

#### Test 3.5: Update Team as Manager ?
```http
PUT {{base_url}}/api/teams/{{team_id}}
Authorization: Bearer {{manager_token}}
Content-Type: application/json

{
  "id": {{team_id}},
  "name": "Attempted Update",
  "description": "Should fail"
}
```
**Expected Response:**
- Status: 403 Forbidden

**? Pass if:**
- Returns 403 Forbidden
- Team is NOT updated

---

#### Test 3.6: Delete Team as Admin ?
```http
DELETE {{base_url}}/api/teams/{{team_id}}
Authorization: Bearer {{admin_token}}
```
**Expected Response:**
- Status: 204 No Content

**? Pass if:**
- Returns 204 No Content
- Team is deleted from database

---

#### Test 3.7: Delete Non-Existent Team
```http
DELETE {{base_url}}/api/teams/99999
Authorization: Bearer {{admin_token}}
```
**Expected Response:**
- Status: 404 Not Found

**? Pass if:**
- Returns 404 Not Found

---

### Test Suite 4: Tasks - Read Operations

#### Test 4.1: Get All Tasks
```http
GET {{base_url}}/api/worktasks
Authorization: Bearer {{token}}
```
**Expected Response:**
- Status: 200 OK
- Returns array of WorkTaskDTO objects
- Each task includes all related information

**? Pass if:**
- Returns 200 OK
- Each task has statusName, teamName, assignedToUserName, createdByUserName

---

#### Test 4.2: Get Task by ID
```http
GET {{base_url}}/api/worktasks/1
Authorization: Bearer {{token}}
```
**Expected Response:**
- Status: 200 OK if task exists
- Status: 404 Not Found if task doesn't exist

**? Pass if:**
- Valid task ID returns 200 OK with complete WorkTaskDTO
- Invalid task ID returns 404 Not Found

---

#### Test 4.3: Get Tasks by Team
```http
GET {{base_url}}/api/worktasks/team/{{team_id}}
Authorization: Bearer {{token}}
```
**Expected Response:**
- Status: 200 OK
- Returns filtered list of tasks for that team

**? Pass if:**
- Returns 200 OK
- All returned tasks have matching teamId

---

### Test Suite 5: Tasks - Write Operations

#### Test 5.1: Create Task as Manager ?
```http
POST {{base_url}}/api/worktasks
Authorization: Bearer {{manager_token}}
Content-Type: application/json

{
  "title": "Test Task 1",
  "description": "Testing task creation",
  "statusId": 1,
  "teamId": 1,
  "assignedToUserId": "employee-user-id",
  "dueDate": "2025-02-28T00:00:00"
}
```
**Expected Response:**
- Status: 201 Created
- Response includes id, all fields, related entity names
- createdByUserName should be manager's display name
- Store `id` in `task_id` variable

**? Pass if:**
- Returns 201 Created
- createdByUserId matches manager's user ID
- All related data loaded correctly

---

#### Test 5.2: Create Task as Admin ?
```http
POST {{base_url}}/api/worktasks
Authorization: Bearer {{admin_token}}
Content-Type: application/json

{
  "title": "Test Task 2",
  "description": "Admin creating task",
  "statusId": 1,
  "teamId": 1,
  "assignedToUserId": "manager-user-id",
  "dueDate": "2025-03-15T00:00:00"
}
```
**Expected Response:**
- Status: 201 Created

**? Pass if:**
- Returns 201 Created
- createdByUserId matches admin's user ID

---

#### Test 5.3: Create Task as Employee ?
```http
POST {{base_url}}/api/worktasks
Authorization: Bearer {{employee_token}}
Content-Type: application/json

{
  "title": "Test Task 3",
  "description": "Should fail",
  "statusId": 1,
  "teamId": 1,
  "assignedToUserId": "some-user-id",
  "dueDate": "2025-02-28T00:00:00"
}
```
**Expected Response:**
- Status: 403 Forbidden

**? Pass if:**
- Returns 403 Forbidden
- Task is NOT created

---

#### Test 5.4: Update Task as Manager ?
```http
PUT {{base_url}}/api/worktasks/{{task_id}}
Authorization: Bearer {{manager_token}}
Content-Type: application/json

{
  "id": {{task_id}},
  "title": "Updated Task Title",
  "description": "Updated description",
  "statusId": 2,
  "teamId": 1,
  "assignedToUserId": "new-user-id",
  "dueDate": "2025-03-20T00:00:00"
}
```
**Expected Response:**
- Status: 200 OK
- Response includes updated task data

**? Pass if:**
- Returns 200 OK
- All fields updated correctly
- StatusId changed to 2 (In Progress)

---

#### Test 5.5: Update Task Status
```http
PUT {{base_url}}/api/worktasks/{{task_id}}
Authorization: Bearer {{manager_token}}
Content-Type: application/json

{
  "id": {{task_id}},
  "title": "Test Task 1",
  "description": "Testing task creation",
  "statusId": 3,
  "teamId": 1,
  "assignedToUserId": "employee-user-id",
  "dueDate": "2025-02-28T00:00:00"
}
```
**Expected Response:**
- Status: 200 OK
- statusName should be "Done"

**? Pass if:**
- Returns 200 OK
- StatusId updated to 3
- statusName returns "Done"

---

#### Test 5.6: Delete Task as Admin ?
```http
DELETE {{base_url}}/api/worktasks/{{task_id}}
Authorization: Bearer {{admin_token}}
```
**Expected Response:**
- Status: 204 No Content

**? Pass if:**
- Returns 204 No Content
- Task is deleted from database

---

#### Test 5.7: Delete Non-Existent Task
```http
DELETE {{base_url}}/api/worktasks/99999
Authorization: Bearer {{admin_token}}
```
**Expected Response:**
- Status: 404 Not Found

**? Pass if:**
- Returns 404 Not Found

---

### Test Suite 6: Authorization Edge Cases

#### Test 6.1: Expired Token
```http
GET {{base_url}}/api/teams
Authorization: Bearer {expired_token}
```
**Expected Response:**
- Status: 401 Unauthorized

**? Pass if:**
- Returns 401 Unauthorized

---

#### Test 6.2: Invalid Token Format
```http
GET {{base_url}}/api/teams
Authorization: Bearer invalid.token.format
```
**Expected Response:**
- Status: 401 Unauthorized

**? Pass if:**
- Returns 401 Unauthorized

---

#### Test 6.3: Missing Authorization Header
```http
GET {{base_url}}/api/teams
```
**Expected Response:**
- Status: 401 Unauthorized

**? Pass if:**
- Returns 401 Unauthorized

---

### Test Suite 7: Data Validation

#### Test 7.1: Create Team with Missing Required Fields
```http
POST {{base_url}}/api/teams
Authorization: Bearer {{admin_token}}
Content-Type: application/json

{
  "description": "Missing name field"
}
```
**Expected Response:**
- Status: 400 Bad Request or 422 Unprocessable Entity

**? Pass if:**
- Returns error status code
- Error message indicates missing "name" field

---

#### Test 7.2: Create Task with Invalid Team ID
```http
POST {{base_url}}/api/worktasks
Authorization: Bearer {{manager_token}}
Content-Type: application/json

{
  "title": "Test",
  "description": "Test",
  "statusId": 1,
  "teamId": 99999,
  "assignedToUserId": "user-id",
  "dueDate": "2025-02-28T00:00:00"
}
```
**Expected Response:**
- Status: 400 Bad Request or 422 Unprocessable Entity

**? Pass if:**
- Returns error indicating invalid teamId

---

#### Test 7.3: Create Task with Invalid Status ID
```http
POST {{base_url}}/api/worktasks
Authorization: Bearer {{manager_token}}
Content-Type: application/json

{
  "title": "Test",
  "description": "Test",
  "statusId": 99999,
  "teamId": 1,
  "assignedToUserId": "user-id",
  "dueDate": "2025-02-28T00:00:00"
}
```
**Expected Response:**
- Status: 400 Bad Request or 422 Unprocessable Entity

---

### Test Suite 8: Data Integrity

#### Test 8.1: Verify Team Tasks Relationship
```http
GET {{base_url}}/api/teams/1
Authorization: Bearer {{token}}
```
**Expected Response:**
- Status: 200 OK
- Team object includes tasks array
- Tasks array contains correct team's tasks

**? Pass if:**
- Tasks in response match TeamId

---

#### Test 8.2: Verify Task User Relationships
```http
GET {{base_url}}/api/worktasks/1
Authorization: Bearer {{token}}
```
**Expected Response:**
- Status: 200 OK
- Response includes:
  - statusName (matches statusId)
  - teamName (matches teamId)
  - assignedToUserName (matches assignedToUserId)
  - createdByUserName (matches createdByUserId)

**? Pass if:**
- All user names correctly populated
- All entity names correspond to IDs

---

## ?? Performance Tests

### Load Test: Get All Teams
```bash
# Using Apache Bench
ab -n 1000 -c 10 -H "Authorization: Bearer {{token}}" http://localhost:5000/api/teams

# Expected:
# - Should complete successfully
# - Response time < 500ms for 1000 concurrent requests
```

### Load Test: Create Tasks
```bash
# Using Apache Bench with POST
ab -n 100 -c 10 -p task.json -T application/json \
   -H "Authorization: Bearer {{token}}" \
   http://localhost:5000/api/worktasks

# Expected:
# - All requests succeed
# - Response time < 200ms per request
```

---

## ?? Test Results Summary Template

```
Test Suite: ________________________
Date: ________________________
Tester: ________________________

Test Category: Authentication
?? Test 1.1: Login as Admin ............... [ PASS / FAIL ]
?? Test 1.2: Login as Manager ............ [ PASS / FAIL ]
?? Test 1.3: Invalid Credentials ......... [ PASS / FAIL ]

Test Category: Teams - Read
?? Test 2.1: Get All Teams ............... [ PASS / FAIL ]
?? Test 2.2: Get Team by ID ............. [ PASS / FAIL ]
?? Test 2.3: Without Authentication ...... [ PASS / FAIL ]

Test Category: Teams - Write
?? Test 3.1: Create Team (Admin) ......... [ PASS / FAIL ]
?? Test 3.2: Create Team (Manager) ....... [ PASS / FAIL ]
?? Test 3.3: Create Team (Employee) ...... [ PASS / FAIL ]
?? Test 3.4: Update Team (Admin) ......... [ PASS / FAIL ]
?? Test 3.5: Update Team (Manager) ....... [ PASS / FAIL ]
?? Test 3.6: Delete Team (Admin) ......... [ PASS / FAIL ]
?? Test 3.7: Delete Non-Existent ........ [ PASS / FAIL ]

Test Category: Tasks - Read
?? Test 4.1: Get All Tasks ............... [ PASS / FAIL ]
?? Test 4.2: Get Task by ID ............. [ PASS / FAIL ]
?? Test 4.3: Get Tasks by Team ........... [ PASS / FAIL ]

Test Category: Tasks - Write
?? Test 5.1: Create Task (Manager) ....... [ PASS / FAIL ]
?? Test 5.2: Create Task (Admin) ......... [ PASS / FAIL ]
?? Test 5.3: Create Task (Employee) ...... [ PASS / FAIL ]
?? Test 5.4: Update Task (Manager) ....... [ PASS / FAIL ]
?? Test 5.5: Update Task Status .......... [ PASS / FAIL ]
?? Test 5.6: Delete Task (Admin) ......... [ PASS / FAIL ]
?? Test 5.7: Delete Non-Existent ........ [ PASS / FAIL ]

Test Category: Authorization Edge Cases
?? Test 6.1: Expired Token ............... [ PASS / FAIL ]
?? Test 6.2: Invalid Token Format ........ [ PASS / FAIL ]
?? Test 6.3: Missing Auth Header ......... [ PASS / FAIL ]

Test Category: Data Validation
?? Test 7.1: Missing Required Fields ..... [ PASS / FAIL ]
?? Test 7.2: Invalid Team ID ............. [ PASS / FAIL ]
?? Test 7.3: Invalid Status ID ........... [ PASS / FAIL ]

Test Category: Data Integrity
?? Test 8.1: Team Tasks Relationship .... [ PASS / FAIL ]
?? Test 8.2: Task User Relationships .... [ PASS / FAIL ]

Overall Result: [ ALL PASS / SOME FAILED / ALL FAILED ]

Notes:
_____________________________________________________
_____________________________________________________

Signed by: ________________________
```

---

## ? Test Completion Checklist

- [ ] All authentication tests pass
- [ ] All team CRUD tests pass
- [ ] All task CRUD tests pass
- [ ] Authorization policies work correctly
- [ ] Edge cases handled properly
- [ ] Error responses have correct status codes
- [ ] Data integrity verified
- [ ] Performance acceptable
- [ ] No SQL errors in logs
- [ ] No unhandled exceptions
- [ ] JWT tokens generated correctly
- [ ] User roles properly assigned
- [ ] CreatedByUser automatically set
- [ ] Related entities properly loaded
- [ ] Deletions cascade properly

---

**Ready for testing! ??**
