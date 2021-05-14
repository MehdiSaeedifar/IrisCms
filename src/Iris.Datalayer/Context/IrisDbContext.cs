using Iris.DomainClasses.Entities;
using Iris.DomainClasses.EntityConfiguration;
using Microsoft.EntityFrameworkCore;

namespace Iris.Datalayer.Context
{
    public class IrisDbContext : DbContext, IUnitOfWork
    {
        public IrisDbContext(DbContextOptions<IrisDbContext> options) : base(options)
        {
        }

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
        public DbSet<AnonymousUser> AnonymousUsers { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Option> Options { get; set; }
        public DbSet<ForgottenPassword> ForgottenPasswords { get; set; }
        public DbSet<LabelPost> LabelPosts { get; set; }
        public DbSet<LikeUsersArticle> LikeUsersArticles { get; set; }
        public DbSet<LikeUsersComment> LikeUsersComments { get; set; }
        public DbSet<LikeUsersPage> LikeUsersPages { get; set; }
        public DbSet<LikeUsersPost> LikeUsersPosts { get; set; }
        public DbSet<DownloadLink> DownloadLinks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new BookConfig());
            modelBuilder.ApplyConfiguration(new BookImageConfig());
            modelBuilder.ApplyConfiguration(new CommentConfig());
            modelBuilder.ApplyConfiguration(new LabelConfig());
            modelBuilder.ApplyConfiguration(new PageConfig());
            modelBuilder.ApplyConfiguration(new ArticleConfig());
            modelBuilder.ApplyConfiguration(new UserConfig());
            modelBuilder.ApplyConfiguration(new MessageConfig());
            modelBuilder.ApplyConfiguration(new MessageAnswareConfig());
            modelBuilder.ApplyConfiguration(new PostConfig());
            modelBuilder.ApplyConfiguration(new UserMetaDataConfig());
            modelBuilder.ApplyConfiguration(new AnonymousUserConfig());
            modelBuilder.ApplyConfiguration(new DownloadLinkConfig());
            modelBuilder.ApplyConfiguration(new CategoryConfig());
            modelBuilder.ApplyConfiguration(new OptionConfig());
            modelBuilder.ApplyConfiguration(new ForgottenPasswordConfig());
            modelBuilder.ApplyConfiguration(new LabelPostConfig());
            modelBuilder.ApplyConfiguration(new LikeUsersArticleConfig());
            modelBuilder.ApplyConfiguration(new LikeUsersCommentConfig());
            modelBuilder.ApplyConfiguration(new LikeUsersPageConfig());
            modelBuilder.ApplyConfiguration(new LikeUsersPostConfig());
            modelBuilder.ApplyConfiguration(new LuceneBookModelConfig());
            modelBuilder.ApplyConfiguration(new MorePostsLikeThisConfig());
            modelBuilder.ApplyConfiguration(new AutoCompleteSearchBookModelConfig());
        }

        #region IUnitOfWork Members

        public new DbSet<TEntity> Set<TEntity>() where TEntity : class
        {
            return base.Set<TEntity>();
        }

        #endregion
    }
}