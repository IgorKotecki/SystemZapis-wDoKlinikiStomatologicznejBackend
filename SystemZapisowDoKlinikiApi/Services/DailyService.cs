using ProjektSemestralnyTinWebApi.Security;

namespace SystemZapisowDoKlinikiApi.Services;

public class DailyService : BackgroundService
{
    private readonly IEmailService _emailService;
    private readonly IAppointmentService _appointmentService;

    public DailyService(IEmailService emailService, IAppointmentService appointmentService)
    {
        _emailService = emailService;
        _appointmentService = appointmentService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var now = DateTime.Now;
            var nextRun = DateTime.Today.AddHours(8);

            if (now >= nextRun)
                nextRun = nextRun.AddDays(1);

            await Task.Delay(nextRun - now, stoppingToken);

            await DoWork(stoppingToken, DateTime.Now);
        }
    }

    private async Task DoWork(CancellationToken stoppingToken, DateTime dateTime)
    {
        var appointments = await _appointmentService.GetAppointmentsByDate("pl", dateTime.AddDays(1));
        foreach (var appointment in appointments)
        {
            var patientEmail = appointment.User.Email;
            var topic = "Reminder: Upcoming Appointment";
            var body = "Dear " + appointment.User.Name + "," +
                       "\n\nWe hope this message finds you well. This is a friendly reminder about your upcoming appointment at our clinic." +
                       "\n\n**Appointment Details:**" +
                       "\n- **Date:** " + appointment.StartTime.ToString("yyyy-MM-dd") +
                       "\n- **Time:** " + appointment.StartTime.ToString("HH:mm") +
                       "\n\nPlease ensure to arrive at least 10 minutes before your scheduled time. If you have any questions or need to reschedule, feel free to contact us at " +
                       "+48" + "123456789" + " or reply to this email." +
                       "\n\nThank you for choosing our clinic. We look forward to seeing you!" +
                       "\n\nBest regards";
            await _emailService.SendEmailAsync(patientEmail, topic, body);
        }
    }
}