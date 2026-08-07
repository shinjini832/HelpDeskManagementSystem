# Help Desk Ticket Management System

A complete ASP.NET Core support ticket management system. This application is built using a clean three-project solution structure incorporating Web API endpoints, an MVC front-end, Entity Framework Core database access (SQL Server LocalDB), and Moq-based xUnit unit tests.

---

## Solutions & Projects Structure

The solution contains the following directory tree and projects:

```text
HelpDeskManagement/
│
├── HelpDeskManagement.slnx        # Solution file (Visual Studio XML format)
├── README.md                      # Project documentation
├── StudentDetails.xlsx            # Submission details spreadsheet
│
├── HelpDesk.Api/                  # ASP.NET Core Web API Project
│   ├── Controllers/               # REST API controllers (TicketController)
│   ├── Data/                      # Entity Framework DbContext
│   ├── Migrations/                # EF Core Schema Migrations
│   ├── Models/                    # Shared domain models (Ticket)
│   ├── Repositories/              # Repository Interface & Implementation
│   ├── Program.cs                 # API host and Dependency Injection registration
│   └── appsettings.json           # Database connection string configuration
│
├── HelpDesk.Mvc/                  # ASP.NET Core MVC Front-End Project
│   ├── Controllers/               # MVC Controllers (Home, Tickets)
│   ├── Services/                  # Service Layer consuming Web API (TicketService)
│   ├── Views/                     # Razor views (Index, Details, Create, Edit)
│   ├── wwwroot/                   # Static assets (site.css, js, bootstrap)
│   └── Program.cs                 # MVC startup and HTTP Client configuration
│
└── HelpDesk.Tests/                # xUnit Unit Testing Project
    ├── TicketControllerTests.cs   # Unit tests covering TicketController endpoints
    └── HelpDesk.Tests.csproj      # Test dependencies (xUnit, Moq)
```

---

## Features

### 1. Dashboard View
- Displays summary widgets for:
  - **Total Tickets**
  - **Open Tickets**
  - **Closed Tickets**
- Built using modern glassmorphic card widgets with dynamic counting metrics.

### 2. Support Tickets List
- A clean, modern table displaying ticket properties: Title, Priority (color-coded badges), Status (color-coded status pills), Raised By, and Created Date.
- Offers instant actions: **Details**, **Edit**, and secure POST-based **Delete** with confirmation.

### 3. Filters
- Filter tickets dynamically by status: **Open**, **In Progress**, **Closed**, or view **All Statuses** using a drop-down filter panel.

### 4. Raise New Ticket
- Status is automatically hardcoded to **Open** on creation.
- Priority selection via dropdown.
- Fully validated inputs matching nullable reference requirements.

### 5. Edit Ticket
- Allows modifications of: Title, Description, Priority, and Status.
- Uses custom, styled **Radio Tiles** instead of default checkboxes or dropdowns for a premium user experience.

---

## Tech Stack & Architecture

- **Backend framework**: .NET 10.0 (ASP.NET Core Web API / MVC)
- **Database ORM**: Entity Framework Core
- **Database Engine**: SQL Server LocalDB (`MSSQLLocalDB`)
- **Testing suite**: xUnit & Moq
- **Styling**: Vanilla CSS (Outfit typography, glassmorphism, responsive grid layouts, custom button components, and micro-animations)

---

## Setup and Installation

### Prerequisites
- .NET 10.0 SDK
- SQL Server LocalDB installed (default instance: `(localdb)\MSSQLLocalDB`)

### 1. Database Configuration & Migrations
Create the database and apply the schema migrations:
```bash
# From the solution root folder
dotnet ef database update --project HelpDesk.Api/HelpDesk.Api.csproj
```

### 2. Running the Application
To run the system, launch both the API and MVC projects. 

**Run Web API:**
```bash
dotnet run --project HelpDesk.Api/HelpDesk.Api.csproj --launch-profile http
# API starts at: http://localhost:5282
```

**Run MVC Frontend:**
```bash
dotnet run --project HelpDesk.Mvc/HelpDesk.Mvc.csproj --launch-profile http
# MVC Application starts at: http://localhost:5107
```

*Open [http://localhost:5107](http://localhost:5107) in your browser to interact with the application.*

---

## Running Unit Tests
Execute the test project to verify all controller logic:
```bash
dotnet test HelpDesk.Tests/HelpDesk.Tests.csproj
```
The test suite consists of **12 xUnit tests** mocking the repository layer with `Moq` (no database connection required).

---

## Submission Info
- **Student ID**: `IN101`
- **Student Name**: `Rahul Sharma`
- **GitHub Repository URL**: `https://github.com/rahulsharma/HelpDeskManagement`
