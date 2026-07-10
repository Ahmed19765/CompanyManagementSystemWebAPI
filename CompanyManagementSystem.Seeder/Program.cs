using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using CompanyManagementSystem.Seeder.Data;
using CompanyManagementSystem.Seeder.Models;
using CompanyManagementSystem.Seeder.Models.Enums;

bool forceReseed = args.Contains("--force");

Console.WriteLine("╔══════════════════════════════════════════════════════════════╗");
Console.WriteLine("║       CompanyManagementSystem — Data Seeder v1.0             ║");
Console.WriteLine("╚══════════════════════════════════════════════════════════════╝");
if (forceReseed) Console.WriteLine("  ⚡  --force flag detected: existing data will be cleared.");
Console.WriteLine();

// ── 1. Configuration ──────────────────────────────────────────────────────────
var config = new ConfigurationBuilder()
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: false)
    .Build();

var connectionString = config.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

// ── 2. DbContext ──────────────────────────────────────────────────────────────
var optionsBuilder = new DbContextOptionsBuilder<SeederDbContext>();
optionsBuilder.UseSqlServer(connectionString);

await using var db = new SeederDbContext(optionsBuilder.Options);

Console.WriteLine("Connecting to database...");
await db.Database.OpenConnectionAsync();
Console.WriteLine("✔  Connected successfully.");
Console.WriteLine();

// ── 3. Idempotency guard ──────────────────────────────────────────────────────
bool alreadySeeded = await db.Users.AnyAsync();
if (alreadySeeded && !forceReseed)
{
    Console.WriteLine("⚠  Database already contains data. Seeding skipped to avoid duplicates.");
    Console.WriteLine("   Run with --force to wipe existing data and re-seed.");
    Console.WriteLine("   Example:  dotnet run -- --force");
    return;
}

if (alreadySeeded && forceReseed)
{
    Console.WriteLine("⚡  Clearing existing data before re-seeding...");
    // Delete in reverse dependency order to avoid FK violations
    db.TaskDetails.RemoveRange(db.TaskDetails);
    db.Tasks.RemoveRange(db.Tasks);
    db.CompanyOffers.RemoveRange(db.CompanyOffers);
    db.ProjectTeams.RemoveRange(db.ProjectTeams);
    db.Projects.RemoveRange(db.Projects);
    db.UserTeams.RemoveRange(db.UserTeams);
    db.Teams.RemoveRange(db.Teams);
    db.Departments.RemoveRange(db.Departments);
    db.CompanyUsers.RemoveRange(db.CompanyUsers);
    db.Companies.RemoveRange(db.Companies);
    db.Users.RemoveRange(db.Users);
    await db.SaveChangesAsync();
    Console.WriteLine("  ✔  Existing data cleared.");
    Console.WriteLine();
}

// ── 4. Password hasher ────────────────────────────────────────────────────────
// Uses ASP.NET Identity v3 PBKDF2 — same algorithm as the API
var hasher = new PasswordHasher<SeederUser>();

string Hash(string password, SeederUser dummyUser) =>
    hasher.HashPassword(dummyUser, password);

Console.WriteLine("Seeding data...");
Console.WriteLine();

// ═══════════════════════════════════════════════════════════════════════════════
//  USERS
// ═══════════════════════════════════════════════════════════════════════════════

// Fixed GUIDs so we can reference them deterministically
var ownerId1    = Guid.Parse("11111111-0000-0000-0000-000000000001");
var ownerId2    = Guid.Parse("11111111-0000-0000-0000-000000000002");
var customerId1 = Guid.Parse("22222222-0000-0000-0000-000000000001");
var customerId2 = Guid.Parse("22222222-0000-0000-0000-000000000002");
var customerId3 = Guid.Parse("22222222-0000-0000-0000-000000000003");
var engId1      = Guid.Parse("33333333-0000-0000-0000-000000000001");
var engId2      = Guid.Parse("33333333-0000-0000-0000-000000000002");
var engId3      = Guid.Parse("33333333-0000-0000-0000-000000000003");
var engId4      = Guid.Parse("33333333-0000-0000-0000-000000000004");
var engId5      = Guid.Parse("33333333-0000-0000-0000-000000000005");
var engId6      = Guid.Parse("33333333-0000-0000-0000-000000000006");

