﻿using AutoMapper;
using MamaFit.BusinessObjects.DTO.ChatMessageDto;
using MamaFit.BusinessObjects.DTO.MaternityDressTaskDto;
using MamaFit.BusinessObjects.DTO.MeasurementDto;
using MamaFit.BusinessObjects.DTO.MilestoneDto;
using MamaFit.BusinessObjects.DTO.NotificationDto;
using MamaFit.BusinessObjects.DTO.OrderItemDto;
using MamaFit.BusinessObjects.DTO.OrderItemTaskDto;
using MamaFit.BusinessObjects.Entity;
using MamaFit.BusinessObjects.Enum;
using MamaFit.Repositories.Implement;
using MamaFit.Repositories.Interface;
using MamaFit.Services.ExternalService.Redis;
using MamaFit.Services.Hubs;
using MamaFit.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using System.Text.Json;

namespace MamaFit.Services.Service;

public class OrderItemTaskService : IOrderItemTaskService
{
    private readonly IOrderItemTaskRepository _repo;
    private readonly IMapper _mapper;
    private readonly IValidationService _validation;
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMilestoneService _milestoneService;
    private readonly ICacheService _cacheService;
    private readonly IChatService _chatService;
    private readonly INotificationService _notificationService;
    private readonly IHubContext<ChatHub> _chatHubContext;

    public OrderItemTaskService(IOrderItemTaskRepository repo, IMapper mapper, IValidationService validation,
        IHttpContextAccessor contextAccessor, IUnitOfWork unitOfWork, IMilestoneService milestoneService,
        IChatService chatService, INotificationService notificationService,
        IHubContext<ChatHub> chatHubContext,
        ICacheService cacheService)

    {
        _repo = repo;
        _mapper = mapper;
        _validation = validation;
        _contextAccessor = contextAccessor;
        _unitOfWork = unitOfWork;
        _milestoneService = milestoneService;
        _chatService = chatService;
        _notificationService = notificationService;
        _chatHubContext = chatHubContext;
        _cacheService = cacheService;
    }

