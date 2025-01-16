using System.Diagnostics;
using System.Net;
using BankTransaction.WebApi.Models.Response;

namespace BankTransaction.WebApi.Helper;

public static class BadRequestModelerHelper {
    public static ResponseBase<ErrorBadRequestValidationResponse> IsBadRequestModeler(ErrorBadRequestValidationResponse err) {
        var stopwatch = Stopwatch.StartNew();
        DateTime startTime = DateTime.Now;

        stopwatch.Stop();
        TimeSpan executionTime = stopwatch.Elapsed;
        return new ResponseBase<ErrorBadRequestValidationResponse> {
            StatusCode = HttpStatusCode.BadRequest,
            Status = "Error",
            Error = "Invalid Validation",
            StartTIme = startTime,
            EndTime = startTime.Add(executionTime),
            Data = err
        };
    }
}