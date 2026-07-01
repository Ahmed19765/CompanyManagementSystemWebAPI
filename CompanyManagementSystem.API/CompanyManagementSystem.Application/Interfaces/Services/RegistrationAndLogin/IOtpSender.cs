using System;
using System.Collections.Generic;
using System.Text;

namespace CompanyManagementSystem.Application.Interfaces.Services.RegistrationAndLogin
{
    public interface IOtpSender<OTPMessageBody>
    {
        Task SendOtpAsync(string recipient, OTPMessageBody messageBody, SenderType Via);
    }

    public enum SenderType
    {
        Email,
        Sms // Not Supported Yet
    }
}
