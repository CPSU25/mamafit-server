﻿using AutoMapper;
using MamaFit.BusinessObjects.DTO.PresetDto;
using MamaFit.BusinessObjects.Entity;
using MamaFit.BusinessObjects.Enum;
using MamaFit.Repositories.Implement;
using MamaFit.Repositories.Infrastructure;
using MamaFit.Services.Interface;
using Microsoft.AspNetCore.Http;

namespace MamaFit.Services.Service
{
    public class PresetService : IPresetService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IValidationService _validationService;

        public PresetService(IHttpContextAccessor httpContextAccessor, IUnitOfWork unitOfWork, IMapper mapper, IValidationService validationService)
        {
            _httpContextAccessor = httpContextAccessor;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _validationService = validationService;
        }

        private string GetCurrentUserId()
        {
            return _httpContextAccessor.HttpContext?.User.FindFirst("userId")?.Value ?? string.Empty;
        }

        public async Task CreatePresetAsync(PresetCreateRequestDto request)
        {
            await _validationService.ValidateAndThrowAsync(request);

            var optionList = new List<ComponentOption>();
            foreach (var optionId in request.ComponentOptionIds)
            {
                var option = await _unitOfWork.ComponentOptionRepository.GetByIdNotDeletedAsync(optionId);
                _validationService.CheckNotFound(option, $"Component option with ID {optionId} not found.");

                optionList.Add(option);
            }

            var preset = _mapper.Map<Preset>(request);
            preset.ComponentOptions = optionList;

            await _unitOfWork.PresetRepository.InsertAsync(preset);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeletePresetAsync(string id)
        {
            var preset = _unitOfWork.PresetRepository.GetByIdNotDeletedAsync(id);
            _validationService.CheckNotFound(preset, $"Preset with ID {id} not found.");

            await _unitOfWork.PresetRepository.SoftDeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<PaginatedList<PresetGetAllResponseDto>> GetAll(int index, int pageSize, string? search, EntitySortBy? sortBy)
        {
            var presets = await _unitOfWork.PresetRepository.GetAll(index, pageSize, search, sortBy);

            var responseItems = presets.Items
                .Select(preset => _mapper.Map<PresetGetAllResponseDto>(preset))
                .ToList();

            return new PaginatedList<PresetGetAllResponseDto>(responseItems, presets.TotalCount, presets.PageNumber, pageSize);
        }

        public async Task<PresetGetByIdResponseDto> GetById(string id)
        {
            var preset = await _unitOfWork.PresetRepository.GetDetailById(id);
            _validationService.CheckNotFound(preset, $"Preset with ID {id} not found.");
            return _mapper.Map<PresetGetByIdResponseDto>(preset);
        }

        public async Task UpdatePresetAsync(string id, PresetUpdateRequestDto request)
        {
            await _validationService.ValidateAndThrowAsync(request);

            var preset = await _unitOfWork.PresetRepository.GetDetailById(id);
            _validationService.CheckNotFound(preset, $"Preset with ID {id} not found.");

            _mapper.Map(request, preset);

            await _unitOfWork.PresetRepository.UpdateAsync(preset);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
