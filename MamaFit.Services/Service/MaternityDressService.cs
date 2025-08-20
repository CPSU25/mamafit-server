using AutoMapper;
using MamaFit.BusinessObjects.DTO.MaternityDressDto;
using MamaFit.BusinessObjects.Entity;
using MamaFit.BusinessObjects.Enum;
using MamaFit.Repositories.Implement;
using MamaFit.Repositories.Infrastructure;
using MamaFit.Services.Interface;

namespace MamaFit.Services.Service
{
    public class MaternityDressService : IMaternityDressService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IValidationService _validation;

        public MaternityDressService(IUnitOfWork unitOfWork, IMapper mapper, IValidationService validation)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _validation = validation;
        }

        public async Task CreateAsync(MaternityDressRequestDto requestDto)
        {
            await _validation.ValidateAndThrowAsync(requestDto);

            var newMaternityDress = _mapper.Map<MaternityDress>(requestDto);

            newMaternityDress.GlobalStatus = GlobalStatus.INACTIVE;
            newMaternityDress.SKU = await GenerateUniqueDressSkuAsync();

            await _unitOfWork.MaternityDressRepository.InsertAsync(newMaternityDress);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<List<AutocompleteDto>> GetAutocompletesAsync(string query)
        {
            var dressList = await _unitOfWork.MaternityDressRepository.Autocomplete(query);
            _validation.CheckNotFound(dressList, "Not found");

            return dressList.Select(x => new AutocompleteDto
            {
                Name = x.Name,
                Id = x.Id,
                Images = x.Images,
            }).ToList();
        }

        public async Task DeleteAsync(string id)
        {
            var oldMaternityDress = await _unitOfWork.MaternityDressRepository.GetById(id);// Tìm oldMaternity
            _validation.CheckNotFound(oldMaternityDress, "MaternityDress not found!");


            foreach (MaternityDressDetail detail in oldMaternityDress.Details)
            {
                await _unitOfWork.MaternityDressDetailRepository.SoftDeleteAsync(detail.Id);
                await _unitOfWork.SaveChangesAsync();
            }

            await _unitOfWork.MaternityDressRepository.SoftDeleteAsync(id); // Deleted + Save changes
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<PaginatedList<MaternityDressGetAllResponseDto>> GetAllAsync(int index, int pageSize, string? search, string? styleId, EntitySortBy? sortBy)
        {
            var maternityDressList = await _unitOfWork.MaternityDressRepository.GetAllAsync(index, pageSize, search, styleId, sortBy);

            // Map từng phần tử trong danh sách Items
            var responseList = maternityDressList.Items.Select(item => _mapper.Map<MaternityDressGetAllResponseDto>(item)).ToList();

            // Tạo PaginatedList mới với các đối tượng đã map
            var paginatedResponse = new PaginatedList<MaternityDressGetAllResponseDto>(
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
            _validation.CheckNotFound(oldMaternityDress, "MaternityDress not found!");

            return _mapper.Map<MaternityDressResponseDto>(oldMaternityDress);
        }

        public async Task UpdateAsync(string id, MaternityDressRequestDto requestDto)
        {
            await _validation.ValidateAndThrowAsync(requestDto);

            var oldMaternityDress = await _unitOfWork.MaternityDressRepository.GetByIdAsync(id);
            _validation.CheckNotFound(oldMaternityDress, "MaternityDress not found!");

            _mapper.Map(requestDto, oldMaternityDress); //Auto mapper Dto => dress

            await _unitOfWork.MaternityDressRepository.UpdateAsync(oldMaternityDress); //Update + Save changes
            await _unitOfWork.SaveChangesAsync();
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
