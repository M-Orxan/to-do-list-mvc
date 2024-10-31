
using System.Net;
using System.Net.Mail;

namespace ToDoList.Services
{
    public class EmailSender : IEmailSender
    {
       

        public Task SendEmailAsync(string name, string fromEmail, string subject, string body)
        {
            var toEmail = "orxanm385@gmail.com";
            var password = "bujg jlxb smmb xfdq";

            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(toEmail, password)
            };

            var message = new MailMessage();
            message.From = new MailAddress(toEmail);
            message.To.Add(toEmail);
            message.Subject = subject;
            message.IsBodyHtml = true;
            message.Body = $"<p><strong>Name:</strong> {name}</p>" +
                          $"<p><strong>Email:</strong> {fromEmail}</p>" +
                          $"<p><strong>Message:</strong></p><p>{body}</p>";

            return client.SendMailAsync(message);




        }

    }
}
