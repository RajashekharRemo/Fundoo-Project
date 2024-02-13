using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Send
    { 
        public string SendingMail(string emailTo, string token)
        {
            try
            {
                string emailFrom = "rajasekharraja7799@gmail.com";

                MailMessage message = new MailMessage(emailFrom, emailTo);

                string mailBody = "Token generated : " + token;
                message.Subject = "Generated token willexpire after 1 hour";
                message.Body = mailBody.ToString();
                message.BodyEncoding = Encoding.UTF8;
                message.IsBodyHtml = true;

                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
                NetworkCredential credential = new NetworkCredential(
                    "rajasekharraja7799@gmail.com", "glyn jaip ygyh bwxq");

                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = credential;
                smtpClient.Send(message);
                return emailFrom;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
