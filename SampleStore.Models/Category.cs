using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SampleStore.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        [DisplayName("Display Order")]
        [Range(1, 50, ErrorMessage = "The display order should be between 1 and 50")]
        public int DisplayOrder { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
