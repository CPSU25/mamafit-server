using MamaFit.BusinessObjects.DTO.FeedbackDto;
using MamaFit.Repositories.Infrastructure;
using MamaFit.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace MamaFit.API.Controllers;

[ApiController]
[Route("api/feedback")]
public class FeedbackController : ControllerBase
{
    private readonly IFeedbackService _feedbackService;
    public FeedbackController(IFeedbackService feedbackService)
    {
        _feedbackService = feedbackService;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] int index = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] DateTime? startDate = null,
        [FromQuery] DateTime? endDate = null)
    {
        var result = await _feedbackService.GetAllAsync(index, pageSize, startDate, endDate);
        return Ok(new ResponseModel<PaginatedList<FeedbackResponseDto>>(
            StatusCodes.Status200OK,
            ApiCodes.SUCCESS,
            result,
            "Get all feedbacks successfully!"
        ));
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] string id)
    {
        var result = await _feedbackService.GetByIdAsync(id);
        return Ok(new ResponseModel<FeedbackResponseDto>(
            StatusCodes.Status200OK,
            ApiCodes.SUCCESS,
            result,
            "Get feedback successfully!"
        ));
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateFeedback([FromBody] FeedbackRequestDto model)
    {
        await _feedbackService.CreateAsync(model);
        return Ok(new ResponseModel<string>(
            StatusCodes.Status200OK,
            ApiCodes.SUCCESS,
            "Feedback created successfully!"
        ));
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateFeedback([FromRoute] string id, [FromBody] FeedbackRequestDto model)
    {
        await _feedbackService.UpdateAsync(id, model);
        return Ok(new ResponseModel<string>(
            StatusCodes.Status200OK,
            ApiCodes.SUCCESS,
            "Feedback updated successfully!"
        ));
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteFeedback([FromRoute] string id)
    {
        await _feedbackService.DeleteAsync(id);
        return Ok(new ResponseModel<string>(
            StatusCodes.Status200OK,
            ApiCodes.SUCCESS,
            "Feedback deleted successfully!"
        ));
    }
}