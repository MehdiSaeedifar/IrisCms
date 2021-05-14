using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Iris.Web.Helpers
{
    public static class AjaxDialog
    {
        public static IHtmlContent LinkDialog(this IHtmlHelper ajaxHelper, string linkText, string src, string method,
            string data, string targetId, DialogOptions dialogOptions, string successFunction = "", string cssClass = "",
            string id = "")
        {
            var tag = new TagBuilder("a");
            if (!string.IsNullOrEmpty(cssClass))
            {
                tag.AddCssClass(cssClass);
            }
            if (!string.IsNullOrEmpty(id))
            {
                tag.MergeAttribute("id", id);
            }
            if (!string.IsNullOrEmpty(successFunction))
            {
                tag.MergeAttribute("data-i-success-function", successFunction);
            }
            tag.InnerHtml.AppendHtml(linkText);
            tag.MergeAttribute("href", "javascript:;");
            tag.MergeAttribute("data-i-ajax", "true");
            tag.MergeAttribute("data-i-src", src);
            tag.MergeAttribute("data-i-ajax-method", method);
            tag.MergeAttribute("data-i-target-id", "#" + targetId);
            tag.MergeAttribute("data-i-show-dialog", "true");
            tag.MergeAttribute("data-i-dialog-width", dialogOptions.Width);
            tag.MergeAttribute("data-i-dialog-height", dialogOptions.Height);
            tag.MergeAttribute("data-i-dialog-show-effect", dialogOptions.ShowEffect);
            tag.MergeAttribute("data-i-dialog-show-dir", dialogOptions.ShowDir);
            tag.MergeAttribute("data-i-dialog-hide-effect", dialogOptions.HideEffect);
            tag.MergeAttribute("data-i-dialog-hide-dir", dialogOptions.HideDir);
            tag.MergeAttribute("data-i-ismodal", dialogOptions.IsModal);
            tag.MergeAttribute("data-i-dialog-title", dialogOptions.Title);
            return tag;
        }


        public static IHtmlContent ButtonDialog(this IHtmlHelper ajaxHelper, string linkText, string src, string method,
            string data, string targetId, DialogOptions dialogOptions, string cssClass = "", string id = "")
        {
            var tag = new TagBuilder("button");
            if (!string.IsNullOrEmpty(cssClass))
            {
                tag.AddCssClass(cssClass);
            }
            if (!string.IsNullOrEmpty(id))
            {
                tag.MergeAttribute("id", id);
            }
            tag.InnerHtml.AppendHtml(linkText);
            tag.MergeAttribute("href", "javascript:;");
            tag.MergeAttribute("data-i-ajax", "true");
            tag.MergeAttribute("data-i-src", src);
            tag.MergeAttribute("data-i-ajax-method", method);
            tag.MergeAttribute("data-i-target-id", "#" + targetId);
            tag.MergeAttribute("data-i-show-dialog", "true");
            tag.MergeAttribute("data-i-data", data);
            tag.MergeAttribute("data-i-dialog-width", dialogOptions.Width);
            tag.MergeAttribute("data-i-dialog-height", dialogOptions.Height);
            tag.MergeAttribute("data-i-dialog-show-effect", dialogOptions.ShowEffect);
            tag.MergeAttribute("data-i-dialog-show-dir", dialogOptions.ShowDir);
            tag.MergeAttribute("data-i-dialog-hide-effect", dialogOptions.HideEffect);
            tag.MergeAttribute("data-i-dialog-hide-dir", dialogOptions.HideDir);
            tag.MergeAttribute("data-i-ismodal", dialogOptions.IsModal);
            tag.MergeAttribute("data-i-dialog-title", dialogOptions.Title);
            return tag;
        }
    }
}