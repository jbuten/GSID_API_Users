using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSID.Users
{

    public class MailSettings : IMailSettings
    {
        public string User { get; set; }
        public string Password { get; set; }
        public string From { get; set; }
        public string UrlMailRow { get; set; }
        
    }
}
