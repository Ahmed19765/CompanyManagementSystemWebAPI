using System;
using System.Collections.Generic;
using System.Text;

namespace CompanyManagementSystem.Application.Interfaces.Services.Communication
{
    public interface IEmailSender
    {
        Task SendAsync(string to, string subject, string body);
    }
}
