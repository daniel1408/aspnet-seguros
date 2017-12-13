using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using segchatbot.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;

namespace segchatbot.Service
{
    public class Email
    {
        public void EnviaEmail(string emailDestinatario)
        {
            string meuEmail = "dani.frears@gmail.com";
            string minhaSenha = "Swagger1408";

            MailMessage objEmail = new MailMessage();
            objEmail.From = new MailAddress(meuEmail, "", Encoding.UTF8);

            objEmail.To.Add(emailDestinatario);

            objEmail.Priority = MailPriority.High;
            objEmail.IsBodyHtml = true;
            objEmail.Subject = "Texas Seguros: Contratação de Pacote";
            objEmail.Body = "Corpo do Email";
            objEmail.SubjectEncoding = Encoding.GetEncoding("ISO-8859-1");
            objEmail.BodyEncoding = Encoding.GetEncoding("ISO-8859-1");

            var objSmtp = new SmtpClient();
            {
                objSmtp.UseDefaultCredentials = false;
                objSmtp.Host = "smtp.gmail.com";
                objSmtp.Port = 587;
                objSmtp.Credentials = new NetworkCredential(meuEmail, minhaSenha);
                objSmtp.EnableSsl = true;
                objSmtp.Timeout = 15000;
            }
            objSmtp.Send(objEmail);
        }
    }
}