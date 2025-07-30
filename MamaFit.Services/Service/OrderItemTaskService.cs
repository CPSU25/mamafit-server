using AutoMapper;
using MamaFit.Repositories.Implement;
using MamaFit.Services.Interface;

namespace MamaFit.Services.Service;

public class OrderItemTaskService : IOrderItemTaskService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IValidationService _validation;
    
    public OrderItemTaskService(IUnitOfWork unitOfWork, IMapper mapper, IValidationService validation)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _validation = validation;
    }
    
    
}