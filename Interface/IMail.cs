namespace Psychiatrist_Management_System.Interface
{
    public interface IMail
    {
        Task<dynamic> SendEmailAsync(string toEmail, string subject, string body);

    }
}
