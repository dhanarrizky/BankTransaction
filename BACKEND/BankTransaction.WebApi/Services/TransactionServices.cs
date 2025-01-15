using System.Diagnostics;
using BankTransaction.DataAccessAndBusiness.IRepositories;
using BankTransaction.WebApi.Models.Commond;
using BankTransaction.WebApi.Models.Response;
using BankTransaction.WebApi.Helper;
using System.Threading.Tasks;

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

    public async Task<(ResponseBase<SuccessMessage>, int)> AddNewTransaction(TransactionModel ts) {   
        if (ts == null) {
            throw new ArgumentNullException(nameof(ts), "Transaction Model can't be null.");
        }

        var stopwatch = Stopwatch.StartNew();

        int statusCode = 200;

        string? errMessage = null;

        DateTime startTime = DateTime.Now;
        var successMessage = new SuccessMessage();

        try {
            await _repo.AddNewTransaction(ts.AccountNumber, ts.TransactionType.ToString(), ts.Amount);
            successMessage.Message = "Add New Transaction Has been successfully";
        } catch (Exception e) {
            switch (e.Message) {
                case "Transaction failed: Insufficient balance.":
                    statusCode = 400;
                    errMessage = "Insufficient balance to make a withdrawal.";
                    break;
                case "Transaction failed: Account Not Found":
                    statusCode = 400;
                    errMessage = "The specified account does not exist.";
                    break;
                default:
                    statusCode = 500;
                    errMessage = e.Message;
                    break;
            }
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

        return (response, statusCode);
    }


    public async Task<ResponseBase<List<TransactionModel>>> GetHistoryTransaction(string an) {
        try {
            var transactions = await _repo.GetTransationHistoryByAccountNumber(an);
            var listObj = DataTableHelper.ConvertDataTableToList<TransactionModel>(transactions);

            return new ResponseBase<List<TransactionModel>>
            {
                Status = "Success",
                Data = listObj
            };
        } catch (Exception e) {
            return new ResponseBase<List<TransactionModel>>
            {
                Status = "Error",
                Data = null,
                Error = e.Message
            };
        }
    }
}