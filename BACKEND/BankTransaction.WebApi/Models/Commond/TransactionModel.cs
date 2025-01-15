using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BankTransaction.WebApi.Models.Enum;

namespace BankTransaction.WebApi.Models.Commond;

public class TransactionModel {
    
    public string? TransactionID {get; set;}

    [Required(ErrorMessage = "AccountNumber is required.")]
    [MaxLength(200)]
    public string AccountNumber {get; set;} = null!;

    [Required(ErrorMessage = "TransactionType is required.")]
    [EnumDataType(typeof(TransactionType))]
    public TransactionType TransactionType {get; set;}

    [Required(ErrorMessage = "Amount is required.")]
    [Range(0.01, (double)decimal.MaxValue, ErrorMessage = "Amount must be greater than 0")]
    [Column(TypeName = "decimal(19,4)")]
    public decimal Amount {get; set;}

    public DateTime TransactionDate {get; set;} = DateTime.Now;
}