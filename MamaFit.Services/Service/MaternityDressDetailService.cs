using AutoMapper;
using MamaFit.BusinessObjects.DTO.MaternityDressDetailDto;
using MamaFit.BusinessObjects.Entity;
using MamaFit.BusinessObjects.Enum;
using MamaFit.Repositories.Helper;
using MamaFit.Repositories.Implement;
using MamaFit.Repositories.Infrastructure;
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
            var dress = await _unitOfWork.MaternityDressRepository.GetByIdAsync(requestDto.MaternityDressId);

            if (dress == null)
                throw new ErrorException(StatusCodes.Status400BadRequest, ApiCodes.NOT_FOUND, "Maternity dress does not exist.");

            dress.GlobalStatus = GlobalStatus.ACTIVE;
            await _unitOfWork.MaternityDressRepository.UpdateAsync(dress);

            if (dress.SKU == null)
            {
                dress.SKU = await GenerateUniqueDressSkuAsync();
                await _unitOfWork.MaternityDressRepository.UpdateAsync(dress);
            }
                
            var entity = _mapper.Map<MaternityDressDetail>(requestDto);
            entity.MaternityDress = dress;
            entity.CreatedAt = DateTime.UtcNow;
            entity.CreatedBy = GetCurrentUserName();
            
            entity.SKU = await GenerateNextDetailSkuAsync(dress.SKU);

            await _unitOfWork.MaternityDressDetailRepository.InsertAsync(entity);
            await _unitOfWork.SaveChangesAsync();
        }


        public async Task DeleteAsync(string id)
        {
            var entity = await _unitOfWork.MaternityDressDetailRepository.GetDetailById(id);

            if (entity == null)
            {
                throw new ErrorException(StatusCodes.Status404NotFound, ApiCodes.NOT_FOUND, "Maternity dress detail not found.");
            }
            await _unitOfWork.MaternityDressDetailRepository.SoftDeleteAsync(id);

            if (entity.MaternityDress.Details.Where(x => x.IsDeleted).Count() >= entity.MaternityDress.Details.Count())
                entity.MaternityDress.GlobalStatus = GlobalStatus.INACTIVE;

            await _unitOfWork.MaternityDressRepository.UpdateAsync(entity.MaternityDress);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<PaginatedList<MaternityDressDetailResponseDto>> GetAllAsync(int index, int pageSize, string? search, EntitySortBy? sortBy)
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
                throw new ErrorException(StatusCodes.Status404NotFound, ApiCodes.NOT_FOUND, "Maternity dress detail not found.");
            }

            return _mapper.Map<MaternityDressDetailResponseDto>(entity);
        }

        public async Task UpdateAsync(string id, MaternityDressDetailRequestDto requestDto)
        {
            var entity = await _unitOfWork.MaternityDressDetailRepository.GetByIdAsync(id);

            if (entity.IsDeleted || entity == null)
                throw new ErrorException(StatusCodes.Status404NotFound, ApiCodes.NOT_FOUND, "Maternity dress detail not found.");

            if (!string.IsNullOrWhiteSpace(requestDto.MaternityDressId) && requestDto.MaternityDressId != entity.MaternityDressId)
            {
                var exists = await _unitOfWork.MaternityDressRepository.GetByIdAsync(id);

                if (exists == null)
                    throw new ErrorException(StatusCodes.Status404NotFound, ApiCodes.NOT_FOUND, "Maternity dress does not exist.");
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
        
        private async Task<string> GenerateNextDetailSkuAsync(string baseSku)
        {
            if (string.IsNullOrWhiteSpace(baseSku))
                throw new ArgumentException("baseSku is required.", nameof(baseSku));

            var prefix = baseSku.Trim() + "-";

            var lastSku = await _unitOfWork
                .MaternityDressDetailRepository
                .GetLastSkuByPrefixAsync(prefix);

            var next = 1;

            if (!string.IsNullOrEmpty(lastSku) && lastSku!.Length > prefix.Length)
            {
                var suffix = lastSku.Substring(prefix.Length);

                if (int.TryParse(suffix.AsSpan(), out var n))
                    next = n + 1;
            }

            return $"{baseSku}-{next:000}";
        }
        
        private async Task<string> GenerateUniqueDressSkuAsync()
        {
            const string prefix = "MD";
            var rnd = new Random();

            while (true)
            {
                var number = rnd.Next(0, 1000);
                var sku = $"{prefix}{number:000}";

                var exists = await _unitOfWork.MaternityDressRepository
                    .IsEntityExistsAsync(x => x.SKU == sku);

                if (!exists) return sku;
            }
        }
    }
}
