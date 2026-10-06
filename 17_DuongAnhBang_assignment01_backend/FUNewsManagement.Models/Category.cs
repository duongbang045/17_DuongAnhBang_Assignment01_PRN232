using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FUNewsManagement.Models
{
    public class Category
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public short CategoryID { get; set; }

        [Required]
        [MaxLength(100)]
        public string CategoryName { get; set; }

        [Required]
        [MaxLength(250)]
        public string CategoryDesciption { get; set; }

        public short? ParentCategoryID { get; set; }

        public bool? IsActive { get; set; }

        [ForeignKey("ParentCategoryID")]
        public Category ParentCategory { get; set; }

        public ICollection<Category> InverseParentCategory { get; set; }

        public ICollection<NewsArticle> NewsArticles { get; set; }
    }
}
