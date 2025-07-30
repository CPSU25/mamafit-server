namespace MamaFit.BusinessObjects.DTO.OrderItemTaskDto;

public class StaffTasksGroupedResponse
{
    public List<StaffOrderItemTaskGroupDto> Data { get; set; } = new();
    public List<MilestoneResponseMinDto> Milestones { get; set; } = new();
}

public class MilestoneResponseMinDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public int SequenceOrder { get; set; }
    public List<string> ApplyFor { get; set; } = new();
}
