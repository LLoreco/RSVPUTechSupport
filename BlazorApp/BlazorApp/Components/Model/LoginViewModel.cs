using System.ComponentModel.DataAnnotations;

namespace BlazorApp.Components.Model
{
    public class LoginViewModel
    {
        [Required(AllowEmptyStrings = false,ErrorMessage = "Укажите имя пользователя")]
        public string? UserName {  get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Укажите пароль")]
        public string? Password { get; set; }
    }
}
