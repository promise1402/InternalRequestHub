# Internal Request Hub

## 1. Project Overview

**Internal Request Hub** is a full-stack internal request and support management platform for organizations.

The system allows employees to create and track internal requests such as:

- Software access
- Hardware issues
- Laptop/monitor requests
- VPN/access requests
- IT support
- Office maintenance
- Purchase requests
- Account or permission requests
- Other configurable internal requests

Support staff can review, assign, process, and resolve requests. Administrators can manage users, departments, request types, priorities, SLAs, and system configuration.

The project is intentionally designed as a **modular monolithic backend + separate frontend application**. This gives a beginner the opportunity to learn real-world .NET architecture without the operational complexity of microservices.

---

# 2. Main Goals

The project should teach and demonstrate:

- C# fundamentals in a practical application
- ASP.NET Core Web API
- REST API design
- HTTP methods and status codes
- Dependency Injection
- Middleware
- DTOs
- Service and repository concepts
- Entity Framework Core
- SQL Server
- Database relationships
- LINQ
- Authentication and authorization
- JWT
- Role-based access control
- Input validation
- Exception handling
- Logging
- Pagination
- Filtering and sorting
- File uploads
- Background jobs
- Notifications
- Audit logging
- Unit and integration testing
- Git/GitHub workflow
- Docker
- API documentation with Swagger/OpenAPI
- React + TypeScript integration

---

# 3. Architecture Decision

## Recommended Architecture

Use:

```text
React + TypeScript
        |
        | HTTP / REST API
        |
        v
ASP.NET Core Web API
        |
        +-- Controllers
        +-- Services
        +-- DTOs
        +-- Validation
        +-- Business Logic
        +-- EF Core
        |
        v
SQL Server
```

This is a **separate frontend and backend**, but the backend itself is initially a **modular monolith**.

### Frontend

A separate React + TypeScript application.

### Backend

A single ASP.NET Core Web API application.

### Database

A separate SQL Server database accessed through Entity Framework Core.

---

# 4. Why Not Microservices?

Microservices are intentionally **not recommended for version 1**.

A beginner can learn the important concepts without introducing:

- Multiple APIs
- Multiple deployments
- Service discovery
- API gateways
- Distributed authentication
- Inter-service communication
- Message brokers
- Distributed transactions
- Multiple databases
- Container orchestration

Those concepts are useful later, but they would add infrastructure complexity before the core application is understood.

The backend can instead be organized into logical modules:

```text
Auth
Users
Requests
Request Types
Departments
Notifications
Audit
Dashboard
```

All modules initially run inside one ASP.NET Core application.

If the project becomes large enough, individual modules could later be extracted into services.

---

# 5. Recommended Tech Stack

## Backend

- C#
- .NET / ASP.NET Core Web API
- Entity Framework Core
- SQL Server
- LINQ
- JWT Bearer Authentication
- Swagger / OpenAPI
- FluentValidation or equivalent validation approach
- Serilog or built-in .NET logging
- xUnit
- Moq
- Docker

## Frontend

- React
- TypeScript
- Vite
- React Router
- Axios
- Tailwind CSS
- React Hook Form
- Zod or equivalent validation library

## Development Tools

- Visual Studio or VS Code
- SQL Server
- SQL Server Management Studio (SSMS)
- Git
- GitHub
- Postman
- Swagger UI
- Docker Desktop

---

# 6. User Roles

## Employee

Employees can:

- Register/login
- View their profile
- Create requests
- View their own requests
- Search/filter their requests
- Add comments
- Upload attachments
- Cancel eligible requests
- View request history
- Receive notifications

## Support Staff

Support staff can:

- View requests assigned to them
- View requests in their department
- Assign/reassign requests
- Change request status
- Add comments
- Upload attachments
- Resolve requests
- View request history
- Monitor SLA deadlines

## Admin

Administrators can:

- Manage users
- Manage departments
- Manage request types
- Manage priorities
- Configure SLA rules
- Assign support staff
- View all requests
- View dashboards
- View audit logs
- Manage system configuration

---

