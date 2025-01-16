using BankTransaction.WebApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace BankTransaction.WebApi.Controllers;

[ApiController]
[Route("/api/v1/initialize-app")]
public class InitializeController: ControllerBase {
    private readonly ILogger<InitializeController> _logger;
    private readonly TransactionServices _services;

    public InitializeController(ILogger<InitializeController> logger, TransactionServices services) {
        _logger = logger;
        _services = services;
    }

    [HttpGet]
    public async Task<IActionResult> InitializeDatabase()
    {
        _logger.LogInformation("initialize database bank transaction");
        var result = await _services.InitalizeDatabasesAsync();
        if(result.Status == "Success") {
            _logger.LogInformation("Database bank transaction initialized successfully.");
            return Ok(result);
        } else {
            _logger.LogError($"Error : {result.Error}");
            return StatusCode(500, result); 
        }
    }
}