using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FUNewsManagement.Models
{
    public class NewsTag
    {
        [Key]
        [Column(Order = 0)]
        [MaxLength(20)]
        public string NewsArticleID { get; set; }

        [Key]
        [Column(Order = 1)]
        public int TagID { get; set; }

        [ForeignKey("NewsArticleID")]
        public NewsArticle NewsArticle { get; set; }

        [ForeignKey("TagID")]
        public Tag Tag { get; set; }
    }
}
