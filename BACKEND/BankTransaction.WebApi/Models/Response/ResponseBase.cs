using System.Net;

namespace BankTransaction.WebApi.Models.Response;
public class ResponseBase<T>
{
    public string ExecutionId { get; set; } = null!;
    public HttpStatusCode StatusCode { get; set; }
    public string Status { get; set; } = null!;
    public string? Error { get; set; }
    public DateTime StartTIme { get; set; }
    public DateTime EndTime { get; set; }
    public TimeSpan Duration => EndTime - StartTIme;
    public T? Data { get; set; }
}

public class SuccessMessage {
    public string? Message { get; set; }
}