using System.Net;
using System.Net.Mail;

namespace QL_BanGiay.Areas.Admin.Models
{
    public class SendMails
    {
        private static string pasword = "itkaoocwlhxckjuf";
        private static string email = "vdkhoa0512@gmail.com";
        public static bool SendMail(string name, string subject, string content, string toMail)
        {
            try
            {
                MailMessage message = new MailMessage();
                var smtp = new SmtpClient();
                {
                    smtp.Host = "smtp.gmail.com";
                    smtp.Port = 587;
                    smtp.EnableSsl = true;
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.Credentials = new NetworkCredential(email, pasword);
                    smtp.Timeout = 10000;
                }
                MailAddress fromAddress = new MailAddress(email, name);
                message.From = fromAddress;
                message.To.Add(toMail);
                message.Subject = subject;
                message.IsBodyHtml = true;
                message.Body = content;
                smtp.Send(message);
                smtp.Dispose();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return false;
            }
        }
    }
}
