
namespace GSID.Users
{
    using Microsoft.Identity.Client;
    using System.Security;

    public class AzureAD
    {
        private readonly string scop;
        private readonly string clientAppId;

        public AzureAD(string Scop = "https://graph.microsoft.com/.default", string ClientAppId = "")
        {
            scop = Scop;
            clientAppId = ClientAppId;
            
        }
        public string GetGraphToken(string username, string pwd)
        {

            string token = "KO";

            var app = PublicClientApplicationBuilder
                      .Create(clientAppId)
                      .WithAuthority(AadAuthorityAudience.AzureAdMultipleOrgs).Build();

            SecureString password = new SecureString();
            foreach (char c in pwd) { password.AppendChar(c); }

            var Scopes = new string[] { scop };

            var App = new PublicAppUsingUsernamePassword(app);
            var result = App.AcquireATokenFromCacheOrUsernamePasswordAsync(Scopes, username, password).GetAwaiter().GetResult();

            if (result != null)
            {
                token = result.AccessToken;
            }

            return token;
        }
    }
}
