using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODEL.Entities;

[Table("TBL_Transaction")]
public class Transaction
{
    [Key]
    public Guid TransactionID { get; set; }
    [Column("UserID")]
    public Guid UserID { get; set; }
    [Column("Amount")]
    public decimal Amount { get; set; }
    [Column("TransactionDate")]
    public DateTime TransactionDate { get; set; }
    [Column("TransactionType")]
    public string TransactionType { get; set; }

    public Transaction() { }
    public Transaction(Guid userId, decimal amount, string transactionType)
    {
        TransactionID = Guid.NewGuid();
        UserID = userId;
        Amount = amount;
        TransactionDate = DateTime.Now;
        TransactionType = transactionType;
    }
}
