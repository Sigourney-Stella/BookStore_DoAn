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

        [Column(TypeName = "datetime")]
        public DateTime OrderDate { get; set; }

        //khoá ngoại
        public int AccountId { get; set; }
        //
        public Account Account { get; set; }
    }
}