// Helper: build a fully-populated IdentityUser-compatible SeederUser
SeederUser MakeUser(Guid id, string firstName, string lastName, string email,
                    string password, UserRoleEnum role)
{
    var u = new SeederUser
    {
        Id                   = id,
        FirstName            = firstName,
        LastName             = lastName,
        UserName             = email,
        NormalizedUserName   = email.ToUpperInvariant(),
        Email                = email,
        NormalizedEmail      = email.ToUpperInvariant(),
        EmailConfirmed       = true,   // ← email verified
        IsBanned             = false,  // ← not banned
        Role                 = role,
        SecurityStamp        = Guid.NewGuid().ToString("D"),
        ConcurrencyStamp     = Guid.NewGuid().ToString("D"),
        CreatedAt            = DateTime.UtcNow,
        LockoutEnabled       = false,
        AccessFailedCount    = 0
    };
    u.PasswordHash = Hash(password, u);
    return u;
}

// ── Owners ──────────────────────────────────────────────────────────────────
var owner1 = MakeUser(ownerId1, "Alice",   "Johnson",  "alice.owner@cms.dev",    "Owner@1234",  UserRoleEnum.Owner);
var owner2 = MakeUser(ownerId2, "Robert",  "Smith",    "robert.owner@cms.dev",   "Owner@5678",  UserRoleEnum.Owner);

// ── Customers ───────────────────────────────────────────────────────────────
var cust1  = MakeUser(customerId1, "Michael", "Brown",    "michael.cust@cms.dev",  "Cust@1234",  UserRoleEnum.Customer);
var cust2  = MakeUser(customerId2, "Sara",    "Williams", "sara.cust@cms.dev",     "Cust@5678",  UserRoleEnum.Customer);
var cust3  = MakeUser(customerId3, "James",   "Davis",    "james.cust@cms.dev",    "Cust@9012",  UserRoleEnum.Customer);

// ── Engineers ───────────────────────────────────────────────────────────────
var eng1   = MakeUser(engId1, "Liam",   "Garcia",   "liam.eng@cms.dev",     "Eng@1234",   UserRoleEnum.Engineer);
var eng2   = MakeUser(engId2, "Emma",   "Martinez", "emma.eng@cms.dev",     "Eng@5678",   UserRoleEnum.Engineer);
var eng3   = MakeUser(engId3, "Noah",   "Wilson",   "noah.eng@cms.dev",     "Eng@9012",   UserRoleEnum.Engineer);
var eng4   = MakeUser(engId4, "Olivia", "Anderson", "olivia.eng@cms.dev",   "Eng@3456",   UserRoleEnum.Engineer);
var eng5   = MakeUser(engId5, "Ethan",  "Thomas",   "ethan.eng@cms.dev",    "Eng@7890",   UserRoleEnum.Engineer);
var eng6   = MakeUser(engId6, "Sophia", "Jackson",  "sophia.eng@cms.dev",   "Eng@2345",   UserRoleEnum.Engineer);

var allUsers = new[]
{
    owner1, owner2,
    cust1, cust2, cust3,
    eng1, eng2, eng3, eng4, eng5, eng6
};

db.Users.AddRange(allUsers);
await db.SaveChangesAsync();
Console.WriteLine($"  ✔  Users seeded ({allUsers.Length})");

// ═══════════════════════════════════════════════════════════════════════════════
//  COMPANIES
// ═══════════════════════════════════════════════════════════════════════════════
var company1 = new SeederCompany
{
    CompanyName        = "TechNova Solutions",
    CompanyDescription = "A leading software development company specialising in enterprise-grade web and cloud applications.",
    JoinCode           = "TNOVA-001",
    OwnerId            = ownerId1
};
var company2 = new SeederCompany
{
    CompanyName        = "BlueWave Systems",
    CompanyDescription = "Expert IT consulting and digital transformation firm serving global clients since 2015.",
    JoinCode           = "BWAVE-002",
    OwnerId            = ownerId2
};

db.Companies.AddRange(company1, company2);
await db.SaveChangesAsync();
Console.WriteLine($"  ✔  Companies seeded (2)");

