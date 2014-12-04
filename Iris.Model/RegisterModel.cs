using System.ComponentModel.DataAnnotations;

namespace Iris.Model
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "نام کاربری باید وارد شود"),
         MaxLength(17, ErrorMessage = "نام کاربری باید کمتر از 16 حرف باشد"),
         MinLength(3, ErrorMessage = "نام کاربری باید بیشتر از 2 حرف باشد")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "ایمیل باید وارد شود"), MaxLength(50, ErrorMessage = "ایمیل باید کمتر از 50 حرف باشد"),
         MinLength(4, ErrorMessage = "ایمیل باید بیشتر از 4 حرف باشد"),
         RegularExpression(
             @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$",
             ErrorMessage = "ایمیل وارد شده معتبر نمی باشد")]
        public string Email { get; set; }

        [Required(ErrorMessage = "کلمه عبور باید وارد شود"),
         MaxLength(20, ErrorMessage = "کلمه عبور باید کمتر از 20 حرف باشد"),
         MinLength(6, ErrorMessage = "کلمه عبور باید بیشتر از 6 حرف باشد")]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "کلمه عبور و تکرارش باید یکسان باشند")]
        public string ConfirmPassword { get; set; }
    }
}