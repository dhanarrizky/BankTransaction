using System.ComponentModel.DataAnnotations;
using BankTransaction.WebApi.Models.Commond;
using BankTransaction.WebApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace BankTransaction.WebApi.Controllers;

[ApiController]
[Route("/api/v1/transaction")]
public class TransactionController: ControllerBase {
    private readonly ILogger<TransactionController> _logger;
    private readonly TransactionServices _services;

    public TransactionController(ILogger<TransactionController> logger, TransactionServices services) {
        _logger = logger;
        _services = services;
    }

        [HttpPost]
    public IActionResult CreateTransaction([FromBody] TransactionModel t)
    {
        _logger.LogInformation("Add New Transaction running");

         if (!ModelState.IsValid)
        {
            return BadRequest(new
            {
                Message = "Validation failed.",
                Errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
            });
        }

        return Ok(new { Message = "Transaction is valid and processed successfully." });
    }

    [HttpGet("{accountNumber}")]
    public IActionResult HistoryTransaction(string accountNumber)
    {
        _logger.LogInformation("Get transaction history");

        if (string.IsNullOrWhiteSpace(accountNumber))
        {
            return BadRequest(new { Message = "AccountNumber is required." });
        }
        return Ok();
    }
    
}