// ═══════════════════════════════════════════════════════════════════════════════
//  COMPANY USERS  (memberships)
// ═══════════════════════════════════════════════════════════════════════════════
// Company 1: owner1 is leader + 3 engineers
// Company 2: owner2 is leader + 3 engineers
db.CompanyUsers.AddRange(
    // TechNova
    new SeederCompanyUser { CompanyId = company1.CompanyId, UserId = ownerId1, Rank = CompanyRankEnum.Leader,  JoinedAt = DateTime.UtcNow.AddMonths(-12) },
    new SeederCompanyUser { CompanyId = company1.CompanyId, UserId = engId1,   Rank = CompanyRankEnum.Member,  JoinedAt = DateTime.UtcNow.AddMonths(-10) },
    new SeederCompanyUser { CompanyId = company1.CompanyId, UserId = engId2,   Rank = CompanyRankEnum.Member,  JoinedAt = DateTime.UtcNow.AddMonths(-9)  },
    new SeederCompanyUser { CompanyId = company1.CompanyId, UserId = engId3,   Rank = CompanyRankEnum.Member,  JoinedAt = DateTime.UtcNow.AddMonths(-8)  },
    // BlueWave
    new SeederCompanyUser { CompanyId = company2.CompanyId, UserId = ownerId2, Rank = CompanyRankEnum.Leader,  JoinedAt = DateTime.UtcNow.AddMonths(-15) },
    new SeederCompanyUser { CompanyId = company2.CompanyId, UserId = engId4,   Rank = CompanyRankEnum.Member,  JoinedAt = DateTime.UtcNow.AddMonths(-12) },
    new SeederCompanyUser { CompanyId = company2.CompanyId, UserId = engId5,   Rank = CompanyRankEnum.Member,  JoinedAt = DateTime.UtcNow.AddMonths(-11) },
    new SeederCompanyUser { CompanyId = company2.CompanyId, UserId = engId6,   Rank = CompanyRankEnum.Member,  JoinedAt = DateTime.UtcNow.AddMonths(-7)  }
);
await db.SaveChangesAsync();
Console.WriteLine($"  ✔  CompanyUsers seeded (8)");

// ═══════════════════════════════════════════════════════════════════════════════
//  DEPARTMENTS
// ═══════════════════════════════════════════════════════════════════════════════
// 3 per company
var dept1 = new SeederDepartment { DepartmentName = "Engineering",        CompanyId = company1.CompanyId };
var dept2 = new SeederDepartment { DepartmentName = "Quality Assurance",  CompanyId = company1.CompanyId };
var dept3 = new SeederDepartment { DepartmentName = "Product Management",  CompanyId = company1.CompanyId };
var dept4 = new SeederDepartment { DepartmentName = "Development",         CompanyId = company2.CompanyId };
var dept5 = new SeederDepartment { DepartmentName = "DevOps",              CompanyId = company2.CompanyId };
var dept6 = new SeederDepartment { DepartmentName = "Data & Analytics",    CompanyId = company2.CompanyId };

db.Departments.AddRange(dept1, dept2, dept3, dept4, dept5, dept6);
await db.SaveChangesAsync();
Console.WriteLine($"  ✔  Departments seeded (6)");

// ═══════════════════════════════════════════════════════════════════════════════
//  TEAMS
// ═══════════════════════════════════════════════════════════════════════════════
// 2 teams per department, each led by an owner/engineer
var team1  = new SeederTeam { TeamName = "Backend Team Alpha",   TeamDescription = "Handles server-side API development.", LeaderId = ownerId1, DepartmentId = dept1.DepartmentId };
var team2  = new SeederTeam { TeamName = "Frontend Team Beta",   TeamDescription = "Responsible for all UI/UX implementations.", LeaderId = engId1, DepartmentId = dept1.DepartmentId };
var team3  = new SeederTeam { TeamName = "QA Automation",        TeamDescription = "Automated testing and CI pipeline integration.", LeaderId = engId2, DepartmentId = dept2.DepartmentId };
var team4  = new SeederTeam { TeamName = "Manual Testing Squad", TeamDescription = "Exploratory and regression testing.", LeaderId = ownerId1, DepartmentId = dept2.DepartmentId };
var team5  = new SeederTeam { TeamName = "PM Core",              TeamDescription = "Roadmap planning and sprint management.", LeaderId = engId3, DepartmentId = dept3.DepartmentId };
var team6  = new SeederTeam { TeamName = "CloudOps Team",        TeamDescription = "Cloud infrastructure and cost optimisation.", LeaderId = ownerId2, DepartmentId = dept4.DepartmentId };
var team7  = new SeederTeam { TeamName = "Full-Stack Warriors",  TeamDescription = "End-to-end feature development.",   LeaderId = engId4, DepartmentId = dept4.DepartmentId };
var team8  = new SeederTeam { TeamName = "Pipeline Masters",     TeamDescription = "CI/CD and deployment automation.",    LeaderId = engId5, DepartmentId = dept5.DepartmentId };
var team9  = new SeederTeam { TeamName = "Data Engineers",       TeamDescription = "ETL pipelines and data warehousing.", LeaderId = engId6, DepartmentId = dept6.DepartmentId };
var team10 = new SeederTeam { TeamName = "Analytics Guild",      TeamDescription = "Business intelligence and dashboards.", LeaderId = ownerId2, DepartmentId = dept6.DepartmentId };

