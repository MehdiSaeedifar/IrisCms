using System.Collections.Generic;
using System.Web.Mvc;
using Iris.Servicelayer.EFServices.Enums;

namespace Iris.Web.Helpers
{
    public class DropDownList
    {
        public static SelectList OrderList(Order order)
        {
            var selectListOrder = new List<SelectListItem>
            {
                new SelectListItem {Text = "صعودی", Value = "Asscending"},
                new SelectListItem {Text = "نزولی", Value = "Descending"}
            };

            return new SelectList(selectListOrder, "Value", "Text", order);
        }

        public static SelectList CountList(int count)
        {
            var selectListCount = new List<SelectListItem>
            {
                new SelectListItem {Text = "10", Value = "10"},
                new SelectListItem {Text = "30", Value = "30"},
                new SelectListItem {Text = "50", Value = "50"}
            };

            return new SelectList(selectListCount, "Value", "Text", count);
        }

        public static SelectList Status(string status = "visible")
        {
            var selectListStatus = new List<SelectListItem>
            {
                new SelectListItem {Text = "نمایان", Value = "visible"},
                new SelectListItem {Text = "پنهان", Value = "hidden"},
                new SelectListItem {Text = "پیش نویس", Value = "draft"},
                new SelectListItem {Text = "آرشیو", Value = "آرشیو"}
            };

            return new SelectList(selectListStatus, "Value", "Text", status);
        }

        public static SelectList CommentStatus(bool? status = true)
        {
            var selectListStatus = new List<SelectListItem>
            {
                new SelectListItem {Text = "باز", Value = "true"},
                new SelectListItem {Text = "بسته", Value = "false"}
            };

            return new SelectList(selectListStatus, "Value", "Text", status);
        }
    }
}