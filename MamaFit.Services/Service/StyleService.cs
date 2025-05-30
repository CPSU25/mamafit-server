using AutoMapper;
using MamaFit.BusinessObjects.DTO.StyleDto;
using MamaFit.Repositories.Implement;
using MamaFit.Repositories.Infrastructure;
using MamaFit.Services.Interface;
using Microsoft.AspNetCore.Http;

namespace MamaFit.Services.Service
{
    public class StyleService : IStyleService
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IMapper _mapper;

        public StyleService(UnitOfWork unitOfWork, IHttpContextAccessor contextAccessor, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _contextAccessor = contextAccessor;
            _mapper = mapper;
        }

        public Task CreateAsync(StyleRequestDto requestDto)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<PaginatedList<StyleResponseDto>> GetAllAsync(int index, int pageSize, string? search, string? sortBy)
        {
            throw new NotImplementedException();
        }

        public Task<StyleResponseDto> GetByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(string id, StyleRequestDto requestDto)
        {
            throw new NotImplementedException();
        }
    }
}
