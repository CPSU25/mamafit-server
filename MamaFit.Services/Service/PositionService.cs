using AutoMapper;
using MamaFit.BusinessObjects.DTO.PositionDto;
using MamaFit.BusinessObjects.Enum;
using MamaFit.Repositories.Implement;
using MamaFit.Repositories.Infrastructure;
using MamaFit.Services.Interface;

namespace MamaFit.Services.Service;

public class PositionService : IPositionService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidationService _validationService;
    private readonly IMapper _mapper;
    
    public PositionService(IUnitOfWork unitOfWork, IValidationService validationService, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _validationService = validationService;
        _mapper = mapper;
    }
    
    public async Task<PaginatedList<PositionDto>> GetAllAsync(int index, int pageSize, string? search, EntitySortBy? sortBy)
    {
        var positionList = await _unitOfWork.PositionRepository.GetAllAsync(index, pageSize, search, sortBy);
        
        var responseList = positionList.Items.Select(item => _mapper.Map<PositionDto>(item)).ToList();
        
        var paginatedResponse = new PaginatedList<PositionDto>(
            responseList,
            positionList.TotalCount,
            positionList.PageNumber,
            positionList.PageSize
        );
        
        return paginatedResponse;
    }
    
    public async Task CreateAsync(PositionRequestDto requestDto)
    {
        await _validationService.ValidateAndThrowAsync(requestDto);

        var newPosition = _mapper.Map<BusinessObjects.Entity.Position>(requestDto);

        await _unitOfWork.PositionRepository.InsertAsync(newPosition);
        await _unitOfWork.SaveChangesAsync();
    }
    
    public async Task<PositionDto> GetByIdAsync(string id)
    {
        var position = await _unitOfWork.PositionRepository.GetByIdAsync(id);
        _validationService.CheckNotFound(position, "Position not found!");
        return _mapper.Map<PositionDto>(position);
    }
    
    public async Task UpdateAsync(string id, PositionRequestDto requestDto)
    {
        var oldPosition = await _unitOfWork.PositionRepository.GetByIdAsync(id);
        _validationService.CheckNotFound(oldPosition, "Position not found!");
        await _validationService.ValidateAndThrowAsync(requestDto);

        var updatedPosition = _mapper.Map(requestDto, oldPosition);
        updatedPosition.Id = id;

        _unitOfWork.PositionRepository.Update(updatedPosition);
        await _unitOfWork.SaveChangesAsync();
    }
    
    public async Task DeleteAsync(string id)
    {
        var oldPosition = await _unitOfWork.PositionRepository.GetByIdAsync(id);
        _validationService.CheckNotFound(oldPosition, "Position not found!");

        oldPosition.IsDeleted = true;
        _unitOfWork.PositionRepository.Update(oldPosition);
        await _unitOfWork.SaveChangesAsync();
    }
}