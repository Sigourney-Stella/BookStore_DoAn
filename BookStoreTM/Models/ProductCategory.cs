using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStoreTM.Models
{
    [Table("ProductCategory")]
    public class ProductCategory : CommonAbstract
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(500)]
        [Column(TypeName = "nvarchar(500)")]
        public string Name { get; set; }

        [Column(TypeName = "nvarchar(250)")]
        public int? Icon { get; set; }

        [Column(TypeName = "nvarchar(500)")]
        public string? SeoTitle { get; set; }

        [Column(TypeName = "nvarchar(500)")]
        public string? SeoKeywords { get; set; }

        [Column(TypeName = "nvarchar(500)")]
        public string? SeoDescription { get; set; }
    }
}