    public async Task<List<OrderItemTaskGetByTokenResponse>> GetTasksByAssignedStaffAsync()
    {
        var userId = GetCurrentUserId();
        var orderItemTasks = await _repo.GetTasksByAssignedStaffAsync(userId);
        _validation.CheckNotFound(orderItemTasks, "No tasks found for the assigned staff");

        var result = orderItemTasks
            .Where(x => x.OrderItem != null)
            .GroupBy(x => x.OrderItem.Id)
            .Select(orderGroup =>
            {
                var representative = orderGroup.First();
                var orderItemId = representative.OrderItem.Id;

                var milestoneGroups = orderGroup
                    .Where(x => x.MaternityDressTask?.Milestone != null)
                    .GroupBy(x => x.MaternityDressTask.Milestone.Id)
                    .Select(milestoneGroup =>
                    {
                        var milestoneRep = milestoneGroup.First().MaternityDressTask.Milestone;

                        var totalTaskCount = milestoneRep.MaternityDressTasks.Count;
                        var doneTaskCount = milestoneRep.MaternityDressTasks
                            .Count(t => t.OrderItemTasks.Any(o =>
                                o.OrderItemId == orderItemId &&
                                (o.Status == OrderItemTaskStatus.DONE || o.Status == OrderItemTaskStatus.PASS)));

                        bool isQcMilestone = milestoneRep.Name != null &&
                                             ((milestoneRep.Name.Contains("qc", StringComparison.OrdinalIgnoreCase)
                                               || milestoneRep.Name.Contains("quality check",
                                                   StringComparison.OrdinalIgnoreCase))
                                              && !milestoneRep.ApplyFor.Contains(ItemType.QC_FAIL));

                        var qcTaskCount = milestoneRep.MaternityDressTasks
                            .Count(t => t.OrderItemTasks.Any(o =>
                                o.OrderItemId == orderItemId &&
                                (o.Status == OrderItemTaskStatus.FAIL || o.Status == OrderItemTaskStatus.PASS)));

                        float progress = isQcMilestone
                            ? (qcTaskCount == totalTaskCount && totalTaskCount > 0 ? 100 : 0)
                            : (totalTaskCount == 0 ? 0 : (float)doneTaskCount / totalTaskCount * 100);

                        return new MilestoneGetByIdOrderTaskResponseDto
                        {
                            Name = milestoneRep.Name,
                            Description = milestoneRep.Description,
                            ApplyFor = milestoneRep.ApplyFor,
                            SequenceOrder = milestoneRep.SequenceOrder,
                            Progress = progress,
                            MaternityDressTasks = milestoneGroup
                                .Select(orderItemTask =>
                                {
                                    var taskDto =
                                        _mapper.Map<MaternityDressTaskOrderTaskResponseDto>(orderItemTask
                                            .MaternityDressTask);
                                    taskDto.Status = orderItemTask.Status;
                                    taskDto.Note = orderItemTask.Note;
                                    taskDto.Image = orderItemTask.Image;
                                    taskDto.UpdatedAt = orderItemTask.UpdatedAt;
                                    return taskDto;
                                })
                                .ToList()
                        };
                    })
                    .OrderBy(m => m.SequenceOrder)
                    .ToList();

                return new OrderItemTaskGetByTokenResponse
                {
                    OrderItem = _mapper.Map<OrderItemResponseDto>(representative.OrderItem),
                    AddressId = representative.OrderItem.Order.AddressId,
                    Measurement = _mapper.Map<MeasurementResponseDto>(representative.OrderItem.Order.Measurement),
                    OrderCode = representative.OrderItem.Order.Code,
                    OrderStatus = representative.OrderItem.Order.Status,
                    Milestones = milestoneGroups
                };
            })
            .ToList();

        return result;
    }
    public async Task<List<OrderItemTaskGetByTokenResponse>> GetTasksByOrderItemIdAsync(string orderItemId)
    {
        var listOrderItemTask = await GetTasksByAssignedStaffAsync();

        var response = listOrderItemTask.Where(x => x.OrderItem.Id == orderItemId).ToList();
        _validation.CheckNotFound(response, $"OrderItemTask with OrderItemId: {orderItemId} is not found");

        return response;
    }
    public async Task UpdateStatusAsync(string dressTaskId, string orderItemId, OrderItemTaskUpdateRequestDto request,
        bool? severity)
    {
        var cacheKey = "MilestoneAchiveOrderItemResponseDto";

        //Lấy Task & Validate
        var task = await _repo.GetByIdAsync(dressTaskId, orderItemId);
        _validation.CheckNotFound(task, "Order item task not found");

        var orderItem = await _unitOfWork.OrderItemRepository.GetDetailById(orderItemId);
        _validation.CheckNotFound(orderItem, $"OrderItem with id: {orderItemId} not found");

        var order = task.OrderItem!.Order;

        //Update thông tin Task
        task.Note = request.Note ?? task.Note;
        task.Status = request.Status;
        task.Image = request.Image ?? task.Image;

        await _repo.UpdateAsync(task);
        await _cacheService.RemoveByPrefixAsync(cacheKey);

        //Lấy progress & milestones để xử lý status
        var progress = await _milestoneService.GetMilestoneByOrderItemId(orderItemId);
        var milestones = await _unitOfWork.MilestoneRepository.GetAllWithInclude();

        //Điều hướng theo Status
        switch (request.Status)
        {
            case OrderItemTaskStatus.IN_PROGRESS:
                await HandleInProgressAsync(task, orderItem, order);
                break;

            case OrderItemTaskStatus.DONE:
                await HandleDoneAsync(orderItem, order, progress, milestones, task);
                break;

            case OrderItemTaskStatus.PASS:
                await HandlePassAsync(order, progress, task);
                break;

            case OrderItemTaskStatus.FAIL:
                await HandleFailAsync(orderItem, order, milestones, progress, severity, task);
                break;
        }
    }
    private async Task HandleInProgressAsync(OrderItemTask task, OrderItem orderItem, Order order)
    {
        if (task.MaternityDressTask.Milestone.SequenceOrder == 1)
        {
            if (orderItem.ItemType == ItemType.DESIGN_REQUEST)
            {
                order.Status = OrderStatus.IN_PROGRESS;
                await SendMessageAndNoti(task);
            }
            else if (orderItem.ItemType == ItemType.PRESET)
            {
                order.Status = OrderStatus.IN_PROGRESS;
            }
        }

        await _unitOfWork.OrderRepository.UpdateAsync(order);
        await _unitOfWork.SaveChangesAsync();
    }
    private async Task HandleDoneAsync(OrderItem orderItem, Order order,
        List<MilestoneAchiveOrderItemResponseDto> progress, IEnumerable<Milestone> milestones,  OrderItemTask currentTask)
    {
        if (orderItem.ItemType == ItemType.DESIGN_REQUEST)
        {
            order.Status = OrderStatus.COMPLETED;
        }
        else if (orderItem.ItemType == ItemType.PRESET || orderItem.ItemType == ItemType.WARRANTY)
        {
            await UpdateStatusForPresetDoneAsync(order, progress);
            // var qcFailProgress = progress.Where(x => x.Milestone.ApplyFor.Contains(ItemType.QC_FAIL));
            // if (qcFailProgress.Any(x => x.Progress == 100 && x.IsDone))
            // {
            //     if (CheckAllItemInLastMilestone(order, milestones))
            //     {
            //         if (order.PaymentType == PaymentType.FULL && order.PaymentStatus == PaymentStatus.PAID_FULL)
            //         {
            //             order.Status = OrderStatus.PACKAGING;
            //         }
            //         else if (order.PaymentType == PaymentType.DEPOSIT &&
            //                  order.PaymentStatus == PaymentStatus.PAID_DEPOSIT)
            //         {
            //             order.Status = OrderStatus.AWAITING_PAID_REST;
            //         }
            //     }
            //     else
            //     {
            //         order.Status = OrderStatus.IN_PROGRESS;
            //     }
            // }
            await UpdateOrderStatusByAllItemsAsync(order, currentTask);
        }

        await _unitOfWork.OrderRepository.UpdateAsync(order);
        await _unitOfWork.SaveChangesAsync();
    }
    private async Task UpdateStatusForPresetDoneAsync(Order order, List<MilestoneAchiveOrderItemResponseDto> progress)
    {
        var presetProgress = progress.Where(x => x.Milestone.ApplyFor.Contains(ItemType.PRESET));
        if (!presetProgress.Any(x => x.Progress == 100 && x.IsDone)) return;

        var hasAddOn = progress.Where(x => x.Milestone.ApplyFor.Contains(ItemType.ADD_ON));
        if (hasAddOn.Any())
        {
            // Nếu có Add-on
            if (hasAddOn.Any(x => x.Progress == 100 && x.IsDone))
            {
                // await UpdateStatusWithQCAsync(order, progress);
                order.Status = OrderStatus.IN_PROGRESS;
            }
        }
        else
        {
            // Không có Add-on
            // await UpdateStatusWithoutAddOnAsync(order, progress);
        }
    }
    // private async Task UpdateStatusWithQCAsync(Order order, List<MilestoneAchiveOrderItemResponseDto> progress)
    // {
    //     var packageProgress = progress.OrderByDescending(x => x.Milestone.SequenceOrder).FirstOrDefault();
    //     var keywordList = new[] { "quality check", "qc" };
    //     var qcProgress = progress.Where(x => keywordList.Any(k => x.Milestone!.Name!.ToLower().Contains(k)));
    //
    //     if (packageProgress?.Progress == 100 && packageProgress.IsDone)
    //     {
    //         order.Status = OrderStatus.PACKAGING;
    //        
    //     }
    //     else if (qcProgress.Any(x => x.Progress == 100))
    //         order.Status = OrderStatus.PACKAGING;
    //     else
    //         order.Status = OrderStatus.IN_PROGRESS;
    // }

