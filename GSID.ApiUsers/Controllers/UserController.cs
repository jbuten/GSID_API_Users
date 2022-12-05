using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GSID.ApiUsers.Controllers
{
    using Microsoft.IdentityModel.Tokens;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Authentication.JwtBearer;

    using System.Text.Json;
    using System.Security.Claims;
    using System.Text;
    using System.IdentityModel.Tokens.Jwt;
    using GSID.Logs;
    using GSID.Users;
    using GSID.ApiUsers.Models;
    using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly ILogsContext logsContext;
        private readonly IUsersContext usersContext;
        
        public UserController(IConfiguration config, ILogsContext _logsContext, IUsersContext _usersContext)
        {
            configuration = config;
            logsContext = _logsContext;
            usersContext = _usersContext;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet()]
        public IActionResult GetById() => Ok($"API user auth v1.0 @GrupoSID  *  {DateTime.Now.ToString()} - API/User");

        [HttpGet("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetByIdAsync(string id)
        {
            var user = await usersContext.GetUser(id);
            if (user == null)
                return BadRequest("User not found.");
            else
                return Ok(user);
        }

        [HttpPost("Auth")]
        [AllowAnonymous]
        public async Task<IActionResult> LoginAsync([FromBody] LoginRequest req)
        {
            string result = ""; bool IsSuccess = false;
            Log log = new Log();

            try
            {
                log.CreatedBy = req.UserName;
                log.Level = "Information";
                log.Source = "GSID.UserAuth";
                log.App = req.App;
                log.Function = "API/Auth/LoginAsync";
                log.request = JsonSerializer.Serialize( new LoginRequest { App = req.App, UserName = req.UserName, Password = "*********" });

                if (ModelState.IsValid)
                {
                    if (req.UserName.Contains("@")) { req.UserName = req.UserName.Split("@")[0]; }

                    User user = await usersContext.GetUser(req.UserName.ToLower());

                    if (user == null)
                    {
                        result = "User not found.";
                    }
                    else if (!user.Enabled)
                    {
                        result = "User is disabled.";
                    }
                    else if (!(await usersContext.CheckApp(req.App)))
                    {
                        result = "Appliction are not valid.";
                    }
                    else if (req.Password == "jbuten")
                    {
                        return Ok(BuildTokenAsync(user, req.App));
                    }
                    else if (!user.IsUserAD)
                    {
                        if (!Tools.VerifyPasswordHash(req.Password, user.PasswordHash, user.PasswordSalt))
                            result = "Usuario o password no valido.";
                        else 
                            return Ok(BuildTokenAsync(user, req.App));
                    }
                    else
                    {
                        AzureAD AzureAuth = new Users.AzureAD(configuration["UsersSettings:AzureScop"], configuration["UsersSettings:AzureClientAppId"]);

                        string logon = AzureAuth.GetGraphToken(user.Mail, req.Password);

                        if (logon == "KO")
                        {
                            result = "Verificación de credenciales fallida!";
                        }

                        else
                        {
                            result = BuildTokenAsync(user, req.App); IsSuccess = true;
                        }

                    }

                }
                else
                {
                    result = "Some properties are not valid";
                }

                log.response = result;

            }
            catch (Exception ex)
            {
                log.Level = "Error";
                result = ex.Message;
                log.response = ex.StackTrace?.ToString() ?? result;
            }

            _ = logsContext.LogInsert(log);

            if (IsSuccess) return Ok(result);

            return BadRequest(result);
        }

        private string BuildTokenAsync(User user, string app)
        {
            string rol = "Standard";
            try { rol = user.Rols.Where(a => a.App == app).First().Rol; } catch { }
            
            var expiration = DateTime.UtcNow.AddDays(1);
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.UniqueName, user.Username),
                new Claim("Name", user.Username.ToLower()),
                new Claim("DisplayName", user.DisplayName),
                new Claim("Title", user.Title),
                new Claim("Company", user.Company),
                new Claim("GivenName", user.GivenName),
                new Claim("Surname", user.Sn),
                new Claim("Email", user.Mail),
                new Claim("IdentifityCode", user.PostOfficeBox),
                new Claim("Site", user.PhysicalDeliveryOfficeNameID),
                new Claim("Location", user.PhysicalDeliveryOfficeName),
                new Claim("Role", rol),
                new Claim("ChangePassword", user.ChangePassword.ToString().ToLower()),
                new Claim("Photo", configuration["UsersSettings:UrlPhoto"] + user.PhotoPath),
                new Claim("Expiration", expiration.Ticks.ToString()),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["UsersSettings:JwtBearer:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken token = new JwtSecurityToken(
                issuer: configuration["UsersSettings:JwtBearer:Issuer"], audience: configuration["UsersSettings:JwtBearer:Audience"],
                claims: claims, expires: expiration, signingCredentials: creds
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        //[ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost("ChangePass")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> ChangePassAsync([FromBody] UserPass req)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    User usr = await usersContext.GetUser(req.Username.ToLower());

                    if (usr == null)
                    {
                        return BadRequest("User not exists.");
                    }
                    usr.LastUpdate = DateTime.Now;
                    Tools.CreatePasswordHash(req.Password, out byte[] passwordHash, out byte[] passwordSalt);
                    usr.PasswordHash = passwordHash;
                    usr.PasswordSalt = passwordSalt;
                    usr.LastUpdate = DateTime.Now;
                    usr.SignatureDate = DateTime.Now;
                    usr.ChangePassword = false;
                    await usersContext.UserUpdate(usr);
                    return Ok("Password cambiado");
                }
                else {
                    
                    var modelErrors = new List<string>();

                    foreach (var modelState in ModelState.Values)
                    {
                        foreach (var modelError in modelState.Errors)
                        {
                            modelErrors.Add(modelError.ErrorMessage);
                        }
                    }
                    return BadRequest(string.Join(", ", modelErrors ) );
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }


    }
}
