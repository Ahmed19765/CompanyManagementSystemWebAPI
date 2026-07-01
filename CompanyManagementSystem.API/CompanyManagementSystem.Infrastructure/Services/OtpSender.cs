using CompanyManagementSystem.Application.Interfaces.Services.Communication;
using CompanyManagementSystem.Application.Interfaces.Services.RegistrationAndLogin;
using System;
using System.Threading.Tasks;

namespace CompanyManagementSystem.Infrastructure.Services
{
    public class OtpSender : IOtpSender<string>
    {
        private readonly IEmailSender _emailSender;

        public OtpSender(IEmailSender emailSender)
        {
            _emailSender = emailSender;
        }

        public async Task SendOtpAsync(string recipient, string messageBody, SenderType Via)
        {
            if (Via == SenderType.Email)
            {
                var htmlBody = $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='utf-8'>
    <title>Verify Your Email</title>
    <style>
        body {{
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            background-color: #f4f7f6;
            margin: 0;
            padding: 0;
            color: #333333;
        }}
        .container {{
            max-width: 600px;
            margin: 40px auto;
            background-color: #ffffff;
            border-radius: 16px;
            box-shadow: 0 8px 30px rgba(0, 0, 0, 0.05);
            overflow: hidden;
            border: 1px solid #eef2f1;
        }}
        .header {{
            background: linear-gradient(135deg, #4f46e5 0%, #3b82f6 100%);
            color: #ffffff;
            padding: 40px 20px;
            text-align: center;
        }}
        .header h1 {{
            margin: 0;
            font-size: 28px;
            font-weight: 700;
            letter-spacing: -0.5px;
        }}
        .content {{
            padding: 40px 30px;
            line-height: 1.6;
            text-align: center;
        }}
        .content h2 {{
            color: #1e293b;
            font-size: 22px;
            margin-top: 0;
        }}
        .content p {{
            font-size: 16px;
            color: #555555;
            margin-bottom: 30px;
        }}
        .otp-card {{
            background-color: #f8fafc;
            border: 2px dashed #cbd5e1;
            border-radius: 12px;
            padding: 20px;
            display: inline-block;
            margin: 10px auto 30px auto;
            min-width: 220px;
        }}
        .otp-code {{
            font-size: 38px;
            font-weight: 800;
            letter-spacing: 8px;
            color: #4f46e5;
            font-family: 'Courier New', Courier, monospace;
            margin: 0;
            padding-left: 8px; /* balanced spacing for letter-spacing */
        }}
        .footer {{
            background-color: #f8fafc;
            padding: 25px 20px;
            text-align: center;
            font-size: 13px;
            color: #94a3b8;
            border-top: 1px solid #f1f5f9;
        }}
        .footer a {{
            color: #3b82f6;
            text-decoration: none;
            font-weight: 600;
        }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h1>Company Management System</h1>
        </div>
        <div class='content'>
            <h2>Verify Your Email Identity</h2>
            <p>Welcome to our platform! Please use the verification code below to complete your registration or sign-in request. This code is active for 5 minutes.</p>
            
            <div class='otp-card'>
                <h1 class='otp-code'>{messageBody}</h1>
            </div>
            
            <p style='font-size: 14px; color: #94a3b8;'>If you did not request this code, you can safely ignore this email.</p>
        </div>
        <div class='footer'>
            <p>&copy; 2026 Company Management System. All rights reserved.</p>
            <p>Need support? Contact <a href='mailto:support@companymanagement.com'>Support Team</a></p>
        </div>
    </div>
</body>
</html>";

                await _emailSender.SendAsync(recipient, "Verify Your Identity - OTP Code", htmlBody);
            }
            else
            {
                throw new NotImplementedException("SMS sending is not supported yet.");
            }
        }
    }
}
