using AutoMapper;
using MamaFit.BusinessObjects.DTO.Role;
using MamaFit.BusinessObjects.DTO.RoleDto;
using MamaFit.BusinessObjects.DTO.Token;
using MamaFit.BusinessObjects.DTO.UserDto;
using MamaFit.BusinessObjects.Entity;
using MamaFit.BusinessObjects.DTO.MaternityDressDto;
using MamaFit.BusinessObjects.DTO.CategoryDto;
using MamaFit.BusinessObjects.DTO.StyleDto;
using MamaFit.BusinessObjects.DTO.ComponentDto;
using MamaFit.BusinessObjects.DTO.ComponentOptionDto;
using MamaFit.BusinessObjects.DTO.MaternityDressDetailDto;
using MamaFit.BusinessObjects.DTO.DesignRequestDto;
using MamaFit.BusinessObjects.DTO.Appointment;
using MamaFit.BusinessObjects.DTO.BranchDto;
using MamaFit.BusinessObjects.DTO.MeasurementDto;

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
                .ForMember(dest => dest.StyleName , otp => otp.MapFrom(x => x.Style!.Name))
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
            CreateMap<Branch,BranchResponseDto>().ReverseMap();
            
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
        }
    }
}
