using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;

namespace GSID.Users
{
    public class MailAdmin : IMailAdmin
    {
        private IMailSettings _settings;
        private int _Port= 587;
        private string _Host= "smtp-mail.outlook.com";
        private bool _EnableSsl=true;
        private bool _UseDefaultCredentials=false;
        private bool _IsBodyHtml;
        private string _user;
        private string _pwd;

        public MailAdmin(IMailSettings settings)
        {
            _settings = settings;
            _pwd = settings.Password;
            _user = settings.User;
        }
        public MailAdmin() { 
            
        }

        private bool Enviar(MailMessage mailMessage)
        {
            try
            {
                using (SmtpClient smtp = new SmtpClient())
                            {
                                smtp.Port = _Port;
                                smtp.Host = _Host;
                                smtp.EnableSsl = _EnableSsl;
                                smtp.UseDefaultCredentials = _UseDefaultCredentials;

                                System.Net.NetworkCredential networkCredential = new System.Net.NetworkCredential();

                                networkCredential.UserName = (_user!="")?_user: "notificaciones.apps@mercasid.com.do";
                                networkCredential.Password = (_pwd!="")?_pwd:"emmasofia1990";

                                smtp.Credentials = networkCredential;
                                smtp.Send(mailMessage);
                                return true;
                            }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool EnviarCorreoConHTML(ICorreo correo)
        {
            try
            {
                using (MailMessage mailMessage = new MailMessage())
                {
                    correo.De = _user;
                    mailMessage.From = new MailAddress(correo.De, correo.DeLabel);
                    this.getMailPara(mailMessage,correo.Para);
                    //this.getMailCC(mailMessage,correo.CopiaA);
                    mailMessage.Subject = correo.Asunto;
                    mailMessage.Body = correo.Cuerpo;
                    mailMessage.Priority = correo.Prioridad;
                    mailMessage.IsBodyHtml = true;
                    return this.Enviar(mailMessage);
                }
                return false;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool EnviarCorreoSimple(ICorreo correo)
        {
            try
            {
                using (MailMessage mailMessage = new MailMessage())
                {
                    correo.De = _user;
                    mailMessage.From = new MailAddress(correo.De, correo.DeLabel);
                    this.getMailPara(mailMessage, correo.Para);
                    mailMessage.Subject = correo.Asunto;
                    mailMessage.Body = correo.Cuerpo;
                    mailMessage.Priority = correo.Prioridad;
                    mailMessage.IsBodyHtml = false;
                    return this.Enviar(mailMessage);
                }
                return false;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        private void getMailPara(MailMessage mailMessage, Dictionary<string,string> paras) {
            foreach (var item in paras)
            {
                mailMessage.To.Add(new MailAddress(item.Key, item.Value));
            }
        }
        private void getMailCC(MailMessage mailMessage, Dictionary<string, string> paras)
        {
            foreach (var item in paras)
            {
                mailMessage.CC.Add(new MailAddress(item.Key, item.Value));
            }
        }
        private void getMailCCO(MailMessage mailMessage, Dictionary<string, string> paras)
        {
            foreach (var item in paras)
            {
                mailMessage.Bcc.Add(new MailAddress(item.Key, item.Value));
            }
        }

         
        public void EnviarNotificacion(List<MailRow> mailRows, Dictionary<string,string> listadoPara, string path)
        {
            string cuerpoTBody = "";
            /*
            //Raiz plantilla
            string path = webHostEnvironment.ContentRootPath + "wwwroot\\AcuerdosDePago\\";

            if (!OperatingSystem.IsWindows())
            {
                path = webHostEnvironment?.ContentRootPath + "wwwroot/AcuerdosDePago/";
            }*/

            //Plantilla
            string body = System.IO.File.ReadAllText(path + "email1.html");

            //Agrega un nuevo TR al Tbody, con sus datos por columna
            cuerpoTBody = this.GetTBodyEmail(mailRows, path);

            body = body.Replace("{{bodyAutorizacionesPendientes}}", $"{cuerpoTBody}");

            this.EnviarCorreoConHTML(new GSID.Users.Correo
            {
                De = _settings.From,
                Asunto = "Notificación",
                Cuerpo = body,
                Prioridad = System.Net.Mail.MailPriority.Normal,
                Para = listadoPara
            });
            //Console.WriteLine("Correo enviado..");
        }

        /// <summary>
        /// Configura los TR
        /// </summary>
        /// <param name="acuerdoPagos">List<AcuerdoPago> acuerdoPagos</param>
        /// <param name="path">string path</param>
        /// <returns>string</returns>
        public string GetTBodyEmail(List<MailRow> mailRows, string path)
        {

            //TR Plantilla
            string plantillaTD = System.IO.File.ReadAllText(path + "email_row_plantilla.html");
            plantillaTD = plantillaTD.Replace("{{urlMailRow}}",_settings.UrlMailRow);

            var cuerpoTBody = "";
            foreach (var item in mailRows)
            {
                cuerpoTBody += plantillaTD
                    .Replace("{{clienteCode}}", $"{item.Cliente}")
                    .Replace("{{clienteNombre}}", $"{item.NombreCliente}")
                    .Replace("{{empresa}}", $"{item.NombreEmpresa}")
                    .Replace("{{creadoPor}}", $"{item.Usuario}")
                    .Replace("{{acuerdoId}}", $"{item.AcuerdoId}")
                    ;
            }
            return cuerpoTBody;
        }

    }
}
