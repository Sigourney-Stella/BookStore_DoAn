using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BookStoreTM.Models
{
    [Table("OrderBook")]
    public class OrderBook
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int OrderId { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime OrderDate { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string ReceiveName { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string ReceiveAddress { get; set; }

        [Column(TypeName = "varchar(64)")]
        public string ReceivePhone { get; set; }

        [Column(TypeName = "ntext")]
        public int? Notes { get; set; }
        //khoá ngoại
        public int AccountId { get; set; }
        //public int PaymentId { get; set; }
        //
        public Account Account { get; set; }
        //public Payment Payment { get; set; }
    }
}
