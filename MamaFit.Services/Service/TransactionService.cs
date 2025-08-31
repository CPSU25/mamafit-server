using System.Globalization;
using System.Text;
using AutoMapper;
using MamaFit.BusinessObjects.DTO.SepayDto;
using MamaFit.BusinessObjects.DTO.TransactionDto;
using MamaFit.BusinessObjects.Entity;
using MamaFit.BusinessObjects.Enum;
using MamaFit.Repositories.Implement;
using MamaFit.Repositories.Infrastructure;
using MamaFit.Services.ExternalService.Redis;
using MamaFit.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace MamaFit.Services.Service;

public class TransactionService : ITransactionService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IValidationService _validation;
    private readonly ICacheService _cache;
    private readonly IEmailSenderSevice _emailSenderService;

    public TransactionService(IUnitOfWork unitOfWork, IMapper mapper, IValidationService validation,
        ICacheService cache, IEmailSenderSevice emailSenderService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _validation = validation;
        _cache = cache;
        _emailSenderService = emailSenderService;
    }

    public async Task<PaginatedList<TransactionResponseDto>> GetTransactionsAsync(
        int index, int pageSize, DateTime? startDate = null, DateTime? endDate = null)
    {
        if (startDate.HasValue) startDate = EnsureUtc(startDate.Value);
        if (endDate.HasValue) endDate = EnsureUtc(endDate.Value);

        var transactions = await _unitOfWork.TransactionRepository
            .GetAllAsync(index, pageSize, startDate, endDate);

        var responseItems = transactions.Items
            .Select(transaction => _mapper.Map<TransactionResponseDto>(transaction))
            .ToList();

        return new PaginatedList<TransactionResponseDto>(
            responseItems,
            transactions.TotalCount,
            transactions.PageNumber,
            pageSize
        );
    }


    public async Task<TransactionResponseDto?> GetTransactionByOrderIdAsync(string orderId)
    {
        var transaction = await _unitOfWork.TransactionRepository.GetByOrderIdAsync(orderId);
        _validation.CheckNotFound(transaction, "Transaction not found");
        return _mapper.Map<TransactionResponseDto>(transaction);
    }

    public async Task CreateTransactionAsync(SepayWebhookPayload payload, string orderId, string orderCode)
    {
        var exist = await _unitOfWork.TransactionRepository
            .FindAsync(x => x.SepayId == payload.id);
        if (exist != null)
        {
            throw new ErrorException(StatusCodes.Status409Conflict, ApiCodes.CONFLICT, "Transaction already processed");
        }

        DateTime? transactionDateUtc = DateTime.TryParse(payload.transactionDate, out var parsedDate)
            ? parsedDate.ToUniversalTime()
            : null;

        var transaction = new Transaction
        {
            OrderId = orderId,
            SepayId = payload.id,
            Gateway = payload.gateway,
            TransactionDate = transactionDateUtc,
            AccountNumber = payload.accountNumber,
            Code = payload.code,
            Content = payload.content,
            TransferType = payload.transferType,
            TransferAmount = payload.transferAmount,
            Accumulated = payload.accumulated,
            SubAccount = payload.subAccount,
            ReferenceCode = payload.referenceCode,
            Description = $"Payment received via {payload.gateway} for order {orderCode}"
        };

        await _unitOfWork.TransactionRepository.InsertAsync(transaction);
        await _unitOfWork.SaveChangesAsync();
    }

    private static (DateTime start, DateTime end) ResolveRange(string range)
    {
        var now = DateTime.UtcNow;
        var key = (range ?? "month").ToLowerInvariant();

        if (key is "month" or "this_month")
        {
            var start = new DateTime(now.Year, now.Month, 1, 0, 0, 0, DateTimeKind.Utc);
            var end = start.AddMonths(1).AddTicks(-1);
            return (start, end);
        }

        if (key is "this_year" or "year")
        {
            var start = new DateTime(now.Year, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var end = new DateTime(now.Year, 12, 31, 23, 59, 59, 999, DateTimeKind.Utc);
            return (start, end);
        }

        var defStart = now.AddDays(-30); // đã là UTC
        var defEnd = now;
        return (defStart, defEnd);
    }

    private static DateTime EnsureUtc(DateTime dt)
    {
        return dt.Kind switch
        {
            DateTimeKind.Utc => dt,
            DateTimeKind.Local => dt.ToUniversalTime(),
            _ => DateTime.SpecifyKind(dt, DateTimeKind.Utc) // Unspecified => treat as UTC
        };
    }


    private static decimal PctChange(decimal current, decimal previous)
        => previous == 0 ? (current == 0 ? 0 : 100) : Math.Round(((current - previous) / previous) * 100m, 2);

    // Policy doanh thu: theo đơn Completed (bạn có thể đổi sang PaymentStatus = Paid nếu cần)
    private static IQueryable<Order> ApplyRevenuePolicy(IQueryable<Order> q)
    {
        return q.Where(o => o.PaymentStatus == PaymentStatus.PAID_DEPOSIT
        || o.PaymentStatus == PaymentStatus.PAID_FULL
        || o.PaymentStatus == PaymentStatus.PAID_DEPOSIT_COMPLETED);
    }


    // 1) /dashboard/summary?startTime=...&endTime=...
    public async Task<DashboardSummaryResponse> GetDashboardSummaryAsync(DateTime startTime, DateTime endTime)
    {
        startTime = EnsureUtc(startTime);
        endTime = EnsureUtc(endTime);
        if (startTime == default || endTime == default || endTime < startTime)
            throw new ErrorException(StatusCodes.Status400BadRequest, ApiCodes.BAD_REQUEST, "Invalid time range");

        var ordersQ = await _unitOfWork.OrderRepository.GetAllQueryableAsync();
        var usersQ = await _unitOfWork.UserRepository.GetAllQueryableAsync(); // repo cho ApplicationUser

        var rangeOrders = ordersQ.Where(o => !o.IsDeleted && o.CreatedAt >= startTime && o.CreatedAt <= endTime);

        var revenue = await ApplyRevenuePolicy(rangeOrders).SumAsync(o => (decimal?)(o.TotalAmount ?? 0) ?? 0m);
        var orders = await rangeOrders.CountAsync();

        var newCustomers = await usersQ
            .Where(u => !u.IsDeleted && u.CreatedAt >= startTime && u.CreatedAt <= endTime)
            .CountAsync();

        var aov = orders == 0 ? 0 : Math.Round(revenue / orders, 2);

        // kỳ trước cùng độ dài
        var span = endTime - startTime;
        var prevStart = startTime.AddTicks(-(span.Ticks + 1));
        var prevEnd = startTime.AddTicks(-1);

        var prevOrdersQ = ordersQ.Where(o => !o.IsDeleted && o.CreatedAt >= prevStart && o.CreatedAt <= prevEnd);
        var prevRevenue = await ApplyRevenuePolicy(prevOrdersQ).SumAsync(o => (decimal?)(o.TotalAmount ?? 0) ?? 0m);
        var prevOrders = await prevOrdersQ.CountAsync();
        var prevCustomers = await usersQ.Where(u => !u.IsDeleted && u.CreatedAt >= prevStart && u.CreatedAt <= prevEnd)
            .CountAsync();
        var prevAov = prevOrders == 0 ? 0 : Math.Round(prevRevenue / prevOrders, 2);

        return new DashboardSummaryResponse
        {
            Totals = new TotalsDto
            {
                Revenue = revenue,
                Orders = orders,
                NewCustomer = newCustomers,
                AvgOrderValue = aov
            },
            Trends = new TrendsDto
            {
                RevenuePct = PctChange(revenue, prevRevenue),
                OrdersPct = PctChange(orders, prevOrders),
                NewCustomersPct = PctChange(newCustomers, prevCustomers),
                AovPct = PctChange(aov, prevAov)
            }
        };
    }

    // 2) /analytics/revenue?groupBy=month&range=this_year&compare=yoy=true
    public async Task<List<RevenuePointDto>> GetRevenueAsync(string groupBy, string range, bool compareYoy)
    {
        groupBy = (groupBy ?? "month").ToLowerInvariant();
        if (groupBy != "month")
            throw new ErrorException(StatusCodes.Status400BadRequest, ApiCodes.BAD_REQUEST,
                "Only groupBy=month is supported");

        var (start, end) = ResolveRange(range);

        var ordersQ = (await _unitOfWork.OrderRepository.GetAllQueryableAsync())
            .Where(o => !o.IsDeleted && o.CreatedAt >= start && o.CreatedAt <= end);

        var curAgg = ApplyRevenuePolicy(ordersQ)
            .GroupBy(o => o.CreatedAt.Month)
            .Select(g => new { Month = g.Key, Revenue = g.Sum(x => x.TotalAmount ?? 0), Orders = g.Count() });

        var curList = await curAgg.ToListAsync();

        Dictionary<int, decimal>? lastYear = null;
        if (compareYoy)
        {
            var lyStart = new DateTime(start.Year - 1, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var lyEnd = new DateTime(start.Year - 1, 12, 31, 23, 59, 59, 999, DateTimeKind.Utc);

            var lyQ = (await _unitOfWork.OrderRepository.GetAllQueryableAsync())
                .Where(o => !o.IsDeleted && o.CreatedAt >= lyStart && o.CreatedAt <= lyEnd);

            lastYear = (await ApplyRevenuePolicy(lyQ)
                    .GroupBy(o => o.CreatedAt.Month)
                    .Select(g => new { Month = g.Key, Revenue = g.Sum(x => x.TotalAmount ?? 0) })
                    .ToListAsync())
                .ToDictionary(x => x.Month, x => x.Revenue);
        }

        return Enumerable.Range(1, 12).Select(m =>
        {
            var cur = curList.FirstOrDefault(x => x.Month == m);
            return new RevenuePointDto
            {
                Month = $"T{m}",
                Revenue = cur?.Revenue ?? 0,
                Orders = cur?.Orders ?? 0,
                LastYear = (lastYear != null && lastYear.TryGetValue(m, out var ly)) ? ly : null
            };
        }).ToList();
    }


    // 3) /analytics/orders/status?range=month
    public async Task<OrderStatusResponse> GetOrderStatusAsync(string range)
    {
        var (start, end) = ResolveRange(range);

        var ordersQ = (await _unitOfWork.OrderRepository.GetAllQueryableAsync())
            .Where(o => !o.IsDeleted && o.CreatedAt >= start && o.CreatedAt <= end);

        var raw = await ordersQ
            .GroupBy(o => o.Status)
            .Select(g => new { Status = g.Key, Count = g.Count() })
            .ToListAsync();

        OrderStatus[] expected = new[]
        {
            OrderStatus.CREATED,
            OrderStatus.CONFIRMED,
            OrderStatus.IN_PROGRESS,
            OrderStatus.AWAITING_PAID_REST,
            OrderStatus.PACKAGING,
            OrderStatus.DELIVERING,
            OrderStatus.COMPLETED,
            OrderStatus.CANCELLED,
            OrderStatus.RETURNED,
            OrderStatus.PICKUP_IN_PROGRESS,
            OrderStatus.AWAITING_PAID_WARRANTY,
            OrderStatus.COMPLETED_WARRANTY,
            OrderStatus.RECEIVED_AT_BRANCH
        };

        var mapped = raw.Select(x => new OrderStatusCountDto
        {
            Status = x.Status.HasValue ? x.Status.Value.ToString() : "UNKNOWN",
            Value = x.Count
        }).ToList();

        foreach (var s in expected)
            if (!mapped.Any(m => m.Status == s.ToString()))
                mapped.Add(new OrderStatusCountDto { Status = s.ToString(), Value = 0 });

        mapped = mapped.OrderBy(c => Array.IndexOf(expected.Select(e => e.ToString()).ToArray(), c.Status)).ToList();

        return new OrderStatusResponse { Range = range ?? "month", Counts = mapped };
    }

    // 4) /analytics/branches/top?metric=revenue&limit=5&range=month
    public async Task<BranchTopResponse> GetTopBranchesAsync(string metric, int limit, string range)
    {
        metric = string.IsNullOrWhiteSpace(metric) ? "revenue" : metric.ToLowerInvariant();
        if (limit <= 0 || limit > 50) limit = 5;

        var (start, end) = ResolveRange(range);

        // Lọc: chỉ lấy đơn có BranchId (tránh nhóm rỗng)
        var ordersQ = (await _unitOfWork.OrderRepository.GetAllQueryableAsync())
            .Where(o => !o.IsDeleted
                        && !string.IsNullOrEmpty(o.BranchId)
                        && o.CreatedAt >= start && o.CreatedAt <= end);

        var curAgg = ApplyRevenuePolicy(ordersQ)
            .GroupBy(o => new { o.BranchId, BranchName = o.Branch != null ? o.Branch.Name : null })
            .Select(g => new
            {
                g.Key.BranchId,
                BranchName = g.Key.BranchName ?? "(Không rõ)",
                Revenue = g.Sum(x => x.TotalAmount ?? 0),
                Orders = g.Count()
            });

        var ordered = (metric == "orders")
            ? curAgg.OrderByDescending(x => x.Orders)
            : curAgg.OrderByDescending(x => x.Revenue);

        var top = await ordered.Take(limit).ToListAsync();

        // Kỳ trước cùng độ dài — dùng CHUNG tiêu chí lọc
        var span = end - start;
        var prevStart = start.AddTicks(-(span.Ticks + 1));
        var prevEnd = start.AddTicks(-1);

        var prevQ = (await _unitOfWork.OrderRepository.GetAllQueryableAsync())
            .Where(o => !o.IsDeleted
                        && !string.IsNullOrEmpty(o.BranchId)
                        && o.CreatedAt >= prevStart && o.CreatedAt <= prevEnd);

        var prevAgg = await ApplyRevenuePolicy(prevQ)
            .GroupBy(o => o.BranchId)
            .Select(g => new { BranchId = g.Key, Revenue = g.Sum(x => x.TotalAmount ?? 0), Orders = g.Count() })
            .ToDictionaryAsync(x => x.BranchId, x => x);

        var items = top.Select(x =>
        {
            decimal growth = 0;
            if (prevAgg.TryGetValue(x.BranchId!, out var p))
            {
                var curVal = metric == "orders" ? (decimal)x.Orders : x.Revenue;
                var prevVal = metric == "orders" ? (decimal)p.Orders : p.Revenue;
                growth = PctChange(curVal, prevVal);
            }

            return new BranchPerformanceDto
            {
                BranchId = x.BranchId!, // đã lọc != null/empty ở trên
                BranchName = string.IsNullOrWhiteSpace(x.BranchName) ? "(Không rõ)" : x.BranchName,
                Revenue = x.Revenue,
                Orders = x.Orders,
                GrowthPct = growth
            };
        }).ToList();

        return new BranchTopResponse { Metric = metric, Items = items };
    }

    // 5) /orders/recent?limit=10
    public async Task<RecentOrdersResponse> GetRecentOrdersAsync(int limit)
    {
        if (limit <= 0 || limit > 100) limit = 10;

        var ordersQ = await _unitOfWork.OrderRepository.GetAllQueryableAsync();

        var items = await ordersQ
            .Where(o => !o.IsDeleted)
            .OrderByDescending(o => o.CreatedAt)
            .Take(limit)
            .Select(o => new RecentOrderItemDto
            {
                Id = o.Id,
                Code = o.Code ?? "",
                Customer = new CustomerMiniDto
                {
                    Id = o.UserId ?? "",
                    Name = o.User.FullName ?? "",
                    Avatar = o.User.ProfilePicture,
                    Phone = o.User.PhoneNumber
                },
                PrimaryItem = new PrimaryItemDto
                {
                    // đổi theo entity thật của OrderItem nếu tên khác
                    MaternityDressName = o.OrderItems
                        .OrderBy(oi => oi.CreatedAt)
                        .Select(oi => oi.MaternityDressDetail != null ? oi.MaternityDressDetail.Name : null)
                        .FirstOrDefault(),
                    PresetName = o.OrderItems
                        .OrderBy(oi => oi.CreatedAt)
                        .Select(oi => oi.Preset != null ? oi.Preset.Name : null)
                        .FirstOrDefault()
                },
                Branch = new BranchMiniDto
                {
                    Id = o.BranchId ?? "",
                    Name = o.Branch != null ? (o.Branch.Name ?? "") : ""
                },
                Amount = o.TotalAmount ?? 0,
                Status = o.Status.HasValue ? o.Status.Value.ToString() : "UNKNOWN",
                CreatedAt = o.CreatedAt
            })
            .ToListAsync();

        return new RecentOrdersResponse { Items = items };
    }
    
    public async Task SendPaymentReceiptAsync(Order order, Transaction txn, PaymentStatus newPaymentStatus)
    {
        var email = order.User.UserEmail;
        if (string.IsNullOrWhiteSpace(email))
            throw new ErrorException(StatusCodes.Status400BadRequest, ApiCodes.BAD_REQUEST,
                "User email is not set, cannot send receipt");

        var subject = newPaymentStatus switch
        {
            PaymentStatus.PAID_DEPOSIT => $"[MamaFit] Xác nhận thanh toán CỌC cho đơn {order.Code}",
            PaymentStatus.PAID_DEPOSIT_COMPLETED => $"[MamaFit] Xác nhận thanh toán PHẦN CÒN LẠI cho đơn {order.Code}",
            PaymentStatus.PAID_FULL => $"[MamaFit] Xác nhận thanh toán THÀNH CÔNG đơn {order.Code}",
            _ => $"[MamaFit] Cập nhật thanh toán đơn {order.Code}"
        };

        var html = BuildReceiptHtml(order, txn, newPaymentStatus);
        await _emailSenderService.SendEmailAsync(email, subject, html);
    }

    private static string BuildReceiptHtml(Order order, Transaction txn, PaymentStatus status)
    {
        var vn = new CultureInfo("vi-VN");
        var tz = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time"); // Asia/Ho_Chi_Minh (Windows)
        var paidAt = txn?.TransactionDate ?? DateTime.UtcNow;
        var localPaidAt = TimeZoneInfo.ConvertTimeFromUtc(DateTime.SpecifyKind(paidAt, DateTimeKind.Utc), tz);

        var paymentTypeText = order.PaymentType == PaymentType.DEPOSIT ? "Thanh toán cọc" : "Thanh toán toàn bộ";
        if (status == PaymentStatus.PAID_DEPOSIT_COMPLETED) paymentTypeText = "Thanh toán phần còn lại";

        var amount = (decimal?)txn?.TransferAmount ?? 0m;
        var total = order.TotalAmount ?? 0m;
        var subtotal = order.SubTotalAmount ?? 0m;
        var shipping = order.ShippingFee;
        var discount = order.DiscountSubtotal ?? 0m;

        var itemsHtml = new StringBuilder();
        if (order?.OrderItems != null)
        {
            foreach (var it in order.OrderItems)
            {
                var name =
                    it.Preset?.Name ??
                    it.MaternityDressDetail?.Name ??
                    (it.DesignRequest != null ? "Yêu cầu thiết kế" : "Sản phẩm");
                itemsHtml.Append($@"
                    <tr>
                        <td style=""padding:8px 0"">{name}</td>
                        <td style=""padding:8px 0; text-align:center"">{it.Quantity}</td>
                        <td style=""padding:8px 0; text-align:right"">{(it.Price).ToString("c0", vn)}</td>
                    </tr>");
            }
        }

        var addressLine = order.DeliveryMethod == DeliveryMethod.DELIVERY
            ? $"{order?.Address?.Street}, {order?.Address?.Ward}, {order?.Address?.District}, {order?.Address?.Province}"
            : $"Nhận tại chi nhánh: {order?.Branch?.Name}";

        var preheader = $"Xác nhận {paymentTypeText} cho đơn {order?.Code}";

        return $@"
<!DOCTYPE html>
<html lang=""vi"">
<head>
<meta charset=""UTF-8"">
<meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
<title>Biên nhận thanh toán</title>
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
.badge {{ display:inline-block; padding:6px 10px; background:#e8f3ff; color:#2266cc; border-radius:999px; font-size:12px; }}
.total-row td {{ font-weight:bold; }}
</style>
</head>
<body>
<span style=""display:none!important;"">{preheader}</span>
<div class=""container"">
    <div class=""brand"">MamaFit</div>
    <div class=""sub""><span class=""badge"">{paymentTypeText}</span></div>

    <div class=""section-title"">Thông tin đơn hàng</div>
    <table class=""table"">
        <tr><td>Mã đơn</td><td class=""right"">{order?.Code}</td></tr>
        <tr><td>Thời gian thanh toán</td><td class=""right"">{localPaidAt:dd/MM/yyyy HH:mm}</td></tr>
        <tr><td>Phương thức</td><td class=""right"">{order?.PaymentMethod}</td></tr>
        <tr><td>Trạng thái thanh toán</td><td class=""right"">{status}</td></tr>
        {(string.IsNullOrEmpty(addressLine) ? "" : $@"<tr><td>Giao nhận</td><td class=""right"">{addressLine}</td></tr>")}
        {(string.IsNullOrEmpty(txn?.ReferenceCode) ? "" : $@"<tr><td>Mã tham chiếu</td><td class=""right"">{txn?.ReferenceCode}</td></tr>")}
        {(string.IsNullOrEmpty(txn?.Code) ? "" : $@"<tr><td>Mã giao dịch</td><td class=""right"">{txn?.Code}</td></tr>")}
    </table>

    <div class=""section-title"">Chi tiết sản phẩm</div>
    <table class=""table"">
        <thead>
            <tr><th style=""text-align:left"">Sản phẩm</th><th>Số lượng</th><th class=""right"">Đơn giá</th></tr>
        </thead>
        <tbody>
            {itemsHtml}
        </tbody>
        <tfoot>
            <tr><td colspan=""2"" class=""right"">Tạm tính</td><td class=""right"">{subtotal.ToString("c0", vn)}</td></tr>
            {(discount > 0 ? $@"<tr><td colspan=""2"" class=""right"">Giảm giá</td><td class=""right"">- {discount.ToString("c0", vn)}</td></tr>" : "")}
            <tr><td colspan=""2"" class=""right"">Phí vận chuyển</td><td class=""right"">{shipping.ToString("c0", vn)}</td></tr>
            <tr class=""total-row""><td colspan=""2"" class=""right"">Tổng cộng</td><td class=""right"">{total.ToString("c0", vn)}</td></tr>
            <tr><td colspan=""2"" class=""right"">Số tiền đã thanh toán trong lần này</td><td class=""right"">{amount.ToString("c0", vn)}</td></tr>
        </tfoot>
    </table>

    <div class=""footer"">
        Nếu có sai sót, vui lòng phản hồi email này hoặc liên hệ MamaFit để được hỗ trợ.<br/>
        &copy; {DateTime.Now.Year} MamaFit. All rights reserved.
    </div>
</div>
</body>
</html>";
    }
}