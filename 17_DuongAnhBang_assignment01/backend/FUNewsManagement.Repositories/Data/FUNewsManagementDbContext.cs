using Microsoft.EntityFrameworkCore;
using FUNewsManagement.Models;

namespace FUNewsManagement.Repositories.Data
{
    public class FUNewsManagementDbContext : DbContext
    {
        public FUNewsManagementDbContext(DbContextOptions<FUNewsManagementDbContext> options)
            : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<NewsArticle> NewsArticles { get; set; }
        public DbSet<NewsTag> NewsTags { get; set; }
        public DbSet<SystemAccount> SystemAccounts { get; set; }
        public DbSet<Tag> Tags { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<NewsTag>()
                .HasKey(nt => new { nt.NewsArticleID, nt.TagID });

            modelBuilder.Entity<NewsTag>()
                .HasOne(nt => nt.NewsArticle)
                .WithMany(na => na.NewsTags)
                .HasForeignKey(nt => nt.NewsArticleID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<NewsTag>()
                .HasOne(nt => nt.Tag)
                .WithMany(t => t.NewsTags)
                .HasForeignKey(nt => nt.TagID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<NewsArticle>()
                .HasOne(na => na.Category)
                .WithMany(c => c.NewsArticles)
                .HasForeignKey(na => na.CategoryID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<NewsArticle>()
                .HasOne(na => na.CreatedByAccount)
                .WithMany(sa => sa.NewsArticlesCreated)
                .HasForeignKey(na => na.CreatedByID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<NewsArticle>()
                .HasOne(na => na.UpdatedByAccount)
                .WithMany(sa => sa.NewsArticlesUpdated)
                .HasForeignKey(na => na.UpdatedByID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Category>()
                .HasOne(c => c.ParentCategory)
                .WithMany(c => c.InverseParentCategory)
                .HasForeignKey(c => c.ParentCategoryID)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