    // private async Task UpdateStatusWithoutAddOnAsync(Order order, List<MilestoneAchiveOrderItemResponseDto> progress)
    // {
    //     var packageProgress = progress.OrderByDescending(x => x.Milestone.SequenceOrder).FirstOrDefault();
    //     if (packageProgress?.Progress == 100 && packageProgress.IsDone)
    //     {
    //         order.Status = OrderStatus.PACKAGING;
    //     }
    //     else
    //         order.Status = OrderStatus.IN_PROGRESS;
    // }
    private async Task HandlePassAsync(Order order, List<MilestoneAchiveOrderItemResponseDto> progress, OrderItemTask currentTask)
    {
        if (order.Type == OrderType.WARRANTY)
        {
            var in_warrantyKey = new[] { "in warranty" };
            var in_warrantyProgess =
                progress.Where(x => in_warrantyKey.Any(k => x.Milestone!.Name!.ToLower().Contains(k)));

            if (in_warrantyProgess.Any(x => x.Progress == 100))
            {
                order.Status = OrderStatus.IN_PROGRESS;
            }
            else
            {
                var warrantyKey = new[] { "validation" };
                var warrantyProgess =
                    progress.Where(x => warrantyKey.Any(k => x.Milestone!.Name!.ToLower().Contains(k)));

                if (warrantyProgess.Any(x => x.Progress == 100))
                {
                    order.Status = OrderStatus.IN_PROGRESS;
                }
            }
        }

        // var keywordList = new[] { "quality check", "qc" };
        // var qcProgress = progress.Where(x => keywordList.Any(k => x.Milestone!.Name!.ToLower().Contains(k)));
        //
        // if (qcProgress.Any(x => x.Progress == 100))
        // {
        //     var keyword = new[] { "fail" };
        //     var qcFailProgress = progress.Where(x => keyword.Any(k => x.Milestone!.Name!.ToLower().Contains(k)));
        //
        //     if (qcFailProgress.Any(x => x.Progress == 100 && x.IsDone))
        //     {
        //         if (order.PaymentType == PaymentType.FULL && order.PaymentStatus == PaymentStatus.PAID_FULL)
        //         {
        //             order.Status = OrderStatus.PACKAGING;
        //         }
        //         else if (order.PaymentType == PaymentType.DEPOSIT && order.PaymentStatus == PaymentStatus.PAID_DEPOSIT)
        //         {
        //             order.Status = OrderStatus.AWAITING_PAID_REST;
        //         }
        //     }
        //
        //     if (!qcFailProgress.Any())
        //     {
        //         if (order.PaymentType == PaymentType.FULL && (order.PaymentStatus == PaymentStatus.PAID_FULL ||
        //                                                       order.PaymentStatus == PaymentStatus.WARRANTY))
        //         {
        //             order.Status = OrderStatus.PACKAGING;
        //         }
        //         else if (order.PaymentType == PaymentType.DEPOSIT && order.PaymentStatus == PaymentStatus.PAID_DEPOSIT)
        //         {
        //             order.Status = OrderStatus.AWAITING_PAID_REST;
        //         }
        //     }
        // }
        await UpdateOrderStatusByAllItemsAsync(order, currentTask);
        await _unitOfWork.OrderRepository.UpdateAsync(order);
        await _unitOfWork.SaveChangesAsync();
    }
    private async Task HandleFailAsync(
        OrderItem orderItem,
        Order order,
        IEnumerable<Milestone> milestones,
        List<MilestoneAchiveOrderItemResponseDto> progress,
        bool? severity,
        OrderItemTask task)
    {
        var keywordList = new[] { "fail" };
        var qcProgress = progress.Where(x => keywordList.Any(k => x.Milestone!.Name!.ToLower().Contains(k)));
        if (task.MaternityDressTask.Milestone.Name.ToLower().Contains("validation"))
        {
            order.Status = OrderStatus.RETURNED;
            //  xóa task trong orderitem trừ warranty check
            await _unitOfWork.OrderRepository.UpdateAsync(order);
            await _unitOfWork.SaveChangesAsync();
            return;
        }

        if (severity == true) // Task nặng -> reset progress
        {
            if (milestones.Any(x =>
                    x.ApplyFor.Contains(ItemType.WARRANTY) && x.Name.ToLower().Contains("quality check warranty")))
            {
                order.Status = OrderStatus.IN_PROGRESS;
            }
            else
            {
                order.Status = OrderStatus.IN_PROGRESS;
            }

            foreach (var t in orderItem.OrderItemTasks)
                t.Status = OrderItemTaskStatus.PENDING;
        }

        if (severity == false) // Task nhẹ -> assign task mới
        {
            var qcFailTasks = milestones.SelectMany(x => x.MaternityDressTasks).Where(x =>
                keywordList.Any(k => x.Milestone.ApplyFor!.Contains(ItemType.QC_FAIL)));
            foreach (var failTask in qcFailTasks)
            {
                // Kiểm tra xem đã tồn tại OrderItemTask cho MaternityDressTask này chưa
                var existingTask = orderItem.OrderItemTasks
                    .FirstOrDefault(t => t.MaternityDressTaskId == failTask.Id);

                if (existingTask != null)
                {
                    // Nếu đã có Note thì nối thêm chuỗi, nếu chưa thì gán mới
                    if (string.IsNullOrEmpty(existingTask.Note))
                        existingTask.Note = task.MaternityDressTask.Name;
                    else
                        existingTask.Note += "|" + task.MaternityDressTask.Name;

                    existingTask.UpdatedAt = DateTime.UtcNow;
                    existingTask.Status = OrderItemTaskStatus.PENDING;
                }
                else
                {
                    // Nếu chưa có task thì tạo mới
                    orderItem.OrderItemTasks.Add(new OrderItemTask
                    {
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                        MaternityDressTask = failTask,
                        MaternityDressTaskId = failTask.Id,
                        CreatedBy = "System",
                        Note = task.MaternityDressTask.Name,
                        OrderItem = orderItem,
                        OrderItemId = orderItem.Id,
                        Status = OrderItemTaskStatus.PENDING,
                    });
                }
            }
        }

        await _unitOfWork.OrderRepository.UpdateAsync(order);
        await _unitOfWork.SaveChangesAsync();
    }
    private string GetCurrentUserId()
    {
        return _contextAccessor.HttpContext.User.FindFirst("userId").Value ?? "System";
    }
    private async Task SendMessageAndNoti(OrderItemTask task)
    {
        var designerId = GetCurrentUserId();
        var customerId = task.OrderItem!.Order!.UserId;

        if (!string.IsNullOrEmpty(customerId) && !string.IsNullOrEmpty(designerId))
        {
            try
            {
                // Tạo chat room giữa designer và khách hàng
                var chatRoom = await _chatService.CreateChatRoomAsync(designerId, customerId);

                // Tạo object JSON đơn giản với 3 fields theo yêu cầu
                var messageData = new
                {
                    messageContent =
                        "Xin chào bạn, tôi là nhân viên thiết kế đầm bầu của MamaFit, hôm nay tôi sẽ cùng bạn thiết kế chiếc đầm bầu thật đẹp nhé bạn yêu !!!",
                    OrderId = task.OrderItem.Order.Id,
                    DesignRequestId = task.OrderItem.DesignRequest?.Id ?? ""
                };

                // Serialize JSON với options đúng để giữ Unicode
                var jsonMessage = JsonSerializer.Serialize(messageData, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                    WriteIndented = false
                });

                var welcomeMessage = new ChatMessageCreateDto()
                {
                    SenderId = designerId,
                    ChatRoomId = chatRoom.Id,
                    Message = jsonMessage,
                    Type = MessageType.Design_Request
                };

                var sentMessage = await _chatService.CreateChatMessageAsync(welcomeMessage);

                // Gửi tin nhắn đến customer qua SignalR
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

                // Gửi thông báo đến khách hàng qua SignalR
                await _chatHubContext.Clients.User(customerId)
                    .SendAsync("InvitedToRoom", chatRoom.Id);

                // Tạo và gửi notification
                await _notificationService.SendAndSaveNotificationAsync(new NotificationRequestDto()
                {
                    ReceiverId = customerId,
                    NotificationTitle = "Designer đã bắt đầu làm việc trên thiết kế của bạn",
                    NotificationContent =
                        $"Designer đã bắt đầu xử lý yêu cầu thiết kế của bạn. Bạn có thể trò chuyện trực tiếp với designer để thảo luận chi tiết.",
                    Type = NotificationType.ORDER_PROGRESS,
                    ActionUrl = $"/chat/{chatRoom.Id}",
                    Metadata = new Dictionary<string, string>
                    {
                        { "orderId", task.OrderItem.Order.Id },
                        { "orderItemId", task.OrderItemId! },
                        { "dressTaskId", task.MaternityDressTaskId! },
                        { "chatRoomId", chatRoom.Id }
                    }
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating chat room or sending notification: {ex.Message}");
            }
        }
    }
    // private bool CheckAllItemInLastMilestone(Order order, IEnumerable<Milestone> milestones)
    // {
    //     var lastMilestone = milestones.OrderByDescending(x => x.SequenceOrder).FirstOrDefault();
    //
    //     if (order.OrderItems.All(x => x.OrderItemTasks.Select(x => x.MaternityDressTask.Milestone) == lastMilestone))
    //     {
    //         return true;
    //     }
    //
    //     return false;
    // }

    //help me time
    private static bool NameLike(string? name, params string[] keywords)
    {
        if (string.IsNullOrWhiteSpace(name)) return false;
        return keywords.Any(k => name.Contains(k, StringComparison.OrdinalIgnoreCase));
    }
    private static bool ItemReadyForPackaging(List<MilestoneAchiveOrderItemResponseDto> progress)
    {
        // QC fail progress (highest priority)
        var qcFailReady = progress.Any(x =>
            x.Milestone.ApplyFor.Contains(ItemType.QC_FAIL) &&
            x.Progress == 100 && x.IsDone);

        // QC progress (quality check)
        var qcReady = progress.Any(x =>
            NameLike(x.Milestone?.Name, "quality check", "qc") &&
            !x.Milestone.ApplyFor.Contains(ItemType.QC_FAIL) && // Exclude QC_FAIL milestones
            x.Progress == 100);

        return qcFailReady || qcReady;
    }
    // Chỉ xét các item có sản xuất/thẩm định (PRESET/WARRANTY/...)
    // Nếu bạn dùng ADD_ON như một OrderItem độc lập thì thêm vào filter dưới đây
    private static bool IsProductionItem(OrderItem oi) =>
        oi.ItemType == ItemType.PRESET || oi.ItemType == ItemType.WARRANTY;
    private async Task<bool> AreAllItemsReadyForPackagingAsync(Order order)
    {
        var itemIds = order.OrderItems
            .Where(IsProductionItem)
            .Select(oi => oi.Id)
            .ToList();

        if (itemIds.Count == 0) return false; // không có item cần QC

        var tasks = itemIds.Select(id => _milestoneService.GetMilestoneByOrderItemId(id));
        var allProgress = await Task.WhenAll(tasks);

        foreach (var p in allProgress)
            if (!ItemReadyForPackaging(p))
                return false;

        return true;
    }
    private async Task UpdateOrderStatusByAllItemsAsync(Order order, OrderItemTask currentTask)
    {
        var currentMilestone = currentTask.MaternityDressTask?.Milestone;
        var currentMilestoneName = currentMilestone?.Name?.ToLower() ?? "";

        // Kiểm tra xem task hiện tại có thuộc milestone packaging không
        bool isPackagingMilestone = NameLike(currentMilestoneName, "packing", "pack", "gói");

        // Nếu đang ở PACKAGING và task hiện tại thuộc về packaging phase, giữ nguyên status
        if (order.Status == OrderStatus.PACKAGING && isPackagingMilestone)
        {
            // Đang trong phase packaging, giữ nguyên status PACKAGING
            return;
        }

        // Nếu order đang AWAITING_PAID_REST, không thay đổi status
        if (order.Status == OrderStatus.AWAITING_PAID_REST)
        {
            return;
        }

        // Logic kiểm tra tất cả items ready for packaging
        var ready = await AreAllItemsReadyForPackagingAsync(order);

        if (!ready)
        {
            // Chỉ set IN_PROGRESS nếu order chưa ở phase cao hơn
            if (order.Status != OrderStatus.PACKAGING && order.Status != OrderStatus.COMPLETED)
            {
                order.Status = OrderStatus.IN_PROGRESS;
            }
            return;
        }

        // Tất cả items ready, check điều kiện thanh toán để quyết định status
        if (order.PaymentType == PaymentType.FULL && (order.PaymentStatus == PaymentStatus.PAID_FULL  || order.PaymentStatus == PaymentStatus.WARRANTY))
        {
            order.Status = OrderStatus.PACKAGING;
        }
        else if (order.PaymentType == PaymentType.DEPOSIT && order.PaymentStatus == PaymentStatus.PAID_DEPOSIT)
        {
            order.Status = OrderStatus.AWAITING_PAID_REST;
        }else if (order.PaymentStatus == PaymentStatus.PAID_DEPOSIT_COMPLETED)
        {
            order.Status = OrderStatus.PACKAGING;
        }
        else
        {
            order.Status = OrderStatus.IN_PROGRESS;
        }
    }
}