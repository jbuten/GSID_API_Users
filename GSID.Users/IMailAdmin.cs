using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
namespace GSID.Users
{
    public interface IMailAdmin
    {
        bool EnviarCorreoConHTML(ICorreo correo);
        bool EnviarCorreoSimple(ICorreo correo);
        void EnviarNotificacion(List<MailRow> acuerdosPago, Dictionary<string, string> listadoPara, string path);
    }
}
