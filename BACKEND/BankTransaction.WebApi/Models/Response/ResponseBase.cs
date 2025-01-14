// executionId
// status code
// success status
// data
// error message 
// start time
// end time

namespace BankTransaction.WebApi.Models.Response;
public class ResponseBase<T>
{
    public string ExecutionId { get; set; } = null!;
    public string Status { get; set; } = null!;
    public string? Error { get; set; }
    public DateTime StartTIme { get; set; }
    public DateTime EndTime { get; set; }
    public T? Data { get; set; }
}