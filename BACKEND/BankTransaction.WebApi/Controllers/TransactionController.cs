using Microsoft.AspNetCore.Mvc;

namespace BankTransaction.WebApi.Controllers;

[ApiController]
[Route("/api/v1/transaction")]
public class TransactionController: ControllerBase {
    private readonly ILogger<TransactionController> _logger;
    
}