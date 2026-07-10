using Microsoft.AspNetCore.Identity;
using CompanyManagementSystem.Seeder.Models.Enums;

namespace CompanyManagementSystem.Seeder.Models
{
    // ── User ──────────────────────────────────────────────────────────────────
    // Mirrors Domain.Entities.User : IdentityUser<Guid>
    public class SeederUser : IdentityUser<Guid>
    {
        public string? FirstName  { get; set; }
        public string? LastName   { get; set; }
        public UserRoleEnum Role  { get; set; }
        public bool IsBanned      { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

    // ── Company ───────────────────────────────────────────────────────────────
    public class SeederCompany
    {
        public int     CompanyId          { get; set; }
        public string? CompanyName        { get; set; }
        public string? CompanyDescription { get; set; }
        public string? JoinCode           { get; set; } = Guid.NewGuid().ToString();
        public Guid?   OwnerId            { get; set; }
    }

    // ── CompanyUser ───────────────────────────────────────────────────────────
    public class SeederCompanyUser
    {
        public int             CompanyId { get; set; }
        public Guid            UserId    { get; set; }
        public CompanyRankEnum Rank      { get; set; } = CompanyRankEnum.Member;
        public DateTime        JoinedAt  { get; set; } = DateTime.UtcNow;
    }

    // ── Department ────────────────────────────────────────────────────────────
    public class SeederDepartment
    {
        public int     DepartmentId   { get; set; }
        public string? DepartmentName { get; set; }
        public int     CompanyId      { get; set; }
    }

    // ── Team ──────────────────────────────────────────────────────────────────
    public class SeederTeam
    {
        public int     TeamId          { get; set; }
        public string? TeamName        { get; set; }
        public string? TeamDescription { get; set; }
        public Guid?   LeaderId        { get; set; }
        public int     DepartmentId    { get; set; }
    }

    // ── UserTeam ──────────────────────────────────────────────────────────────
    public class SeederUserTeam
    {
        public Guid     UserId   { get; set; }
        public int      TeamId   { get; set; }
        public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
        public string?  TeamRole { get; set; }
    }

    // ── Project ───────────────────────────────────────────────────────────────
    public class SeederProject
    {
        public int      ProjectId             { get; set; }
        public string?  ProjectTitle          { get; set; }
        public string?  ProjectDescription    { get; set; }
        public string?  ProjectDocumentsUrl   { get; set; }
        public decimal  ProjectOfferedBudget  { get; set; }
        public DateTime? UploadedDate         { get; set; } = DateTime.UtcNow;
        public Guid?    CustomerId            { get; set; }
    }

    // ── ProjectTeam ───────────────────────────────────────────────────────────
    public class SeederProjectTeam
    {
        public int      ProjectId  { get; set; }
        public int      TeamId     { get; set; }
        public DateTime AssignedAt { get; set; } = DateTime.UtcNow;
    }

    // ── CompanyOffer ──────────────────────────────────────────────────────────
    public class SeederCompanyOffer
    {
        public int              CompanyId             { get; set; }
        public int              ProjectId             { get; set; }
        public decimal          OfferedBudget         { get; set; }
        public DateTime         StartDate             { get; set; }
        public DateTime         DeliveryExceptedDate  { get; set; }
        public OfferStatusEnum  Status                { get; set; }
    }

    // ── Task ──────────────────────────────────────────────────────────────────
    public class SeederTask
    {
        public int           TaskId          { get; set; }
        public string?       TaskTitle       { get; set; }
        public string?       TaskDescription { get; set; }
        public TaskStateEnum Status          { get; set; } = TaskStateEnum.Todo;
        public DateTime      CreatedAt       { get; set; } = DateTime.UtcNow;
        public DateTime?     DueDate         { get; set; }
        public int           ProjectId       { get; set; }
        public int?          TeamId          { get; set; }
        public Guid?         AssignedById    { get; set; }
        public Guid?         AssignedToId    { get; set; }
    }

    // ── TaskDetails ───────────────────────────────────────────────────────────
    public class SeederTaskDetails
    {
        public int     TaskDetailsId             { get; set; }
        public int     TaskId                    { get; set; }
        public string? Notes                     { get; set; }
        public string? TaskAttachmentDocumentUrl { get; set; }
        public string? AcceptanceCriteria        { get; set; }
    }
}
