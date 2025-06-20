using AutoMapper;
using MamaFit.BusinessObjects.DTO.BranchDto;
using MamaFit.BusinessObjects.Entity;
using MamaFit.Repositories.Implement;
using MamaFit.Repositories.Infrastructure;
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
            var branchManager = await _unitOfWork.UserRepository.GetByIdAsync(requestDto.BranchManagerId!);
            if (branchManager == null)
                throw new ErrorException(StatusCodes.Status404NotFound, ApiCodes.NOT_FOUND, "Branch Manager not found!");


            if(branchManager.Role!.RoleName != "BranchManager")
                throw new ErrorException(StatusCodes.Status400BadRequest, ApiCodes.BAD_REQUEST, "User is not a branch manager!");

            var newBranch = _mapper.Map<Branch>(requestDto);
            newBranch.BranchManager = branchManager;
            await _unitOfWork.BranchRepository.InsertAsync(newBranch);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var branch = await _unitOfWork.BranchRepository.GetByIdAsync(id);
            if (branch == null || branch.IsDeleted)
                throw new ErrorException(StatusCodes.Status404NotFound, ApiCodes.NOT_FOUND, "Branch not found!");

            if( branch.Appointments.Any() || branch.BranchMaternityDressDetail.Any())
                throw new ErrorException(StatusCodes.Status400BadRequest, ApiCodes.BAD_REQUEST, "Cannot delete this branch due to policy restrict");

            await _unitOfWork.BranchRepository.SoftDeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<PaginatedList<BranchResponseDto>> GetAllAsync(int index, int pageSize, string? search, string? sortBy)
        {
            var branchList = await _unitOfWork.BranchRepository.GetAllAsync(index, pageSize, search, sortBy);
            if (branchList == null)
                throw new ErrorException(StatusCodes.Status404NotFound, ApiCodes.NOT_FOUND, "Branch not found!");

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

        public async Task<BranchResponseDto> GetByIdAsync(string id)
        {
            var branch = await _unitOfWork.BranchRepository.GetByIdAsync(id);
            if (branch == null || branch.IsDeleted)
                throw new ErrorException(StatusCodes.Status404NotFound, ApiCodes.NOT_FOUND, "Branch not found!");

            return _mapper.Map<BranchResponseDto>(branch);
        }

        public async Task UpdateAsync(string id, BranchCreateDto requestDto)
        {
            var oldBranch = await _unitOfWork.BranchRepository.GetByIdAsync(id);
            if (oldBranch == null || oldBranch.IsDeleted)
                throw new ErrorException(StatusCodes.Status404NotFound, ApiCodes.NOT_FOUND, "Branch not found!");

            _mapper.Map(requestDto,oldBranch);
            await _unitOfWork.BranchRepository.UpdateAsync(oldBranch);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
