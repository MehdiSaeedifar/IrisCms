using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Iris.Web.Helpers
{
    public static class TreeViewHelper
    {
        /// <summary>
        /// Create an HTML tree from a recursive collection of items
        /// </summary>
        public static TreeView<T> TreeView<T>(this IHtmlHelper html, IEnumerable<T> items)
        {
            return new TreeView<T>(html, items);
        }
    }

    /// <summary>
    /// Create an HTML tree from a resursive collection of items
    /// </summary>
    public class TreeView<T>
    {
        private readonly IHtmlHelper _html;
        private readonly IEnumerable<T> _items;
        private Func<T, string> _displayProperty = item => item.ToString();
        private Func<T, IEnumerable<T>> _childrenProperty;
        private string _emptyContent = "No children";
        private IDictionary<string, object> _htmlAttributes = new Dictionary<string, object>();
        private IDictionary<string, object> _childHtmlAttributes = new Dictionary<string, object>();
        private Func<T, HelperResult> _itemTemplate;

        public TreeView(IHtmlHelper html, IEnumerable<T> items)
        {
            _html = html ?? throw new ArgumentNullException(nameof(html));
            _items = items;
            // The ItemTemplate will default to rendering the DisplayProperty
            //_itemTemplate = item => new HelperResult(writer => writer.Write(_displayProperty(item)));
        }

        /// <summary>
        /// The property which will display the text rendered for each item
        /// </summary>
        public TreeView<T> ItemText(Func<T, string> selector)
        {
            _displayProperty = selector ?? throw new ArgumentNullException(nameof(selector));
            return this;
        }


        /// <summary>
        /// The template used to render each item in the tree view
        /// </summary>
        public TreeView<T> ItemTemplate(Func<T, HelperResult> itemTemplate)
        {
            _itemTemplate = itemTemplate ?? throw new ArgumentNullException(nameof(itemTemplate));
            return this;
        }


        /// <summary>
        /// The property which returns the children items
        /// </summary>
        public TreeView<T> Children(Func<T, IEnumerable<T>> selector)
        {
            _childrenProperty = selector ?? throw new ArgumentNullException(nameof(selector));
            return this;
        }

        /// <summary>
        /// Content displayed if the list is empty
        /// </summary>
        public TreeView<T> EmptyContent(string emptyContent)
        {
            _emptyContent = emptyContent ?? throw new ArgumentNullException(nameof(emptyContent));
            return this;
        }

        /// <summary>
        /// HTML attributes appended to the root ul node
        /// </summary>
        public TreeView<T> HtmlAttributes(object htmlAttributes)
        {
            HtmlAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
            return this;
        }

        /// <summary>
        /// HTML attributes appended to the root ul node
        /// </summary>
        public TreeView<T> HtmlAttributes(IDictionary<string, object> htmlAttributes)
        {
            _htmlAttributes = htmlAttributes ?? throw new ArgumentNullException(nameof(htmlAttributes));
            return this;
        }

        /// <summary>
        /// HTML attributes appended to the children items
        /// </summary>
        public TreeView<T> ChildrenHtmlAttributes(object htmlAttributes)
        {
            ChildrenHtmlAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
            return this;
        }

        /// <summary>
        /// HTML attributes appended to the children items
        /// </summary>
        public TreeView<T> ChildrenHtmlAttributes(IDictionary<string, object> htmlAttributes)
        {
            _childHtmlAttributes = htmlAttributes ?? throw new ArgumentNullException(nameof(htmlAttributes));
            return this;
        }

        //public void Render()
        //{
        //    var writer = _html.ViewContext.Writer;
        //    using (var textWriter = new HtmlTextWriter(writer))
        //    {
        //        textWriter.Write(ToString());
        //    }
        //}

        private void ValidateSettings()
        {
            if (_childrenProperty == null)
            {
                throw new InvalidOperationException("You must call the Children() method to tell the tree view how to find child items");
            }
        }

        public IHtmlContent Render()
        {
            ValidateSettings();

            var listItems = _items.ToList();

            var ul = new TagBuilder("ul")
            {
                TagRenderMode = TagRenderMode.Normal
            };

            ul.MergeAttributes(_htmlAttributes);

            if (listItems.Count == 0)
            {
                var li = new TagBuilder("li");
                li.InnerHtml.AppendHtml(_emptyContent);
                ul.InnerHtml.AppendHtml(li);
            }

            foreach (var item in listItems)
            {
                BuildNestedTag(ul, item, _childrenProperty);
            }

            return new HtmlContentBuilder().AppendHtml(ul);
        }

        private void AppendChildren(TagBuilder parentTag, T parentItem, Func<T, IEnumerable<T>> childrenProperty)
        {
            var children = childrenProperty(parentItem)?.ToList();
            if (children == null || !children.Any())
            {
                return;
            }

            var innerUl = new TagBuilder("ul");
            innerUl.MergeAttributes(_childHtmlAttributes);

            foreach (var item in children)
            {
                BuildNestedTag(innerUl, item, childrenProperty);
            }

            parentTag.InnerHtml.AppendHtml(innerUl);
        }

        private void BuildNestedTag(TagBuilder parentTag, T parentItem, Func<T, IEnumerable<T>> childrenProperty)
        {
            var li = GetLi(parentItem);
            parentTag.InnerHtml.AppendHtml(li.RenderStartTag());
            AppendChildren(li, parentItem, childrenProperty);
            parentTag.InnerHtml.AppendHtml(li.InnerHtml).AppendHtml(li.RenderEndTag());
        }

        private TagBuilder GetLi(T item)
        {
            var li = new TagBuilder("li");

            li.InnerHtml.AppendHtml(_itemTemplate(item).ToHtmlString());

            return li;
        }
    }

    public static class HtmlContentExtensions
    {
        public static string ToHtmlString(this IHtmlContent htmlContent)
        {
            if (htmlContent is HtmlString htmlString)
            {
                return htmlString.Value;
            }

            using var writer = new StringWriter();

            htmlContent.WriteTo(writer, HtmlEncoder.Default);
            return writer.ToString();
        }
    }
}