db.Teams.AddRange(team1, team2, team3, team4, team5, team6, team7, team8, team9, team10);
await db.SaveChangesAsync();
Console.WriteLine($"  ✔  Teams seeded (10)");

// ═══════════════════════════════════════════════════════════════════════════════
//  USER TEAMS  (memberships)
// ═══════════════════════════════════════════════════════════════════════════════
db.UserTeams.AddRange(
    // Team 1 – Backend Alpha
    new SeederUserTeam { UserId = engId1, TeamId = team1.TeamId, TeamRole = "Senior Developer",  JoinedAt = DateTime.UtcNow.AddMonths(-10) },
    new SeederUserTeam { UserId = engId2, TeamId = team1.TeamId, TeamRole = "Developer",          JoinedAt = DateTime.UtcNow.AddMonths(-9)  },
    // Team 2 – Frontend Beta
    new SeederUserTeam { UserId = engId3, TeamId = team2.TeamId, TeamRole = "UI Developer",       JoinedAt = DateTime.UtcNow.AddMonths(-8)  },
    // Team 3 – QA Automation
    new SeederUserTeam { UserId = engId2, TeamId = team3.TeamId, TeamRole = "SDET",               JoinedAt = DateTime.UtcNow.AddMonths(-7)  },
    // Team 6 – CloudOps
    new SeederUserTeam { UserId = engId4, TeamId = team6.TeamId, TeamRole = "Cloud Engineer",     JoinedAt = DateTime.UtcNow.AddMonths(-12) },
    new SeederUserTeam { UserId = engId5, TeamId = team6.TeamId, TeamRole = "DevOps Engineer",    JoinedAt = DateTime.UtcNow.AddMonths(-11) },
    // Team 7 – Full-Stack Warriors
    new SeederUserTeam { UserId = engId4, TeamId = team7.TeamId, TeamRole = "Full-Stack Dev",     JoinedAt = DateTime.UtcNow.AddMonths(-6)  },
    new SeederUserTeam { UserId = engId6, TeamId = team7.TeamId, TeamRole = "Full-Stack Dev",     JoinedAt = DateTime.UtcNow.AddMonths(-5)  },
    // Team 8 – Pipeline Masters
    new SeederUserTeam { UserId = engId5, TeamId = team8.TeamId, TeamRole = "Pipeline Engineer",  JoinedAt = DateTime.UtcNow.AddMonths(-10) },
    // Team 9 – Data Engineers
    new SeederUserTeam { UserId = engId6, TeamId = team9.TeamId, TeamRole = "Data Engineer",      JoinedAt = DateTime.UtcNow.AddMonths(-9)  }
);
await db.SaveChangesAsync();
Console.WriteLine($"  ✔  UserTeams seeded (10)");

