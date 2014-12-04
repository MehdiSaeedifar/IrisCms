using System.Data.Entity.Migrations;
using Iris.Datalayer.Context;
using Iris.DomainClasses;
using Iris.DomainClasses.Entities;
using Iris.Utilities.DateAndTime;
using Iris.Utilities.Security;

namespace Iris.Datalayer.Migrations
{
    public sealed class Configuration : DbMigrationsConfiguration<IrisDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(IrisDbContext context)
        {
            context.Roles.AddOrUpdate(x => new { x.Name, x.Description },
                new Role { Name = "admin", Description = "مدیرکل" },
                new Role { Name = "moderator", Description = "مدیر" },
                new Role { Name = "writer", Description = "نویسنده" },
                new Role { Name = "editor", Description = "ویرایشگر" },
                new Role { Name = "user", Description = "کاربر" });

            context.Options.AddOrUpdate(op => new { op.Name, op.Value }, new Option { Name = "SiteUrl", Value = "" },
                new Option { Name = "BlogName", Value = "" }, new Option { Name = "BlogKeywords", Value = "" },
                new Option { Name = "BlogDescription", Value = "" },
                new Option { Name = "UsersCanRegister", Value = "true" }, new Option { Name = "AdminEmail", Value = "" },
                new Option { Name = "CommentsNotify", Value = "true" },
                new Option { Name = "MailServerUrl", Value = "" }, new Option { Name = "MailServerLogin", Value = "" },
                new Option { Name = "MailServerPass", Value = "" },
                new Option { Name = "MailServerPort", Value = "25" },
                new Option { Name = "CommentModeratingStatus", Value = "true" },
                new Option { Name = "PostPerPage", Value = "10" });

            context.SaveChanges();

            context.Users.AddOrUpdate(u => u.UserName, new User
            {
                CreatedDate = DateAndTime.GetDateTime(),
                Email = "admin@gmail.com",
                IsBaned = false,
                Password = Encryption.EncryptingPassword("123456"),
                Role = context.Roles.Find(1),
                UserName = "admin",
                UserMetaData = new UserMetaData()
            });
        }
    }
}