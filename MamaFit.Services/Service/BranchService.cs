using AutoMapper;
using MamaFit.BusinessObjects.DTO.BranchDto;
using MamaFit.BusinessObjects.Entity;
using MamaFit.Repositories.Infrastructure;
using MamaFit.Repositories.Interface;
using MamaFit.Services.Interface;
using Microsoft.AspNetCore.Http;

namespace MamaFit.Services.Service
{
    public class BranchService : IBranchService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public BranchService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task CreateAsync(BranchCreateDto requestDto)
        {
            var branchManager = await _unitOfWork.UserRepository.GetByIdAsync(requestDto.BranchManagerId);
            if (branchManager == null)
                throw new ErrorException(StatusCodes.Status404NotFound, ErrorCode.NotFound, "Branch Manager not found!");

            var newBranch = _mapper.Map<Branch>(requestDto);
            newBranch.BranchManager = branchManager;
            await _unitOfWork.BranchRepository.InsertAsync(newBranch);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var branch = await _unitOfWork.BranchRepository.GetByIdAsync(id);
            if (branch == null || branch.IsDeleted)
                throw new ErrorException(StatusCodes.Status404NotFound, ErrorCode.NotFound, "Branch not found!");
            await _unitOfWork.BranchRepository.SoftDeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<PaginatedList<BranchResponseDto>> GetAllAsync(int index, int pageSize, string? search, string? sortBy)
        {
            var branchList = await _unitOfWork.BranchRepository.GetAllAsync(index, pageSize, search, sortBy);
            if (branchList == null)
                throw new ErrorException(StatusCodes.Status404NotFound, ErrorCode.NotFound, "Branch not found!");

            var responseList = branchList.Items.Select(item => _mapper.Map<BranchResponseDto>(item)).ToList();

            // Tạo PaginatedList mới với các đối tượng đã map
            var paginatedResponse = new PaginatedList<BranchResponseDto>(
                responseList,
                branchList.TotalCount,
                branchList.PageNumber,
                branchList.PageSize
            );

            return paginatedResponse;
        }

        public Task<BranchResponseDto> GetByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(string id, BranchCreateDto requestDto)
        {
            throw new NotImplementedException();
        }
    }
}
