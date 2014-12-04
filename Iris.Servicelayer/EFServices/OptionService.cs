using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Iris.Datalayer.Context;
using Iris.DomainClasses.Entities;
using Iris.Model;
using Iris.Model.AdminModel;
using Iris.Servicelayer.Interfaces;
using Iris.Utilities.Caching;

namespace Iris.Servicelayer.EFServices
{
    public class OptionService : IOptionService
    {
        private readonly IDbSet<Option> _options;

        public OptionService(IUnitOfWork uow)
        {
            _options = uow.Set<Option>();
        }

        public void Update(UpdateOptionModel model)
        {
            List<Option> options = _options.ToList();
            options.Where(op => op.Name.Equals("SiteUrl")).FirstOrDefault().Value = model.SiteUrl;
            options.Where(op => op.Name.Equals("BlogName")).FirstOrDefault().Value = model.BlogName;
            options.Where(op => op.Name.Equals("BlogKeywords")).FirstOrDefault().Value = model.BlogKeywords;
            options.Where(op => op.Name.Equals("BlogDescription")).FirstOrDefault().Value = model.BlogDescription;
            options.Where(op => op.Name.Equals("UsersCanRegister")).FirstOrDefault().Value =
                model.UsersCanRegister.ToString();
            options.Where(op => op.Name.Equals("AdminEmail")).FirstOrDefault().Value = model.AdminEmail;
            options.Where(op => op.Name.Equals("CommentsNotify")).FirstOrDefault().Value =
                Convert.ToString(model.CommentsNotify);
            options.Where(op => op.Name.Equals("MailServerUrl")).FirstOrDefault().Value = model.MailServerUrl;
            options.Where(op => op.Name.Equals("MailServerLogin")).FirstOrDefault().Value = model.MailServerLogin;
            options.Where(op => op.Name.Equals("MailServerPass")).FirstOrDefault().Value = model.MailServerPass;
            options.Where(op => op.Name.Equals("MailServerPort")).FirstOrDefault().Value =
                model.MailServerPort.ToString();
            options.Where(op => op.Name.Equals("CommentModeratingStatus")).FirstOrDefault().Value =
                model.CommentModeratingStatus
                    .ToString();
            options.Where(op => op.Name.Equals("PostPerPage")).FirstOrDefault().Value = model.PostPerPage.ToString();
        }

        [CacheMethod(SecondsToCache = 20)]
        public SiteConfig GetAll()
        {
            List<Option> options = _options.ToList();
            var model = new SiteConfig
            {
                AdminEmail = options.Where(op => op.Name.Equals("AdminEmail")).FirstOrDefault().Value,
                BlogDescription = options.Where(op => op.Name.Equals("BlogDescription")).FirstOrDefault().Value,
                BlogKeywords = options.Where(op => op.Name.Equals("BlogKeywords")).FirstOrDefault().Value,
                BlogName = options.Where(op => op.Name.Equals("BlogName")).FirstOrDefault().Value,
                CommentsNotify =
                    Convert.ToBoolean(options.Where(op => op.Name.Equals("CommentsNotify")).FirstOrDefault().Value),
                CommentModeratingStatus =
                    Convert.ToBoolean(options.Where(op => op.Name.Equals("CommentModeratingStatus"))
                        .FirstOrDefault().Value),
                MailServerLogin = options.Where(op => op.Name.Equals("MailServerLogin")).FirstOrDefault().Value,
                MailServerPass = options.Where(op => op.Name.Equals("MailServerPass")).FirstOrDefault().Value,
                MailServerPort =
                    Convert.ToInt32(options.Where(op => op.Name.Equals("MailServerPort")).FirstOrDefault().Value),
                MailServerUrl = options.Where(op => op.Name.Equals("MailServerUrl")).FirstOrDefault().Value,
                PostPerPage =
                    Convert.ToInt32(options.Where(op => op.Name.Equals("PostPerPage")).FirstOrDefault().Value),
                SiteUrl = options.Where(op => op.Name.Equals("SiteUrl")).FirstOrDefault().Value,
                UsersCanRegister = Convert.ToBoolean(options.Where(op => op.Name.Equals("UsersCanRegister"))
                    .FirstOrDefault().Value)
            };
            return model;
        }


        public bool ModeratingComment
        {
            get
            {
                return Convert.ToBoolean(_options.Where(option => option.Name == "CommentModeratingStatus")
                    .FirstOrDefault().Value);
            }
        }
    }
}