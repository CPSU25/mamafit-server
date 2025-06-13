using AutoMapper;
using MamaFit.BusinessObjects.DTO.MaternityDressDto;
using MamaFit.BusinessObjects.Entity;
using MamaFit.Repositories.Infrastructure;
using MamaFit.Repositories.Interface;
using MamaFit.Services.Interface;
using Microsoft.AspNetCore.Http;

namespace MamaFit.Services.Service
{
    public class MaternityDressService : IMaternityDressService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public MaternityDressService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task CreateAsync(MaternityDressRequestDto requestDto)
        {

            var newMaternityDress = new MaternityDress // Tạo mới Dress kèm với DressDetail
            {
                StyleId = requestDto.StyleId,
                Name = requestDto.Name,
                Description = requestDto.Description,
                Images = requestDto.Images,
                Slug = requestDto.Slug,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                CreatedBy = GetCurrentUserName()
            };

            await _unitOfWork.MaternityDressRepository.InsertAsync(newMaternityDress); // Insert + Save changes
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id)
        {

            var oldMaternityDress = await _unitOfWork.MaternityDressRepository.GetById(id);// Tìm oldMaternity

            if (oldMaternityDress == null)
                throw new ErrorException(StatusCodes.Status404NotFound,
                ErrorCode.NotFound, "MaternityDress not found!");

            foreach (MaternityDressDetail detail in oldMaternityDress.Details)
            {
                await _unitOfWork.MaternityDressDetailRepository.SoftDeleteAsync(detail.Id);
                await _unitOfWork.SaveChangesAsync();
            }

            await _unitOfWork.MaternityDressRepository.SoftDeleteAsync(id); // Deleted + Save changes
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<PaginatedList<GetAllResponseDto>> GetAllAsync(int index, int pageSize, string? search, string? sortBy)
        {
            var maternityDressList = await _unitOfWork.MaternityDressRepository.GetAllAsync(index, pageSize, search, sortBy);

            // Map từng phần tử trong danh sách Items
            var responseList = maternityDressList.Items.Select(item => _mapper.Map<GetAllResponseDto>(item)).ToList();

            // Tạo PaginatedList mới với các đối tượng đã map
            var paginatedResponse = new PaginatedList<GetAllResponseDto>(
                responseList,
                maternityDressList.TotalCount,
                maternityDressList.PageNumber,
                maternityDressList.PageSize
            );

            return paginatedResponse;
        }

        public async Task<MaternityDressResponseDto> GetByIdAsync(string id)
        {
            var oldMaternityDress = await _unitOfWork.MaternityDressRepository.GetById(id);

            if (oldMaternityDress == null)
                throw new ErrorException(StatusCodes.Status404NotFound,
                ErrorCode.NotFound, "MaternityDress not found!");

            return _mapper.Map<MaternityDressResponseDto>(oldMaternityDress);
        }

        public async Task UpdateAsync(string id, MaternityDressRequestDto requestDto)
        {

            var oldMaternityDress = await _unitOfWork.MaternityDressRepository.GetByIdAsync(id);

            if (oldMaternityDress.IsDeleted || oldMaternityDress == null)
                throw new ErrorException(StatusCodes.Status404NotFound,
                ErrorCode.NotFound, "MaternityDress not found!"); // Nếu không có

            _mapper.Map(requestDto, oldMaternityDress); //Auto mapper Dto => dress
            oldMaternityDress.UpdatedAt = DateTime.UtcNow;
            oldMaternityDress.UpdatedBy = GetCurrentUserName();

            await _unitOfWork.MaternityDressRepository.UpdateAsync(oldMaternityDress); //Update + Save changes
            await _unitOfWork.SaveChangesAsync();
        }

        private string GetCurrentUserName()
        {
            return _httpContextAccessor.HttpContext?.User?.FindFirst("name")?.Value ?? "System";
        }
    }
}
