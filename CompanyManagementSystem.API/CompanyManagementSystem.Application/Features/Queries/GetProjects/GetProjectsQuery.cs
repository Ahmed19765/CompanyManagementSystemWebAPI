using CompanyManagementSystem.Application.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace CompanyManagementSystem.Application.Features.Queries.GetProjects
{
    public class GetProjectsQuery : IRequest<Response<IEnumerable<ProjectsDto>>>
    {
        public Guid OwnerId { get; set; }
    }
}
