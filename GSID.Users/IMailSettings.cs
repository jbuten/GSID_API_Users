using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSID.Users
{
    public interface IMailSettings
    {
        string From { get; set; }
        string User { get; set; }
        string Password { get; set; }
        string UrlMailRow { get; set; }
        
    }
}
