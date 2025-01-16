using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;
using BankTransaction.WebApi.Helper;
using BankTransaction.WebApi.MiddleWare;
using BankTransaction.WebApi.Models.Commond;
using BankTransaction.WebApi.Models.Response;
using BankTransaction.WebApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace BankTransaction.WebApi.Controllers;

// [ApiController]
[Route("/api/v1/transaction")]
public class TransactionController: ControllerBase {
    private readonly ILogger<TransactionController> _logger;
    private readonly TransactionServices _services;
    private readonly MiddleWareServices _middleWareServices;

    public TransactionController(ILogger<TransactionController> logger, TransactionServices services, MiddleWareServices middleWareServices) {
        _logger = logger;
        _services = services;
        _middleWareServices = middleWareServices;
    }

    [HttpGet("trx/generate-exec-id")]
    public IActionResult GenerateExecutionId()
    {
        try {
            return Ok( new { executionId = _middleWareServices.GenerateExecId()});
        } catch {
            return StatusCode( 500 ,new { message = "error"});
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateTransaction([FromBody] TransactionModel t)
    {
        _logger.LogInformation("Add New Transaction running");

        if (!ModelState.IsValid)
        {
            var errorResponse = new ErrorBadRequestValidationResponse
            {
                Parameters = ModelState.Keys.ToList(),
                ErrMessage = ModelState.Values
                                        .SelectMany(v => v.Errors)
                                        .Select(e => e.ErrorMessage)
                                        .ToList()
            };

            var response = BadRequestModelerHelper.IsBadRequestModeler(errorResponse);
            
            return BadRequest(response);
        }

        var execId = Request.Headers["X-execution-id"];
        if (string.IsNullOrEmpty(execId)) {
            return BadRequest("X-execution-id is missing.");
        }

        var (canNext, statusCode, Message) = _middleWareServices.CheckExecIdIsExist(execId.ToString());
        if(!canNext) {
            var res = _middleWareServices.HandlErrResponseMiddleware(statusCode, Message);
            return StatusCode((int)res.StatusCode, res);
        }

        var result = await _services.AddNewTransaction(t);
        if(result.StatusCode == HttpStatusCode.OK) {
            _logger.LogInformation("Add New Transaction completed successfully");
            return Ok(result);
        } else {
            if(result.StatusCode == HttpStatusCode.BadRequest)
                _logger.LogWarning("Add New Transaction encountered an issue. Transaction might be incomplete. Warning: {WarningMessage}", result.Error);
            if(result.StatusCode == HttpStatusCode.InternalServerError)
                _logger.LogError("Add New Transaction failed. Error: {ErrorMessage}", result.Error);

            return StatusCode((int)result.StatusCode, result); 
        }
    }

    [HttpGet("{accountNumber}")]
    public async Task<IActionResult> HistoryTransaction(string accountNumber)
    {
        _logger.LogInformation("Start fetching transaction history for Account: {AccountNumber}", accountNumber);

        if (string.IsNullOrWhiteSpace(accountNumber))
        {
            _logger.LogWarning("Account number is missing in the request.");
            return BadRequest(new { Message = "AccountNumber is required." });
        }

        var execId = Request.Headers["X-execution-id"];
        if (string.IsNullOrEmpty(execId)) {
            return BadRequest("X-execution-id is missing.");
        }

        var (canNext, statusCode, Message) = _middleWareServices.CheckExecIdIsExist(execId.ToString());
        if(!canNext) {
            var res = _middleWareServices.HandlErrResponseMiddleware(statusCode, Message);
            return StatusCode((int)res.StatusCode, res);
        }

        try
        {
            var transactionHistory = await _services.GetHistoryTransaction(accountNumber);

            if (transactionHistory.StatusCode == HttpStatusCode.NotFound) {
                return StatusCode(404, transactionHistory);
            }
            if (transactionHistory.StatusCode == HttpStatusCode.InternalServerError) {
                return StatusCode(500, transactionHistory);
            }

            _logger.LogInformation("Successfully fetched transaction history for Account: {AccountNumber}", accountNumber);
            return Ok(transactionHistory);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to fetch transaction history for Account: {AccountNumber}.", accountNumber);
            return StatusCode(500, new { Message = "An error occurred while retrieving transaction history.", Error = ex.Message });
        }
    }

    
}