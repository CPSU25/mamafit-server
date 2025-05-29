using AutoMapper;
using MamaFit.BusinessObjects.DTO.MaternityDressDto;
using MamaFit.BusinessObjects.Entity;
using MamaFit.Repositories.Infrastructure;
using MamaFit.Repositories.Interface;
using MamaFit.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

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
            var maternityDressRepo = _unitOfWork.GetRepository<MaternityDress>(); //Repo của Dress

            var newMaternityDress = new MaternityDress // Tạo mới Dress kèm với DressDetail
            {
                StyleId = requestDto.StyleId,
                Name = requestDto.Name,
                Description = requestDto.Description,
                Images = requestDto.Images,
                Slug = requestDto.Slug,
                Details = requestDto.Details.Select(detailDto => new MaternityDressDetail
                {
                    Name = detailDto.Name,
                    Description = detailDto.Description,
                    Color = detailDto.Color,
                    Image = detailDto.Image,
                    Size = detailDto.Size,
                    Price = detailDto.Price,
                    Quantity = detailDto.Quantity,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    CreatedBy = GetCurrentUserName()

                }).ToList(),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                CreatedBy = GetCurrentUserName()
            };

            await maternityDressRepo.InsertAsync(newMaternityDress); // Insert + Save changes
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var maternityDressRepo = _unitOfWork.GetRepository<MaternityDress>(); //Repo của Dress

            var oldMaternityDress = await GetByIdAsync(id);// Tìm oldMaternity

            if (oldMaternityDress == null)
                throw new ErrorException(StatusCodes.Status404NotFound,
                ErrorCode.NotFound, "MaternityDress not found!");// Nếu không có

            await maternityDressRepo.DeleteAsync(id); // Deleted + Save changes
            await _unitOfWork.SaveAsync();
        }
        
        public async Task<PaginatedList<MaternityDressResponseDto>> GetAllAsync(int index, int pageSize, string? search, string? sortBy)
        {
            var maternityDressRepo = _unitOfWork.GetRepository<MaternityDress>(); //Repo của Dress

            var query = maternityDressRepo.Entities //Select
                .Include(md => md.Details)
                .Where(md => !md.IsDeleted);

            if (!string.IsNullOrWhiteSpace(search)) // Search
            {
                query = query.Where(u => u.Name.Contains(search));
            }

            query = sortBy?.ToLower() switch
            {
                "name_asc" => query.OrderBy(u => u.Name),
                "name_desc" => query.OrderByDescending(u => u.Name),
                "price_asc" => query.OrderBy(u => u.Details.Min(d => d.Price)),
                "price_desc" => query.OrderByDescending(u => u.Details.Max(d => d.Price)),
                "createdat_asc" => query.OrderBy(u => u.CreatedAt),
                "createdat_desc" => query.OrderByDescending(u => u.CreatedAt),
                _ => query.OrderByDescending(u => u.CreatedAt) // default
            };

            var pagedResult = await maternityDressRepo.GetPagging(query, index, pageSize); // Paging

            var listMaternityDress = pagedResult.Items
                .Select(_mapper.Map<MaternityDressResponseDto>) //Mapping
                .ToList();

            var responsePaginatedList = new PaginatedList<MaternityDressResponseDto>(
                listMaternityDress,
                pagedResult.TotalCount,
                pagedResult.PageNumber,
                pagedResult.TotalPages
            );

            return responsePaginatedList;

        }

        public async Task<MaternityDressResponseDto> GetByIdAsync(string id)
        {
            var maternityDressRepo = _unitOfWork.GetRepository<MaternityDress>(); //Repo của Dress

            var oldMaternityDress = await maternityDressRepo.Entities
                .Include(md => md.Details)
                .Where(md => !md.IsDeleted)
                .FirstOrDefaultAsync(md => md.Id.Equals(id)); // Tìm oldMaternity

            if (oldMaternityDress == null)
                throw new ErrorException(StatusCodes.Status404NotFound,
                ErrorCode.NotFound, "MaternityDress not found!"); // Nếu không có

            return _mapper.Map<MaternityDressResponseDto>(oldMaternityDress);
        }

        public async Task UpdateAsync(string id, MaternityDressRequestDto requestDto)
        {
            var maternityDressRepo = _unitOfWork.GetRepository<MaternityDress>(); //Repo của Dress 

            var oldMaternityDress = await maternityDressRepo.Entities
                .Include(md => md.Details)
                .Where(md => !md.IsDeleted)
                .FirstOrDefaultAsync(md => md.Id.Equals(id)); // Tìm oldMaternity

            if (oldMaternityDress == null)
                throw new ErrorException(StatusCodes.Status404NotFound,
                ErrorCode.NotFound, "MaternityDress not found!"); // Nếu không có

            _mapper.Map(requestDto, oldMaternityDress); //Auto mapper Dto => dress
            oldMaternityDress.Details.Clear();          //Put DressDetail
            oldMaternityDress.Details = requestDto.Details.Select(detailDto => _mapper.Map<MaternityDressDetail>(detailDto)).ToList();

            await maternityDressRepo.UpdateAsync(oldMaternityDress); //Update + Save changes
            await maternityDressRepo.SaveAsync();
        }

        private string GetCurrentUserName()
        {
            return _httpContextAccessor.HttpContext?.User?.FindFirst("name")?.Value ?? "System";
        }
    }
}
