using CompanyManagementSystem.Application.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace CompanyManagementSystem.Application.Features.Commands.EngineerTaskProgress
{
    public class EngineerTaskProgressCommandHandler : IRequestHandler<EngineerTaskProgressCommand, Response<string>>
    {
        public EngineerTaskProgressCommandHandler() 
        {

        }

        public async Task<Response<string>> Handle(EngineerTaskProgressCommand Request, CancellationToken cancellationToken)
        {
            return Response<string>.Ok(null!, "Not implemented yet.");
        }
    }
}
