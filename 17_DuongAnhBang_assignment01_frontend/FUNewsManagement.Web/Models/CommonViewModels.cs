using System.ComponentModel.DataAnnotations;

namespace FUNewsManagement.Web.Models
{
    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }

    public class UserSession
    {
        public string Token { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string Name { get; set; }
        public short? AccountID { get; set; }
    }

    public class Category
    {
        public short CategoryID { get; set; }
        [Required]
        [MaxLength(100)]
        public string CategoryName { get; set; }
        [Required]
        [MaxLength(250)]
        public string CategoryDesciption { get; set; }
        public short? ParentCategoryID { get; set; }
        public bool? IsActive { get; set; }
    }

    public class Tag
    {
        public int TagID { get; set; }
        public string TagName { get; set; }
        public string Note { get; set; }
    }

    public class NewsTag
    {
        public string NewsArticleID { get; set; }
        public int TagID { get; set; }
        public Tag Tag { get; set; }
    }

    public class SystemAccount
    {
        public short AccountID { get; set; }
        [Required]
        [MaxLength(100)]
        public string AccountName { get; set; }
        [Required]
        [EmailAddress]
        [MaxLength(70)]
        public string AccountEmail { get; set; }
        public int? AccountRole { get; set; }
        [MaxLength(70)]
        public string AccountPassword { get; set; }
    }

    public class NewsArticle
    {
        public string NewsArticleID { get; set; }
        [MaxLength(400)]
        public string NewsTitle { get; set; }
        [Required]
        [MaxLength(150)]
        public string Headline { get; set; }
        public DateTime? CreatedDate { get; set; }
        [MaxLength(4000)]
        public string NewsContent { get; set; }
        [MaxLength(400)]
        public string NewsSource { get; set; }
        public short? CategoryID { get; set; }
        public bool? NewsStatus { get; set; }
        public short? CreatedByID { get; set; }
        public short? UpdatedByID { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public Category Category { get; set; }
        public SystemAccount CreatedByAccount { get; set; }
        public ICollection<NewsTag> NewsTags { get; set; }
    }

    public class NewsArticleCreateViewModel
    {
        public string NewsArticleID { get; set; }
        [MaxLength(400)]
        public string NewsTitle { get; set; }
        [Required]
        [MaxLength(150)]
        public string Headline { get; set; }
        public DateTime? CreatedDate { get; set; }
        [MaxLength(4000)]
        public string NewsContent { get; set; }
        [MaxLength(400)]
        public string NewsSource { get; set; }
        public short? CategoryID { get; set; }
        public bool? NewsStatus { get; set; }
        public List<int> TagIds { get; set; } = new List<int>();
        public IEnumerable<Category> Categories { get; set; }
        public IEnumerable<Tag> Tags { get; set; }
    }

    public class NewsArticleUpdateViewModel
    {
        public string NewsArticleID { get; set; }
        [MaxLength(400)]
        public string NewsTitle { get; set; }
        [Required]
        [MaxLength(150)]
        public string Headline { get; set; }
        [MaxLength(4000)]
        public string NewsContent { get; set; }
        [MaxLength(400)]
        public string NewsSource { get; set; }
        public short? CategoryID { get; set; }
        public bool? NewsStatus { get; set; }
        public List<int> TagIds { get; set; } = new List<int>();
        public IEnumerable<Category> Categories { get; set; }
        public IEnumerable<Tag> Tags { get; set; }
    }

    public class NewsArticleDetailViewModel
    {
        public string NewsArticleID { get; set; }
        public string NewsTitle { get; set; }
        public string Headline { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string NewsContent { get; set; }
        public string NewsSource { get; set; }
        public short? CategoryID { get; set; }
        public string CategoryName { get; set; }
        public bool? NewsStatus { get; set; }
        public short? CreatedByID { get; set; }
        public string CreatedByName { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public List<Tag> Tags { get; set; }
    }

    public class ReportStatistic
    {
        public string CategoryName { get; set; }
        public int NewsCount { get; set; }
    }

    public class UpdateProfileViewModel
    {
        [Required]
        [MaxLength(100)]
        public string AccountName { get; set; }
        [Required]
        [EmailAddress]
        [MaxLength(70)]
        public string AccountEmail { get; set; }
        [MaxLength(70)]
        public string AccountPassword { get; set; }
    }
}
