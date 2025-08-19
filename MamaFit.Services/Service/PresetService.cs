using AutoMapper;
using MamaFit.BusinessObjects.DTO.PresetDto;
using MamaFit.BusinessObjects.Entity;
using MamaFit.BusinessObjects.Enum;
using MamaFit.Repositories.Implement;
using MamaFit.Repositories.Infrastructure;
using MamaFit.Services.Interface;
using Microsoft.AspNetCore.Http;
using MamaFit.Services.Hubs;
using Microsoft.AspNetCore.SignalR;
using MamaFit.BusinessObjects.DTO.ChatMessageDto;
using System.Text.Json;
using MamaFit.Repositories.Helper;

namespace MamaFit.Services.Service
{
    public class PresetService : IPresetService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IValidationService _validationService;
        private readonly IChatService _chatService;
        private readonly IHubContext<ChatHub> _chatHubContext;
        private readonly IConfigService _configService;

        public PresetService(IHttpContextAccessor httpContextAccessor, IUnitOfWork unitOfWork, IMapper mapper, IValidationService validationService, IChatService chatService, IHubContext<ChatHub> chatHubContext, IConfigService configService)
        {
            _httpContextAccessor = httpContextAccessor;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _validationService = validationService;
            _chatService = chatService;
            _chatHubContext = chatHubContext;
            _configService = configService;
        }

        private string GetCurrentUserId()
        {
            return _httpContextAccessor.HttpContext?.User.FindFirst("userId")?.Value ?? string.Empty;
        }
        public async Task CreatePresetAsync(PresetCreateRequestDto request)
        {
            await _validationService.ValidateAndThrowAsync(request);
            var style = await _unitOfWork.StyleRepository.GetByIdNotDeletedAsync(request.StyleId);
            _validationService.CheckNotFound(style, $"Style with ID {request.StyleId} not found.");

            var user = await _unitOfWork.UserRepository.GetByIdNotDeletedAsync(GetCurrentUserId());
            _validationService.CheckNotFound(user, "User not found.");


            var preset = _mapper.Map<Preset>(request);
            preset.ComponentOptionPresets = new List<ComponentOptionPreset>();

            foreach (var optionId in request.ComponentOptionIds)
            {
                var option = await _unitOfWork.ComponentOptionRepository.GetByIdNotDeletedAsync(optionId);
                _validationService.CheckNotFound(option, $"Component option with ID {optionId} not found.");

                preset.ComponentOptionPresets.Add(new ComponentOptionPreset
                {
                    Preset = preset,
                    PresetsId = preset.Id,
                    ComponentOption = option,
                    ComponentOptionsId = optionId
                });
            }

            preset.UserId = GetCurrentUserId();
            preset.Style = style;
            preset.SKU = CodeHelper.GenerateCode('P');

            await _unitOfWork.PresetRepository.InsertAsync(preset);
            await _unitOfWork.SaveChangesAsync();
        }
        
