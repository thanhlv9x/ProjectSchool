using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace WebServerAPI.Controllers
{
    public class SendMail
    {
        public static void SendEmail(string email, string title, string message, string subject, string adminName)

        {
            //calling for creating the email body with html template   

            string body = createEmailBody(email, title, message, adminName);

            SendHtmlFormattedEmail(subject, body, email);

        }

        private static string createEmailBody(string userName, string title, string message, string adminName)

        {

            string body = string.Empty;
            //using streamreader for reading my htmltemplate   

            using (StreamReader reader = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + @"\Views\HtmlEmailTemplate\Template1.html"))

            {

                body = reader.ReadToEnd();

            }

            body = body.Replace("{UserName}", userName); //replacing the required things  

            body = body.Replace("{Title}", title);

            body = body.Replace("{message}", message);

            body = body.Replace("{adminName}", adminName);

            return body;

        }

        private static void SendHtmlFormattedEmail(string subject, string body, string email)

        {

            using (MailMessage mailMessage = new MailMessage())

            {

                mailMessage.From = new MailAddress(ConfigurationManager.AppSettings["UserName"]);

                mailMessage.Subject = subject;

                mailMessage.Body = body;

                mailMessage.IsBodyHtml = true;

                mailMessage.To.Add(new MailAddress(email));

                SmtpClient smtp = new SmtpClient();

                smtp.Host = ConfigurationManager.AppSettings["Host"];

                smtp.EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableSsl"]);

                System.Net.NetworkCredential NetworkCred = new System.Net.NetworkCredential();

                NetworkCred.UserName = ConfigurationManager.AppSettings["UserName"]; //reading from web.config  

                NetworkCred.Password = ConfigurationManager.AppSettings["Password"]; //reading from web.config  

                smtp.UseDefaultCredentials = true;

                smtp.Credentials = NetworkCred;

                smtp.Port = int.Parse(ConfigurationManager.AppSettings["Port"]); //reading from web.config  

                smtp.Send(mailMessage);
                // https://myaccount.google.com/lesssecureapps
            }

        }
    }
}