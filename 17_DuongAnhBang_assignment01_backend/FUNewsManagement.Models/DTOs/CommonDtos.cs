using System.ComponentModel.DataAnnotations;

namespace FUNewsManagement.Models.DTOs
{
    public class LoginRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }

    public class LoginResponse
    {
        public string Token { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string Name { get; set; }
        public short? AccountID { get; set; }
    }

    public class NewsArticleCreateDto
    {
        public string NewsArticleID { get; set; }
        public string NewsTitle { get; set; }
        public string Headline { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string NewsContent { get; set; }
        public string NewsSource { get; set; }
        public short? CategoryID { get; set; }
        public bool? NewsStatus { get; set; }
        public short? CreatedByID { get; set; }
        public List<int> TagIds { get; set; }
    }

    public class NewsArticleUpdateDto
    {
        public string NewsTitle { get; set; }
        public string Headline { get; set; }
        public string NewsContent { get; set; }
        public string NewsSource { get; set; }
        public short? CategoryID { get; set; }
        public bool? NewsStatus { get; set; }
        public short? UpdatedByID { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public List<int> TagIds { get; set; }
    }

    public class NewsArticleDto
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
        public short? UpdatedByID { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public List<TagDto> Tags { get; set; }
    }

    public class TagDto
    {
        public int TagID { get; set; }
        public string TagName { get; set; }
        public string Note { get; set; }
    }

    public class ReportStatisticDto
    {
        public string CategoryName { get; set; }
        public int NewsCount { get; set; }
    }

    public class UpdateProfileDto
    {
        public string AccountName { get; set; }
        public string AccountEmail { get; set; }
        public string AccountPassword { get; set; }
    }
}
