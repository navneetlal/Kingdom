using System.Net;
using System.Net.Mail;
using System.Text;

namespace KingdomApi.Services
{
    public class MailManager
    {
        public static bool SendMail(string subject, string body, string to)
        {
            MailMessage message = new MailMessage("", to)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true,
                BodyEncoding = Encoding.UTF8
            };

            SmtpClient client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("yourmail id", "Password")
            };
            try
            {
                client.Send(message);
            }

            catch (System.Exception ex)
            {
                throw ex;
            }

            return true;
        }
    }
}
