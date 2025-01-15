namespace BankTransaction.WebApi.Models.Response;

public class ErrorBadRequestValidationResponse {
    public List<string>? Parameters { get; set; }
    public List<string>? ErrMessage { get; set; }
}