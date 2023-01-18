using System.ComponentModel.DataAnnotations;

namespace FileServer.Models.Account
{
    public class SignUpModel
    {
        [Required(ErrorMessage = "Придумайте никнейм")] 
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Никнейм должен состоять минимум из 3-ёх символов")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Введите емаил")]
        [EmailAddress(ErrorMessage = "Пожалуйста, введите верный адрес электронной почты")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Придумайте пароль")]
        [StringLength(50, MinimumLength = 8, ErrorMessage = "Пароль должен состоять минимум из 8-и символов и 3-ёх уникальных символов")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Подтвердите пароль")]
        [StringLength(50, MinimumLength = 8, ErrorMessage = "Пароль должен состоять минимум из 8-и символов и 3-ёх уникальных символов")]
        [Compare("Password", ErrorMessage = "Пожалуйста, введите такой же пароль")]
        public string ConfirmPassword { get; set; }
    }
}
