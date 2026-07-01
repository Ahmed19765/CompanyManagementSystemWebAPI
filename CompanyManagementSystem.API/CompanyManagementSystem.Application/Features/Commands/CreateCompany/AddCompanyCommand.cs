using CompanyManagementSystem.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace CompanyManagementSystem.Application.Features.Commands.CreateCompany
{
    public class AddCompanyCommand : IRequest<CompanyCreationResponse>
    {
        [JsonIgnore]
        public Guid OwnerId { get; set; }

        public string CompanyName { get; set; } = null!;
        public string CompanyDescription { get; set; } = string.Empty;
        
    }
}