# 7. Core Request Workflow

The main workflow is:

```text
Employee creates request
          |
          v
       Pending
          |
          v
     Under Review
          |
       +--+--+
       |     |
       v     v
   Rejected  Approved
               |
               v
          Assigned
               |
               v
          In Progress
               |
               v
            Resolved
               |
               v
            Closed
```

Some requests may have a different workflow.

For example:

```text
Employee
   |
   v
Software Access Request
   |
   v
Manager Approval
   |
   v
IT Approval
   |
   v
Access Granted
   |
   v
Closed
```

The application should therefore avoid hard-coding every workflow into the UI.

---

# 8. Request Statuses

Initial statuses:

```text
Pending
UnderReview
Approved
Rejected
Assigned
InProgress
Resolved
Closed
Cancelled
```

The exact set can be adjusted during development.

---

# 9. Priority Levels

```text
Low
Medium
High
Critical
```

Example SLA configuration:

| Priority | Example SLA |
|---|---:|
| Low | 5 days |
| Medium | 3 days |
| High | 24 hours |
| Critical | 4 hours |

The SLA values should eventually be configurable rather than hard-coded.

---

# 10. Request Types

Initial request types:

```text
Software Access
Hardware Issue
Hardware Request
VPN Access
Account Access
Password/Account Issue
Office Maintenance
Purchase Request
Network Issue
Other
```

Administrators should eventually be able to create and modify request types.

---

# 11. Core Features

## 11.1 Authentication

Features:

- Registration
- Login
- Logout
- Password hashing
- JWT access token
- Token expiration
- Role-based authorization
- Protected API endpoints

Example:

```http
POST /api/auth/register
POST /api/auth/login
GET  /api/auth/me
```

---

# 12. User Management

Admins can:

- Create users
- Update users
- Activate/deactivate users
- Change roles
- Assign departments
- View user details

Example APIs:

```http
GET    /api/users
GET    /api/users/{id}
POST   /api/users
PUT    /api/users/{id}
DELETE /api/users/{id}
```

---

# 13. Department Management

Example departments:

```text
IT
HR
Finance
Operations
Sales
Marketing
Administration
```

APIs:

```http
GET    /api/departments
GET    /api/departments/{id}
POST   /api/departments
PUT    /api/departments/{id}
DELETE /api/departments/{id}
```

---

# 14. Request Management

Main request APIs:

```http
GET    /api/requests
GET    /api/requests/{id}
POST   /api/requests
PUT    /api/requests/{id}
DELETE /api/requests/{id}
```

Additional operations:

```http
PUT /api/requests/{id}/status
PUT /api/requests/{id}/assign
POST /api/requests/{id}/comments
POST /api/requests/{id}/attachments
GET /api/requests/{id}/history
POST /api/requests/{id}/cancel
```

---

# 15. Request Creation

An employee creates a request.

Example:

```json
{
  "title": "Need Visual Studio access",
  "description": "I need Visual Studio for the .NET project.",
  "requestTypeId": 2,
  "priority": "Medium"
}
```

The backend should:

1. Validate the request.
2. Identify the authenticated user.
3. Create the request.
4. Calculate the SLA deadline.
5. Create the initial status history entry.
6. Create an audit log.
7. Create notifications if required.
8. Return the created request.

---

# 16. Request Assignment

Support staff or admins can assign requests.

Example:

```http
PUT /api/requests/1042/assign
```

Body:

```json
{
  "assignedToId": 18
}
```

The system should record:

```text
Previous Assignee
New Assignee
Changed By
Changed At
```

---

# 17. Status History

Every important status change should be recorded.

Example:

```text
Request #1042

Pending
   |
   +--> Approved
           |
           +--> Assigned
                    |
                    +--> InProgress
                              |
                              +--> Resolved
                                       |
                                       +--> Closed
```

Database records:

```text
RequestId
OldStatus
NewStatus
ChangedBy
ChangedAt
Comment
```

This prevents losing historical information.

---

# 18. Comments

Users can add comments to requests.

Example:

