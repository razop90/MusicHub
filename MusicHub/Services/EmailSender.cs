using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net;

namespace MusicHub.Services
{
    public class EmailSender : IEmailSender
    {
        private EmailSenderInfo Information { get; set; }

        /// <summary>
        /// Getting the noreply email information from the secrets file.
        /// </summary>
        /// <param name="informationAccessor">information object</param>
        public EmailSender(IOptions<EmailSenderInfo> informationAccessor)
        {
            Information = informationAccessor.Value;
        }

        public Task SendEmailAsync(string email, string subject, string message)
        {
            return SendEmail(email, subject, message);
        }

        private Task SendEmail(string sendto, string subject, string body)
        {
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(Information.NorplyAddress);
            //configuring the SMTP client
            SmtpClient smtp = new SmtpClient();
            smtp.Port = 587;  //most of the people claim to success with this port number
            smtp.EnableSsl = true;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.UseDefaultCredentials = false;
            //the username and pass of the sender email - noreplymusichub
            smtp.Credentials = new NetworkCredential(mail.From.User, Information.NoreplyPassword);
            smtp.Host = "smtp.gmail.com";

            //recipient address
            mail.To.Add(new MailAddress(sendto));

            //Formatted mail body
            mail.IsBodyHtml = true;
            mail.Subject = subject;
            mail.Body = body;

            return smtp.SendMailAsync(mail);
        }
    }
}