using AutoMapper;
using MamaFit.BusinessObjects.DTO.MaternityDressDetailDto;
using MamaFit.BusinessObjects.Entity;
using MamaFit.Repositories.Infrastructure;
using MamaFit.Repositories.Interface;
using MamaFit.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

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
            var maternityDressRepo = _unitOfWork.GetRepository<MaternityDress>();
            var maternityDressDetailRepo = _unitOfWork.GetRepository<MaternityDressDetail>();

            var exists = await maternityDressRepo.Entities
                .AnyAsync(x => x.Id == requestDto.MaternityDressId && !x.IsDeleted);

            if (!exists)
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ErrorCode.NotFound, "Maternity dress does not exist.");
            }

            var entity = _mapper.Map<MaternityDressDetail>(requestDto);
            entity.CreatedAt = DateTime.UtcNow;
            entity.CreatedBy = GetCurrentUserName();

            await maternityDressDetailRepo.InsertAsync(entity);
            await maternityDressDetailRepo.SaveAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var repo = _unitOfWork.GetRepository<MaternityDressDetail>();
            var entity = await repo.GetByIdAsync(id);

            if (entity == null)
            {
                throw new ErrorException(StatusCodes.Status404NotFound, ErrorCode.NotFound, "Maternity dress detail not found.");
            }

            await repo.SoftDeleteAsync(id);
            await repo.SaveAsync();
        }

        public async Task<PaginatedList<MaternityDressDetailResponseDto>> GetAllAsync(int index, int pageSize, string? search, string? sortBy)
        {
            var repo = _unitOfWork.GetRepository<MaternityDressDetail>();

            var query = repo.Entities.Where(x => !x.IsDeleted);

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(x => x.Name.Contains(search));
            }

            query = sortBy?.ToLower() switch
            {
                "name_asc" => query.OrderBy(x => x.Name),
                "name_desc" => query.OrderByDescending(x => x.Name),
                "createdat_asc" => query.OrderBy(x => x.CreatedAt),
                "createdat_desc" => query.OrderByDescending(x => x.CreatedAt),
                _ => query.OrderByDescending(x => x.CreatedAt)
            };

            var paged = await repo.GetPagging(query, index, pageSize);
            var list = paged.Items.Select(_mapper.Map<MaternityDressDetailResponseDto>).ToList();

            return new PaginatedList<MaternityDressDetailResponseDto>(list, paged.TotalCount, paged.PageNumber, pageSize);
        }

        public async Task<MaternityDressDetailResponseDto> GetByIdAsync(string id)
        {
            var repo = _unitOfWork.GetRepository<MaternityDressDetail>();
            var entity = await repo.GetByIdAsync(id);

            if (entity == null)
            {
                throw new ErrorException(StatusCodes.Status404NotFound, ErrorCode.NotFound, "Maternity dress detail not found.");
            }

            return _mapper.Map<MaternityDressDetailResponseDto>(entity);
        }

        public async Task UpdateAsync(string id, MaternityDressDetailRequestDto requestDto)
        {
            var repo = _unitOfWork.GetRepository<MaternityDressDetail>();
            var entity = await repo.GetByIdAsync(id);

            if (entity == null)
            {
                throw new ErrorException(StatusCodes.Status404NotFound, ErrorCode.NotFound, "Maternity dress detail not found.");
            }

            if (!string.IsNullOrWhiteSpace(requestDto.MaternityDressId) && requestDto.MaternityDressId != entity.MaternityDressId)
            {
                var maternityDressRepo = _unitOfWork.GetRepository<MaternityDress>();
                var exists = await maternityDressRepo.Entities
                    .AnyAsync(x => x.Id == requestDto.MaternityDressId && !x.IsDeleted);

                if (!exists)
                {
                    throw new ErrorException(StatusCodes.Status400BadRequest, ErrorCode.NotFound, "Maternity dress does not exist.");
                }
            }

            _mapper.Map(requestDto, entity);
            entity.UpdatedAt = DateTime.UtcNow;
            entity.UpdatedBy = GetCurrentUserName();

            await repo.UpdateAsync(entity);
            await repo.SaveAsync();
        }

        private string GetCurrentUserName()
        {
            return _httpContextAccessor.HttpContext?.User?.FindFirst("name")?.Value ?? "System";
        }
    }
}
