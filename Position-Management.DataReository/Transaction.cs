using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Position_Management.DataRepository
{
    public class Transaction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TransactionID { get; set; }

        public int TradeID { get; set; }

        public int Version { get; set; }

        [Required]
        [MaxLength(10)]
        public string SecurityCode { get; set; } = string.Empty;

        public int Quantity { get; set; }

        [Required]
        [MaxLength(10)]
        public string InsertUpdateCancel { get; set; } = string.Empty;

        [Required]
        [MaxLength(4)]
        public string BuySell { get; set; } = string.Empty;
    }
}