using AutoMapper;
using MamaFit.BusinessObjects.DTO.AddOnDto;
using MamaFit.BusinessObjects.DTO.AddOnOptionDto;
using MamaFit.BusinessObjects.DTO.AddressDto;
using MamaFit.BusinessObjects.DTO.AppointmentDto;
using MamaFit.BusinessObjects.DTO.BranchDto;
using MamaFit.BusinessObjects.DTO.BranchMaternityDressDetailDto;
using MamaFit.BusinessObjects.DTO.CartItemDto;
using MamaFit.BusinessObjects.DTO.CategoryDto;
using MamaFit.BusinessObjects.DTO.ChatMessageDto;
using MamaFit.BusinessObjects.DTO.ChatRoomDto;
using MamaFit.BusinessObjects.DTO.ChatRoomMemberDto;
using MamaFit.BusinessObjects.DTO.ComponentDto;
using MamaFit.BusinessObjects.DTO.ComponentOptionDto;
using MamaFit.BusinessObjects.DTO.DesignRequestDto;
using MamaFit.BusinessObjects.DTO.FeedbackDto;
using MamaFit.BusinessObjects.DTO.MaternityDressDetailDto;
using MamaFit.BusinessObjects.DTO.MaternityDressDto;
using MamaFit.BusinessObjects.DTO.MaternityDressTaskDto;
using MamaFit.BusinessObjects.DTO.MeasurementDto;
using MamaFit.BusinessObjects.DTO.MilestoneDto;
using MamaFit.BusinessObjects.DTO.NotificationDto;
using MamaFit.BusinessObjects.DTO.OrderDto;
using MamaFit.BusinessObjects.DTO.OrderItemDto;
using MamaFit.BusinessObjects.DTO.OrderItemTaskDto;
using MamaFit.BusinessObjects.DTO.PositionDto;
using MamaFit.BusinessObjects.DTO.PresetDto;
using MamaFit.BusinessObjects.DTO.RoleDto;
using MamaFit.BusinessObjects.DTO.SizeDto;
using MamaFit.BusinessObjects.DTO.StyleDto;
using MamaFit.BusinessObjects.DTO.TokenDto;
using MamaFit.BusinessObjects.DTO.TransactionDto;
using MamaFit.BusinessObjects.DTO.UserDto;
using MamaFit.BusinessObjects.DTO.VoucherBatchDto;
using MamaFit.BusinessObjects.DTO.VoucherDiscountDto;
using MamaFit.BusinessObjects.DTO.WarrantyHistoryDto;
using MamaFit.BusinessObjects.DTO.WarrantyRequestDto;
using MamaFit.BusinessObjects.DTO.WarrantyRequestItemDto;
using MamaFit.BusinessObjects.Entity;
using MamaFit.BusinessObjects.Entity.ChatEntity;
using MamaFit.BusinessObjects.Enum;
using Newtonsoft.Json;

