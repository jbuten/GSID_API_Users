

namespace GSID.Users
{
    using MongoDB.Bson.Serialization.Attributes;
    using System.ComponentModel.DataAnnotations;

    [BsonIgnoreExtraElements]
    public class User
    {
        [BsonId]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required(ErrorMessage = "El {0} es un valor requerido")]
        public string Username { get; set; } = "";

        [BsonIgnoreIfNull]
        public string Password { get; set; } = "";
        [BsonIgnoreIfNull]
        public byte[]? PasswordHash { get; set; }
        [BsonIgnoreIfNull]
        public byte[]? PasswordSalt { get; set; }
        public bool ChangePassword { get; set; } = false;
        public string Mail { get; set; } = string.Empty;
        public bool IsUserAD { get; set; }


        public string Cn { get; set; } = string.Empty;
        public string Sn { get; set; } = string.Empty;
        public string C { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
        public string PostOfficeBox { get; set; } = string.Empty;
        public string PhysicalDeliveryOfficeName { get; set; } = string.Empty;
        public string PhysicalDeliveryOfficeNameID { get; set; } = string.Empty;
        public string GivenName { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string Co { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public string DepartmentID { get; set; } = string.Empty;
        public string Company { get; set; } = string.Empty;
        public string CompanyID { get; set; } = string.Empty;
        public string TargetAddress { get; set; } = string.Empty;
        public string MailNickname { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string UserAccountControl { get; set; } = string.Empty;
        public string CountryCode { get; set; } = string.Empty;
        public string PrimaryGroupID { get; set; } = string.Empty;
        public string SAMAccountName { get; set; } = string.Empty;
        public string SAMAccountType { get; set; } = string.Empty;
        public string UserPrincipalName { get; set; } = string.Empty;


        [BsonIgnoreIfNull]
        public string Mobile { get; set; } = "";
        [BsonIgnoreIfNull]
        public string Manager { get; set; } = string.Empty;


        public bool Enabled { get; set; }
        public DateTime LastUpdate { get; set; }

        [BsonIgnoreIfNull]
        public string IPAddress { get; set; } = "";

        public string TelephoneNumber { get; set; } = "";
        public string Expiration { get; set; } = "";
        public string PhotoPath { get; set; } = "user.png";
        public string Photo { get; set; } = "";
        public string WhenCreated { get; set; } = "";
        public List<UserRol> Rols { get; set; } = new List<UserRol>();

        public string Signature { get; set; } = "i.jpg";
        public string SignatureBy { get; set; } = "";
        public DateTime SignatureDate { get; set; }
        public string SignatureDateDisplay
        {
            get { return (Signature == "i.jpg") ? "" : SignatureDate.ToString("dd-MM-yyyy hh:mm tt"); }
        }

    }
}