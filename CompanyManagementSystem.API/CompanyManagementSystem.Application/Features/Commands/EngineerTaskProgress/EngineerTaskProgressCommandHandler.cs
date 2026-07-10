using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace CompanyManagementSystem.Application.Features.Commands.EngineerTaskProgress
{
    public class EngineerTaskProgressCommandHandler : IRequestHandler<EngineerTaskProgressCommand, EngineerTaskProgressResponse>
    {
        public EngineerTaskProgressCommandHandler() 
        {

        }

        public async Task<EngineerTaskProgressResponse> Handle(EngineerTaskProgressCommand Request, CancellationToken cancellationToken)
        {
            return new EngineerTaskProgressResponse() { Message = "Not Implemented Yet!" };
        }
    }
}
