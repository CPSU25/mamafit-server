// using AutoMapper;
// using MamaFit.BusinessObjects.DTO.MeasurementDiaryDto;
// using MamaFit.BusinessObjects.Entity;
// using MamaFit.Repositories.Infrastructure;
// using MamaFit.Repositories.Interface;
// using MamaFit.Services.Interface;
// using Microsoft.AspNetCore.Http;
// using Microsoft.EntityFrameworkCore;
//
// namespace MamaFit.Services.Service;
//
// public class MeasurementDiaryService : IMeasurementDiaryService
// {
//     private readonly IUnitOfWork _unitOfWork;
//     private readonly IMapper _mapper;
//     private readonly IHttpContextAccessor _httpContextAccessor;
//
//     public MeasurementDiaryService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
//     {
//         _unitOfWork = unitOfWork;
//         _mapper = mapper;
//         _httpContextAccessor = httpContextAccessor;
//     }
//
//     private string GetCurrentUserName()
//     {
//         return _httpContextAccessor.HttpContext?.User?.FindFirst("name")?.Value ?? "System";
//     }
//
//     public async Task<PaginatedList<MeasurementDiaryResponseDto>> GetAllAsync(int index = 1, int pageSize = 10,
//         string? nameSearch = null)
//     {
//         var measurementDiaryRepo = _unitOfWork.GetRepository<MeasurementDiary>();
//         var query = measurementDiaryRepo.Entities.Where(md => !md.IsDeleted);
//         if (!string.IsNullOrEmpty(nameSearch))
//         {
//             query = query.Where(md => md.Name.Contains(nameSearch, StringComparison.OrdinalIgnoreCase));
//         }
//
//         var pagedResult = await measurementDiaryRepo.GetPagging(query, index, pageSize);
//         var responseDtos = _mapper.Map<List<MeasurementDiaryResponseDto>>(pagedResult.Items);
//
//         var result = new PaginatedList<MeasurementDiaryResponseDto>(
//             responseDtos,
//             pagedResult.TotalCount,
//             pagedResult.PageNumber,
//             pageSize
//         );
//         return result;
//     }
//
//     public async Task<MeasurementDiaryResponseDto> GetByIdAsync(string id)
//     {
//         var repo = _unitOfWork.GetRepository<MeasurementDiary>();
//         var measurementDiary = await repo.Entities.FirstOrDefaultAsync(md => md.Id == id && !md.IsDeleted);
//         if (measurementDiary == null)
//             throw new ErrorException(StatusCodes.Status404NotFound, ErrorCode.NotFound,
//                 $"MeasurementDiary with id {id} was not found");
//         return _mapper.Map<MeasurementDiaryResponseDto>(measurementDiary);
//     }
//
//     public async Task<MeasurementDiaryResponseDto> CreateAsync(MeasurementDiaryRequestDto requestDto)
//     {
//         var repo = _unitOfWork.GetRepository<MeasurementDiary>();
//         bool exists = await repo.Entities.AnyAsync(md => md.Name == requestDto.Name && !md.IsDeleted);
//         if (exists)
//             throw new ErrorException(StatusCodes.Status409Conflict, ErrorCode.Conflicted,
//                 "MeasurementDiary with this name already exists");
//         var measurementDiary = _mapper.Map<MeasurementDiary>(requestDto);
//         measurementDiary.CreatedAt = DateTime.UtcNow;
//         measurementDiary.CreatedBy = GetCurrentUserName();
//
//         await repo.InsertAsync(measurementDiary);
//         await _unitOfWork.SaveAsync();
//         return _mapper.Map<MeasurementDiaryResponseDto>(measurementDiary);
//     }
//
//     public async Task<MeasurementDiaryResponseDto> UpdateAsync(string id, UpdateMeasurementDiaryDto model)
//     {
//         var repo = _unitOfWork.GetRepository<MeasurementDiary>();
//         var measurementDiary = await repo.GetByIdAsync(id);
//         if (measurementDiary == null || measurementDiary.IsDeleted)
//             throw new ErrorException(StatusCodes.Status404NotFound, ErrorCode.NotFound,
//                 $"MeasurementDiary with id {id} was not found");
//
//         bool exists = await repo.Entities
//             .AnyAsync(md => md.Name == model.Name && md.Id != id && !md.IsDeleted);
//         if (exists)
//             throw new ErrorException(StatusCodes.Status409Conflict, ErrorCode.Conflicted,
//                 "MeasurementDiary with this name already exists");
//         _mapper.Map(model, measurementDiary);
//         measurementDiary.UpdatedAt = DateTime.UtcNow;
//         measurementDiary.UpdatedBy = GetCurrentUserName();
//
//         await _unitOfWork.SaveAsync();
//         return _mapper.Map<MeasurementDiaryResponseDto>(measurementDiary);
//     }
//
//     public async Task DeleteAsync(string id)
//     {
//         var repo = _unitOfWork.GetRepository<MeasurementDiary>();
//         var measurementDiary = await repo.GetByIdAsync(id);
//         if (measurementDiary == null || measurementDiary.IsDeleted)
//             throw new ErrorException(StatusCodes.Status404NotFound, ErrorCode.NotFound,
//                 $"MeasurementDiary with id {id} was not found");
//
//         measurementDiary.IsDeleted = true;
//         measurementDiary.UpdatedAt = DateTime.UtcNow;
//         measurementDiary.UpdatedBy = GetCurrentUserName();
//
//         await repo.UpdateAsync(measurementDiary);
//         await _unitOfWork.SaveAsync();
//     }
// }