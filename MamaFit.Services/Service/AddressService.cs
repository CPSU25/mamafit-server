using System.IdentityModel.Tokens.Jwt;
using AutoMapper;
using MamaFit.BusinessObjects.DTO.AddressDto;
using MamaFit.BusinessObjects.Entity;
using MamaFit.Repositories.Helper;
using MamaFit.Repositories.Implement;
using MamaFit.Repositories.Infrastructure;
using MamaFit.Services.Interface;

namespace MamaFit.Services.Service;

public class AddressService : IAddressService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IValidationService _validation;
    
    public AddressService(IUnitOfWork unitOfWork, IMapper mapper, IValidationService validation)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _validation = validation;
    }
    
    public async Task<PaginatedList<AddressResponseDto>> GetAllAsync(int index, int pageSize)
    {
        var addresses = await _unitOfWork.AddressRepository.GetAllAsync(index, pageSize);
        var responseItems = addresses.Items
            .Select(address => _mapper.Map<AddressResponseDto>(address))
            .ToList();

        return new PaginatedList<AddressResponseDto>(
            responseItems,
            addresses.TotalCount,
            addresses.PageNumber,
            pageSize
        );
    }
    
    public async Task<List<AddressResponseDto>> GetByAccessTokenAsync(string accessToken)
    {
        var userId = JwtTokenHelper.ExtractUserId(accessToken);
        var addresses = await _unitOfWork.AddressRepository.GetByUserId(userId);
        _validation.CheckNotFound(addresses, "No addresses found for this user");
        return _mapper.Map<List<AddressResponseDto>>(addresses);
    }
    
    public async Task<AddressResponseDto> GetByIdAsync(string id)
    {
        var address = await _unitOfWork.AddressRepository.GetByIdAsync(id);
        _validation.CheckNotFound(address, "Address not found");
        return _mapper.Map<AddressResponseDto>(address);
    }
    
    public async Task<AddressResponseDto> CreateAsync(AddressRequestDto requestDto, string accessToken)
    {
        
        var exist = await _unitOfWork.AddressRepository.IsEntityExistsAsync(x => x.MapId == requestDto.MapId);
        _validation.CheckConflict(exist, "Address with this MapId already exists");
        var entity = _mapper.Map<Address>(requestDto);
        entity.UserId = JwtTokenHelper.ExtractUserId(accessToken);
        await _unitOfWork.AddressRepository.InsertAsync(entity);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<AddressResponseDto>(entity);
    }
    
    public async Task UpdateAsync(string id, AddressRequestDto requestDto)
    {
        var address = await _unitOfWork.AddressRepository.GetByIdAsync(id);
        _validation.CheckNotFound(address, "Address not found");
        
        var exist = await _unitOfWork.AddressRepository.IsEntityExistsAsync(x => x.MapId == requestDto.MapId && x.Id != id);
        _validation.CheckConflict(exist, "Address with this MapId already exists");
        
        _mapper.Map(requestDto, address);
        _unitOfWork.AddressRepository.Update(address);
        await _unitOfWork.SaveChangesAsync();
    }
    
    public async Task DeleteAsync(string id)
    {
        var address = await _unitOfWork.AddressRepository.GetByIdAsync(id);
        _validation.CheckNotFound(address, "Address not found");
        
        await _unitOfWork.AddressRepository.SoftDeleteAsync(id);
        await _unitOfWork.SaveChangesAsync();
    }
}