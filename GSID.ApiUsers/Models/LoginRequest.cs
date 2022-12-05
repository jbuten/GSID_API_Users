namespace GSID.ApiUsers.Models
{
    using System.ComponentModel.DataAnnotations;
    public class LoginRequest
    {
        [Display(Name = "Aplicacion")]
        [Required(ErrorMessage = "{0}, valor requerido")]
        [StringLength(140, ErrorMessage = "{0}, debe tener al menos {2} un maximo de {1} caracteres. ", MinimumLength = 4)]
        public string App { get; set; } = string.Empty!;

        [Display(Name = "Usuario")]
        [Required(ErrorMessage = "{0}, valor requerido")]
        [StringLength(50, ErrorMessage = "{0}, debe tener al menos {2} un maximo de {1} caracteres. ", MinimumLength = 4)]
        public string UserName { get; set; } = string.Empty!;

        [Display(Name = "Password")]
        [Required(ErrorMessage = "{0}, valor requerido")]
        [StringLength(50, ErrorMessage = "{0}, debe tener al menos {2} un maximo de {1} caracteres. ", MinimumLength = 4)]
        public string Password { get; set; } = string.Empty!;
    }
}
