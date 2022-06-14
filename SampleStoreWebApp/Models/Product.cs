using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SampleStoreWebApp.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        [DisplayName("Quantity")]
        [Range(1, 50, ErrorMessage = "The max quantity should be between 1 and 50")]
        public int Qty { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
