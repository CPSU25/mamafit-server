using AutoMapper;
using MamaFit.BusinessObjects.DTO.TicketDto;
using MamaFit.BusinessObjects.Entity;
using MamaFit.Repositories.Implement;
using MamaFit.Services.Interface;
using Microsoft.AspNetCore.Http;

namespace MamaFit.Services.Service
{
    public class TicketService : ITicketService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidationService _validationService;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IMapper _mapper;

        public TicketService(IMapper mapper, IValidationService validationService, IUnitOfWork unitOfWork, IHttpContextAccessor contextAccessor)
        {
            _mapper = mapper;
            _validationService = validationService;
            _unitOfWork = unitOfWork;
            _contextAccessor = contextAccessor;
        }

        public async Task<string> CreateTicket(TicketRequestCreateDto request)
        {
            var orderItem = await _unitOfWork.OrderItemRepository.GetDetailById(request.OrderItemId);
            _validationService.CheckNotFound(orderItem, $"Order item with Id: {request.OrderItemId} is not found ");

            var ticket = _mapper.Map<Ticket>(request);
            ticket.OrderItem = orderItem;

            await _unitOfWork.TicketRepository.InsertAsync(ticket);
            await _unitOfWork.SaveChangesAsync();

            return ticket.Id;
        }

        public async Task DeleteTicket(string ticketId)
        {
            var ticket = await _unitOfWork.TicketRepository.GetByIdAsync(ticketId);
            _validationService.CheckNotFound(ticket, $"Ticket with id: {ticketId} not found");

            await _unitOfWork.TicketRepository.SoftDeleteAsync(ticket);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<List<TicketResponseWithOrderDto>> GetAll()
        {
            var ticketList = await _unitOfWork.TicketRepository.GetAllTicketAsync();

            var response = ticketList.Select(x => _mapper.Map<TicketResponseWithOrderDto>(x)).ToList();
            return response;
        }

        public async Task<List<TicketResponseWithOrderDto>> GetAllWithUserId()
        {
            var userId = GetCurrentUserId();
            _validationService.CheckNotFound(userId, "Please sigin!");

            var ticketList = await _unitOfWork.TicketRepository.GetAllTicketByUserIdAsync(userId);

            var response = ticketList.Select(x => _mapper.Map<TicketResponseWithOrderDto>(x)).ToList();
            return response;
        }

        public async Task<TicketResponseWithOrderDto> GetById(string ticketId)
        {
            var ticket = await _unitOfWork.TicketRepository.GetTicketByIdAsync(ticketId);

            var response = _mapper.Map<TicketResponseWithOrderDto>(ticket);
            return response;
        }

        public async Task UpdateTicket(TicketRequestUpdateDto request, string ticketId)
        {
            var ticket = await _unitOfWork.TicketRepository.GetTicketByIdAsync(ticketId);
            _validationService.CheckNotFound(ticket, $"Ticket with Id: {ticketId} not found");

            _mapper.Map(ticket, request);

            await _unitOfWork.TicketRepository.UpdateAsync(ticket);
            await _unitOfWork.SaveChangesAsync();
        }

        private string GetCurrentUserId()
        {
            return _contextAccessor.HttpContext.User.FindFirst("userId").Value;
        }
    }
}
