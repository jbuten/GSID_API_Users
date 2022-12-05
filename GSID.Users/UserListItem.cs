

namespace GSID.Users
{
    using System.Text.Json.Serialization;
    public class UserListItem
    {
        public string UserName { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string PhotoPath { get; set; } = string.Empty;
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Company { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Email { get; set; } = string.Empty;

        /// <summary>
        /// SAMAccountName, UserDomain
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? UsuarioSAP { get; set; } = string.Empty;

    }
}
