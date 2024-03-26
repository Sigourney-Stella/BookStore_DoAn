using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStoreTM.Models
{
    [Table("PaymentDetails")]
    public class PaymentDetails
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int PayDetailsId { get; set; }

        [Range(0, double.MaxValue)]
        public Decimal TotalMoney { get; set; }

        [Column(TypeName = "ntext")]
        public int? Notes { get; set; }

        //khoá ngoại
        public int PaymentId { get; set; }
        //
        public Payment Payment { get; set; }

    }
}
