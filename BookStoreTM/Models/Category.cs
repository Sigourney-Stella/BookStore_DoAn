using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStoreTM.Models
{
    [Table("Category")]
    public class Category : CommonAbstract
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int CategoryId { get; set; }

        [Required]
        [StringLength(500)]
        [Column(TypeName = "nvarchar(500)")]
        public string Title { get; set; }
        public string Alias { get; set; }

        [Column(TypeName = "int")]
        public int? Position { get; set; }

        [Column(TypeName = "nvarchar(500)")]
        public string Description { get; set; }

        [Column(TypeName = "nvarchar(500)")]
        public string SeoTitle { get; set; }

        [Column(TypeName = "nvarchar(500)")]
        public string SeoKeywords { get; set; }

        [Column(TypeName = "nvarchar(500)")]
        public string SeoDescription { get; set; }

        public ICollection<News> News { get; set; }
    }
}
