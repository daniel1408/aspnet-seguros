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
        public void EnviaEmail(string email)
        {
            string meuEmail = "dani.frears@gmail.com";
            string minhaSenha = "";

            MailMessage objEmail = new MailMessage();
            objEmail.From = new MailAddress(meuEmail, "", Encoding.UTF8);


            objEmail.To.Add(email);

            objEmail.Priority = MailPriority.High;
            objEmail.IsBodyHtml = true;
            objEmail.Subject = "Texas Seguros: Contratação de Pacote";
            objEmail.Body = "Pacote enviado para avaliação\n\n Assim que avaliarmos os seus dados, iremos mandar o boteto de cobrança";

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
