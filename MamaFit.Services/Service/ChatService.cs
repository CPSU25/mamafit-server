using AutoMapper;
using MamaFit.BusinessObjects.DTO.ChatMessageDto;
using MamaFit.BusinessObjects.DTO.ChatRoomDto;
using MamaFit.BusinessObjects.Entity.ChatEntity;
using MamaFit.Repositories.Implement;
using MamaFit.Repositories.Infrastructure;
using MamaFit.Repositories.Interface;
using MamaFit.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;

namespace MamaFit.Services.Service
{
    public class ChatService : IChatService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ChatService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ChatMessageResponseDto> CreateChatMessageAsync(ChatMessageCreateDto requestDto)
        {
            var sender = await _unitOfWork.UserRepository.GetByIdAsync(requestDto.SenderId);
            if (sender == null)
                throw new ErrorException(StatusCodes.Status404NotFound, ApiCodes.NOT_FOUND, "User not found!");

            var chatroom = await _unitOfWork.ChatRepository.GetChatRoomById(requestDto.ChatRoomId);
            if (chatroom == null)
                throw new ErrorException(StatusCodes.Status404NotFound, ApiCodes.NOT_FOUND, "Chatroom is not avaiable!");

            var chatMessage = _mapper.Map<ChatMessage>(requestDto);
            chatMessage.IsRead = false;
            await _unitOfWork.ChatRepository.CreateChatMessageAsync(chatMessage);

            return _mapper.Map<ChatMessageResponseDto>(chatMessage);
        }

        public async Task CreateChatRoomAsync(string userId1, string userId2)
        {
            var user1 = await _unitOfWork.UserRepository.GetByIdAsync(userId1);
            if (user1 == null)
                throw new ErrorException(StatusCodes.Status404NotFound, ApiCodes.NOT_FOUND, $"User with id:{userId1} not found!");

            var user2 = await _unitOfWork.UserRepository.GetByIdAsync(userId2);
            if (user2 == null)
                throw new ErrorException(StatusCodes.Status404NotFound, ApiCodes.NOT_FOUND, $"User with id:{userId2} not found!");

            await _unitOfWork.ChatRepository.CreateChatRoomAsync(userId1, userId2);
        }

        public async Task<List<ChatMessageResponseDto>> GetChatHistoryAsync(string chatRoomId, int index, int pageSize)
        {
            var chatHistory = await _unitOfWork.ChatRepository.GetChatHistoryAsync(chatRoomId, index, pageSize);
            if (chatHistory == null)
                throw new ErrorException(StatusCodes.Status404NotFound, ApiCodes.NOT_FOUND, "Chat room not found!");

            return _mapper.Map<List<ChatMessageResponseDto>>(chatHistory);
        }

        public async Task<ChatMessageResponseDto> GetChatMessageById(string messageId)
        {
            var message = await _unitOfWork.ChatRepository.GetChatMessageById(messageId);
            if (message == null)
                throw new ErrorException(StatusCodes.Status404NotFound, ApiCodes.NOT_FOUND, "Message not found!");

            return _mapper.Map<ChatMessageResponseDto>(message);
        }

        public async Task<ChatRoomResponseDto> GetChatRoomById(string chatRoomId)
        {
            var chatRoom = await _unitOfWork.ChatRepository.GetChatRoomById(chatRoomId);
            if (chatRoom == null)
                throw new ErrorException(StatusCodes.Status404NotFound, ApiCodes.NOT_FOUND, "ChatRoom not found!");
            return _mapper.Map<ChatRoomResponseDto>(chatRoom);
        }

        public async Task<List<ChatRoomResponseDto>> GetUserChatRoom(string userId)
        {
            var chatRooms = await _unitOfWork.ChatRepository.GetUserChatRoom(userId);
            if (chatRooms == null)
                throw new ErrorException(StatusCodes.Status404NotFound, ApiCodes.NOT_FOUND, "User has no Chat room!");

            return _mapper.Map<List<ChatRoomResponseDto>>(chatRooms);
        }

        public async Task MarkMessageAsReadAsync(string messageId, string userId, string chatRoomId)
        {
            var message = await _unitOfWork.ChatRepository.GetChatMessageById(messageId);
            if (message == null)
                throw new ErrorException(StatusCodes.Status404NotFound, ApiCodes.NOT_FOUND, $"Message with id:{messageId} is not found!");
            if (message.IsRead)
                throw new ErrorException(StatusCodes.Status400BadRequest, ApiCodes.BAD_REQUEST, $"Message with id:{messageId} is read!");

            message.IsRead = true;
            await _unitOfWork.ChatRepository.UpdateMessageAsync(message);
        }
    }
}
