using System.ComponentModel.DataAnnotations;

namespace Iris.Model
{
    public class AddUserCommentModel
    {
        public int? ReplyId { get; set; }
        public int Id { get; set; }
        public bool RemindeMe { get; set; }

        [Required(ErrorMessage = "متن پیام نمی تواند خالی باشد"),
         MinLength(3, ErrorMessage = "متن پیام باید بیشتر از سه حرف باشد"),
         MaxLength(4000, ErrorMessage = "متن پیام باید کمتر از 4000 حرف باشد")]
        public string Body { get; set; }
    }
}