```text
Employee:
"I have attached a screenshot of the error."

Support:
"Please restart the application and try again."

Employee:
"The issue is still occurring."
```

API:

```http
GET  /api/requests/{id}/comments
POST /api/requests/{id}/comments
```

---

# 19. Attachments

Users can attach:

- Images
- PDFs
- Text files
- Log files

Example:

```text
Request #1042

Attachments:
- error.png
- application-log.txt
```

The backend should validate:

- File size
- File extension
- MIME type
- File name

---

# 20. Search, Filtering and Sorting

Requests should support:

```text
Search
Status
Priority
Request Type
Department
Assigned User
Created Date
Due Date
```

Example:

```http
GET /api/requests?
search=laptop
&status=InProgress
&priority=High
&page=1
&pageSize=10
&sortBy=createdAt
&sortOrder=desc
```

---

# 21. Pagination

The API should not return thousands of records at once.

Example response:

```json
{
  "data": [],
  "page": 1,
  "pageSize": 10,
  "totalRecords": 134,
  "totalPages": 14
}
```

---

# 22. SLA Tracking

Each request receives a due date based on priority/request type.

Example:

```text
Created:
10:00 AM

Priority:
High

SLA:
24 hours

Due:
Tomorrow 10:00 AM
```

Possible states:

```text
On Track
Approaching SLA
Overdue
Resolved
```

The dashboard can show:

```text
12 requests approaching SLA
5 requests overdue
```

---

# 23. Notifications

Initial notification types:

```text
Request Created
Request Assigned
Status Changed
Comment Added
SLA Approaching
SLA Breached
Request Resolved
```

Database:

```text
Notifications

Id
UserId
Message
Type
IsRead
CreatedAt
```

---

# 24. Audit Logging

Important actions should be recorded.

Example:

```text
User Promise created Request #1042

User John assigned Request #1042

User John changed:
Approved -> InProgress

User Admin changed:
Priority Medium -> High
```

Suggested table:

```text
AuditLogs

Id
UserId
Action
EntityType
EntityId
OldValue
NewValue
CreatedAt
```

---

# 25. Dashboard

## Employee Dashboard

Show:

```text
My Requests
Pending
In Progress
Resolved
Closed
```

## Support Dashboard

Show:

```text
Assigned to Me
Unassigned
High Priority
Overdue
Approaching SLA
```

## Admin Dashboard

Show:

```text
Total Requests
Open Requests
Completed Requests
Overdue Requests
Requests by Department
Requests by Priority
Requests by Type
Average Resolution Time
SLA Breach Percentage
```

---

# 26. SQL Database Design

Initial tables:

```text
Users
Roles
Departments
RequestTypes
Requests
RequestComments
RequestAttachments
RequestStatusHistory
RequestAssignments
Notifications
AuditLogs
SlaPolicies
```

Relationships:

```text
Role
 |
 +---- Users
          |
          +---- Requests
          |
          +---- Comments
          |
          +---- Notifications
          |
          +---- AuditLogs

Department
 |
 +---- Users

RequestType
 |
 +---- Requests

Request
 |
 +---- Comments
 |
 +---- Attachments
 |
 +---- StatusHistory
 |
 +---- Assignments
```

---

# 27. Important SQL Concepts to Practice

During development, deliberately practice:

- CREATE TABLE
- PRIMARY KEY
- FOREIGN KEY
- UNIQUE
- NOT NULL
- DEFAULT
- CHECK constraints
- INNER JOIN
- LEFT JOIN
- GROUP BY
- HAVING
- ORDER BY
- WHERE
- COUNT
- SUM
- AVG
- MIN/MAX
- CASE
- Subqueries
- Indexes
- Transactions
- Views
- Stored procedures only where they genuinely help

Do not rely entirely on EF Core. Write SQL queries manually while learning.

---

# 28. Entity Framework Core

Learn:

```text
DbContext
DbSet
Entity configuration
Relationships
Migrations
LINQ
Tracking
AsNoTracking
Include
ThenInclude
Transactions
```

Example:

```csharp
public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Request> Requests { get; set; }
    public DbSet<Department> Departments { get; set; }
}
```

