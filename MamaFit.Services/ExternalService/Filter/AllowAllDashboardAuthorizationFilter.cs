using Hangfire.Dashboard;

namespace MamaFit.Services.ExternalService.Filter;

public class AllowAllDashboardAuthorizationFilter : IDashboardAuthorizationFilter
{
    public bool Authorize(DashboardContext context) => true;
}