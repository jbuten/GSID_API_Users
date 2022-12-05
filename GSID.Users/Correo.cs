using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

 namespace GSID.Users
{
    public class Correo :ICorreo
    {
        /// <summary>
        /// Correo desde el que se enviara
        /// </summary>
        public string De { get; set; } = "test.eudy.arias@outlook.es";

        /// <summary>
        /// Label para el correo desde donde se enviara
        /// </summary>
        public string DeLabel { get; set; } = "App Portal";

        /// <summary>
        /// Key:Correo, Value:Label
        /// </summary>
        public Dictionary<string,string> Para { get; set; }
        /// <summary>
        /// Asunto del correo
        /// </summary>
        public string Asunto { get; set; }
        /// <summary>
        /// Cuerpo del correo
        /// </summary>
        public string Cuerpo { get; set; }
        /// <summary>
        /// Nivel de prioridad del correo 
        /// </summary>
        public System.Net.Mail.MailPriority Prioridad { get; set; }
        /// <summary>
        /// Key:Correo, Value:Label
        /// </summary>
        public Dictionary<string, string> CopiaA { get; set; }
        /// <summary>
        /// Key:Correo, Value:Label
        /// </summary>
        public Dictionary<string, string> CopiaOcultaA { get; set; }
        
    }


   
}