---

# 29. Backend Project Structure

Recommended starting structure:

```text
InternalRequestHub.Api/
│
├── Controllers/
│   ├── AuthController.cs
│   ├── RequestsController.cs
│   ├── UsersController.cs
│   ├── DepartmentsController.cs
│   └── DashboardController.cs
│
├── Data/
│   └── AppDbContext.cs
│
├── Models/
│   ├── User.cs
│   ├── Role.cs
│   ├── Department.cs
│   ├── Request.cs
│   ├── RequestType.cs
│   ├── RequestComment.cs
│   ├── RequestAttachment.cs
│   ├── RequestStatusHistory.cs
│   ├── Notification.cs
│   ├── AuditLog.cs
│   └── SlaPolicy.cs
│
├── DTOs/
│   ├── Auth/
│   ├── Requests/
│   ├── Users/
│   └── Departments/
│
├── Services/
│   ├── AuthService.cs
│   ├── RequestService.cs
│   ├── UserService.cs
│   └── NotificationService.cs
│
├── Interfaces/
│   ├── IAuthService.cs
│   ├── IRequestService.cs
│   └── IUserService.cs
│
├── Middleware/
│   ├── ExceptionMiddleware.cs
│   └── RequestLoggingMiddleware.cs
│
├── Validators/
│
├── Migrations/
│
├── Program.cs
├── appsettings.json
└── appsettings.Development.json
```

---

# 30. Frontend Structure

```text
internal-request-hub-web/
│
├── src/
│   ├── components/
│   ├── pages/
│   ├── layouts/
│   ├── services/
│   │   └── api.ts
│   ├── hooks/
│   ├── types/
│   ├── store/
│   ├── utils/
│   ├── routes/
│   ├── App.tsx
│   └── main.tsx
│
├── public/
├── package.json
└── vite.config.ts
```

---

# 31. Frontend Pages

Initial pages:

```text
/login
/register
/dashboard
/requests
/requests/new
/requests/:id
/profile
```

Admin:

```text
/admin/users
/admin/departments
/admin/request-types
/admin/sla
/admin/audit-logs
```

---

# 32. API Response Strategy

Use consistent responses.

Successful response:

```json
{
  "success": true,
  "data": {},
  "message": "Request created successfully."
}
```

Error:

```json
{
  "success": false,
  "message": "Request not found.",
  "errors": []
}
```

For validation:

```json
{
  "success": false,
  "message": "Validation failed.",
  "errors": [
    {
      "field": "title",
      "message": "Title is required."
    }
  ]
}
```

---

# 33. HTTP Status Codes to Learn

Use status codes appropriately:

```text
200 OK
201 Created
204 No Content
400 Bad Request
401 Unauthorized
403 Forbidden
404 Not Found
409 Conflict
422 Unprocessable Entity
500 Internal Server Error
```

---

# 34. Error Handling

Implement centralized exception handling.

Instead of every controller doing:

```csharp
try
{
}
catch
{
}
```

create middleware that handles unexpected exceptions.

The API should return a consistent error response.

---

# 35. Validation

Validate incoming requests.

Examples:

```text
Title cannot be empty
Title maximum length
Description cannot exceed maximum length
Priority must be valid
Request type must exist
Assigned user must exist
```

Validation should happen at the API boundary before business logic executes.

---

# 36. Security Requirements

The project should eventually include:

- Password hashing
- JWT authentication
- Role-based authorization
- Input validation
- File validation
- CORS configuration
- Secure configuration
- No passwords stored in plain text
- No secrets committed to Git
- Proper authorization checks
- SQL injection protection through parameterized access/EF Core

---

# 37. Testing

## Unit Tests

Test services and business rules.

Examples:

```text
Should create request
Should reject invalid request
Should calculate SLA correctly
Should not allow closed request modification
Should allow only authorized status transitions
```

## Integration Tests

Test API endpoints against a test database/environment.

Examples:

```text
POST /api/auth/login
POST /api/requests
GET /api/requests
PUT /api/requests/{id}/status
```

---

# 38. Git Workflow

