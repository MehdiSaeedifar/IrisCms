using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Iris.Model
{
    public class ContactUsModel
    {
        [Required(ErrorMessage = "موضوع باید وارد شود"),
         MaxLength(500, ErrorMessage = "موضوع باید کمتر از 500 حرف باشد"),
         MinLength(3, ErrorMessage = "موضوع باید بیشتر از 3 حرف باشد")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "متن باید وارد شود"), MinLength(4, ErrorMessage = "متن باید بیشتر از 3 حرف باشد")]
        [AllowHtml]
        public string Body { get; set; }
    }
}