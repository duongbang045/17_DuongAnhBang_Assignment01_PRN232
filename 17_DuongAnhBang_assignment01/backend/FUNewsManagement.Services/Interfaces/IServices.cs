using FUNewsManagement.Models;
using FUNewsManagement.Models.DTOs;

namespace FUNewsManagement.Services.Interfaces
{
    public interface IAuthService
    {
        LoginResponse Authenticate(LoginRequest request, string adminEmail, string adminPassword,
            string jwtSecret, string jwtIssuer, string jwtAudience);
    }

    public interface IAccountService
    {
        IEnumerable<SystemAccount> GetAll();
        SystemAccount GetById(short id);
        IEnumerable<SystemAccount> Search(string keyword);
        void Insert(SystemAccount account);
        void Update(SystemAccount account);
        (bool success, string message) Delete(short id);
        SystemAccount GetByEmail(string email);
    }

    public interface ICategoryService
    {
        IEnumerable<Category> GetAll();
        Category GetById(short id);
        IEnumerable<Category> Search(string keyword);
        void Insert(Category category);
        void Update(Category category);
        (bool success, string message) Delete(short id);
    }

    public interface INewsArticleService
    {
        IEnumerable<NewsArticleDto> GetAll();
        IEnumerable<NewsArticleDto> GetActiveNews();
        NewsArticleDto GetById(string id);
        IEnumerable<NewsArticleDto> Search(string keyword);
        IEnumerable<NewsArticleDto> GetByCreatedBy(short accountId);
        IEnumerable<NewsArticleDto> GetByDateRange(DateTime startDate, DateTime endDate);
        IEnumerable<ReportStatisticDto> GetReportStatistics(DateTime startDate, DateTime endDate);
        void Insert(NewsArticleCreateDto dto);
        void Update(string id, NewsArticleUpdateDto dto);
        void Delete(string id);
    }

    public interface ITagService
    {
        IEnumerable<Tag> GetAll();
        Tag GetById(int id);
        IEnumerable<Tag> Search(string keyword);
        void Insert(Tag tag);
        void Update(Tag tag);
        void Delete(int id);
    }

    public interface IProfileService
    {
        SystemAccount GetProfile(short accountId);
        void UpdateProfile(short accountId, UpdateProfileDto dto);
    }
}