        public async Task<string> CreatePresetForDesignRequestAsync(PresetCreateForDesignRequestDto request)
        {
            await _validationService.ValidateAndThrowAsync(request);

            var designerId = GetCurrentUserId();

            var designRequest = await _unitOfWork.DesignRequestRepository.GetDetailByIdAsync(request.DesignRequestId);
            _validationService.CheckNotFound(designRequest, $"Design request with ID {request.DesignRequestId} not found.");

            var config = await _configService.GetConfig();

            bool isOutOfTime = designRequest.Presets.Count() >= config.Fields.PresetVersions;
            if (isOutOfTime)
                throw new ErrorException(StatusCodes.Status400BadRequest, ApiCodes.BAD_REQUEST, $"Design with id: {request.DesignRequestId} is out of time create preset");

            var user = await _unitOfWork.UserRepository.GetByIdNotDeletedAsync(designerId);
            _validationService.CheckNotFound(user, "User not found.");

            var preset = _mapper.Map<Preset>(request);
            preset.UserId = designerId;
            preset.DesignRequestId = request.DesignRequestId;
            preset.DesignRequest = designRequest;

            await _unitOfWork.PresetRepository.InsertAsync(preset);
            await _unitOfWork.SaveChangesAsync();

            if (designRequest != null)
            {
                await SendPresetToCustomer(preset, designRequest, request.OrderId);
            }

            return preset.Id;
        }
        private async Task SendPresetToCustomer(Preset preset, DesignRequest designRequest, string orderId)
        {
            try
            {
                var designerId = GetCurrentUserId();
                var customerId = designRequest.UserId;


                if (!string.IsNullOrEmpty(customerId) && !string.IsNullOrEmpty(designerId))
                {
                    var chatRoom = await _chatService.CreateChatRoomAsync(designerId, customerId);


                    var presetData = new
                    {
                        presetId = preset.Id,
                        orderId = orderId,
                    };
                    var jsonMessage = JsonSerializer.Serialize(presetData, new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                        WriteIndented = false
                    });


                    var messageDto = new ChatMessageCreateDto
                    {
                        SenderId = designerId,
                        ChatRoomId = chatRoom.Id,
                        Message = jsonMessage,
                        Type = MessageType.Preset
                    };

                    var sentMessage = await _chatService.CreateChatMessageAsync(messageDto);

                    if (sentMessage != null)
                    {
                        await _chatHubContext.Clients.Group(chatRoom.Id).SendAsync("ReceiveMessage", new
                        {
                            id = sentMessage.Id,
                            message = sentMessage.Message,
                            senderId = sentMessage.SenderId,
                            senderName = sentMessage.SenderName,
                            senderAvatar = sentMessage.SenderAvatar,
                            chatRoomId = sentMessage.ChatRoomId,
                            type = sentMessage.Type,
                            messageTimestamp = sentMessage.MessageTimestamp.ToString("O")
                        });
                    }

                    await _chatHubContext.Clients.User(customerId)
                        .SendAsync("PresetReady", new
                        {
                            chatRoomId = chatRoom.Id,
                            presetId = preset.Id,
                            designRequestId = designRequest.Id,

                        });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending preset to customer: {ex.Message}");
            }
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
        public async Task<PresetGetByIdResponseDto> GetDefaultPresetByStyleId(string styleId)
        {
            var style = await _unitOfWork.StyleRepository.GetByIdNotDeletedAsync(styleId);
            _validationService.CheckNotFound(style, $"Style with ID {styleId} not found.");

            var preset = await _unitOfWork.PresetRepository.GetDefaultPresetByStyleId(styleId);
            _validationService.CheckNotFound(preset, $"Default preset for style with ID {styleId} not found.");
            return _mapper.Map<PresetGetByIdResponseDto>(preset);
        }
        public async Task<List<PresetGetByIdResponseDto>> GetAllPresetByComponentOptionId(List<string> componentOptionIds, string styleId)
        {

            foreach (var componentOptionId in componentOptionIds)
            {
                var componentOption = await _unitOfWork.ComponentOptionRepository.GetByIdNotDeletedAsync(componentOptionId);
                _validationService.CheckNotFound(componentOption, $"Component option with ID {componentOptionId} not found.");
            }

            var presets = await _unitOfWork.PresetRepository.GetAllPresetByComponentOptionId(componentOptionIds,styleId);
            _validationService.CheckNotFound(presets, $"No presets found for component option with ID {componentOptionIds}.");
            return presets.Select(preset => _mapper.Map<PresetGetByIdResponseDto>(preset)).ToList();
        }
        public async Task<List<PresetGetByIdResponseDto>> GetPresetByDesignRequestId(string designRequestId)
        {
            var presets = await _unitOfWork.PresetRepository.GetPresetByDesignRequestId(designRequestId);
            _validationService.CheckNotFound(presets, $"No presets found for design request with ID {designRequestId}.");

            var result = presets.Select(preset => _mapper.Map<PresetGetByIdResponseDto>(preset)).ToList();

            return result;
        }
        public async Task<PaginatedList<PresetRatedResponseDto>> GetMostSelledPreset(int index, int pageSize, DateTime? startDate, DateTime? endDate, OrderStatus? filterBy)
        {
            var presets = await _unitOfWork.PresetRepository.GetMostSelledPreset(index, pageSize, startDate, endDate, filterBy);
            var responseItems = presets.Items.Select(x => _mapper.Map<PresetRatedResponseDto>(x)).ToList();

            return new PaginatedList<PresetRatedResponseDto>(responseItems, presets.TotalCount,presets.PageNumber,pageSize);
        }
    }
}
