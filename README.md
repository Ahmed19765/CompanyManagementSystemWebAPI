# CompanyManagementSystemWebAPI
This Web App for building skills and training
<div align="center">

# 🏢 Company Management System

**A role-based Web API for managing companies, projects, teams, and tasks**

Built with ASP.NET Core, CQRS (MediatR), and Onion Architecture

[![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/)
[![C#](https://img.shields.io/badge/C%23-239120?logo=csharp&logoColor=white)](https://learn.microsoft.com/en-us/dotnet/csharp/)
[![Entity Framework Core](https://img.shields.io/badge/EF%20Core-Data%20Access-blueviolet)](https://learn.microsoft.com/en-us/ef/core/)
[![MediatR](https://img.shields.io/badge/MediatR-CQRS-orange)](https://github.com/jbogard/MediatR)
[![Architecture](https://img.shields.io/badge/Architecture-Onion-8A2BE2)](#architecture)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)

</div>

---

## 📖 Overview

**Company Management System** models a small marketplace-style workflow between three roles:

| | |
|---|---|
| 👤 **Customer** | Posts projects and reviews company offers |
| 🏢 **Owner** | Owns companies, submits offers, organizes departments & teams |
| ⚙️ **Engineer** | Executes and tracks tasks assigned by their team |

The API is built around clean separation of concerns — every use case flows through a CQRS command or query, and business rules live entirely in the Domain layer, independent of frameworks or infrastructure.

## 📑 Table of Contents

- [Features](#-features)
- [Architecture](#-architecture)
- [Tech Stack](#-tech-stack)
- [Project Structure](#-project-structure)
- [API Reference](#-api-reference)
- [Getting Started](#-getting-started)
- [Roles & Permissions](#-roles--permissions)
- [Roadmap](#-roadmap)
- [License](#-license)

## ✨ Features

- **🔐 Authentication & Authorization** — Registration, login, email verification via OTP, password reset, JWT access/refresh tokens, and a two-step account deletion flow
- **🏢 Company Management** — Create/delete companies, manage members and ranks, submit offers on projects
- **📋 Project Lifecycle** — Customers create, update, and delete projects (within a 3-day edit window, and only before an offer is accepted); Owners assign/unassign projects to teams
- **🗂️ Departments & Teams** — Organize a company into departments containing teams; teams are unlinked (not deleted) when a department is removed
- **✅ Task Tracking** — Owners/Engineers create tasks; assigned Engineers update status based on rank (Members: `Todo`, `InProgress`, `Pending` — Leaders: full range including `Done`/`Failed`)
- **🛡️ Role-Based Access Control** — Fine-grained permissions enforced declaratively via `[Authorize(Roles = "...")]`

## 🏗️ Architecture

This project follows **Onion Architecture**, keeping business logic independent of frameworks, UI, and infrastructure. Dependencies always point **inward** — the Domain layer has zero outward dependencies, and the API layer only knows about Application-layer abstractions.

```
┌────────────────────────────────────────────────────┐
│                        API                          │
│         Controllers · Middleware · DI Setup         │
├────────────────────────────────────────────────────┤
│                    Application                      │
│   CQRS Commands & Queries · MediatR Handlers · DTOs │
├────────────────────────────────────────────────────┤
│                       Domain                        │
│         Entities · Enums · Core Business Rules      │
├────────────────────────────────────────────────────┤
│                   Infrastructure                    │
│     EF Core · Repositories · External Services      │
└────────────────────────────────────────────────────┘
```

### CQRS with MediatR

Every use case is a discrete **Command** (writes) or **Query** (reads), dispatched through `IMediator` / `ISender` and handled by a single-responsibility handler. Controllers stay thin — they map the HTTP request to a command, attach the current user's ID from their claims, and forward it to MediatR:

```csharp
[Authorize(Roles = "Owner")]
[HttpPost("owner/create-company")]
public async Task<IActionResult> AddCompany([FromBody] AddCompanyCommand command)
{
    command.OwnerId = GetCurrentUserId();
    return Ok(await _mediator.Send(command));
}
```

This keeps controllers free of business logic and makes every feature independently testable.

## 🛠️ Tech Stack

| Layer | Technology |
|---|---|
| API Framework | ASP.NET Core Web API |
| CQRS / Mediator | MediatR |
| Data Access | Entity Framework Core (Fluent API) |
| Auth | ASP.NET Core Identity, JWT access & refresh tokens |
| Database | SQL Server *(or your configured provider)* |

## 📁 Project Structure

```
CompanyManagementSystem/
├── CompanyManagementSystem.API/             # Controllers, middleware, DI setup
├── CompanyManagementSystem.Application/     # Commands, Queries, Handlers, DTOs, Validators
├── CompanyManagementSystem.Domain/          # Entities, Enums, domain logic
└── CompanyManagementSystem.Infrastructure/  # DbContext, Repositories, Identity, external services
```

## 🔌 API Reference

<details open>
<summary><strong>🔐 Auth</strong></summary>

| Method | Endpoint | Access | Description |
|---|---|---|---|
| `POST` | `/api/auth/register` | Public | Register a new account |
| `POST` | `/api/auth/login` | Public | Authenticate and receive tokens |
| `POST` | `/api/auth/verify-email` | Public | Verify email via OTP |
| `POST` | `/api/auth/resend-email-verification-otp` | Public | Resend verification OTP |
| `POST` | `/api/auth/refresh` | Public | Refresh access token |
| `POST` | `/api/auth/request-password-reset` | Public | Request a password reset |
| `POST` | `/api/auth/reset-password` | Public | Reset password with token |
| `POST` | `/api/auth/request-delete-account` | Authenticated | Request account deletion |
| `DELETE` | `/api/auth/delete-account` | Authenticated | Confirm account deletion |

</details>

<details>
<summary><strong>🏢 Company</strong> — Owner</summary>

| Method | Endpoint | Description |
|---|---|---|
| `POST` | `/api/company/owner/create-company` | Create a new company |
| `PUT` | `/api/company/owner/company-members/rank-up` | Promote a company member's rank |
| `POST` | `/api/company/owner/add-members` | Add a member to the company |
| `POST` | `/api/company/owner/add-offer` | Submit an offer on a pending project |
| `DELETE` | `/api/company/owner/remove-member` | Remove a member from the company |
| `DELETE` | `/api/company/owner/delete-company` | Soft-delete a company (preserves project history) |
| `GET` | `/api/company/owner/my-companies` | List companies owned by the current user |
| `GET` | `/api/company/owner/{companyId}/members` | List members of a company |
| `GET` | `/api/company/owner/{companyId}/departments` | List departments of a company |
| `GET` | `/api/company/owner/accepted-projects` | List projects with an accepted offer + progress |

</details>

<details>
<summary><strong>📋 Project</strong> — Customer & Owner</summary>

| Method | Endpoint | Access | Description |
|---|---|---|---|
| `POST` | `/api/project/customer/createProject` | Customer | Create a project |
| `PUT` | `/api/project/customer/update-project` | Customer | Update a project *(within 3 days, no accepted offer)* |
| `DELETE` | `/api/project/customer/delete-project` | Customer | Delete a project *(within 3 days, no accepted offer)* |
| `GET` | `/api/project/customer/my-projects` | Customer | List own projects with progress & offers |
| `GET` | `/api/project/customer/project-offers` | Customer | View offers submitted on a project |
| `POST` | `/api/project/customer/accept-offer` | Customer | Accept an offer (rejects all others) |
| `POST` | `/api/project/owner/assign-to-team` | Owner | Assign a project to a team |
| `DELETE` | `/api/project/owner/unassign-from-team` | Owner | Unassign a project from a team |
| `GET` | `/api/project/owner/customers-projects` | Owner | List all customer projects |

</details>

<details>
<summary><strong>🗂️ Department</strong> — Owner</summary>

| Method | Endpoint | Description |
|---|---|---|
| `POST` | `/api/department` | Create a department |
| `PUT` | `/api/department/{departmentId}` | Update a department |
| `DELETE` | `/api/department/owner/delete-department` | Delete a department *(teams unlinked, not deleted)* |
| `GET` | `/api/department/{departmentId}/teams` | List teams in a department |

</details>

<details>
<summary><strong>👥 Team</strong> — Owner & Engineer</summary>

| Method | Endpoint | Access | Description |
|---|---|---|---|
| `POST` | `/api/team/createTeam` | Owner | Create a team |
| `GET` | `/api/team/{teamId}/members` | Owner, Engineer | List team members |
| `GET` | `/api/team/{teamId}/tasks` | Owner, Engineer | List tasks assigned to a team |

</details>

<details>
<summary><strong>✅ Task</strong> — Owner & Engineer</summary>

| Method | Endpoint | Access | Description |
|---|---|---|---|
| `POST` | `/api/task/createTask` | Owner, Engineer | Create a task |
| `PUT` | `/api/task/update-status` | Engineer | Update a task's status *(range depends on rank)* |
| `GET` | `/api/task/my-tasks` | Engineer | List tasks assigned to the current user |

</details>

## 🚀 Getting Started

### Prerequisites

- [.NET SDK 8.0+](https://dotnet.microsoft.com/download)
- SQL Server *(or update the connection string for your provider)*

### Setup

```bash
# 1. Clone the repository
git clone https://github.com/<your-username>/CompanyManagementSystem.git
cd CompanyManagementSystem

# 2. Update the connection string in appsettings.json

# 3. Apply migrations
dotnet ef database update \
  --project CompanyManagementSystem.Infrastructure \
  --startup-project CompanyManagementSystem.API

# 4. Run the API
dotnet run --project CompanyManagementSystem.API
```

Then open `https://localhost:<port>/swagger` to explore the endpoints interactively.

## 🛡️ Roles & Permissions

| Role | Description |
|---|---|
| `Customer` | Posts and manages projects, reviews and accepts company offers |
| `Owner` | Owns and manages one or more companies, departments, teams, and offers |
| `Engineer` | Member of a team, assigned to and updates the status of tasks |

## 🗺️ Roadmap

- [ ] Notifications for offer acceptance / task assignment
- [ ] Pagination and filtering on list endpoints
- [ ] Unit and integration test coverage
- [ ] Dockerized deployment

## 📄 License

This project is licensed under the [MIT License](LICENSE).

---

<div align="center">

Built by **Ahmed** — Back-End .NET Developer

</div>
