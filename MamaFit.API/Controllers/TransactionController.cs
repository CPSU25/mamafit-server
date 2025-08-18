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
    
    [HttpGet("dashboard/summary")]
    public async Task<IActionResult> GetDashboardSummary(
        [FromQuery] DateTime startTime,
        [FromQuery] DateTime endTime)
    {
        var data = await _transactionService.GetDashboardSummaryAsync(startTime, endTime);
        return Ok(new ResponseModel<DashboardSummaryResponse>(
            StatusCodes.Status200OK,
            ApiCodes.SUCCESS,
            data,
            "Get dashboard summary successfully!"
        ));
    }

    // GET /analytics/revenue?groupBy=month&range=this_year&compare=yoy=true
    [HttpGet("analytics/revenue")]
    public async Task<IActionResult> GetRevenue(
        [FromQuery] string groupBy = "month",
        [FromQuery] string range = "this_year",
        // chấp nhận compare=yoy | compare=true | compare=yoy=true
        [FromQuery(Name = "compare")] string? compare = null)
    {
        bool compareYoy =
            string.Equals(compare, "yoy", StringComparison.OrdinalIgnoreCase) ||
            string.Equals(compare, "yoy=true", StringComparison.OrdinalIgnoreCase) ||
            string.Equals(compare, "true", StringComparison.OrdinalIgnoreCase);

        var data = await _transactionService.GetRevenueAsync(groupBy, range, compareYoy);
        var payload = new { data };
        return Ok(new ResponseModel<object>(
            StatusCodes.Status200OK,
            ApiCodes.SUCCESS,
            payload,
            "Get revenue analytics successfully!"
        ));
    }

    // GET /analytics/orders/status?range=month
    [HttpGet("analytics/orders/status")]
    public async Task<IActionResult> GetOrderStatus([FromQuery] string range = "month")
    {
        var data = await _transactionService.GetOrderStatusAsync(range);
        return Ok(new ResponseModel<OrderStatusResponse>(
            StatusCodes.Status200OK,
            ApiCodes.SUCCESS,
            data,
            "Get order status breakdown successfully!"
        ));
    }

    // GET /analytics/branches/top?metric=revenue&limit=5&range=month
    [HttpGet("analytics/branches/top")]
    public async Task<IActionResult> GetTopBranches(
        [FromQuery] string metric = "revenue",
        [FromQuery] int limit = 5,
        [FromQuery] string range = "month")
    {
        var data = await _transactionService.GetTopBranchesAsync(metric, limit, range);
        return Ok(new ResponseModel<BranchTopResponse>(
            StatusCodes.Status200OK,
            ApiCodes.SUCCESS,
            data,
            "Get top branches successfully!"
        ));
    }

    // GET /orders/recent?limit=10
    [HttpGet("orders/recent")]
    public async Task<IActionResult> GetRecentOrders([FromQuery] int limit = 10)
    {
        var data = await _transactionService.GetRecentOrdersAsync(limit);
        return Ok(new ResponseModel<RecentOrdersResponse>(
            StatusCodes.Status200OK,
            ApiCodes.SUCCESS,
            data,
            "Get recent orders successfully!"
        ));
    }
}