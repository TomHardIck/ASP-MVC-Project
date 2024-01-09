using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Не указан номер!")]
        public string PhoneNumber { get; set; } = null!;
        [Required(ErrorMessage = "Не указан пароль!")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;
    }
}
