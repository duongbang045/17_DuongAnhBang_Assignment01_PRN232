using FUNewsManagement.Models;

namespace FUNewsManagement.Repositories.Interfaces
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        IEnumerable<Category> SearchByName(string keyword);
        bool HasNewsArticles(short categoryId);
    }

    public interface INewsArticleRepository : IGenericRepository<NewsArticle>
    {
        IEnumerable<NewsArticle> GetActiveNews();
        IEnumerable<NewsArticle> SearchByTitleOrContent(string keyword);
        IEnumerable<NewsArticle> GetByCreatedBy(short accountId);
        IEnumerable<NewsArticle> GetByDateRange(DateTime startDate, DateTime endDate);
    }

    public interface ISystemAccountRepository : IGenericRepository<SystemAccount>
    {
        SystemAccount GetByEmail(string email);
        SystemAccount GetByEmailAndPassword(string email, string password);
        IEnumerable<SystemAccount> SearchByNameOrEmail(string keyword);
        bool HasCreatedNews(short accountId);
    }

    public interface ITagRepository : IGenericRepository<Tag>
    {
        IEnumerable<Tag> SearchByName(string keyword);
    }

    public interface INewsTagRepository : IGenericRepository<NewsTag>
    {
        IEnumerable<NewsTag> GetByNewsArticleId(string newsArticleId);
        void DeleteByNewsArticleId(string newsArticleId);
    }
}
