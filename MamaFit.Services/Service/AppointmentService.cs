using AutoMapper;
using MamaFit.BusinessObjects.DTO.Appointment;
using MamaFit.BusinessObjects.DTO.AppointmentDto;
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
                throw new ErrorException(StatusCodes.Status404NotFound, ErrorCode.NotFound, "Branch is not available");

            if (token == "System") //For stranger want to set up an Appointment
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
            else // For User
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

        public async Task<PaginatedList<AppointmentResponseDto>> GetAllAsync(int index, int pageSize, string? search, AppointmentOrderBy? sortBy)
        {
            var appointmentRepo = _unitOfWork.GetRepository<Appointment>();

            var query = appointmentRepo.Entities
                .AsNoTracking()
                .Where(a => a.IsDeleted.Equals(false));

            query = sortBy switch
            {
                AppointmentOrderBy.UPCOMMING_AT_ASC => query
                .Where(u => u.BookingTime > DateTime.UtcNow)
                .OrderBy(u => u.BookingTime),

                AppointmentOrderBy.UPCOMMING_AT_DESC => query
                .Where(u => u.BookingTime > DateTime.UtcNow)
                .OrderByDescending(u => u.BookingTime),

                AppointmentOrderBy.CREATED_AT_ASC => query.OrderBy(u => u.CreatedAt),

                AppointmentOrderBy.CREATED_AT_DESC => query.OrderByDescending(u => u.CreatedAt),
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

            var oldAppointment = await appointmentRepo.GetByIdAsync(id);
            if (oldAppointment == null)
                throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, $"Appointment not found with {id}");

            return _mapper.Map<AppointmentResponseDto>(oldAppointment);
        }

        public async Task UpdateAsync(string id, AppointmentRequestDto requestDto)
        {
            var appointmentRepo = _unitOfWork.GetRepository<Appointment>();

            var appointment = await appointmentRepo.GetByIdAsync(id);
            if (appointment == null)
                throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, $"Appointment not found with {id}");

            _mapper.Map(requestDto, appointment);
            appointment.UpdatedAt = DateTime.UtcNow;
            appointment.UpdatedBy = GetCurrentUserName();

            await appointmentRepo.UpdateAsync(appointment);
            await appointmentRepo.SaveAsync();
        }

        public async Task CheckInAsync(string id)
        {
            var appointmentRepo = _unitOfWork.GetRepository<Appointment>();

            var oldAppointment = await appointmentRepo.GetByIdAsync(id);
            if (oldAppointment == null)
                throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, $"Appointment not found with {id}");

            if (oldAppointment.Status != BusinessObjects.Enum.AppointmentStatus.CHECKED_IN)
            {
                oldAppointment.Status = BusinessObjects.Enum.AppointmentStatus.CHECKED_IN;
                oldAppointment.UpdatedAt = DateTime.UtcNow;
                oldAppointment.UpdatedBy = GetCurrentUserName();

                await appointmentRepo.UpdateAsync(oldAppointment);
                await appointmentRepo.SaveAsync();
            }
            else throw new ErrorException(StatusCodes.Status400BadRequest, ResponseCodeConstants.BADREQUEST, $"Appointment with id:{id} already checked-in");
        }

        public async Task CancelAppointment(string id, string reason)
        {
            var appointmentRepo = _unitOfWork.GetRepository<Appointment>();// Repo Appointment

            var oldAppointment = await appointmentRepo.GetByIdAsync(id); // Search Appointment
            if (oldAppointment == null)
                throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, $"Appointment not found with {id}");

            if(oldAppointment.Status != BusinessObjects.Enum.AppointmentStatus.CANCELED)
            {
                oldAppointment.Status = BusinessObjects.Enum.AppointmentStatus.CANCELED;
                oldAppointment.CanceledAt = DateTime.UtcNow;
                oldAppointment.CanceledReason = reason;
                oldAppointment.UpdatedAt = DateTime.UtcNow;
                oldAppointment.UpdatedBy = GetCurrentUserName();

                await appointmentRepo.UpdateAsync(oldAppointment);
                await appointmentRepo.SaveAsync();
            }
            else throw new ErrorException(StatusCodes.Status400BadRequest, ResponseCodeConstants.BADREQUEST, $"Appointment with id:{id} already cancelled");
        }

        public async Task CheckOutAsync(string id)
        {
            var appointmentRepo = _unitOfWork.GetRepository<Appointment>();

            var oldAppointment = await appointmentRepo.GetByIdAsync(id);
            if (oldAppointment == null)
                throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, $"Appointment not found with {id}");

            if (oldAppointment.Status != BusinessObjects.Enum.AppointmentStatus.CHECKED_OUT)
            {
                oldAppointment.Status = BusinessObjects.Enum.AppointmentStatus.CHECKED_OUT;
                oldAppointment.UpdatedAt = DateTime.UtcNow;
                oldAppointment.UpdatedBy = GetCurrentUserName();

                await appointmentRepo.UpdateAsync(oldAppointment);
                await appointmentRepo.SaveAsync();
            }
            else throw new ErrorException(StatusCodes.Status400BadRequest, ResponseCodeConstants.BADREQUEST, $"Appointment with id:{id} already checked-out");
        }
    }
}