// ═══════════════════════════════════════════════════════════════════════════════
//  PROJECTS  (owned by customers)
// ═══════════════════════════════════════════════════════════════════════════════
var proj1 = new SeederProject { ProjectTitle = "E-Commerce Platform Rebuild",     ProjectDescription = "Migrating a legacy e-commerce site to a modern microservices architecture with React front-end.", ProjectDocumentsUrl = "https://docs.cms.dev/proj/ecommerce",    ProjectOfferedBudget = 85000m,  UploadedDate = DateTime.UtcNow.AddMonths(-6), CustomerId = customerId1 };
var proj2 = new SeederProject { ProjectTitle = "HR Management System",            ProjectDescription = "A comprehensive HRMS covering payroll, leave tracking, performance reviews and onboarding workflows.", ProjectDocumentsUrl = "https://docs.cms.dev/proj/hrms",         ProjectOfferedBudget = 55000m,  UploadedDate = DateTime.UtcNow.AddMonths(-5), CustomerId = customerId1 };
var proj3 = new SeederProject { ProjectTitle = "IoT Fleet Monitoring Dashboard",  ProjectDescription = "Real-time vehicle fleet tracking system with predictive maintenance alerts using IoT sensor data.", ProjectDocumentsUrl = "https://docs.cms.dev/proj/fleet",        ProjectOfferedBudget = 120000m, UploadedDate = DateTime.UtcNow.AddMonths(-4), CustomerId = customerId2 };
var proj4 = new SeederProject { ProjectTitle = "Mobile Banking App (iOS/Android)", ProjectDescription = "Cross-platform banking application with secure authentication, transfers, and statement exports.", ProjectDocumentsUrl = "https://docs.cms.dev/proj/mobilebank",   ProjectOfferedBudget = 200000m, UploadedDate = DateTime.UtcNow.AddMonths(-3), CustomerId = customerId2 };
var proj5 = new SeederProject { ProjectTitle = "Inventory & Warehouse System",    ProjectDescription = "Cloud-based inventory management with barcode scanning, real-time stock levels and supplier integration.", ProjectDocumentsUrl = "https://docs.cms.dev/proj/inventory",    ProjectOfferedBudget = 40000m,  UploadedDate = DateTime.UtcNow.AddMonths(-2), CustomerId = customerId3 };
var proj6 = new SeederProject { ProjectTitle = "AI-Powered Customer Support Bot", ProjectDescription = "Conversational AI chatbot integrated with existing CRM to resolve support tickets automatically.", ProjectDocumentsUrl = "https://docs.cms.dev/proj/aibot",        ProjectOfferedBudget = 95000m,  UploadedDate = DateTime.UtcNow.AddMonths(-1), CustomerId = customerId3 };

db.Projects.AddRange(proj1, proj2, proj3, proj4, proj5, proj6);
await db.SaveChangesAsync();
Console.WriteLine($"  ✔  Projects seeded (6)");

// ═══════════════════════════════════════════════════════════════════════════════
//  COMPANY OFFERS
// ═══════════════════════════════════════════════════════════════════════════════
db.CompanyOffers.AddRange(
    // TechNova bids on proj1 & proj2 (company1)
    new SeederCompanyOffer { CompanyId = company1.CompanyId, ProjectId = proj1.ProjectId, OfferedBudget = 80000m,  StartDate = DateTime.UtcNow.AddMonths(1), DeliveryExceptedDate = DateTime.UtcNow.AddMonths(7),  Status = OfferStatusEnum.Accepted  },
    new SeederCompanyOffer { CompanyId = company1.CompanyId, ProjectId = proj2.ProjectId, OfferedBudget = 50000m,  StartDate = DateTime.UtcNow.AddMonths(1), DeliveryExceptedDate = DateTime.UtcNow.AddMonths(5),  Status = OfferStatusEnum.Pending   },
    new SeederCompanyOffer { CompanyId = company1.CompanyId, ProjectId = proj5.ProjectId, OfferedBudget = 38000m,  StartDate = DateTime.UtcNow.AddMonths(2), DeliveryExceptedDate = DateTime.UtcNow.AddMonths(6),  Status = OfferStatusEnum.Rejected  },
    // BlueWave bids on proj3 & proj4 (company2)
    new SeederCompanyOffer { CompanyId = company2.CompanyId, ProjectId = proj3.ProjectId, OfferedBudget = 115000m, StartDate = DateTime.UtcNow.AddMonths(1), DeliveryExceptedDate = DateTime.UtcNow.AddMonths(8),  Status = OfferStatusEnum.Accepted  },
    new SeederCompanyOffer { CompanyId = company2.CompanyId, ProjectId = proj4.ProjectId, OfferedBudget = 195000m, StartDate = DateTime.UtcNow.AddMonths(2), DeliveryExceptedDate = DateTime.UtcNow.AddMonths(14), Status = OfferStatusEnum.Pending   },
    new SeederCompanyOffer { CompanyId = company2.CompanyId, ProjectId = proj6.ProjectId, OfferedBudget = 90000m,  StartDate = DateTime.UtcNow.AddMonths(1), DeliveryExceptedDate = DateTime.UtcNow.AddMonths(5),  Status = OfferStatusEnum.Accepted  }
);
await db.SaveChangesAsync();
Console.WriteLine($"  ✔  CompanyOffers seeded (6)");

