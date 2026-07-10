using System;
using System.Collections.Generic;
using System.Text;

namespace CompanyManagementSystem.Application.Features.Queries.GetProjects
{
    public class GetProjectsResponse
    {
       public IEnumerable<ProjectsDto> Projects { get; set; } = new List<ProjectsDto>();
    }

    public class ProjectsDto
    {
        public int ProjectId { get; set; }
        public string ProjectTitle { get; set; } = null!;
        public string? ProjectDescription { get; set; }
        public decimal OfferedBudget { get; set; }
        public DateTime? UploadedDate { get; set; }
        public string? ProjectState { get; set; }
    }
}
