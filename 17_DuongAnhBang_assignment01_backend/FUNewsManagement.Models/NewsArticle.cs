using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FUNewsManagement.Models
{
    public class NewsArticle
    {
        [Key]
        [MaxLength(20)]
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

        [ForeignKey("CategoryID")]
        public Category Category { get; set; }

        [ForeignKey("CreatedByID")]
        public SystemAccount CreatedByAccount { get; set; }

        [ForeignKey("UpdatedByID")]
        public SystemAccount UpdatedByAccount { get; set; }

        public ICollection<NewsTag> NewsTags { get; set; }
    }
}
