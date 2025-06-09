using System.Text;
using MamaFit.BusinessObjects.DBContext;
using MamaFit.Repositories.Implement;
using MamaFit.Repositories.Interface;
using MamaFit.Repositories.Repository;
using MamaFit.Services.ExternalService;
using MamaFit.Services.Interface;
using MamaFit.Services.Mapper;
using MamaFit.Services.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace MamaFit.API.DependencyInjection
{
    public static class ApplicationServiceExtension
    {
        public static void AddRepositories(this IServiceCollection services)
        {
            // Add your repository registrations here
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<ITokenRepository, TokenRepository>();
            services.AddScoped<IOTPRepository, OTPRepository>();
            services.AddScoped<IBranchRepository, BranchRepository>();
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        }

        public static void AddServices(this IServiceCollection services)
        {
            // Add your service registrations here
            services.AddScoped<IAppointmentService, AppointmentService>();
            //services.AddScoped<IAppointmentRepository,AppointmentRepository>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IDesignRequestService, DesignRequestService>();
            services.AddScoped<IDesignRequestRepository, DesignRequestRepository>();
            services.AddScoped<IMaternityDressService, MaternityDressService>();
            services.AddScoped<IMaternityDressRepository, MaternityDressRepository>();
            services.AddScoped<IMaternityDressDetailService, MaternityDressDetailService>();
            services.AddScoped<IMaternityDressDetailRepository, MaternityDressDetailRepository>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IStyleService, StyleService>();
            services.AddScoped<IStyleRepository, StyleRepository>();
            services.AddScoped<IComponentService, ComponentService>();
            services.AddScoped<IComponentRepository, ComponentRepository>();
            services.AddScoped<IComponentOptionService, ComponentOptionService>();
            services.AddScoped<IComponentOptionRepository, ComponentOptionRepository>();
            services.AddTransient<IEmailSenderSevice, EmailSenderService>();
            services.AddScoped<ICloudinaryService, CloudinaryService>();
            //services.AddScoped<IMeasurementDiaryService, MeasurementDiaryService>();
            services.AddScoped<IBranchRepository, IBranchRepository>();
            services.AddScoped<IOrderItemRepository, IOrderItemRepository>();
        }

        public static IServiceCollection AddHttpClientServices(this IServiceCollection services)
        {
            services.AddHttpClient();
            return services;
        }

        public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("local");
            services.AddDbContext<ApplicationDbContext>(options =>
                //options.UseNpgsql(connectionString)
                options.UseNpgsql(connectionString)
            );
            return services;
        }

        public static IServiceCollection AddConfigSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo() { Title = "MamaFit System", Version = "v1" });
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] { }
                }
                });
                options.MapType<TimeOnly>(() => new OpenApiSchema
                {
                    Type = "string",
                    Format = "time",
                    Example = OpenApiAnyFactory.CreateFromJson("\"13:45:42\"")
                });
                // options.OperationFilter<FileUploadOperationFiler>();
            });
            return services;
        }

        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtSettings = configuration.GetSection("JWT");
            var issuer = jwtSettings["Issuer"];
            var audience = jwtSettings["Audience"];
            var secretKey = jwtSettings["SecretKey"];

            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = issuer,
                        ValidateAudience = true,
                        ValidAudience = audience,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero // Tùy chỉnh: giảm thời gian lệch đồng hồ, cho chặt
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnChallenge = async context =>
                        {
                            context.HandleResponse();
                            context.Response.ContentType = "application/json";

                            string message = "Token is invalid";

                            if (context.AuthenticateFailure is SecurityTokenExpiredException)
                            {
                                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                                message = "Token is expired";
                            }
                            else if (context.AuthenticateFailure != null)
                            {
                                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                                message = "Token is invalid";
                            }
                            else if (string.IsNullOrEmpty(context.Request.Headers["Authorization"]))
                            {
                                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                                message = "No token provided";
                            }

                            var result = System.Text.Json.JsonSerializer.Serialize(new { message });
                            await context.Response.WriteAsync(result);
                        }
                    };
                });

            return services;
        }

        public static void AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper();
            services.AddRepositories();
            services.AddServices();
            services.AddAutoMapper();
        }
        
        private static void AddAutoMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddAutoMapper(typeof(MapperEntities).Assembly);
        }
    }
}