// ═══════════════════════════════════════════════════════════════════════════════
//  PROJECT TEAMS  (which team works on which project)
// ═══════════════════════════════════════════════════════════════════════════════
db.ProjectTeams.AddRange(
    new SeederProjectTeam { ProjectId = proj1.ProjectId, TeamId = team1.TeamId,  AssignedAt = DateTime.UtcNow.AddMonths(-5) },
    new SeederProjectTeam { ProjectId = proj1.ProjectId, TeamId = team2.TeamId,  AssignedAt = DateTime.UtcNow.AddMonths(-5) },
    new SeederProjectTeam { ProjectId = proj2.ProjectId, TeamId = team3.TeamId,  AssignedAt = DateTime.UtcNow.AddMonths(-4) },
    new SeederProjectTeam { ProjectId = proj2.ProjectId, TeamId = team5.TeamId,  AssignedAt = DateTime.UtcNow.AddMonths(-4) },
    new SeederProjectTeam { ProjectId = proj3.ProjectId, TeamId = team6.TeamId,  AssignedAt = DateTime.UtcNow.AddMonths(-3) },
    new SeederProjectTeam { ProjectId = proj3.ProjectId, TeamId = team9.TeamId,  AssignedAt = DateTime.UtcNow.AddMonths(-3) },
    new SeederProjectTeam { ProjectId = proj4.ProjectId, TeamId = team7.TeamId,  AssignedAt = DateTime.UtcNow.AddMonths(-2) },
    new SeederProjectTeam { ProjectId = proj5.ProjectId, TeamId = team8.TeamId,  AssignedAt = DateTime.UtcNow.AddMonths(-2) },
    new SeederProjectTeam { ProjectId = proj6.ProjectId, TeamId = team10.TeamId, AssignedAt = DateTime.UtcNow.AddMonths(-1) }
);
await db.SaveChangesAsync();
Console.WriteLine($"  ✔  ProjectTeams seeded (9)");

// ═══════════════════════════════════════════════════════════════════════════════
//  TASKS  +  TASK DETAILS
// ═══════════════════════════════════════════════════════════════════════════════

// Helper to create a task + its detail in one call
async Task SeedTask(int projectId, int? teamId, Guid? assignedById, Guid? assignedToId,
                    string title, string description, TaskStateEnum status, DateTime? dueDate,
                    string notes, string acceptanceCriteria)
{
    var task = new SeederTask
    {
        TaskTitle       = title,
        TaskDescription = description,
        Status          = status,
        CreatedAt       = DateTime.UtcNow.AddDays(-new Random().Next(1, 60)),
        DueDate         = dueDate,
        ProjectId       = projectId,
        TeamId          = teamId,
        AssignedById    = assignedById,
        AssignedToId    = assignedToId
    };
    db.Tasks.Add(task);
    await db.SaveChangesAsync();

    db.TaskDetails.Add(new SeederTaskDetails
    {
        TaskId                  = task.TaskId,
        Notes                   = notes,
        TaskAttachmentDocumentUrl = $"https://docs.cms.dev/attachments/task-{task.TaskId}.pdf",
        AcceptanceCriteria      = acceptanceCriteria
    });
    await db.SaveChangesAsync();
}

// Project 1 – E-Commerce Rebuild (team1, team2)
await SeedTask(proj1.ProjectId, team1.TeamId, ownerId1, engId1,
    "Set up microservices skeleton",
    "Scaffold all microservices using .NET 8 Minimal APIs with shared NuGet packages.",
    TaskStateEnum.Done, DateTime.UtcNow.AddMonths(-3),
    "Services: auth, catalog, cart, order, notification.",
    "All services build successfully and health endpoints return 200.");

await SeedTask(proj1.ProjectId, team1.TeamId, ownerId1, engId2,
    "Implement Product Catalog API",
    "CRUD endpoints for products, categories, and search with Elasticsearch integration.",
    TaskStateEnum.InProgress, DateTime.UtcNow.AddMonths(1),
    "Must include pagination and filtering.",
    "GET /api/products returns paginated results with correct total count.");

await SeedTask(proj1.ProjectId, team2.TeamId, engId1, engId3,
    "Build Shopping Cart UI",
    "React component for adding/removing items, quantity adjustment and mini-cart dropdown.",
    TaskStateEnum.Todo, DateTime.UtcNow.AddMonths(2),
    "Follow Figma designs shared in Confluence.",
    "Cart persists across page refreshes via localStorage.");

