using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace CompanyManagementSystem.Application.Features.Queries.GetProjects
{
    public class GetProjectsQuery : IRequest<GetProjectsResponse>
    {
        public Guid OwnerId { get; set; }
    }
}
