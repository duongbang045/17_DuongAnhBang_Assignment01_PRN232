using System.ComponentModel.DataAnnotations;

namespace FUNewsManagement.Models
{
    public class SystemAccount
    {
        [Key]
        public short AccountID { get; set; }

        [MaxLength(100)]
        public string AccountName { get; set; }

        [MaxLength(70)]
        public string AccountEmail { get; set; }

        public int? AccountRole { get; set; }

        [MaxLength(70)]
        public string AccountPassword { get; set; }

        public ICollection<NewsArticle> NewsArticlesCreated { get; set; }

        public ICollection<NewsArticle> NewsArticlesUpdated { get; set; }
    }
}
