using FUNewsManagement.Models;
using FUNewsManagement.Models.DTOs;
using FUNewsManagement.Repositories.Interfaces;
using FUNewsManagement.Services.Interfaces;

namespace FUNewsManagement.Services.Implementations
{
    public class NewsArticleService : INewsArticleService
    {
        private readonly INewsArticleRepository _newsArticleRepository;
        private readonly INewsTagRepository _newsTagRepository;
        private readonly ITagRepository _tagRepository;

        public NewsArticleService(
            INewsArticleRepository newsArticleRepository,
            INewsTagRepository newsTagRepository,
            ITagRepository tagRepository)
        {
            _newsArticleRepository = newsArticleRepository;
            _newsTagRepository = newsTagRepository;
            _tagRepository = tagRepository;
        }

        private NewsArticleDto MapToDto(NewsArticle na)
        {
            if (na == null) return null;
            return new NewsArticleDto
            {
                NewsArticleID = na.NewsArticleID,
                NewsTitle = na.NewsTitle,
                Headline = na.Headline,
                CreatedDate = na.CreatedDate,
                NewsContent = na.NewsContent,
                NewsSource = na.NewsSource,
                CategoryID = na.CategoryID,
                CategoryName = na.Category?.CategoryName,
                NewsStatus = na.NewsStatus,
                CreatedByID = na.CreatedByID,
                CreatedByName = na.CreatedByAccount?.AccountName,
                UpdatedByID = na.UpdatedByID,
                ModifiedDate = na.ModifiedDate,
                Tags = na.NewsTags?.Select(nt => new TagDto
                {
                    TagID = nt.Tag.TagID,
                    TagName = nt.Tag.TagName,
                    Note = nt.Tag.Note
                }).ToList()
            };
        }

        public IEnumerable<NewsArticleDto> GetAll()
        {
            return _newsArticleRepository.GetAll().Select(MapToDto).ToList();
        }

        public IEnumerable<NewsArticleDto> GetActiveNews()
        {
            return _newsArticleRepository.GetActiveNews().Select(MapToDto).ToList();
        }

        public NewsArticleDto GetById(string id)
        {
            return MapToDto(_newsArticleRepository.GetById(id));
        }

        public IEnumerable<NewsArticleDto> Search(string keyword)
        {
            return _newsArticleRepository.SearchByTitleOrContent(keyword).Select(MapToDto).ToList();
        }

        public IEnumerable<NewsArticleDto> GetByCreatedBy(short accountId)
        {
            return _newsArticleRepository.GetByCreatedBy(accountId).Select(MapToDto).ToList();
        }

        public IEnumerable<NewsArticleDto> GetByDateRange(DateTime startDate, DateTime endDate)
        {
            return _newsArticleRepository.GetByDateRange(startDate, endDate).Select(MapToDto).ToList();
        }

        public IEnumerable<ReportStatisticDto> GetReportStatistics(DateTime startDate, DateTime endDate)
        {
            var list = _newsArticleRepository.GetByDateRange(startDate, endDate);
            return list
                .GroupBy(na => na.Category?.CategoryName ?? "Uncategorized")
                .Select(g => new ReportStatisticDto
                {
                    CategoryName = g.Key,
                    NewsCount = g.Count()
                })
                .OrderByDescending(r => r.NewsCount)
                .ToList();
        }

        public void Insert(NewsArticleCreateDto dto)
        {
            var article = new NewsArticle
            {
                NewsArticleID = dto.NewsArticleID,
                NewsTitle = dto.NewsTitle,
                Headline = dto.Headline,
                CreatedDate = dto.CreatedDate ?? DateTime.Now,
                NewsContent = dto.NewsContent,
                NewsSource = dto.NewsSource,
                CategoryID = dto.CategoryID,
                NewsStatus = dto.NewsStatus ?? true,
                CreatedByID = dto.CreatedByID,
                ModifiedDate = dto.CreatedDate ?? DateTime.Now
            };
            _newsArticleRepository.Insert(article);

            if (dto.TagIds != null && dto.TagIds.Count > 0)
            {
                foreach (var tagId in dto.TagIds)
                {
                    _newsTagRepository.Insert(new NewsTag
                    {
                        NewsArticleID = dto.NewsArticleID,
                        TagID = tagId
                    });
                }
            }
        }

        public void Update(string id, NewsArticleUpdateDto dto)
        {
            var article = _newsArticleRepository.GetById(id);
            if (article == null) return;

            if (!string.IsNullOrEmpty(dto.NewsTitle)) article.NewsTitle = dto.NewsTitle;
            if (!string.IsNullOrEmpty(dto.Headline)) article.Headline = dto.Headline;
            if (!string.IsNullOrEmpty(dto.NewsContent)) article.NewsContent = dto.NewsContent;
            if (!string.IsNullOrEmpty(dto.NewsSource)) article.NewsSource = dto.NewsSource;
            if (dto.CategoryID.HasValue) article.CategoryID = dto.CategoryID.Value;
            if (dto.NewsStatus.HasValue) article.NewsStatus = dto.NewsStatus.Value;
            if (dto.UpdatedByID.HasValue) article.UpdatedByID = dto.UpdatedByID.Value;
            article.ModifiedDate = dto.ModifiedDate ?? DateTime.Now;

            _newsArticleRepository.Update(article);

            _newsTagRepository.DeleteByNewsArticleId(id);
            if (dto.TagIds != null && dto.TagIds.Count > 0)
            {
                foreach (var tagId in dto.TagIds)
                {
                    _newsTagRepository.Insert(new NewsTag
                    {
                        NewsArticleID = id,
                        TagID = tagId
                    });
                }
            }
        }

        public void Delete(string id)
        {
            _newsTagRepository.DeleteByNewsArticleId(id);
            var article = _newsArticleRepository.GetById(id);
            if (article != null) _newsArticleRepository.Delete(article);
        }
    }
}
