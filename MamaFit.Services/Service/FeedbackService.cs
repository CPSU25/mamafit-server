using AutoMapper;
using MamaFit.BusinessObjects.DTO.FeedbackDto;
using MamaFit.BusinessObjects.Entity;
using MamaFit.Repositories.Implement;
using MamaFit.Repositories.Infrastructure;
using MamaFit.Services.Interface;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace MamaFit.Services.Service;

public class FeedbackService : IFeedbackService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IValidationService _validation;
    private readonly IHttpContextAccessor _contextAccessor;

    public FeedbackService(IUnitOfWork unitOfWork, IMapper mapper, IValidationService validation, IHttpContextAccessor contextAccessor)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _validation = validation;
        _contextAccessor = contextAccessor;
    }

    public async Task<PaginatedList<FeedbackResponseDto>> GetAllAsync(int index, int pageSize, DateTime? startDate,
        DateTime? endDate)
    {
        var feedbacks = await _unitOfWork.FeedbackRepository.GetAllAsync(index, pageSize, startDate, endDate);
        
        var responseItems = feedbacks.Items
            .Select(feedback => _mapper.Map<FeedbackResponseDto>(feedbacks))
            .ToList();
        
        return new PaginatedList<FeedbackResponseDto>(
            responseItems,
            feedbacks.TotalCount,
            feedbacks.PageNumber,
            pageSize
        );
    }
    
    public async Task<FeedbackResponseDto> GetByIdAsync(string id)
    {
        var feedback = await _unitOfWork.FeedbackRepository.GetByIdNotDeletedAsync(id);
        _validation.CheckNotFound(feedback, "Feedback not found");
        return _mapper.Map<FeedbackResponseDto>(feedback);
    }
    
    public async Task CreateAsync(FeedbackRequestDto requestDto)
    {
        var entity = _mapper.Map<Feedback>(requestDto);
        await _unitOfWork.FeedbackRepository.InsertAsync(entity);
        await _unitOfWork.SaveChangesAsync();
    }
    
    public async Task UpdateAsync(string id, FeedbackRequestDto requestDto)
    {
        var feedback = await _unitOfWork.FeedbackRepository.GetByIdNotDeletedAsync(id);
        _validation.CheckNotFound(feedback, "Feedback not found");
        _mapper.Map(requestDto, feedback);
        
        await _unitOfWork.FeedbackRepository.UpdateAsync(feedback);
        await _unitOfWork.SaveChangesAsync();
    }
    
    public async Task DeleteAsync(string id)
    {
        var feedback = await _unitOfWork.FeedbackRepository.GetByIdNotDeletedAsync(id);
        _validation.CheckNotFound(feedback, "Feedback not found");
        
        await _unitOfWork.FeedbackRepository.SoftDeleteAsync(feedback);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<List<FeedbackResponseDto>> GetAllByUserId()
    {
        var currentUserId = GetCurrentUserId();

        var feedbacks = await _unitOfWork.FeedbackRepository.GetAllByUserId(currentUserId);
        return _mapper.Map<List<FeedbackResponseDto>>(feedbacks);
    }

    private string GetCurrentUserId()
    {
        return _contextAccessor.HttpContext.User.FindFirst("userId").Value;
    }
}