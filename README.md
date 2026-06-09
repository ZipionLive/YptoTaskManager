# Ypto Task Manager

A task management application developed as part of a technical coding challenge for a full-stack .NET developer position at Ypto.

The solution is composed of a .NET 10 Web API backend and a Blazor Server frontend, following a layered architecture with separation of concerns between API, Business, Data and Domain layers.

---

## Features

### Authentication & Authorization

* JWT-based authentication
* Password hashing and salting
* Role-Based Access Control (RBAC)
* Two user roles:

  * User
  * Admin
* Protected API endpoints

### Task Management

* Create tasks
* Edit tasks
* Soft delete tasks
* Assign tasks to one or more users
* Task status management:

  * To do
  * In progress
  * Done
* Task categorization using hierarchical task types
* Personal task board displaying:

  * Tasks created by the user
  * Tasks assigned to the user

### User Management

Admin users can:

* View all users
* Create users
* Edit users
* Delete users

Regular users can:

* Authenticate
* View and edit their own profile
* Manage tasks created by them or assigned to them

### Frontend

* Blazor Server (.NET 10)
* Bootstrap UI
* Fluxor state management
* Basic localization support
* Responsive layout

### Localization

Localization is implemented using:

* IStringLocalizer
* Resource (.resx) files

---

## Solution Architecture

### Backend

```text
YptoTaskManager.BE.API
│
├── Controllers
├── DTOs
├── Extensions
│
YptoTaskManager.BE.Business
│
├── Services
├── Authentication
├── Security
│
YptoTaskManager.BE.Data
│
├── Repositories
├── Entity Framework
├── Mappings
│
YptoTaskManager.BE.Domain
│
├── Entities
├── Enums
│
YptoTaskManager.BE.Migrations
```

### Frontend

```text
YptoTaskManager.FE.Web
│
├── Pages
├── Components
├── State (Fluxor)
├── Localization
├── Http Clients
├── Authentication
```

---

## Technologies

### Backend

* .NET 10
* ASP.NET Core Web API
* Entity Framework Core with migrations
* SQL Server
* JWT Authentication
* Swagger / OpenAPI

### Frontend

* Blazor Server
* Fluxor
* Bootstrap 5
* Bootstrap Icons

---

## Security

Implemented security measures:

* JWT authentication
* Password hashing and salting
* Role-based authorization
* Protected API endpoints
* Soft delete strategy
* Separation of responsibilities through layered architecture

---

## Running the Backend

### Prerequisites

* .NET 10 SDK
* SQL Server

### Database

Update the connection string in:

```json
appsettings.Development.json
```

Example:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.\\SQLExpress;Database=YptoTaskManager;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

Apply migrations:

```bash
dotnet ef database update
```

Run the API:

```bash
dotnet run
```

... or using the package manager console from the migrations project:

```bash
update-database
```

Swagger will be available at:

```text
http://localhost:8888/swagger
```

---

## Running the Frontend

Update:

```json
appsettings.json
```

with the API base URL:

```json
{
  "ApiSettings": {
    "BaseUrl": "https://localhost:8888/"
  }
}
```

Run:

```bash
dotnet run
```

---

## Design Decisions

Several design choices were intentionally made to keep the solution maintainable and focused on the requirements:

* Repository pattern for data access
* Service layer for business logic
* CQRS-inspired separation between query and command repositories
* Fluxor for frontend state management
* JWT authentication for stateless security
* Enum-based role management (User / Admin) instead of a dedicated roles table, due to the fixed number of roles required by the challenge

---

## Future Improvements

Potential enhancements include:

* Persistent authentication using browser storage
* Drag-and-drop Kanban board
* Refresh token support
* User invitations and/or sign-in form
* Advanced search and filtering
* Automated tests

---

## Author

Alexandre Engelhardt

Senior .NET Developer
