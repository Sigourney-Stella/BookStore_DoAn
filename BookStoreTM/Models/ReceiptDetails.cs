using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStoreTM.Models
{
    [Table("ReceiptDetails")]
    public class ReceiptDetails
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int ReceiptDetailsId { get; set; }

        [Column(TypeName = "int")]
        public int Quatity { get; set; }

        [Range(0, double.MaxValue)]
        public Decimal TotalMoney { get; set; }

        //khoá ngoại
        public int ProductId { get; set; }
        public int ReceiptId { get; set; }
        //
        public Product Product { get; set; }
        public Receipt Receipt { get; set; }
    }
}
