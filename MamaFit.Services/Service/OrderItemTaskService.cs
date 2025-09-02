using AutoMapper;
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
using System.Globalization;
using System.Text;

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
    private readonly IHubContext<NotificationHub> _notificationHubContext;
    private readonly IEmailSenderSevice _emailSenderService;

    public OrderItemTaskService(IOrderItemTaskRepository repo, IMapper mapper, IValidationService validation,
        IHttpContextAccessor contextAccessor, IUnitOfWork unitOfWork, IMilestoneService milestoneService,
        IChatService chatService, INotificationService notificationService,
        IHubContext<ChatHub> chatHubContext, IHubContext<NotificationHub> notificationHubContext,
        ICacheService cacheService, IEmailSenderSevice emailSenderService)

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
        _notificationHubContext = notificationHubContext;
        _cacheService = cacheService;
        _emailSenderService = emailSenderService;
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
                                    taskDto.Deadline = orderItemTask.Deadline;
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

        // Kiểm tra và gửi mail chỉ sau khi tất cả xử lý hoàn tất
        // Chỉ kiểm tra với DONE và PASS vì đây là 2 status có thể dẫn đến hoàn thành milestone
        if (request.Status == OrderItemTaskStatus.DONE || request.Status == OrderItemTaskStatus.PASS)
        {
            // Reload order để có đầy đủ thông tin mới nhất
            var refreshedOrder = await _unitOfWork.OrderRepository.GetWithItemsAndDressDetails(order.Id);
            if (refreshedOrder != null)
            {
                // CẬP NHẬT ORDER STATUS TRƯỚC KHI CHECK COMPLETION (dựa trên ready for packaging)
                await UpdateOrderStatusByAllItemsAsync(refreshedOrder, task);
                
                // Sau khi update status, check xem TẤT CẢ MILESTONE PACKING đã DONE chưa để gửi email
                var isOrderCompleted = await IsOrderFullyCompletedAsync(refreshedOrder);
                
                if (isOrderCompleted)
                {
                    Console.WriteLine($"[INFO] Order {refreshedOrder.Code} is fully completed (all packing milestones done) after {request.Status}, sending notification and email...");
                    await SendOrderCompletedNotificationAndEmailAsync(refreshedOrder);
                }
                else
                {
                    Console.WriteLine($"[INFO] Order {refreshedOrder.Code} not fully completed yet after {request.Status}. Status: {refreshedOrder.Status}");
                }
            }
            else
            {
                Console.WriteLine($"[WARNING] Could not reload order {order.Id} for completion check");
            }
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
            else if (orderItem.ItemType == ItemType.PRESET || orderItem.ItemType == ItemType.READY_TO_BUY)
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
        else if (orderItem.ItemType == ItemType.PRESET || orderItem.ItemType == ItemType.WARRANTY || orderItem.ItemType == ItemType.READY_TO_BUY)
        {
            await UpdateStatusForPresetDoneAsync(order, progress);
            await UpdateOrderStatusByAllItemsAsync(order, currentTask);
        }

        await _unitOfWork.OrderRepository.UpdateAsync(order);
        await _unitOfWork.SaveChangesAsync();

    }
    private async Task UpdateStatusForPresetDoneAsync(Order order, List<MilestoneAchiveOrderItemResponseDto> progress)
    {
        // Kiểm tra PRESET progress
        var presetProgress = progress.Where(x => x.Milestone.ApplyFor.Contains(ItemType.PRESET));
        if (!presetProgress.Any(x => x.Progress == 100 && x.IsDone)) return;

        // Kiểm tra READY_TO_BUY progress
        var readyToBuyProgress = progress.Where(x => x.Milestone.ApplyFor.Contains(ItemType.READY_TO_BUY));
        if (readyToBuyProgress.Any() && !readyToBuyProgress.Any(x => x.Progress == 100 && x.IsDone)) return;

        var hasAddOn = progress.Where(x => x.Milestone.ApplyFor.Contains(ItemType.ADD_ON));
        if (hasAddOn.Any())
        {
            // Nếu có Add-on
            if (hasAddOn.Any(x => x.Progress == 100 && x.IsDone))
            {
                order.Status = OrderStatus.IN_PROGRESS;
            }
        }
        else
        {
            // Không có Add-on
            order.Status = OrderStatus.IN_PROGRESS;
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

        await UpdateOrderStatusByAllItemsAsync(order, currentTask);
        await _unitOfWork.OrderRepository.UpdateAsync(order);
        await _unitOfWork.SaveChangesAsync();
        
        Console.WriteLine($"[INFO] Task PASS processed for order {order.Code}. Status: {order.Status}");
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

        // QC progress (quality check) - áp dụng cho tất cả item types
        var qcReady = progress.Any(x =>
            NameLike(x.Milestone?.Name, "quality check", "qc") &&
            !x.Milestone.ApplyFor.Contains(ItemType.QC_FAIL) && // Exclude QC_FAIL milestones
            x.Progress == 100);

        // Đặc biệt cho READY_TO_BUY - có thể ready ngay sau khi hoàn thành milestone chính
        var readyToBuyReady = progress.Any(x =>
            x.Milestone.ApplyFor.Contains(ItemType.READY_TO_BUY) &&
            x.Progress == 100 && x.IsDone);

        var result = qcFailReady || qcReady || readyToBuyReady;
        
        Console.WriteLine($"[DEBUG] ItemReadyForPackaging result: {result} (qcFailReady: {qcFailReady}, qcReady: {qcReady}, readyToBuyReady: {readyToBuyReady})");
        
        if (!result)
        {
            Console.WriteLine($"[DEBUG] Progress details:");
            foreach (var p in progress)
            {
                Console.WriteLine($"[DEBUG] - Milestone: {p.Milestone?.Name}, ApplyFor: {string.Join(",", p.Milestone?.ApplyFor ?? new List<ItemType>())}, Progress: {p.Progress}%, IsDone: {p.IsDone}");
            }
        }
        
        return result;
    }
    // Chỉ xét các item có sản xuất/thẩm định (PRESET/WARRANTY/READY_TO_BUY)
    // Nếu bạn dùng ADD_ON như một OrderItem độc lập thì thêm vào filter dưới đây
    private static bool IsProductionItem(OrderItem oi) =>
        oi.ItemType == ItemType.PRESET || oi.ItemType == ItemType.WARRANTY || oi.ItemType == ItemType.READY_TO_BUY;
    private async Task<bool> AreAllItemsReadyForPackagingAsync(Order order)
    {
        var itemIds = order.OrderItems
            .Where(IsProductionItem)
            .Select(oi => oi.Id)
            .ToList();

        Console.WriteLine($"[DEBUG] Checking packaging readiness for order {order.Code}, production items count: {itemIds.Count}");

        if (itemIds.Count == 0) 
        {
            Console.WriteLine($"[DEBUG] No production items found for order {order.Code}");
            return false; // không có item cần QC
        }

        var tasks = itemIds.Select(id => _milestoneService.GetMilestoneByOrderItemId(id));
        var allProgress = await Task.WhenAll(tasks);

        for (int i = 0; i < allProgress.Length; i++)
        {
            var itemProgress = allProgress[i];
            var itemId = itemIds[i];
            var ready = ItemReadyForPackaging(itemProgress);
            
            if (!ready)
            {
                return false;
            }
        }

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



    /// <summary>
    /// Kiểm tra xem toàn bộ ORDER (tất cả orderItem) đã hoàn thành chưa
    /// </summary>
    private async Task<bool> IsOrderFullyCompletedAsync(Order order)
    {
        try
        {
            // Kiểm tra xem tất cả production items đã hoàn thành milestone Packing chưa
            var packingCompleted = await AreAllPackingMilestonesCompletedAsync(order);
            Console.WriteLine($"[DEBUG] AreAllPackingMilestonesCompletedAsync result: {packingCompleted}");
            
            if (!packingCompleted)
            {
                Console.WriteLine($"[DEBUG] Order {order.Code} not fully completed - packing milestones not completed");
                return false;
            }
            
            // Kiểm tra payment condition để đảm bảo order có thể chuyển sang PACKAGING
            bool paymentReady = false;
            if (order.PaymentType == PaymentType.FULL && (order.PaymentStatus == PaymentStatus.PAID_FULL || order.PaymentStatus == PaymentStatus.WARRANTY))
            {
                paymentReady = true;
            }
            else if (order.PaymentType == PaymentType.DEPOSIT && order.PaymentStatus == PaymentStatus.PAID_DEPOSIT_COMPLETED)
            {
                paymentReady = true;
            }
            
            var result = packingCompleted && paymentReady;
            Console.WriteLine($"[DEBUG] Final completion check for order {order.Code}: packingCompleted={packingCompleted}, paymentReady={paymentReady}, result={result}");
            
            return result;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ERROR] Error checking order completion: {ex.Message}");
            return false;
        }
    }

    /// <summary>
    /// Kiểm tra tất cả milestone Packing của order đã hoàn thành chưa
    /// </summary>
    private async Task<bool> AreAllPackingMilestonesCompletedAsync(Order order)
    {
        var itemIds = order.OrderItems
            .Where(IsProductionItem)
            .Select(oi => oi.Id)
            .ToList();

        Console.WriteLine($"[DEBUG] Checking packing completion for order {order.Code}, production items count: {itemIds.Count}");

        if (itemIds.Count == 0) 
        {
            Console.WriteLine($"[DEBUG] No production items found for order {order.Code}");
            return false; // không có item cần kiểm tra
        }

        var tasks = itemIds.Select(id => _milestoneService.GetMilestoneByOrderItemId(id));
        var allProgress = await Task.WhenAll(tasks);

        for (int i = 0; i < allProgress.Length; i++)
        {
            var itemProgress = allProgress[i];
            var itemId = itemIds[i];
            var packingCompleted = IsPackingMilestoneCompleted(itemProgress);
            Console.WriteLine($"[DEBUG] Item {itemId} packing milestone completed: {packingCompleted}");
            
            if (!packingCompleted)
            {
                Console.WriteLine($"[DEBUG] Order {order.Code} not ready - item {itemId} packing milestone not completed");
                return false;
            }
        }

        Console.WriteLine($"[DEBUG] All packing milestones completed for order {order.Code}");
        return true;
    }

    /// <summary>
    /// Kiểm tra milestone Packing của 1 item đã hoàn thành chưa (tất cả task trong milestone Packing đều DONE)
    /// </summary>
    private static bool IsPackingMilestoneCompleted(List<MilestoneAchiveOrderItemResponseDto> progress)
    {
        // Tìm milestone có tên chứa "packing", "pack", "gói", "đóng gói"
        var packingMilestone = progress.FirstOrDefault(x =>
            NameLike(x.Milestone?.Name, "packing", "pack", "gói", "đóng gói"));

        if (packingMilestone == null)
        {
            Console.WriteLine($"[DEBUG] No packing milestone found for this item");
            return false; // Không có milestone packing
        }

        // Kiểm tra milestone packing đã 100% và IsDone = true
        var isCompleted = packingMilestone.Progress == 100 && packingMilestone.IsDone;
        
        Console.WriteLine($"[DEBUG] Packing milestone '{packingMilestone.Milestone?.Name}' - Progress: {packingMilestone.Progress}%, IsDone: {packingMilestone.IsDone}, Result: {isCompleted}");
        
        return isCompleted;
    }

    /// <summary>
    /// Gửi thông báo và email khi toàn bộ ORDER hoàn thành
    /// </summary>
    private async Task SendOrderCompletedNotificationAndEmailAsync(Order order)
    {
        try
        {
            // Kiểm tra trong cache xem đã gửi email cho order này chưa
            var cacheKey = $"order_completed_email_sent_{order.Id}";
            var alreadySent = await _cacheService.GetAsync<bool>(cacheKey);
            
            if (alreadySent)
            {
                Console.WriteLine($"[INFO] Email for order {order.Code} already sent, skipping...");
                return;
            }

            // Tạo danh sách người nhận: User + tất cả Manager
            var receiverIds = new List<string>();
            
            // 1. Thêm User vào danh sách
            if (!string.IsNullOrEmpty(order.UserId))
            {
                receiverIds.Add(order.UserId);
            }
            
            // 2. Thêm tất cả Manager vào danh sách
            var managers = await GetAllManagersAsync();
            receiverIds.AddRange(managers.Select(m => m.Id));

            // Gửi thông báo cho tất cả (User + Managers)
            if (receiverIds.Any())
            {
                await _notificationService.SendAndSaveNotificationToMultipleAsync(new NotificationMultipleRequestDto
                {
                    ReceiverIds = receiverIds,
                    NotificationTitle = "Đơn hàng hoàn thành sản xuất",
                    NotificationContent = $"Đơn hàng {order.Code} đã hoàn thành tất cả các công đoạn sản xuất và sẵn sàng đóng gói.",
                    Type = NotificationType.ORDER_PROGRESS,
                    Metadata = new Dictionary<string, string>
                    {
                        { "orderId", order.Id },
                        { "orderCode", order.Code ?? "" },
                        { "status", "PRODUCTION_COMPLETED" }
                    }
                });
            }

            // Gửi email riêng cho User và Manager
            if (!string.IsNullOrEmpty(order.UserId))
            {
                await SendOrderCompletedEmailToUserAsync(order);
            }
            
            await SendOrderCompletedEmailToManagersAsync(order);
            
            // Đánh dấu đã gửi email cho order này (cache 24 giờ)
            await _cacheService.SetAsync(cacheKey, true, TimeSpan.FromHours(24));
            
            Console.WriteLine($"[INFO] Successfully sent completion notification and email for order {order.Code}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ERROR] Error sending order completion notification: {ex.Message}");
        }
    }

    /// <summary>
    /// Lấy tất cả Manager (theo role name)
    /// </summary>
    private async Task<List<ApplicationUser>> GetAllManagersAsync()
    {
        // Lấy manager theo role name
        var managerRoleNames = new[] { "Manager", "Admin", "Production Manager" };
        var managers = new List<ApplicationUser>();
        
        foreach (var roleName in managerRoleNames)
        {
            try
            {
                var role = await _unitOfWork.RoleRepository.GetByNameAsync(roleName);
                if (role != null)
                {
                    var usersInRole = await _unitOfWork.UserRepository.GetUsersByRoleIdAsync(role.Id, true);
                    managers.AddRange(usersInRole);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting users for role {roleName}: {ex.Message}");
            }
        }
        
        // Remove duplicates
        return managers.GroupBy(m => m.Id).Select(g => g.First()).ToList();
    }

    /// <summary>
    /// Lấy tên sản phẩm từ OrderItem
    /// </summary>
    private string GetProductName(OrderItem orderItem)
    {
        return orderItem.MaternityDressDetail?.MaternityDress?.Name ?? 
               orderItem.Preset?.Name ?? 
               (orderItem.DesignRequest != null ? "Yêu cầu thiết kế" : "Sản phẩm");
    }

    /// <summary>
    /// Gửi email thông báo hoàn thành sản xuất cho User
    /// </summary>
    private async Task SendOrderCompletedEmailToUserAsync(Order order)
    {
        if (order.User?.UserEmail == null) return;

        var subject = $"[MamaFit] Đơn hàng {order.Code} đã hoàn thành sản xuất";
        var htmlContent = BuildOrderCompletedEmailHtml(order, "USER");
        
        await _emailSenderService.SendEmailAsync(order.User.UserEmail, subject, htmlContent);
    }

    /// <summary>
    /// Gửi email thông báo hoàn thành sản xuất cho Manager
    /// </summary>
    private async Task SendOrderCompletedEmailToManagersAsync(Order order)
    {
        var managers = await GetAllManagersAsync();
        var managersWithEmail = managers.Where(m => !string.IsNullOrEmpty(m.UserEmail)).ToList();
        
        if (!managersWithEmail.Any()) return;

        var subject = $"[MamaFit] Đơn hàng {order.Code} hoàn thành sản xuất";
        var htmlContent = BuildOrderCompletedEmailHtml(order, "MANAGER");
        
        foreach (var manager in managersWithEmail)
        {
            await _emailSenderService.SendEmailAsync(manager.UserEmail!, subject, htmlContent);
        }
    }

    /// <summary>
    /// Tạo nội dung HTML cho email thông báo hoàn thành sản xuất
    /// </summary>
    private string BuildOrderCompletedEmailHtml(Order order, string receiverType)
    {
        var vn = new CultureInfo("vi-VN");
        
        var preheader = receiverType == "USER" 
            ? $"Đơn hàng {order.Code} đã hoàn thành sản xuất"
            : $"Cần xử lý giao hàng - Đơn {order.Code} đã hoàn thành sản xuất";

        var greeting = receiverType == "USER" 
            ? $"Xin chào <strong>{order.User?.FullName ?? "Quý khách"}</strong>,"
            : "Xin chào <strong>Quý anh/chị</strong>,";

        var content = receiverType == "USER"
            ? $"Chúng tôi vui mừng thông báo rằng đơn hàng <strong>{order.Code}</strong> đã hoàn thành tất cả các công đoạn sản xuất."
            : $"Đơn hàng <strong>{order.Code}</strong> đã hoàn thành tất cả các công đoạn sản xuất và sẵn sàng cho bước đóng gói và giao hàng.";

        var nextSteps = receiverType == "USER"
            ? "Chúng tôi sẽ tiến hành đóng gói và giao hàng cho bạn trong thời gian sớm nhất."
            : "Vui lòng tiến hành đóng gói và chuẩn bị giao hàng cho khách hàng. Kiểm tra thông tin địa chỉ giao hàng và liên hệ với khách hàng nếu cần thiết.";

        // Tạo danh sách sản phẩm
        var itemsHtml = new StringBuilder();
        if (order.OrderItems != null && order.OrderItems.Any())
        {
            foreach (var item in order.OrderItems)
            {
                var productName = GetProductName(item);
                itemsHtml.Append($@"
            <tr>
                <td style=""padding:8px 0"">{productName}</td>
                <td style=""padding:8px 0; text-align:center"">{item.Quantity}</td>
                <td style=""padding:8px 0; text-align:right"">{item.Price.ToString("c0", vn)}</td>
            </tr>");
            }
        }

        return $@"
    <!DOCTYPE html>
    <html lang=""vi"">
    <head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>Hoàn thành sản xuất</title>
    <style>
    body {{ font-family: Arial, Helvetica, sans-serif; background:#f7f7f7; margin:0; padding:0; }}
    .container {{ max-width: 600px; margin:40px auto; background:#fff; border-radius:8px; box-shadow:0 2px 10px rgba(0,0,0,0.05); padding:24px; }}
    .brand {{ font-size:22px; font-weight:bold; color:#2266cc; text-align:center; margin-bottom:6px; }}
    .sub {{ text-align:center; color:#666; margin-bottom:16px; }}
    .section-title {{ font-size:16px; font-weight:bold; margin:18px 0 8px; }}
    .table {{ width:100%; border-collapse:collapse; }}
    .table th, .table td {{ border-bottom:1px solid #eee; padding:8px 0; font-size:14px; }}
    .right {{ text-align:right; }}
    .footer {{ margin-top:24px; font-size:12px; color:#888; text-align:center; }}
    .badge {{ display:inline-block; padding:6px 10px; background:#d4edda; color:#155724; border-radius:999px; font-size:12px; }}
    .highlight {{ background:#e8f3ff; padding:12px; border-radius:6px; margin:12px 0; }}
    .success-box {{ background:#d4edda; border:1px solid #c3e6cb; color:#155724; padding:16px; border-radius:8px; margin:16px 0; }}
    </style>
    </head>
    <body>
    <span style=""display:none!important;"">{preheader}</span>
    <div class=""container"">
        <div class=""brand"">MamaFit</div>
        <div class=""sub""><span class=""badge"">{(receiverType == "USER" ? "Hoàn thành sản xuất" : "Cần xử lý giao hàng")}</span></div>

        <p>{greeting}</p>
        
        <p>{content}</p>

        <div class=""section-title"">Thông tin đơn hàng</div>
        <table class=""table"">
            <tr><td>Mã đơn hàng</td><td class=""right"">{order.Code}</td></tr>
            <tr><td>Trạng thái</td><td class=""right"">Hoàn thành sản xuất</td></tr>
            <tr><td>Ngày hoàn thành</td><td class=""right"">{DateTime.Now.ToString("dd/MM/yyyy HH:mm", vn)}</td></tr>
            <tr><td>Chi nhánh</td><td class=""right"">{order.Branch?.Name ?? "Không xác định"}</td></tr>
            {(receiverType == "MANAGER" ? $@"
            <tr><td>Khách hàng</td><td class=""right"">{order.User?.FullName ?? "Không xác định"}</td></tr>
            <tr><td>Số điện thoại</td><td class=""right"">{order.User?.PhoneNumber ?? "Không có"}</td></tr>
            <tr><td>Email</td><td class=""right"">{order.User?.UserEmail ?? "Không có"}</td></tr>
            <tr><td>Địa chỉ giao hàng</td><td class=""right"">{(order.Address != null ? $"{order.Address.Street}, {order.Address.Ward}, {order.Address.District}, {order.Address.Province}" : "Chưa có địa chỉ")}</td></tr>" : "")}
        </table>

        <div class=""section-title"">Chi tiết sản phẩm</div>
        <table class=""table"">
            <thead>
                <tr><th style=""text-align:left"">Sản phẩm</th><th>Số lượng</th><th class=""right"">Đơn giá</th></tr>
            </thead>
            <tbody>
                {itemsHtml}
            </tbody>
        </table>

        <div class=""success-box"">
            <p><strong>🎉 {(receiverType == "USER" ? "Chúc mừng! Đơn hàng đã hoàn thành sản xuất." : "Đơn hàng đã sẵn sàng để giao hàng!")}</strong></p>
            <p>{nextSteps}</p>
            {(receiverType == "MANAGER" ? $@"
            <div style=""margin-top:16px; padding:12px; background:#fff3cd; border:1px solid #ffeaa7; border-radius:6px;"">
                <p><strong>📦 Cần xử lý:</strong></p>
                <ul style=""margin:8px 0; padding-left:20px;"">
                    <li>Kiểm tra và đóng gói sản phẩm</li>
                    <li>Xác nhận thông tin giao hàng</li>
                    <li>Liên hệ khách hàng nếu cần thiết</li>
                    <li>Cập nhật trạng thái giao hàng</li>
                </ul>
            </div>" : "")}
        </div>

        <div class=""highlight"">
            <div class=""section-title"">{(receiverType == "USER" ? "Liên hệ hỗ trợ" : "Thông tin liên hệ và hướng dẫn")}</div>
            {(receiverType == "USER" ? @"
            <p><strong>📧 Email:</strong> support@mamafit.vn</p>
            <p><strong>📞 Hotline:</strong> 1900 1234</p>
            <p><strong>🕒 Thời gian:</strong> 8:00 - 22:00 (Thứ 2 - Chủ nhật)</p>" : @"
            <p><strong>📞 Liên hệ khách hàng:</strong> Gọi trước khi giao hàng để xác nhận</p>
            <p><strong>📦 Quy trình đóng gói:</strong> Kiểm tra chất lượng trước khi đóng gói</p>
            <p><strong>🚚 Giao hàng:</strong> Cập nhật tracking number cho khách hàng</p>
            <p><strong>💬 Hỗ trợ:</strong> support@mamafit.vn | 1900 1234</p>")}
        </div>

        <div class=""footer"">
            Cảm ơn bạn đã tin tưởng và sử dụng dịch vụ MamaFit!<br/>
            &copy; {DateTime.Now.Year} MamaFit. All rights reserved.
        </div>
    </div>
    </body>
    </html>";
    }
}