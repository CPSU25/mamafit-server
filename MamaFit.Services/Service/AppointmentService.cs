using AutoMapper;
using MamaFit.BusinessObjects.DTO.Appointment;
using MamaFit.BusinessObjects.DTO.AppointmentDto;
using MamaFit.BusinessObjects.Entity;
using MamaFit.BusinessObjects.Enum;
using MamaFit.Repositories.Infrastructure;
using MamaFit.Repositories.Interface;
using Microsoft.AspNetCore.Http;

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
            var token = GetCurrentUserName();

            var branch = await _unitOfWork.BranchRepository.GetByIdAsync(requestDto.BranchId);
            if (branch == null || branch.IsDeleted)
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

                await _unitOfWork.AppointmentRepository.InsertAsync(newAppointmentWithStranger);
                await _unitOfWork.SaveChangesAsync();
            }
            else // For User
            {
                var user = await _unitOfWork.UserRepository.GetByIdAsync(requestDto.UserId);
                if (user == null)
                    throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "User is not available");

                var newAppointment = _mapper.Map<Appointment>(requestDto);
                newAppointment.User = user;
                newAppointment.Branch = branch;
                newAppointment.CreatedAt = DateTime.UtcNow;
                newAppointment.CreatedBy = GetCurrentUserName();

                await _unitOfWork.AppointmentRepository.InsertAsync(newAppointment);
                await _unitOfWork.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(string id)
        {
            var oldAppointment = await _unitOfWork.AppointmentRepository.GetByIdAsync(id);
            if (oldAppointment == null || oldAppointment.IsDeleted)
                throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, $"Appointment not found with id {id}");

            await _unitOfWork.AppointmentRepository.SoftDeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<PaginatedList<AppointmentResponseDto>> GetAllAsync(int index, int pageSize, string? search, AppointmentOrderBy? sortBy)
        {
            var appointmentList = await _unitOfWork.AppointmentRepository.GetAllAsync(index, pageSize, search, sortBy);

            // Map từng phần tử trong danh sách Items
            var responseList = appointmentList.Items.Select(item => _mapper.Map<AppointmentResponseDto>(item)).ToList();

            // Tạo PaginatedList mới với các đối tượng đã map
            var paginatedResponse = new PaginatedList<AppointmentResponseDto>(
                responseList,
                appointmentList.TotalCount,
                appointmentList.PageNumber,
                appointmentList.PageSize
            );

            return paginatedResponse;
        }


        public async Task<AppointmentResponseDto> GetByIdAsync(string id)
        {
            var oldAppointment = await _unitOfWork.AppointmentRepository.GetByIdAsync(id);
            if (oldAppointment == null || oldAppointment.IsDeleted)
                throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, $"Appointment not found with id {id}");

            return _mapper.Map<AppointmentResponseDto>(oldAppointment);
        }

        public async Task UpdateAsync(string id, AppointmentRequestDto requestDto)
        {
            var appointment = await _unitOfWork.AppointmentRepository.GetByIdAsync(id);
            if (appointment == null || appointment.IsDeleted)
                throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, $"Appointment not found with id {id}");

            _mapper.Map(requestDto, appointment);
            appointment.UpdatedAt = DateTime.UtcNow;
            appointment.UpdatedBy = GetCurrentUserName();

            await _unitOfWork.AppointmentRepository.UpdateAsync(appointment);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task CheckInAsync(string id)
        {
            var oldAppointment = await _unitOfWork.AppointmentRepository.GetByIdAsync(id);
            if (oldAppointment == null || oldAppointment.IsDeleted || oldAppointment.Status == AppointmentStatus.CANCELED)
                throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, $"Appointment not found with id {id}");

            if (oldAppointment.Status != BusinessObjects.Enum.AppointmentStatus.CHECKED_IN)
            {
                oldAppointment.Status = BusinessObjects.Enum.AppointmentStatus.CHECKED_IN;
                oldAppointment.UpdatedAt = DateTime.UtcNow;
                oldAppointment.UpdatedBy = GetCurrentUserName();

                await _unitOfWork.AppointmentRepository.UpdateAsync(oldAppointment);
                await _unitOfWork.SaveChangesAsync();
            }
            else
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ResponseCodeConstants.BADREQUEST, $"Appointment with id {id} already checked-in");
            }
        }

        public async Task CancelAppointment(string id, string reason)
        {
            var oldAppointment = await _unitOfWork.AppointmentRepository.GetByIdAsync(id);
            if (oldAppointment == null || oldAppointment.IsDeleted)
                throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, $"Appointment not found with id {id}");

            if (oldAppointment.Status != BusinessObjects.Enum.AppointmentStatus.CANCELED)
            {
                oldAppointment.Status = BusinessObjects.Enum.AppointmentStatus.CANCELED;
                oldAppointment.CanceledAt = DateTime.UtcNow;
                oldAppointment.CanceledReason = reason;
                oldAppointment.UpdatedAt = DateTime.UtcNow;
                oldAppointment.UpdatedBy = GetCurrentUserName();

                await _unitOfWork.AppointmentRepository.UpdateAsync(oldAppointment);
                await _unitOfWork.SaveChangesAsync();
            }
            else
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ResponseCodeConstants.BADREQUEST, $"Appointment with id {id} already cancelled");
            }
        }

        public async Task CheckOutAsync(string id)
        {
            var oldAppointment = await _unitOfWork.AppointmentRepository.GetByIdAsync(id);
            if (oldAppointment == null || oldAppointment.IsDeleted)
                throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, $"Appointment not found with id {id}");

            if (oldAppointment.Status != BusinessObjects.Enum.AppointmentStatus.CHECKED_OUT || oldAppointment.Status != AppointmentStatus.CANCELED)
            {
                oldAppointment.Status = BusinessObjects.Enum.AppointmentStatus.CHECKED_OUT;
                oldAppointment.UpdatedAt = DateTime.UtcNow;
                oldAppointment.UpdatedBy = GetCurrentUserName();

                await _unitOfWork.AppointmentRepository.UpdateAsync(oldAppointment);
                await _unitOfWork.SaveChangesAsync();
            }
            else
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ResponseCodeConstants.BADREQUEST, $"Appointment with id {id} already checked-out");
            }
        }

        public async Task<PaginatedList<AppointmentResponseDto>> GetByUserId(string userId, int index, int pageSize, string? search, AppointmentOrderBy? sortBy)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);
            if (user == null)
                throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, $"User not found with id {userId}");

            var appointmentList = await _unitOfWork.AppointmentRepository.GetByUserId(userId,index, pageSize, search, sortBy);

            // Map từng phần tử trong danh sách Items
            var responseList = appointmentList.Items.Select(item => _mapper.Map<AppointmentResponseDto>(item)).ToList();

            // Tạo PaginatedList mới với các đối tượng đã map
            var paginatedResponse = new PaginatedList<AppointmentResponseDto>(
                responseList,
                appointmentList.TotalCount,
                appointmentList.PageNumber,
                appointmentList.PageSize
            );

            return paginatedResponse;
        }
    }
}
