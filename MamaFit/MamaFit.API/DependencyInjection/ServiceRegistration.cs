using MamaFit.Services.Mapper;

namespace MamaFit.Configuration
{
    public static class ServiceRegistration
    {
        public static IServiceCollection Register(this IServiceCollection services)
        {
            //Configure AutoMapper
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddAutoMapper(typeof(MapperEntities).Assembly);

            #region DependencyInjection

            //External services
            #endregion

            return services;

        }
    }
}
