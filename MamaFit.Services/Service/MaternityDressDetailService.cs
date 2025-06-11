using AutoMapper;
using MamaFit.BusinessObjects.DTO.MaternityDressDetailDto;
using MamaFit.BusinessObjects.Entity;
using MamaFit.Repositories.Infrastructure;
using MamaFit.Repositories.Interface;
using MamaFit.Services.Interface;
using Microsoft.AspNetCore.Http;

namespace MamaFit.Services.Service
{
    public class MaternityDressDetailService : IMaternityDressDetailService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public MaternityDressDetailService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        public async Task CreateAsync(MaternityDressDetailRequestDto requestDto)
        {
            var exists = await _unitOfWork.MaternityDressRepository.GetByIdAsync(requestDto.MaternityDressId);

            if (exists == null)
                throw new ErrorException(StatusCodes.Status400BadRequest, ErrorCode.NotFound, "Maternity dress does not exist.");

            var entity = _mapper.Map<MaternityDressDetail>(requestDto);

            entity.MaternityDress = exists;
            entity.CreatedAt = DateTime.UtcNow;
            entity.CreatedBy = GetCurrentUserName();

            await _unitOfWork.MaternityDressDetailRepository.InsertAsync(entity);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var entity = await _unitOfWork.MaternityDressDetailRepository.GetByIdAsync(id);

            if (entity == null)
            {
                throw new ErrorException(StatusCodes.Status404NotFound, ErrorCode.NotFound, "Maternity dress detail not found.");
            }

            await _unitOfWork.MaternityDressDetailRepository.SoftDeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<PaginatedList<MaternityDressDetailResponseDto>> GetAllAsync(int index, int pageSize, string? search, string? sortBy)
        {

            var maternityDressDetailList = await _unitOfWork.MaternityDressDetailRepository.GetAllAsync(index, pageSize, search, sortBy);

            // Map từng phần tử trong danh sách Items
            var responseList = maternityDressDetailList.Items.Select(item => _mapper.Map<MaternityDressDetailResponseDto>(item)).ToList();

            // Tạo PaginatedList mới với các đối tượng đã map
            var paginatedResponse = new PaginatedList<MaternityDressDetailResponseDto>(
                responseList,
                maternityDressDetailList.TotalCount,
                maternityDressDetailList.PageNumber,
                maternityDressDetailList.PageSize
            );

            return paginatedResponse;
        }

        public async Task<MaternityDressDetailResponseDto> GetByIdAsync(string id)
        {
            var entity = await _unitOfWork.MaternityDressDetailRepository.GetByIdAsync(id);

            if (entity == null)
            {
                throw new ErrorException(StatusCodes.Status404NotFound, ErrorCode.NotFound, "Maternity dress detail not found.");
            }

            return _mapper.Map<MaternityDressDetailResponseDto>(entity);
        }

        public async Task UpdateAsync(string id, MaternityDressDetailRequestDto requestDto)
        {
            var entity = await _unitOfWork.MaternityDressDetailRepository.GetByIdAsync(id);

            if (entity == null)
                throw new ErrorException(StatusCodes.Status404NotFound, ErrorCode.NotFound, "Maternity dress detail not found.");

            if (!string.IsNullOrWhiteSpace(requestDto.MaternityDressId) && requestDto.MaternityDressId != entity.MaternityDressId)
            {
                var exists = await _unitOfWork.MaternityDressRepository.GetByIdAsync(id);

                if (exists == null)
                    throw new ErrorException(StatusCodes.Status400BadRequest, ErrorCode.NotFound, "Maternity dress does not exist.");
            }

            _mapper.Map(requestDto, entity);
            entity.UpdatedAt = DateTime.UtcNow;
            entity.UpdatedBy = GetCurrentUserName();

            await _unitOfWork.MaternityDressDetailRepository.UpdateAsync(entity);
            await _unitOfWork.SaveChangesAsync();
        }

        private string GetCurrentUserName()
        {
            return _httpContextAccessor.HttpContext?.User?.FindFirst("name")?.Value ?? "System";
        }
    }
}
