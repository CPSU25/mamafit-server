using AutoMapper;
using MamaFit.BusinessObjects.DTO.Appointment;
using MamaFit.BusinessObjects.Entity;
using MamaFit.Repositories.Infrastructure;
using MamaFit.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace MamaFit.Services.Service
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _contextAccessor;

        public AppointmentService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor contextAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _contextAccessor = contextAccessor;
        }

        private string GetCurrentUserName()
        {
            return _contextAccessor.HttpContext?.User?.FindFirst("name")?.Value ?? "System";
        }



        public async Task CreateAsync(AppointmentRequestDto requestDto)
        {
            var userRepo = _unitOfWork.GetRepository<ApplicationUser>();
            var branchRepo = _unitOfWork.GetRepository<Branch>();
            var appointmentRepo = _unitOfWork.GetRepository<Appointment>();

            var token = GetCurrentUserName();

            var branch = await branchRepo.GetByIdAsync(requestDto.BranchId);
            if (branch == null)
                throw new ErrorException(StatusCodes.Status404NotFound,ErrorCode.NotFound, "Branch is not available");

            if (token == "System")
            {
                var newAppointmentWithStranger = new Appointment
                {
                    BookingTime = requestDto.BookingTime,
                    PhoneNumber = requestDto.PhoneNumber,
                    Branch = branch,
                    FullName = requestDto.FullName,
                    Note = requestDto.Note,
                    Status = BusinessObjects.Enum.AppointmentStatus.UP_COMMING,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = requestDto.FullName
                };

                await appointmentRepo.InsertAsync(newAppointmentWithStranger);
                await appointmentRepo.SaveAsync();
            }
            else
            {
                var user = await userRepo.GetByIdAsync(requestDto.UserId);
                if (user == null)
                    throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "User is not available");

                if (requestDto.StaffId != null)
                {
                    var staff = await userRepo.GetByIdAsync(requestDto.StaffId);
                    if (staff == null || !branch.Staffs.Contains(staff))
                        throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Staff is not available");
                }

                var newAppointment = _mapper.Map<Appointment>(requestDto);
                newAppointment.User = user;
                newAppointment.Branch = branch;
                newAppointment.CreatedAt = DateTime.UtcNow;
                newAppointment.CreatedBy = GetCurrentUserName();

                await appointmentRepo.InsertAsync(newAppointment);
                await appointmentRepo.SaveAsync();
            }
        }

        public async Task DeleteAsync(string id)
        {
            var appointmentRepo = _unitOfWork.GetRepository<Appointment>();

            var oldAppointment = await appointmentRepo.GetByIdAsync(id);
            if (oldAppointment == null)
                throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, $"Appointment not found with {id}");

            await appointmentRepo.SoftDeleteAsync(id);
            await appointmentRepo.SaveAsync();
        }

        public async Task<PaginatedList<AppointmentResponseDto>> GetAllAsync(int index, int pageSize, string? search, string? sortBy)
        {
            var appointmentRepo = _unitOfWork.GetRepository<Appointment>();

            var query = appointmentRepo.Entities
                .AsNoTracking()
                .Where(a => a.IsDeleted.Equals(false));

            query = sortBy?.ToLower() switch
            {
                "name_asc" => query.OrderBy(u => u.FullName),
                "name_desc" => query.OrderByDescending(u => u.FullName),
                "createdat_asc" => query.OrderBy(u => u.CreatedAt),
                "createdat_desc" => query.OrderByDescending(u => u.CreatedAt),
                _ => query.OrderByDescending(u => u.CreatedAt) // default
            };

            var pagedResult = await appointmentRepo.GetPagging(query, index, pageSize);

            var listAppointment = pagedResult.Items
                .Select(_mapper.Map<AppointmentResponseDto>)
                .ToList();

            var responseAppointmentList = new PaginatedList<AppointmentResponseDto>
                (listAppointment, pagedResult.TotalCount, pagedResult.PageNumber, pageSize);
            return responseAppointmentList;
        }

        public async Task<AppointmentResponseDto> GetByIdAsync(string id)
        {

            var appointmentRepo = _unitOfWork.GetRepository<Appointment>();
            var appointment = await appointmentRepo.GetByIdAsync(id);

            return _mapper.Map<AppointmentResponseDto>(appointment);
        }

        public async Task UpdateAsync(string id, AppointmentRequestDto requestDto)
        {
            var appointmentRepo = _unitOfWork.GetRepository<Appointment>();

            var appointment = await appointmentRepo.GetByIdAsync(id);

            _mapper.Map(requestDto, appointment);
            appointment.UpdatedAt = DateTime.UtcNow;
            appointment.UpdatedBy = GetCurrentUserName();

            await appointmentRepo.UpdateAsync(appointment);
            await appointmentRepo.SaveAsync();
        }
    }
}
