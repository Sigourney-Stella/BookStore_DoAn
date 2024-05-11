using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStoreTM.Models
{
    [Table("Receipt")]
    public class Receipt : CommonAbstract
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int ReceiptId { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(100)")]
        public string Code { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime OrderDate { get; set; }

        public ICollection<ReceiptDetails> ReceiptDetails { get; set; }

    }
}
