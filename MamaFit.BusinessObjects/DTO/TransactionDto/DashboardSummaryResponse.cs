namespace MamaFit.BusinessObjects.DTO.TransactionDto;

public class DashboardSummaryResponse
{
    public TotalsDto Totals { get; set; } = new();
    public TrendsDto Trends { get; set; } = new();
}
public class TotalsDto
{
    public decimal Revenue { get; set; }
    public int Orders { get; set; }
    public int NewCustomer { get; set; }
    public decimal AvgOrderValue { get; set; }
}
public class TrendsDto
{
    public decimal RevenuePct { get; set; }
    public decimal OrdersPct { get; set; }
    public decimal NewCustomersPct { get; set; }
    public decimal AovPct { get; set; }
}

public class RevenuePointDto
{
    public string Month { get; set; } = "";
    public decimal Revenue { get; set; }
    public decimal? LastYear { get; set; }
    public int Orders { get; set; }
}

public class OrderStatusCountDto
{
    public string Status { get; set; } = "";
    public int Value { get; set; }
}
public class OrderStatusResponse
{
    public string Range { get; set; } = "";
    public List<OrderStatusCountDto> Counts { get; set; } = new();
}

public class BranchPerformanceDto
{
    public string BranchId { get; set; } = "";
    public string BranchName { get; set; } = "";
    public decimal Revenue { get; set; }
    public int Orders { get; set; }
    public decimal GrowthPct { get; set; }
}
public class BranchTopResponse
{
    public string Metric { get; set; } = "revenue";
    public List<BranchPerformanceDto> Items { get; set; } = new();
}

public class RecentOrderItemDto
{
    public string Id { get; set; } = "";
    public string Code { get; set; } = "";
    public CustomerMiniDto Customer { get; set; } = new();
    public PrimaryItemDto PrimaryItem { get; set; } = new();
    public BranchMiniDto Branch { get; set; } = new();
    public decimal Amount { get; set; }
    public string? Status { get; set; } = "";
    public DateTime CreatedAt { get; set; }
}
public class CustomerMiniDto
{
    public string Id { get; set; } = "";
    public string Name { get; set; } = "";
    public string? Avatar { get; set; }
    public string? Phone { get; set; }
}
public class BranchMiniDto
{
    public string Id { get; set; } = "";
    public string Name { get; set; } = "";
}
public class PrimaryItemDto
{
    public string? MaternityDressName { get; set; }
    public string? PresetName { get; set; }
}
public class RecentOrdersResponse
{
    public List<RecentOrderItemDto> Items { get; set; } = new();
}