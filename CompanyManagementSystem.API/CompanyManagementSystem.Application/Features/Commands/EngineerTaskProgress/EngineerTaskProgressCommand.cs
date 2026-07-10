using CompanyManagementSystem.Application.Features.Commands.CreateTeam;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace CompanyManagementSystem.Application.Features.Commands.EngineerTaskProgress
{
    public class EngineerTaskProgressCommand : IRequest<EngineerTaskProgressResponse>
    {
        [JsonIgnore]
        public Guid EngineerId { get; set; }
        
    }
}