namespace MamaFit.Services.Mapper
{
    public class MapperEntities : Profile
    {
        public MapperEntities()
        {
            #region Token Mapper

            CreateMap<ApplicationUserToken, TokenResponseDto>().ReverseMap();
            CreateMap<ApplicationUserToken, RefreshTokenRequestDto>().ReverseMap();

            #endregion

            #region User Mapper

            CreateMap<ApplicationUser, UserReponseDto>()
                .ForMember(dest => dest.RoleName,
                    opt => opt.MapFrom(src => src.Role != null ? src.Role.RoleName : null))
                .ReverseMap();
            CreateMap<ApplicationUser, PermissionResponseDto>()
                .ForMember(dest => dest.RoleName,
                    opt => opt.MapFrom(src => src.Role != null ? src.Role.RoleName : null));
            CreateMap<ApplicationUser, RegisterUserRequestDto>();
            CreateMap<RegisterUserRequestDto, ApplicationUser>()
                .ForMember(dest => dest.HashPassword, opt => opt.Ignore())
                .ForMember(dest => dest.Salt, opt => opt.Ignore());
            CreateMap<ApplicationUser, SendOTPRequestDto>().ReverseMap();

            #endregion

            #region Role Mapper

            CreateMap<ApplicationUserRole, RoleResponseDto>().ReverseMap();
            CreateMap<ApplicationUserRole, RoleRequestDto>().ReverseMap();

            #endregion

            #region MaternityDress Mapper

            CreateMap<MaternityDress, MaternityDressRequestDto>().ReverseMap();
            CreateMap<MaternityDress, MaternityDressResponseDto>()
                .ForMember(dest => dest.StyleName, otp => otp.MapFrom(x => x.Style!.Name))
                .ReverseMap();
            CreateMap<MaternityDress, MaternityDressGetAllResponseDto>()
                .ForMember(dest => dest.Price, otp => otp.MapFrom(x => x.Details!.Select(mdd => mdd.Price)))
                .ReverseMap();

            #endregion

            #region MaternityDressDetail Mapper

            CreateMap<MaternityDressDetail, MaternityDressDetailRequestDto>().ReverseMap();
            CreateMap<MaternityDressDetail, MaternityDressDetailResponseDto>().ReverseMap();

            #endregion

            #region Category Mapper

            CreateMap<Category, CategoryRequestDto>().ReverseMap();
            CreateMap<Category, CategoryResponseDto>().ReverseMap();
            CreateMap<Category, CategoryGetByIdResponse>().ReverseMap();

            #endregion

            #region Style Mapper

            CreateMap<Style, StyleRequestDto>().ReverseMap();
            CreateMap<Style, StyleResponseDto>().ReverseMap();
            CreateMap<Style, StyleGetByIdResponseDto>()
                .ForMember(dest => dest.Presets, otp => otp.MapFrom(src => src.Presets))
                .ReverseMap();

            #endregion

            #region Component Mapper

            CreateMap<Component, ComponentRequestDto>().ReverseMap();
            CreateMap<Component, ComponentResponseDto>().ReverseMap();
            CreateMap<Component, ComponentGetByIdResponseDto>().ReverseMap();

            #endregion

            #region ComponentOption Mapper

            CreateMap<ComponentOption, ComponentOptionRequestDto>().ReverseMap();
            CreateMap<ComponentOption, ComponentOptionResponseDto>()
                .ForMember(dest => dest.ComponentName, otp => otp.MapFrom(src => src.Component!.Name))
                .ForMember(dest => dest.ComponentId, otp => otp.MapFrom(src => src.Component!.Id))
                .ReverseMap();

            #endregion

            #region DesignRequest Mapper

            CreateMap<DesignRequest, DesignRequestCreateDto>().ReverseMap();
            CreateMap<DesignRequest, DesignResponseDto>()
                .ForMember(dest => dest.Username, otp => otp.MapFrom(src => src.User!.UserName))
                .ForMember(dest => dest.UserId, otp => otp.MapFrom(src => src.UserId))
                .ReverseMap();
            CreateMap<DesignRequest, OrderDesignRequestDto>().ReverseMap();

            #endregion

            #region Appointment Mapper

            CreateMap<Appointment, AppointmentRequestDto>().ReverseMap();
            CreateMap<Appointment, AppointmentResponseDto>().ReverseMap();

            #endregion

            #region Branch Mapper

            CreateMap<Branch, BranchCreateDto>().ReverseMap();
            CreateMap<Branch, BranchResponseDto>().ReverseMap();

            #endregion

            #region Measurement Mapper

            CreateMap<Measurement, MeasurementDto>().ReverseMap();
            CreateMap<Measurement, MeasurementCreateDto>().ReverseMap();
            CreateMap<Measurement, CreateMeasurementDto>().ReverseMap();
            CreateMap<Measurement, UpdateMeasurementDto>().ReverseMap();
            CreateMap<Measurement, MeasurementResponseDto>()
                .ForMember(dest => dest.IsLocked, opt => opt.MapFrom(src =>
                    src.Orders!.Any(o =>
                        o.Status != OrderStatus.CANCELLED &&
                        o.Status != OrderStatus.COMPLETED)))
                .ReverseMap();

            #endregion

            #region MeasurementDiary Mapper

            CreateMap<MeasurementDiary, MeasurementDiaryResponseDto>().ReverseMap();
            CreateMap<MeasurementDiary, DiaryWithMeasurementDto>().ReverseMap();
            CreateMap<MeasurementDiary, MeasurementDiaryDto>().ReverseMap();

            #endregion

            #region Chat Mapper

            CreateMap<ChatMessage, ChatMessageCreateDto>().ReverseMap();
            CreateMap<ChatMessage, ChatMessageResponseDto>()
                .ForMember(dest => dest.SenderName, otp => otp.MapFrom(src => src.Sender.FullName))
                .ForMember(dest => dest.SenderAvatar, otp => otp.MapFrom(src => src.Sender.ProfilePicture))
                .ForMember(dest => dest.MessageTimestamp, otp => otp.MapFrom(src => src.CreatedAt))
                .ReverseMap();
            CreateMap<ChatRoom, ChatRoomCreateDto>().ReverseMap();
            CreateMap<ChatRoom, ChatRoomResponseDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
                .ForMember(dest => dest.MemberCount, opt => opt.MapFrom(src => src.Members.Count))
                .ForMember(dest => dest.Members, opt => opt.MapFrom(src => src.Members))
                .ForMember(dest => dest.LastMessage, opt => opt.MapFrom(src => src.Messages
                    .OrderByDescending(m => m.CreatedAt)
                    .Select(m => m.Message)
                    .FirstOrDefault() ?? string.Empty))
                .ForMember(dest => dest.LastTimestamp, opt => opt.MapFrom(src => src.Messages
                    .OrderByDescending(m => m.CreatedAt)
                    .Select(m => m.CreatedAt)
                    .FirstOrDefault()))
                .ForMember(dest => dest.LastUserId, opt => opt.MapFrom(src => src.Messages
                    .OrderByDescending(m => m.CreatedAt)
                    .Select(m => m.SenderId)
                    .FirstOrDefault() ?? string.Empty))
                .ForMember(dest => dest.LastUserName, opt => opt.MapFrom(src => src.Messages
                    .OrderByDescending(m => m.CreatedAt)
                    .Select(m => m.Sender.FullName)
                    .FirstOrDefault() ?? string.Empty));
            CreateMap<ChatRoomMember, ChatRoomMemberResponseDto>()
                .ForMember(dest => dest.MemberId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.MemberAvatar, otp => otp.MapFrom(src => src.User.ProfilePicture))
                .ForMember(dest => dest.MemberName, opt => opt.MapFrom(src => src.User.FullName));

            #endregion

            #region MaternityDressTask Mapper

            CreateMap<MaternityDressTask, MaternityDressTaskRequestDto>().ReverseMap();
            CreateMap<MaternityDressTask, MaternityDressTaskResponseDto>().ReverseMap();
            CreateMap<MaternityDressTask, MaternityDressTaskDetailResponseDto>()
                .ForMember(dest => dest.Detail, opt => opt.MapFrom(src =>
                    src.OrderItemTasks != null ? src.OrderItemTasks.FirstOrDefault() : null))
                .ReverseMap();
            CreateMap<MaternityDressTask, MaternityDressTaskOrderTaskResponseDto>()
                .ForMember(dest => dest.Status, otp => otp.MapFrom(x => x.OrderItemTasks!.FirstOrDefault()!.Status))
                .ForMember(dest => dest.Note, otp => otp.MapFrom(x => x.OrderItemTasks!.FirstOrDefault()!.Note))
                .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.OrderItemTasks!.FirstOrDefault()!.Image))
                .ReverseMap();

