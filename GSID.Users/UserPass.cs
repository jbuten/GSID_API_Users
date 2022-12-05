namespace GSID.Users
{
    using System.ComponentModel.DataAnnotations;
    public class UserPass
    {
        [Required]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "El password es requerido")]
        [StringLength(255, ErrorMessage = "Debe tener entre 4 y 255 caracteres", MinimumLength = 4)]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "La confirmacion del password es requerida")]
        [StringLength(255, ErrorMessage = "Debe tener entre 4 y 255 caracteres", MinimumLength = 4)]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; } = string.Empty;

        public bool ChangePassword { get; set; } = true;
    }
}
