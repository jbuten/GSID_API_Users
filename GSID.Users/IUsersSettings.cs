using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSID.Users
{
    public interface IUsersSettings
    {
        string ConnectionString { get; set; }
        string AzureScop { get; set; }
        string AzureClientAppId { get; set; }
    }

    public class UsersSettings : IUsersSettings
    {
        public string ConnectionString { get; set; } = null!;
        public string AzureScop { get; set; } = string.Empty;
        public string AzureClientAppId { get; set; } = string.Empty;
    }
}
