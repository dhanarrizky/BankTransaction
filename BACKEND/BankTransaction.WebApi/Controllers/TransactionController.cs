using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using BankTransaction.WebApi.Helper;
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

    public TransactionController(ILogger<TransactionController> logger, TransactionServices services) {
        _logger = logger;
        _services = services;
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

        var (result, statusCode) = await _services.AddNewTransaction(t);
        if(result.Status == "Success") {
            return Ok(result);
        } else {
            return StatusCode(statusCode, result); 
        }
    }

    [HttpGet("{accountNumber}")]
    public async Task<IActionResult> HistoryTransaction(string accountNumber)
    {
        _logger.LogInformation("Get transaction history");

        if (string.IsNullOrWhiteSpace(accountNumber))
        {
            return BadRequest(new { Message = "AccountNumber is required." });
        }

        try {
            var res = await _services.GetHistoryTransaction(accountNumber);
            return Ok(res);
        } catch (Exception e) {
            return BadRequest(e);
        }
    }
    
}