            #endregion

            #region VoucherBatch Mapper

            CreateMap<VoucherBatch, VoucherBatchRequestDto>().ReverseMap();
            CreateMap<VoucherBatch, VoucherBatchResponseDto>().ReverseMap();
            CreateMap<VoucherBatch, VoucherBatchDetailResponseDto>()
                .ForMember(dest => dest.Details, opt => opt.MapFrom(src => src.VoucherDiscounts))
                .ReverseMap();

            #endregion

            #region VoucherDiscount Mapper

            CreateMap<VoucherDiscount, VoucherDiscountRequestDto>().ReverseMap();
            CreateMap<VoucherDiscount, VoucherDiscountResponseDto>().ReverseMap();

            #endregion

            #region Order Mapper

            CreateMap<Order, OrderRequestDto>().ReverseMap();
            CreateMap<Order, OrderReadyToBuyRequestDto>().ReverseMap();
            CreateMap<Order, OrderResponseDto>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.OrderItems))
                .ForMember(dest => dest.MeasurementDiary, otp => otp.MapFrom(src => src.Measurement!.MeasurementDiary))
                .ReverseMap();
            CreateMap<Order, OrderWithItemResponseDto>().ReverseMap();
            CreateMap<Order, OrderPresetCreateRequestDto>().ReverseMap();
            CreateMap<Order, OrderGetByIdResponseDto>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.OrderItems))
                .ForMember(dest => dest.MeasurementDiary, otp => otp.MapFrom(src => src.Measurement!.MeasurementDiary))
                .ReverseMap();
            CreateMap<Order, OrderWarrantyOnlyCode>()
                .ForMember(dest => dest.OrderItems, otp => otp.MapFrom(x => x.OrderItems))
                .ReverseMap();

            #endregion

            #region OrderItem Mapper

            CreateMap<OrderItem, OrderItemReadyToBuyRequestDto>().ReverseMap();
            CreateMap<OrderItem, OrderItemResponseDto>()
                .ForMember(dest => dest.AddOnOptions,
                    otp => otp.MapFrom(src => src.OrderItemAddOnOptions!.Select(x => x.AddOnOption)))
                .ForMember(dest => dest.Preset, otp => otp.MapFrom(src => src.Preset))
                .ReverseMap();
            CreateMap<OrderItem, OrderItemGetByIdResponseDto>()
                .ForMember(dest => dest.MaternityDressDetail, opt => opt.MapFrom(src => src.MaternityDressDetail))
                .ForMember(dest => dest.Milestones, opt => opt.MapFrom(src =>
                    src.OrderItemTasks!
                        .Where(x => x.MaternityDressTask!.Milestone != null)
                        .Select(x => x.MaternityDressTask!.Milestone)
                        .Distinct()
                        .ToList()))
                .ReverseMap();
            CreateMap<OrderItem, WarrantyOrderWithOrderItemDto>()
                .ForMember(dest => dest.Order, opt => opt.MapFrom(src => src.Order));

            #endregion

            #region OrderItemTask Mapper

            CreateMap<OrderItemTask, OrderItemTaskResponseDto>()
                .ForMember(dest => dest.ChargeId, otp => otp.MapFrom(src => src.UserId))
                .ForMember(dest => dest.ChargeName, otp => otp.MapFrom(src => src.User!.FullName))
                .ReverseMap();

            CreateMap<OrderItemTask, OrderItemTaskGetByTokenResponse>()
                .ForMember(dest => dest.Milestones, otp => otp.MapFrom(src => src.MaternityDressTask!.Milestone))
                .ReverseMap()
                ;

            #endregion

            #region Milestone Mapper

            CreateMap<Milestone, MilestoneRequestDto>().ReverseMap();
            CreateMap<Milestone, MilestoneResponseDto>().ReverseMap();
            CreateMap<Milestone, MilestoneGetByIdResponseDto>()
                .ForMember(dest => dest.Tasks, otp => otp.MapFrom(src => src.MaternityDressTasks))
                .ReverseMap();
            CreateMap<Milestone, MilestoneGetByIdOrderTaskResponseDto>()
                .ForMember(dest => dest.MaternityDressTasks, otp => otp.MapFrom(src => src.MaternityDressTasks))
                .ReverseMap();

            #endregion

            #region Notification Mapper

            CreateMap<Notification, NotificationResponseDto>()
                .ForMember(dest => dest.Metadata, opt => opt.MapFrom(
                    src => string.IsNullOrEmpty(src.Metadata)
                        ? null
                        : JsonConvert.DeserializeObject<Dictionary<string, string>>(src.Metadata)));

            CreateMap<NotificationRequestDto, Notification>()
                .ForMember(dest => dest.Metadata, opt => opt.MapFrom(
                    src => src.Metadata == null
                        ? null
                        : JsonConvert.SerializeObject(src.Metadata)));

            #endregion

            #region BranchMaternityDressDetail Mapper

            CreateMap<BranchMaternityDressDetail, BranchMaternityDressDetailDto>().ReverseMap();

            #endregion

            #region WarrantyHistory Mapper

            CreateMap<WarrantyHistory, WarrantyHistoryRequestDto>().ReverseMap();
            CreateMap<WarrantyHistory, WarrantyHistoryResponseDto>().ReverseMap();

            #endregion

            #region Adress Mapper

            CreateMap<Address, AddressRequestDto>().ReverseMap();
            CreateMap<Address, AddressResponseDto>().ReverseMap();

            #endregion

            #region Feedback Mapper

            CreateMap<Feedback, FeedbackRequestDto>().ReverseMap();
            CreateMap<Feedback, FeedbackResponseDto>()
                .ForMember(dest => dest.OrderId, otp => otp.MapFrom(src => src.OrderItem!.OrderId))
                .ForMember(dest => dest.OrderCode, otp => otp.MapFrom(src => src.OrderItem!.Order!.Code))
                .ReverseMap();

            #endregion

            #region CartItem Mapper

            CreateMap<CartItem, CartItemRequestDto>().ReverseMap();
            CreateMap<CartItem, CartItemResponseDto>().ReverseMap();

            #endregion

            #region Preset Mapper

            CreateMap<Preset, PresetCreateRequestDto>().ReverseMap();
            CreateMap<Preset, PresetCreateForDesignRequestDto>().ReverseMap();
            CreateMap<Preset, PresetUpdateRequestDto>().ReverseMap();
            CreateMap<Preset, PresetGetAllResponseDto>()
                .ForMember(dest => dest.StyleName, otp => otp.MapFrom(src => src.Style!.Name))
                .ReverseMap();
            CreateMap<Preset, PresetGetByIdResponseDto>()
                .ForMember(dest => dest.ComponentOptions,
                    otp => otp.MapFrom(src => src.ComponentOptionPresets.Select(x => x.ComponentOption)))
                .ReverseMap();
            CreateMap<Preset, PresetRatedResponseDto>()
                .ForMember(dest => dest.FeedbackCount,
                    otp => otp.MapFrom(x => x.OrderItems.Sum(x => x.Feedbacks.Count())))
                .ForMember(dest => dest.AverageRate,
                    otp => otp.MapFrom(src => src.OrderItems
                        .SelectMany(oi => oi.Feedbacks!)
                        .Any()
                        ? src.OrderItems
                            .SelectMany(oi => oi.Feedbacks!)
                            .Average(fb => fb.Rated)
                        : 0))
                .ReverseMap();

            #endregion

            #region WarrantyRequest Mapper

            CreateMap<WarrantyRequest, WarrantyRequestCreateDto>().ReverseMap();
            CreateMap<WarrantyRequest, WarrantyRequestGetAllDto>()
                .ForMember(dest => dest.Customer,
                    otp => otp.MapFrom(src => src.WarrantyRequestItems.FirstOrDefault().OrderItem.Order.User))
                .ForMember(dest => dest.CountItem, otp => otp.MapFrom(src => src.WarrantyRequestItems.Count()))
                .ReverseMap();
            CreateMap<WarrantyRequest, WarrantyGetByIdResponseDto>()
                .IncludeBase<WarrantyRequest, WarrantyRequestGetAllDto>()
                .ForMember(d => d.Items, o => o.MapFrom(s => s.WarrantyRequestItems))
                .AfterMap((src, dest, ctx) =>
                {
                    var items = src.WarrantyRequestItems ?? new List<WarrantyRequestItem>();
                    if (dest.Items == null || dest.Items.Count == 0)
                    {
                        dest.CountItem = 0;
                        return;
                    }

                    foreach (var dto in dest.Items)
                    {
                        var entity = items.FirstOrDefault(e => e.OrderItemId == dto.OrderItemId);
                        var parent = entity?.OrderItem?.ParentOrderItem ?? entity?.OrderItem; // root order item
                        var rootOrder = parent?.Order;
                        if (parent == null)
                        {
                            dto.Orders = new List<OrderWarrantyOnlyCode>();
                            continue;
                        }

                        dto.Orders = new List<OrderWarrantyOnlyCode>
                        {
                            new OrderWarrantyOnlyCode
                            {
                                Id = rootOrder?.Id ?? parent.OrderId,
                                Code = rootOrder?.Code,
                                ReceivedAt = rootOrder?.ReceivedAt ?? DateTime.MinValue,
                                OrderItems = new List<OrderItemGetByIdResponseDto>
                                {
                                    ctx.Mapper.Map<OrderItemGetByIdResponseDto>(entity!.OrderItem)
                                }
                            }
                        };
                    }

                    dest.CountItem = dest.Items.Count;
                })
                .ReverseMap();

            CreateMap<WarrantyRequestItem, WarrantyRequestItemDetailResponseDto>()
                .ForMember(dest => dest.OrderItemId, opt => opt.MapFrom(src => src.OrderItemId))
                .ForMember(dest => dest.DestinationBranchId, opt => opt.MapFrom(src => src.DestinationBranchId))
                .ForMember(dest => dest.TrackingCode, opt => opt.MapFrom(src => src.TrackingCode))
                .ForMember(dest => dest.Fee, opt => opt.MapFrom(src => src.Fee))
                .ForMember(dest => dest.RejectedReason, opt => opt.MapFrom(src => src.RejectedReason))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.EstimateTime, opt => opt.MapFrom(src => src.EstimateTime))
                .ForMember(dest => dest.DestinationType, opt => opt.MapFrom(src => src.DestinationType))
                .ForMember(dest => dest.WarrantyRound, opt => opt.MapFrom(src => src.WarrantyRound))
                .ForMember(dest => dest.Histories, opt => opt.MapFrom(src => src.WarrantyRequest.WarrantyHistories))
                .ForMember(d => d.Orders, o => o.Ignore())
                .ReverseMap();

            #endregion

            #region WarrantyRequestItem Mapper

            CreateMap<WarrantyRequestItem, WarrantyRequestCreateDto>().ReverseMap();
            CreateMap<WarrantyRequestItem, WarrantyRequestItemGetAllDto>()
                .ForMember(dest => dest.OrderWithItems, opt => opt.MapFrom(src => src.OrderItem)).ReverseMap();
            CreateMap<WarrantyRequestItem, WarrantyRequestItemDetailListDto>()
                .ForMember(dest => dest.WarrantyRequestItems, otp => otp.MapFrom(x => x))
                .ForMember(dest => dest.Order, otp => otp.MapFrom(x => x.OrderItem!.Order))
                .ReverseMap();
            CreateMap<WarrantyRequestItem, WarrantyRequestItemOrderResponseDto>()
               .ReverseMap();
            #endregion

            #region AddOn Mapper

            CreateMap<AddOn, AddOnDto>().ReverseMap();
            CreateMap<AddOn, AddOnRequestDto>().ReverseMap();

            #endregion

            #region AddOnOption Mapper

            CreateMap<AddOnOption, AddOnOptionDto>()
                .ForMember(dest => dest.Value,
                    otp => otp.MapFrom(src => src.OrderItemAddOnOptions!.Select(x => x.Value).FirstOrDefault()))
                .ReverseMap();
            CreateMap<AddOnOption, AddOnOptionRequestDto>().ReverseMap();

            #endregion

            #region Position Mapper

            CreateMap<Position, PositionDto>().ReverseMap();
            CreateMap<Position, PositionRequestDto>().ReverseMap();

            #endregion

            #region Size Mapper

            CreateMap<Size, SizeDto>().ReverseMap();
            CreateMap<Size, SizeRequestDto>().ReverseMap();

            #endregion

            #region Transaction Mapper
            CreateMap<Transaction, TransactionResponseDto>()
                .ForMember(dest => dest.OrderId, opt => opt.MapFrom(src => src.OrderId))
                .ReverseMap();

            #endregion
        }
    }
}