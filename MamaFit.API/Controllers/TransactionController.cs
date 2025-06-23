using MamaFit.BusinessObjects.DTO.TransactionDto;
using MamaFit.Repositories.Infrastructure;
using MamaFit.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace MamaFit.API.Controllers;

[ApiController]
[Route("api/transaction")]
public class TransactionController : ControllerBase
{
    private readonly ITransactionService _transactionService;
    
    public TransactionController(ITransactionService transactionService)
    {
        _transactionService = transactionService;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetTransactions(
        [FromQuery] int index = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] DateTime? startDate = null,
        [FromQuery] DateTime? endDate = null)
    {
        var result = await _transactionService.GetTransactionsAsync(index, pageSize, startDate, endDate);
        return Ok(new ResponseModel<PaginatedList<TransactionResponseDto>>(
            StatusCodes.Status200OK,
            ApiCodes.SUCCESS,
            result,
            "Get transactions successfully!"
        ));
    }
    
    [HttpGet("{orderId}")]
    public async Task<IActionResult> GetTransactionByOrderId([FromRoute] string orderId)
    {
        var result = await _transactionService.GetTransactionByOrderIdAsync(orderId);
        return Ok(new ResponseModel<TransactionResponseDto>(
            StatusCodes.Status200OK,
            ApiCodes.SUCCESS,
            result,
            "Get transaction by order ID successfully!"
        ));
    }
}