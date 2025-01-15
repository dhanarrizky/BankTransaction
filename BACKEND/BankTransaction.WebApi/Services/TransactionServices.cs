using System.Diagnostics;
using BankTransaction.DataAccessAndBusiness.IRepositories;
using BankTransaction.WebApi.Models.Commond;
using BankTransaction.WebApi.Models.Response;

namespace BankTransaction.WebApi.Services;

public class TransactionServices {
    private readonly ITransactionRepository _repo;
    public TransactionServices(ITransactionRepository repository) {
        _repo = repository;
    }
    public async Task<ResponseBase<SuccessMessage>> InitalizeDatabasesAsync() {
        var stopwatch = Stopwatch.StartNew();

        string? errMessage = null;

        DateTime startTime = DateTime.Now;
        var successMessage = new SuccessMessage();
        try {
            var messages = new List<string>();
    
            try {
                messages.Add(await _repo.InitalizeDatabasesAsync());
            } catch (Exception e) {
                messages.Add($"Error initializing databases: {e.Message}");
                errMessage = e.Message;
            }
            
            try {
                messages.Add(await _repo.InitializedDummyDataDatabaseAsync());
            } catch (Exception e) {
                messages.Add($"Error initializing dummy data: {e.Message}");
                errMessage = e.Message;
            }
            
            try {
                messages.Add(await _repo.InitializedSPDataDatabaseAsync());
            } catch (Exception e) {
                messages.Add($"Error initializing SP data: {e.Message}");
                errMessage = e.Message;
            }
            
            successMessage.Message = string.Join(" | ", messages);
        } catch (Exception e) {
            errMessage = e.Message;
        }

        stopwatch.Stop();
        TimeSpan executionTime = stopwatch.Elapsed;

        var response = new ResponseBase<SuccessMessage>
        {
            ExecutionId = Guid.NewGuid().ToString(),
            Status =  errMessage == null ? "Success" : "Error",
            StartTIme = startTime,
            EndTime = startTime.Add(executionTime),
            Error = errMessage,
            Data = errMessage != null ? null : successMessage
        };

        return response;
    }

    // public void AddNewTransaction(TransactionModel ts) {   
    //     if (ts == null) {
    //         throw new ArgumentNullException(nameof())
    //     }
    // }

    public static async Task<ResponseBase<TransactionModel>> GetHistoryTransaction(string an) {
        return new ResponseBase<TransactionModel>();
    }
}