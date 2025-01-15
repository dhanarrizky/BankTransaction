using System;
using BankTransaction.WebApi.Models.Enum;

namespace BankTransaction.WebApi.Helper
{
    public static class TransactionTypeHelper
    {
        public static TransactionType GetTransactionType(string transactionType)
        {
            if (Enum.TryParse(transactionType, ignoreCase: true, out TransactionType result))
            {
                return result;
            }

            throw new ArgumentException($"Invalid transaction type: {transactionType}");
        }
        public static string GetTransactionTypeString(TransactionType transactionType)
        {
            return transactionType.ToString();
        }
    }
}
