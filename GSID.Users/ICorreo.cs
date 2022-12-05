using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSID.Users
{
    public interface ICorreo
    {
        public string De { get; set; }
        public string DeLabel { get; set; }
        public Dictionary<string, string> Para { get; set; }
        public Dictionary<string, string> CopiaA { get; set; }
        public Dictionary<string, string> CopiaOcultaA { get; set; }
        public string Asunto { get; set; }
        public string Cuerpo { get; set; }
        public System.Net.Mail.MailPriority Prioridad { get; set; }
    }
}
