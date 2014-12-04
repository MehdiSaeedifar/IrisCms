using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Iris.Web.Filters
{
    public class AllowUploadSpecialFilesOnlyAttribute : ActionFilterAttribute
    {
        private readonly string _extensionsWhiteList;
        private readonly List<string> _toFilter = new List<string>();

        public AllowUploadSpecialFilesOnlyAttribute(string extensionsWhiteList)
        {
            if (string.IsNullOrWhiteSpace(extensionsWhiteList))
                throw new ArgumentNullException("extensionsWhiteList");

            _extensionsWhiteList = extensionsWhiteList;
            string[] extensions = extensionsWhiteList.Split(',');
            foreach (string ext in extensions.Where(ext => !string.IsNullOrWhiteSpace(ext)))
            {
                _toFilter.Add(ext.ToLowerInvariant().Trim());
            }
        }

        private bool CanUpload(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName)) return false;

            string ext = Path.GetExtension(fileName.ToLowerInvariant());
            return _toFilter.Contains(ext);
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpFileCollectionBase files = filterContext.HttpContext.Request.Files;
            foreach (string file in files)
            {
                HttpPostedFileBase postedFile = files[file];
                if (postedFile == null || postedFile.ContentLength == 0) continue;

                if (!CanUpload(postedFile.FileName))
                    throw new InvalidOperationException(
                        string.Format("You are not allowed to upload {0} file. Please upload only these files: {1}.",
                            Path.GetFileName(postedFile.FileName),
                            _extensionsWhiteList));
            }

            base.OnActionExecuting(filterContext);
        }
    }
}