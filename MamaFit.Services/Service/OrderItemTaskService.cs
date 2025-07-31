﻿using AutoMapper;
using MamaFit.BusinessObjects.DTO.DesignRequestDto;
using MamaFit.BusinessObjects.DTO.MaternityDressDetailDto;
using MamaFit.BusinessObjects.DTO.MaternityDressTaskDto;
using MamaFit.BusinessObjects.DTO.MilestoneDto;
using MamaFit.BusinessObjects.DTO.OrderItemDto;
using MamaFit.BusinessObjects.DTO.OrderItemTaskDto;
using MamaFit.BusinessObjects.DTO.PresetDto;
using MamaFit.BusinessObjects.Entity;
using MamaFit.BusinessObjects.Enum;
using MamaFit.Repositories.Helper;
using MamaFit.Repositories.Implement;
using MamaFit.Repositories.Interface;
using MamaFit.Services.Interface;

namespace MamaFit.Services.Service;

public class OrderItemTaskService : IOrderItemTaskService
{
    private readonly IOrderItemTaskRepository _repo;
    private readonly IMapper _mapper;
    private readonly IValidationService _validation;
    
    public OrderItemTaskService(IOrderItemTaskRepository repo, IMapper mapper, IValidationService validation)
    {
        _repo = repo;
        _mapper = mapper;
        _validation = validation;
    }

    public async Task<List<OrderItemTaskGetByTokenResponse>> GetTasksByAssignedStaffAsync(string accessToken)
    {
        var userId = JwtTokenHelper.ExtractUserId(accessToken);
        var orderItemTasks = await _repo.GetTasksByAssignedStaffAsync(userId);
        _validation.CheckNotFound(orderItemTasks, "No tasks found for the assigned staff");

        var groupedByOrderItem = orderItemTasks
        .Where(x => x.OrderItem != null)
        .GroupBy(x => x.OrderItem.Id)
        .Select(orderGroup =>
        {
            var representative = orderGroup.First();

            var milestoneGroups = orderGroup
                .Where(x => x.MaternityDressTask != null && x.MaternityDressTask.Milestone != null)
                .GroupBy(x => x.MaternityDressTask.Milestone.Id)
                .Select(milestoneGroup =>
                {
                    var milestoneRep = milestoneGroup.First().MaternityDressTask.Milestone;

                    return new MilestoneGetByIdOrderTaskResponseDto
                    {
                        Name = milestoneRep.Name,
                        Description = milestoneRep.Description,
                        ApplyFor = milestoneRep.ApplyFor,
                        SequenceOrder = milestoneRep.SequenceOrder,
                        MaternityDressTasks = milestoneGroup
                            .Select(x => _mapper.Map<MaternityDressTaskOrderTaskResponseDto>(x.MaternityDressTask))
                            .ToList()
                    };
                })
                .ToList();

            return new OrderItemTaskGetByTokenResponse
            {
                OrderItem = _mapper.Map<OrderItemResponseDto>(representative.OrderItem),
                Milestones = milestoneGroups
            };
        })
        .ToList();

        return groupedByOrderItem;
    }

    public async Task UpdateStatusAsync(string dressTaskId, string orderItemId, OrderItemTaskStatus status)
    {
        var task = await _repo.GetByIdAsync(dressTaskId, orderItemId);
        _validation.CheckNotFound(task, "Order item task not found");
        
        await _repo.UpdateOrderItemTaskStatusAsync(task, status);
        await _repo.UpdateAsync(task);
    }
}