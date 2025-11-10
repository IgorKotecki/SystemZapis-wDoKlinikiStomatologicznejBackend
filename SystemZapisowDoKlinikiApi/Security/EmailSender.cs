using System.Net;
using System.Net.Mail;

namespace ProjektSemestralnyTinWebApi.Security;

public class EmailSender : IEmailService
{
    private readonly IConfiguration _configuration;
    public EmailSender(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public async Task SendEmailAsync(string to, string subject, string body)
    {
        var mail = _configuration["EmailSettings:Mail"]!;
        var password = _configuration["EmailSettings:Password"]!;
        var smtpHost = _configuration["EmailSettings:Host"]!;
        var smtpPort = int.Parse(_configuration["EmailSettings:Port"] ?? "587");
        
        if (string.IsNullOrWhiteSpace(to))
        {
            throw new ArgumentException("Email recipient cannot be null or empty", nameof(to));
        }
        
        var smtpClient = new SmtpClient(smtpHost, smtpPort)
        {
            Credentials = new NetworkCredential(mail, password),
            EnableSsl = true,
        };
        var mailMessage = new MailMessage
        {
            From = new MailAddress(mail),
            To = { to },
            Subject = subject,
            Body = body,
        };
        try
        {
            await smtpClient.SendMailAsync(mailMessage);
        }
        catch (SmtpException ex)
        {
            Console.WriteLine($"{ex.Message}");
            throw;
        }
        return;
    }
}