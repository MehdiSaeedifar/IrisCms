using System.ComponentModel.DataAnnotations;

namespace Iris.Model
{
    public class AddAnonymousCommentModel
    {
        public int? ReplyId { get; set; }
        public int Id { get; set; }

        [Required(ErrorMessage = "لطفا نام خود را وارد نمایید"),
         MinLength(3, ErrorMessage = "نام وارد شده باید بیشتر از سه حرف باشد"),
         MaxLength(15, ErrorMessage = "نام وارد شده باید کمتر از 15 حرف باشد")]
        public string Name { get; set; }

        [Required(ErrorMessage = "ایمیل باید وارد شود"), MaxLength(50, ErrorMessage = "ایمیل باید کمتر از 50 حرف باشد"),
         MinLength(4, ErrorMessage = "ایمیل باید بیشتر از 4 حرف باشد"),
         RegularExpression(
             @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$",
             ErrorMessage = "ایمیل وارد شده معتبر نمی باشد")]
        public string Email { get; set; }

        [Required(ErrorMessage = "متن پیام نمی تواند خالی باشد"),
         MinLength(3, ErrorMessage = "متن پیام باید بیشتر از سه حرف باشد"),
         MaxLength(4000, ErrorMessage = "متن پیام باید کمتر از 4000 حرف باشد")]
        public string Body { get; set; }
    }
}