using System.Data.Entity;
using Iris.DomainClasses;
using Iris.DomainClasses.Entities;
using Iris.DomainClasses.EntityConfiguration;

namespace Iris.Datalayer.Context
{
    public class IrisDbContext : DbContext, IUnitOfWork
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<BookImage> BookImages { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Label> Labels { get; set; }
        public DbSet<Page> Pages { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<MessageAnsware> MessageAnswares { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<UserMetaData> UserMetaDatas { get; set; }
        public DbSet<AnonymousUser> AnnonymousUsers { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Option> Options { get; set; }
        public DbSet<ForgottenPassword> ForgottenPasswords { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new BookConfig());
            modelBuilder.Configurations.Add(new BookImageConfig());
            modelBuilder.Configurations.Add(new CommentConfig());
            modelBuilder.Configurations.Add(new LabelConfig());
            modelBuilder.Configurations.Add(new PageConfig());
            modelBuilder.Configurations.Add(new ArticleConfig());
            modelBuilder.Configurations.Add(new UserConfig());
            modelBuilder.Configurations.Add(new MessageConfig());
            modelBuilder.Configurations.Add(new MessageAnswareConfig());
            modelBuilder.Configurations.Add(new PostConfig());
            modelBuilder.Configurations.Add(new UserMetaDataConfig());
            modelBuilder.Configurations.Add(new AnonymousUserConfig());
            modelBuilder.Configurations.Add(new DownloadLinkConfig());
            modelBuilder.Configurations.Add(new CategoryConfig());
            modelBuilder.Configurations.Add(new OptionConfig());
            modelBuilder.Configurations.Add(new ForgottenPasswordConfig());

            base.OnModelCreating(modelBuilder);
        }

        #region IUnitOfWork Members

        public new IDbSet<TEntity> Set<TEntity>() where TEntity : class
        {
            return base.Set<TEntity>();
        }

        #endregion
    }
}