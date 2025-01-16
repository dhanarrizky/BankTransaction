using System.Diagnostics;
using System.Net;
using BankTransaction.WebApi.Models.Response;
using Microsoft.Extensions.Caching.Memory;

namespace BankTransaction.WebApi.MiddleWare;
public class MiddleWareServices {
    private readonly IMemoryCache _cache;

    public MiddleWareServices(IMemoryCache cache) {
        _cache = cache;
    }

    public string GenerateExecId() {
        string execId;
        do {
            Console.WriteLine("test");
            execId = Guid.NewGuid().ToString();
        } while (_cache.TryGetValue(execId, out _));

        _cache.Set(execId, true, TimeSpan.FromMinutes(5));

        return execId;
    }

    // if the key does not exist then it returns a not found message, error code 404, and a subsequent execution error condition
    // if the key exists and its contents are still false then this execId has executed, so it cannot execute again, so it will return false, 409, Conflict Detected : please regenerate the executionId
    // if all conditions are met then it will produce the message success, 200, true
    public (bool, HttpStatusCode, string) CheckExecIdIsExist(string execId) {
        if (_cache.TryGetValue(execId, out var value)) {
            Console.WriteLine($"Value found in cache: {value}");
            if(value is bool boolVal && !boolVal) {
                return (false, HttpStatusCode.Conflict,"Conflict Detected : please regenerate the executionId");
            }
            _cache.Set(execId, false, TimeSpan.FromMinutes(5));
            return (true, HttpStatusCode.OK,"Success");
        } else {
            return (false, HttpStatusCode.NotFound,"ExecutionId not found");
        }
    }

    public ResponseBase<string> HandlErrResponseMiddleware(HttpStatusCode statusCode, string Message) {
        var stopwatch = Stopwatch.StartNew();
        DateTime startTime = DateTime.Now;

        stopwatch.Stop();
        TimeSpan executionTime = stopwatch.Elapsed;
        return new ResponseBase<string> {
            StatusCode = statusCode,
            Status = "Error",
            Error = Message,
            StartTIme = startTime,
            EndTime = startTime.Add(executionTime),
            Data = null,
        };
    }
}
