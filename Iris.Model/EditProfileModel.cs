using System;
using System.ComponentModel.DataAnnotations;

namespace Iris.Model
{
    public class EditProfileModel
    {
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string AvatarPath { get; set; }
        public string OldPassword { get; set; }

        [MaxLength(20, ErrorMessage = "کلمه عبور باید کمتر از 20 حرف باشد"),
         MinLength(6, ErrorMessage = "کلمه عبور باید بیشتر از 6 حرف باشد")]
        public string NewPassword { get; set; }

        [Compare("NewPassword", ErrorMessage = "کلمه عبور با تکرارش باید یکسان باشد")]
        public string ConfirmNewPassword { get; set; }

        [Required(ErrorMessage = "ایمیل باید وارد شود"), MaxLength(50, ErrorMessage = "ایمیل باید کمتر از 50 حرف باشد"),
         MinLength(4, ErrorMessage = "ایمیل باید بیشتر از 4 حرف باشد"),
         RegularExpression(
             @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$",
             ErrorMessage = "ایمیل وارد شده معتبر نمی باشد")]
        public string Email { get; set; }

        public string Major { get; set; }
        public string Description { get; set; }
        public DateTime? BirthDay { get; set; }
        public bool? AvatarStatus { get; set; }
    }
}