using MamaFit.BusinessObjects.Entity;
using MamaFit.BusinessObjects.Enum;

namespace MamaFit.Repositories.Helper;

public static class OrderCalculationHelper
{
    public static void RecalculateAmounts(Order order)
    {
        if (order == null)
            throw new ArgumentNullException(nameof(order));

        decimal subTotal = order.SubTotalAmount ?? 0;
        decimal discount = order.DiscountSubtotal ?? 0;
        decimal shipping = order.ShippingFee;
        
        decimal netAmount = subTotal - discount;

        if (netAmount < 0)
            netAmount = 0;

        if (order.Type == OrderType.DEPOSIT)
        {
            decimal depositAmount = netAmount * 0.5m; 
            decimal remaining = netAmount - depositAmount;

            order.DepositSubtotal = depositAmount;
            order.RemainingBalance = remaining;
            order.TotalAmount = depositAmount + shipping;
        }
        else
        {
            order.DepositSubtotal = null;
            order.RemainingBalance = null;
            order.TotalAmount = netAmount + shipping;
        }
    }
}