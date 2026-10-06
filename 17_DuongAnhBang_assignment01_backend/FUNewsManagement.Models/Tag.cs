using System.ComponentModel.DataAnnotations;

namespace FUNewsManagement.Models
{
    public class Tag
    {
        [Key]
        public int TagID { get; set; }

        [MaxLength(50)]
        public string TagName { get; set; }

        [MaxLength(400)]
        public string Note { get; set; }

        public ICollection<NewsTag> NewsTags { get; set; }
    }
}
