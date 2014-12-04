using System.ComponentModel.DataAnnotations;

namespace Iris.Model
{
    public class LogOnModel
    {
        [Required(ErrorMessage = "وارد کردن شناسه لازم است"),
         MinLength(3, ErrorMessage = "شناسه باید حداقل سه حرف باشد")]
        public string Identity { get; set; }

        [Required(ErrorMessage = "وارد کردن کلمه عبور لازم است"),
         MinLength(6, ErrorMessage = "کلمه عبور باید حداقل شش حرف باشد")]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}