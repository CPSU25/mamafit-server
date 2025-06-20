using AutoMapper;
using MamaFit.BusinessObjects.DTO.AppointmentDto;
using MamaFit.BusinessObjects.DTO.RoleDto;
using MamaFit.BusinessObjects.DTO.UserDto;
using MamaFit.BusinessObjects.Entity;
using MamaFit.BusinessObjects.DTO.MaternityDressDto;
using MamaFit.BusinessObjects.DTO.CategoryDto;
using MamaFit.BusinessObjects.DTO.StyleDto;
using MamaFit.BusinessObjects.DTO.ComponentDto;
using MamaFit.BusinessObjects.DTO.ComponentOptionDto;
using MamaFit.BusinessObjects.DTO.MaternityDressDetailDto;
using MamaFit.BusinessObjects.DTO.DesignRequestDto;
using MamaFit.BusinessObjects.DTO.BranchDto;
using MamaFit.BusinessObjects.DTO.MeasurementDto;
using MamaFit.BusinessObjects.Entity.ChatEntity;
using MamaFit.BusinessObjects.DTO.ChatMessageDto;
using MamaFit.BusinessObjects.DTO.ChatRoomDto;
using MamaFit.BusinessObjects.DTO.TokenDto;
using MamaFit.BusinessObjects.DTO.VoucherBatchDto;
using MamaFit.BusinessObjects.DTO.MaternityDressTask;
using MamaFit.BusinessObjects.DTO.ChatRoomMemberDto;
using MamaFit.BusinessObjects.DTO.OrderDto;
using MamaFit.BusinessObjects.DTO.VoucherDiscountDto;
using MamaFit.BusinessObjects.DTO.MilestoneDto;
using MamaFit.BusinessObjects.DTO.OrderItemDto;

namespace MamaFit.Services.Mapper
{
    public class MapperEntities : Profile
    {
        public MapperEntities()
        {
            //TokenDto Mapper
            CreateMap<ApplicationUserToken, TokenResponseDto>().ReverseMap();
            CreateMap<ApplicationUserToken, RefreshTokenRequestDto>().ReverseMap();

            //User Mapper
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

            //Role Mapper
            CreateMap<ApplicationUserRole, RoleResponseDto>().ReverseMap();
            CreateMap<ApplicationUserRole, RoleRequestDto>().ReverseMap();

            //MaternityDress Mapper
            CreateMap<MaternityDress, MaternityDressRequestDto>().ReverseMap();
            CreateMap<MaternityDress, MaternityDressResponseDto>()
                .ForMember(dest => dest.StyleName, otp => otp.MapFrom(x => x.Style!.Name))
                .ReverseMap();

            CreateMap<MaternityDress, GetAllResponseDto>()
                .ForMember(dest => dest.Price, otp => otp.MapFrom(x => x.Details.Select(x => x.Price)))
                .ReverseMap();

            //MaternityDressDetail Mappper
            CreateMap<MaternityDressDetail, MaternityDressDetailRequestDto>().ReverseMap();
            CreateMap<MaternityDressDetail, MaternityDressDetailResponseDto>().ReverseMap();

            //Category Mapper
            CreateMap<Category, CategoryRequestDto>().ReverseMap();
            CreateMap<Category, CategoryResponseDto>().ReverseMap();

            //Style Mapper
            CreateMap<Style, StyleRequestDto>().ReverseMap();
            CreateMap<Style, StyleResponseDto>().ReverseMap();

            //Component Mapper
            CreateMap<Component, ComponentRequestDto>().ReverseMap();
            CreateMap<Component, ComponentResponseDto>().ReverseMap();

            //ComponentOption Mapper
            CreateMap<ComponentOption, ComponentOptionRequestDto>().ReverseMap();
            CreateMap<ComponentOption, ComponentOptionResponseDto>().ReverseMap();

            //DesignRequest Mapper
            CreateMap<DesignRequest, DesignRequestCreateDto>().ReverseMap();
            CreateMap<DesignRequest, DesignResponseDto>().ReverseMap();

            //Appointment Mapper 
            CreateMap<Appointment, AppointmentRequestDto>().ReverseMap();
            CreateMap<Appointment, AppointmentResponseDto>().ReverseMap();

            //Branch Mapper
            CreateMap<Branch, BranchCreateDto>().ReverseMap();
            CreateMap<Branch, BranchResponseDto>().ReverseMap();

            //Measurement Mapper
            CreateMap<Measurement, MeasurementDto>().ReverseMap();
            CreateMap<MeasurementDiary, MeasurementDiaryDto>().ReverseMap();
            CreateMap<Measurement, MeasurementCreateDto>().ReverseMap();
            CreateMap<Measurement, CreateMeasurementDto>().ReverseMap();
            CreateMap<MeasurementDiary, MeasurementDiaryDto>().ReverseMap();
            CreateMap<Measurement, UpdateMeasurementDto>().ReverseMap();
            CreateMap<Measurement, MeasurementResponseDto>().ReverseMap();
            CreateMap<MeasurementDiary, MeasurementDiaryResponseDto>().ReverseMap();
            CreateMap<MeasurementDiary, DiaryWithMeasurementDto>()
                .ReverseMap();

            //Chat Mapper 
            CreateMap<ChatMessage, ChatMessageCreateDto>().ReverseMap();
            CreateMap<ChatMessage, ChatMessageResponseDto>()
                .ForMember(dest => dest.SenderName, otp => otp.MapFrom(src => src.Sender.FullName))
                .ForMember(dest => dest.SenderAvatar, otp => otp.MapFrom(src => src.Sender.ProfilePicture))
                .ForMember(dest => dest.MessageTimestamp, otp => otp.MapFrom(src => src.CreatedAt))
                .ReverseMap();
            CreateMap<ChatRoom, ChatRoomCreateDto>().ReverseMap();
            CreateMap<ChatRoom, ChatRoomResponseDto>().ReverseMap();
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

            //MaternityDressTask Mapper
            CreateMap<MaternityDressTask, MaternityDressTaskRequestDto>().ReverseMap();
            CreateMap<MaternityDressTask, MaternityDressTaskResponseDto>().ReverseMap();
            
            //VoucherBatch Mapper
            CreateMap<VoucherBatch, VoucherBatchDto>().ReverseMap();
            CreateMap<VoucherBatch, VoucherBatchResponseDto>().ReverseMap();
            
            //VoucherDiscount Mapper
            CreateMap<VoucherDiscount, VoucherDiscountRequestDto>().ReverseMap();
            CreateMap<VoucherDiscount, VoucherDiscountResponseDto>().ReverseMap();
            
            //Order Mapper
            CreateMap<Order, OrderRequestDto>().ReverseMap();
            CreateMap<Order, OrderReadyToBuyRequestDto>().ReverseMap();
            CreateMap<Order, OrderResponseDto>().ReverseMap();

            //OrderItem Mapper
            CreateMap<OrderItem, OrderItemReadyToBuyRequestDto>().ReverseMap();
            CreateMap<OrderItem, OrderItemResponseDto>().ReverseMap();

            //Milestone Mapper
            CreateMap<Milestone, MilestoneRequestDto>().ReverseMap();
            CreateMap<Milestone, MilestoneResponseDto>().ReverseMap();
        }
    }
}