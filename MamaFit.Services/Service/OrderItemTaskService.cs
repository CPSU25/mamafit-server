using AutoMapper;
using MamaFit.BusinessObjects.DTO.OrderItemTaskDto;
using MamaFit.Repositories.Helper;
using MamaFit.Repositories.Implement;
using MamaFit.Repositories.Interface;
using MamaFit.Services.Interface;

namespace MamaFit.Services.Service;

public class OrderItemTaskService : IOrderItemTaskService
{
    private readonly IOrderItemTaskRepository _repo;
    private readonly IMapper _mapper;
    private readonly IValidationService _validation;
    
    public OrderItemTaskService(IOrderItemTaskRepository repo, IMapper mapper, IValidationService validation)
    {
        _repo = repo;
        _mapper = mapper;
        _validation = validation;
    }
    
    public async Task<List<AssignStaffDto>> GetTasksByAssignedStaffAsync(string accessToken)
    {
        var userId = JwtTokenHelper.ExtractUserId(accessToken);
        var tasks = await _repo.GetTasksByAssignedStaffAsync(userId);
        return _mapper.Map<List<AssignStaffDto>>(tasks);
    }
}