await SeedTask(proj1.ProjectId, team2.TeamId, engId1, engId3,
    "Checkout & Payment Integration",
    "Integrate Stripe payment gateway with 3DS support and order confirmation email.",
    TaskStateEnum.Todo, DateTime.UtcNow.AddMonths(3),
    "Use Stripe test mode; real keys to be added in prod.",
    "Successful payment creates an order record and sends confirmation email.");

// Project 2 – HR Management System (team3, team5)
await SeedTask(proj2.ProjectId, team3.TeamId, ownerId1, engId2,
    "Payroll Calculation Engine",
    "Build payroll calculation module supporting base salary, allowances, deductions, and tax computation.",
    TaskStateEnum.InProgress, DateTime.UtcNow.AddMonths(1),
    "Use the Egyptian income-tax bracket table for 2024.",
    "Monthly payroll for 500 employees processed in under 10 seconds.");

await SeedTask(proj2.ProjectId, team3.TeamId, ownerId1, engId2,
    "Leave Management Module",
    "Annual leave, sick leave, and unpaid leave with approval workflow and email notifications.",
    TaskStateEnum.Pending, DateTime.UtcNow.AddMonths(2),
    "HR manager must approve all leave requests.",
    "Employee can view remaining balance; manager receives email on new request.");

await SeedTask(proj2.ProjectId, team5.TeamId, engId3, engId1,
    "Employee Onboarding Workflow",
    "Automated onboarding checklist sent to new hires with IT provisioning and document upload.",
    TaskStateEnum.Todo, DateTime.UtcNow.AddMonths(3),
    "Integrate with Azure AD for account creation.",
    "New hire completes all checklist items within 24h of start date.");

// Project 3 – IoT Fleet Monitoring (team6, team9)
await SeedTask(proj3.ProjectId, team6.TeamId, ownerId2, engId4,
    "IoT Device Ingestion Service",
    "MQTT broker setup and Kafka pipeline to ingest telemetry from 10,000+ vehicle sensors.",
    TaskStateEnum.Done, DateTime.UtcNow.AddMonths(-2),
    "Target throughput: 50,000 messages/second.",
    "Load test shows no message loss at 60,000 msg/s for 10 minutes.");

await SeedTask(proj3.ProjectId, team9.TeamId, ownerId2, engId6,
    "Real-time Dashboard — Map View",
    "Leaflet.js map showing live GPS positions of all vehicles with colour-coded status overlays.",
    TaskStateEnum.InProgress, DateTime.UtcNow.AddMonths(1),
    "Refresh rate: 5 seconds. Cluster markers when zoomed out.",
    "Map loads under 2s and markers update without full page refresh.");

await SeedTask(proj3.ProjectId, team9.TeamId, ownerId2, engId6,
    "Predictive Maintenance Alert Engine",
    "ML model (scikit-learn) trained on historical sensor data to predict engine failure 48h in advance.",
    TaskStateEnum.Pending, DateTime.UtcNow.AddMonths(2),
    "Model accuracy baseline: ≥85% recall on test set.",
    "Alert fires at least 24h before historical failure timestamp in validation set.");

// Project 4 – Mobile Banking (team7)
await SeedTask(proj4.ProjectId, team7.TeamId, ownerId2, engId4,
    "Biometric Authentication",
    "Face ID / Touch ID login using Flutter local_auth plugin with fallback PIN.",
    TaskStateEnum.InProgress, DateTime.UtcNow.AddMonths(1),
    "Must degrade gracefully on devices without biometric hardware.",
    "Auth succeeds on iOS 16+ and Android 10+ test devices.");

await SeedTask(proj4.ProjectId, team7.TeamId, ownerId2, engId6,
    "Fund Transfer Feature",
    "Domestic and international wire transfers with OTP confirmation and real-time balance update.",
    TaskStateEnum.Todo, DateTime.UtcNow.AddMonths(3),
    "SWIFT codes for international, IBAN for domestic.",
    "Transfer completes within 3s; balance reflects immediately in app.");

await SeedTask(proj4.ProjectId, team7.TeamId, engId4, engId6,
    "Statement PDF Export",
    "Generate signed PDF statements for any date range with transaction details and running balance.",
    TaskStateEnum.Todo, DateTime.UtcNow.AddMonths(4),
    "Use iTextSharp for PDF generation.",
    "PDF renders correctly on Adobe Reader and mobile PDF viewers.");

