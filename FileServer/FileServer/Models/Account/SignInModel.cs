using System.ComponentModel.DataAnnotations;

namespace FileServer.Models.Account
{
    public class SignInModel
    {
        [Required(ErrorMessage = "Введите никнейм")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Никнейм должен состоять минимум из 3-ёх символов")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Введите пароль")]
        [StringLength(50, MinimumLength = 8, ErrorMessage = "Пароль должен состоять минимум из 8-и символов и 3-ёх уникальных символов")]
        public string Password { get; set; }
    }
}
