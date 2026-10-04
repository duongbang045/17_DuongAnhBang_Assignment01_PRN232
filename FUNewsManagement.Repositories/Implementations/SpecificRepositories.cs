using Microsoft.EntityFrameworkCore;
using FUNewsManagement.Models;
using FUNewsManagement.Repositories.Data;
using FUNewsManagement.Repositories.Interfaces;

namespace FUNewsManagement.Repositories.Implementations
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(FUNewsManagementDbContext context) : base(context) { }

        public IEnumerable<Category> SearchByName(string keyword)
        {
            return _dbSet
                .Where(c => string.IsNullOrEmpty(keyword) || c.CategoryName.Contains(keyword))
                .ToList();
        }

        public bool HasNewsArticles(short categoryId)
        {
            return _context.NewsArticles.Any(na => na.CategoryID == categoryId);
        }
    }

    public class NewsArticleRepository : GenericRepository<NewsArticle>, INewsArticleRepository
    {
        public NewsArticleRepository(FUNewsManagementDbContext context) : base(context) { }

        public IEnumerable<NewsArticle> GetActiveNews()
        {
            return _dbSet
                .Include(na => na.Category)
                .Include(na => na.CreatedByAccount)
                .Include(na => na.NewsTags)
                .ThenInclude(nt => nt.Tag)
                .Where(na => na.NewsStatus == true)
                .ToList();
        }

        public override IEnumerable<NewsArticle> GetAll()
        {
            return _dbSet
                .Include(na => na.Category)
                .Include(na => na.CreatedByAccount)
                .Include(na => na.UpdatedByAccount)
                .Include(na => na.NewsTags)
                .ThenInclude(nt => nt.Tag)
                .ToList();
        }

        public override NewsArticle GetById(object id)
        {
            var articleId = id as string;
            return _dbSet
                .Include(na => na.Category)
                .Include(na => na.CreatedByAccount)
                .Include(na => na.UpdatedByAccount)
                .Include(na => na.NewsTags)
                .ThenInclude(nt => nt.Tag)
                .FirstOrDefault(na => na.NewsArticleID == articleId);
        }

        public IEnumerable<NewsArticle> SearchByTitleOrContent(string keyword)
        {
            return _dbSet
                .Include(na => na.Category)
                .Include(na => na.CreatedByAccount)
                .Include(na => na.NewsTags)
                .ThenInclude(nt => nt.Tag)
                .Where(na => string.IsNullOrEmpty(keyword) ||
                             (na.NewsTitle != null && na.NewsTitle.Contains(keyword)) ||
                             (na.NewsContent != null && na.NewsContent.Contains(keyword)) ||
                             (na.Headline != null && na.Headline.Contains(keyword)))
                .ToList();
        }

        public IEnumerable<NewsArticle> GetByCreatedBy(short accountId)
        {
            return _dbSet
                .Include(na => na.Category)
                .Include(na => na.NewsTags)
                .ThenInclude(nt => nt.Tag)
                .Where(na => na.CreatedByID == accountId)
                .OrderByDescending(na => na.CreatedDate)
                .ToList();
        }

        public IEnumerable<NewsArticle> GetByDateRange(DateTime startDate, DateTime endDate)
        {
            return _dbSet
                .Include(na => na.Category)
                .Where(na => na.CreatedDate >= startDate && na.CreatedDate <= endDate)
                .OrderByDescending(na => na.CreatedDate)
                .ToList();
        }
    }

    public class SystemAccountRepository : GenericRepository<SystemAccount>, ISystemAccountRepository
    {
        public SystemAccountRepository(FUNewsManagementDbContext context) : base(context) { }

        public SystemAccount GetByEmail(string email)
        {
            return _dbSet.FirstOrDefault(sa => sa.AccountEmail == email);
        }

        public SystemAccount GetByEmailAndPassword(string email, string password)
        {
            return _dbSet.FirstOrDefault(sa => sa.AccountEmail == email && sa.AccountPassword == password);
        }

        public IEnumerable<SystemAccount> SearchByNameOrEmail(string keyword)
        {
            return _dbSet
                .Where(sa => string.IsNullOrEmpty(keyword) ||
                             (sa.AccountName != null && sa.AccountName.Contains(keyword)) ||
                             (sa.AccountEmail != null && sa.AccountEmail.Contains(keyword)))
                .ToList();
        }

        public bool HasCreatedNews(short accountId)
        {
            return _context.NewsArticles.Any(na => na.CreatedByID == accountId);
        }
    }

    public class TagRepository : GenericRepository<Tag>, ITagRepository
    {
        public TagRepository(FUNewsManagementDbContext context) : base(context) { }

        public IEnumerable<Tag> SearchByName(string keyword)
        {
            return _dbSet
                .Where(t => string.IsNullOrEmpty(keyword) ||
                            (t.TagName != null && t.TagName.Contains(keyword)))
                .ToList();
        }
    }

    public class NewsTagRepository : GenericRepository<NewsTag>, INewsTagRepository
    {
        public NewsTagRepository(FUNewsManagementDbContext context) : base(context) { }

        public IEnumerable<NewsTag> GetByNewsArticleId(string newsArticleId)
        {
            return _dbSet
                .Include(nt => nt.Tag)
                .Where(nt => nt.NewsArticleID == newsArticleId)
                .ToList();
        }

        public void DeleteByNewsArticleId(string newsArticleId)
        {
            var items = _dbSet.Where(nt => nt.NewsArticleID == newsArticleId).ToList();
            _dbSet.RemoveRange(items);
            _context.SaveChanges();
        }
    }
}