Use Git from the beginning.

Suggested branches:

```text
main
develop

feature/authentication
feature/request-management
feature/comments
feature/attachments
feature/sla
feature/notifications
feature/dashboard
```

Example commits:

```text
feat: create initial ASP.NET Core API
feat: add SQL Server and EF Core
feat: add request entity
feat: add request creation endpoint
feat: add request pagination
fix: validate request status transition
```

---

# 39. Development Phases

## Phase 1 — Project Foundation

Build:

```text
ASP.NET Core API
React frontend
SQL Server connection
EF Core
Swagger
Git repository
```

## Phase 2 — Database

Build:

```text
Users
Roles
Departments
Requests
RequestTypes
```

## Phase 3 — Basic Request API

Build:

```text
Create request
Get requests
Get request by ID
Update request
Delete/cancel request
```

## Phase 4 — Authentication

Build:

```text
Register
Login
JWT
Roles
Authorization
```

## Phase 5 — Request Workflow

Build:

```text
Assignment
Status changes
Status history
Comments
```

## Phase 6 — Frontend

Build:

```text
Login
Dashboard
Request list
Create request
Request details
Comments
```

## Phase 7 — Advanced Backend

Build:

```text
Pagination
Filtering
Sorting
SLA
Notifications
Audit logs
Attachments
```

## Phase 8 — Admin

Build:

```text
User management
Department management
Request type management
SLA configuration
Audit log viewer
```

## Phase 9 — Testing

Build:

```text
Unit tests
Integration tests
API tests
Validation tests
```

## Phase 10 — Deployment

Add:

```text
Docker
Environment variables
CI/CD
Production database
Backend deployment
Frontend deployment
```

---

# 40. MVP Scope

Do NOT build every feature initially.

Your first working MVP should contain only:

```text
Authentication
Users
Roles
Departments
Request Types
Requests
Request Status
Basic assignment
SQL Server
EF Core
Swagger
React + TypeScript frontend
```

Once this works, add:

```text
Comments
Pagination
Filtering
Status History
```

Then:

```text
SLA
Notifications
Audit Logs
Attachments
Dashboard analytics
```

---

# 41. Definition of Done for MVP

The MVP is complete when:

- User can register.
- User can log in.
- User receives authentication token.
- User can create a request.
- User can view their requests.
- Support staff can view assigned requests.
- Support staff can change request status.
- Admin can manage users.
- Data persists in SQL Server.
- API is documented with Swagger.
- React frontend consumes the API.
- Validation exists.
- Authorization exists.
- Errors are handled consistently.
- Code is stored in GitHub.

---

# 42. Future Improvements

After completing the main application, possible advanced features include:

- Email notifications
- Real-time notifications using SignalR
- Background SLA monitoring
- Recurring maintenance requests
- Advanced analytics
- Export to CSV/Excel
- Search using full-text indexing
- Redis caching
- Rate limiting
- Docker Compose
- CI/CD
- Cloud deployment
- API versioning
- Refresh tokens
- Multi-tenant organization support

Microservices can be explored only after the modular monolith is understood.

---

# 43. Resume Project Description

### Internal Request Hub

**Tech:** C#, ASP.NET Core Web API, Entity Framework Core, SQL Server, React, TypeScript, Tailwind CSS

> Developed a full-stack internal request management platform using ASP.NET Core Web API, Entity Framework Core, SQL Server, React and TypeScript. Implemented role-based authentication, request workflows, assignment, status history, SLA tracking, comments, notifications, audit logging, filtering and pagination. Designed RESTful APIs with centralized validation and exception handling and integrated the frontend with the backend through HTTP APIs.

---

# 44. Important Learning Rule

Do not copy the complete project from a tutorial.

Build it incrementally.

For every feature, understand:

```text
Frontend
   ↓
HTTP Request
   ↓
Controller
   ↓
DTO / Validation
   ↓
Service
   ↓
EF Core
   ↓
SQL Server
   ↓
Response
   ↓
Frontend
```

If you understand this flow, you are learning the actual architecture rather than just memorizing code.
