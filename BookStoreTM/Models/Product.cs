using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStoreTM.Models
{
    [Table("Product")]
    public class Product : CommonAbstract
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int ProductId { get; set; }

        [Required]
        [StringLength(500)]
        [Column(TypeName = "nvarchar(500)")]
        public string ProductName { get; set; }
        public string Alias { get; set; }

        [Column(TypeName = "nvarchar(500)")]
        public string Description { get; set; }

        [Column(TypeName = "ntext")]
        public int? Notes { get; set; }

        [Column(TypeName = "nvarchar(500)")]
        public string Images { get; set; }

        [Range(0,double.MaxValue)]
        public Decimal Price { get; set; }

        [Range(0, double.MaxValue)]
        public Decimal? PriceSale { get; set; }

        [Column(TypeName = "int")]
        public int? Quatity { get; set; }
        public bool IsActicve { get; set; }

        [Column(TypeName = "int")]
        public bool IsHome { get; set; }

        [Column(TypeName = "int")]
        public bool IsSale { get; set; }

        [Column(TypeName = "int")]
        public bool IsFeature {  get; set; } // sp nổi bật

        [Column(TypeName = "int")]
        public bool IsHot { get; set; }

        [Column(TypeName = "nvarchar(500)")]
        public string? SeoTitle { get; set; }

        [Column(TypeName = "nvarchar(500)")]
        public string? SeoKeywords { get; set; }

        [Column(TypeName = "nvarchar(500)")]
        public string? SeoDescription { get; set; }

        // khoá ngoại
        public int ProductCategoryId { get; set; }
        public int PublisherId { get; set; }
        public int ProductImgId { get; set; }
        //
        public ProductCategory ProductCategory { get; set; }
        public Publisher Publisher { get; set; }
        public ProductImage ProductImage { get; set; }
    }
}
