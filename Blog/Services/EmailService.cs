using System.Net;
using System.Net.Mail;

namespace Blog.Services
{
    public class EmailService
    {
        public bool Send(
           string toName,
           string toEmail,
           string subject,
           string body,
           string fromName = "Equipe balta.io",
           string fromEmail = "email@balta.io")
        {
            var smptClient = new SmtpClient(Configuration.Smtp.Host, Configuration.Smtp.Port);

            smptClient.Credentials = new NetworkCredential(Configuration.Smtp.UserName, Configuration.Smtp.Password);
            smptClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smptClient.EnableSsl = true;


            var mail = new MailMessage();

            mail.From = new MailAddress(fromEmail, fromName);
            mail.To.Add(new MailAddress(toEmail, toName));
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;

            try
            {
                smptClient.Send(mail);
                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }
    }
}
