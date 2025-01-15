using System.Data;

namespace BankTransaction.DataAccessAndBusiness.IRepositories;

public interface ITransactionRepository {
    public Task<string> InitalizeDatabasesAsync();
    public Task<string> InitializedDummyDataDatabaseAsync();
    public Task<string> InitializedSPDataDatabaseAsync();
    public Task<DataTable> GetAccountDetails(string accountNumber);
    public Task<DataTable> GetTransationHistoryByAccountNumber(string accountNumber);
    public Task<DataTable> AddNewTransaction(string accountNumber, string transactionType, decimal amount);
}