// Project 5 – Inventory System (team8)
await SeedTask(proj5.ProjectId, team8.TeamId, ownerId2, engId5,
    "Barcode Scanner Integration",
    "Integrate Zebra DS series USB scanners for stock-in and stock-out operations.",
    TaskStateEnum.Done, DateTime.UtcNow.AddMonths(-1),
    "Driver install guide included in onboarding docs.",
    "Scan of any EAN-13 barcode creates or updates a product record.");

await SeedTask(proj5.ProjectId, team8.TeamId, ownerId2, engId5,
    "Low-Stock Automated Alerts",
    "Email + SMS notification when SKU quantity falls below configurable threshold per warehouse.",
    TaskStateEnum.InProgress, DateTime.UtcNow.AddMonths(1),
    "Threshold configurable per SKU in admin panel.",
    "Alert sent within 60 seconds of threshold breach.");

// Project 6 – AI Support Bot (team10)
await SeedTask(proj6.ProjectId, team10.TeamId, ownerId2, engId6,
    "Intent Classification Model",
    "Fine-tune a BERT model on 50,000 labelled support tickets to classify user intent.",
    TaskStateEnum.Done, DateTime.UtcNow.AddMonths(-1),
    "Use HuggingFace Transformers; export to ONNX.",
    "Classification accuracy ≥ 92% on held-out test set.");

await SeedTask(proj6.ProjectId, team10.TeamId, ownerId2, engId6,
    "Live Chat Widget",
    "Embeddable JS widget for customer portal with typing indicators and message history.",
    TaskStateEnum.InProgress, DateTime.UtcNow.AddMonths(2),
    "Widget bundle must be < 50kb gzipped.",
    "Widget loads and connects in < 1.5s on a 4G connection.");

await SeedTask(proj6.ProjectId, team10.TeamId, engId6, engId5,
    "CRM Ticket Auto-Resolution",
    "When bot confidence > 95%, auto-close Zendesk ticket and send resolution email to customer.",
    TaskStateEnum.Todo, DateTime.UtcNow.AddMonths(3),
    "Zendesk webhook integration; log all auto-closures for audit.",
    "Auto-closure rate ≥ 40% of incoming tickets in UAT.");

Console.WriteLine($"  ✔  Tasks + TaskDetails seeded (18 tasks)");

// ═══════════════════════════════════════════════════════════════════════════════
//  DONE
// ═══════════════════════════════════════════════════════════════════════════════
Console.WriteLine();
Console.WriteLine("══════════════════════════════════════════════════════════════════");
Console.WriteLine("  🎉  Seeding completed successfully!");
Console.WriteLine("══════════════════════════════════════════════════════════════════");
Console.WriteLine();
Console.WriteLine("  ┌─────────────────────────────────────────────────────────────┐");
Console.WriteLine("  │                  TEST CREDENTIALS                           │");
Console.WriteLine("  ├────────────┬────────────────────────────────┬───────────────┤");
Console.WriteLine("  │ Role       │ Email                          │ Password      │");
Console.WriteLine("  ├────────────┼────────────────────────────────┼───────────────┤");
Console.WriteLine("  │ Owner      │ alice.owner@cms.dev            │ Owner@1234    │");
Console.WriteLine("  │ Owner      │ robert.owner@cms.dev           │ Owner@5678    │");
Console.WriteLine("  │ Customer   │ michael.cust@cms.dev           │ Cust@1234     │");
Console.WriteLine("  │ Customer   │ sara.cust@cms.dev              │ Cust@5678     │");
Console.WriteLine("  │ Customer   │ james.cust@cms.dev             │ Cust@9012     │");
Console.WriteLine("  │ Engineer   │ liam.eng@cms.dev               │ Eng@1234      │");
Console.WriteLine("  │ Engineer   │ emma.eng@cms.dev               │ Eng@5678      │");
Console.WriteLine("  │ Engineer   │ noah.eng@cms.dev               │ Eng@9012      │");
Console.WriteLine("  │ Engineer   │ olivia.eng@cms.dev             │ Eng@3456      │");
Console.WriteLine("  │ Engineer   │ ethan.eng@cms.dev              │ Eng@7890      │");
Console.WriteLine("  │ Engineer   │ sophia.eng@cms.dev             │ Eng@2345      │");
Console.WriteLine("  └────────────┴────────────────────────────────┴───────────────┘");
Console.WriteLine();
