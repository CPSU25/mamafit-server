using System.Text;
using System.Text.Json;
using MamaFit.BusinessObjects.DbContext;
using MamaFit.Repositories.Implement;
using MamaFit.Repositories.Infrastructure;
using MamaFit.Repositories.Interface;
using MamaFit.Repositories.Repository;
using MamaFit.Services.ExternalService;
using MamaFit.Services.ExternalService.CloudinaryService;
using MamaFit.Services.Interface;
using MamaFit.Services.Mapper;
using MamaFit.Services.Service;
using MamaFit.Services.Service.Caculator;
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
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IAppointmentRepository, AppointmentRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<ITokenRepository, TokenRepository>();
            services.AddScoped<IOTPRepository, OTPRepository>();
            services.AddScoped<IBranchRepository, BranchRepository>();
            services.AddScoped<IDesignRequestRepository, DesignRequestRepository>();
            services.AddScoped<IMaternityDressRepository, MaternityDressRepository>();
            services.AddScoped<IMaternityDressDetailRepository, MaternityDressDetailRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IStyleRepository, StyleRepository>();
            services.AddScoped<IComponentRepository, ComponentRepository>();
            services.AddScoped<IComponentOptionRepository, ComponentOptionRepository>();
            services.AddScoped<IOrderItemRepository, OrderItemRepository>();
            services.AddScoped<IMeasurementRepository, MeasurementRepository>();
            services.AddScoped<IMeasurementDiaryRepository, MeasurementDiaryRepository>();
            services.AddScoped<IChatRepository, ChatRepository>();
            services.AddScoped<IVoucherBatchRepository, VoucherBatchRepository>();
            services.AddScoped<IVoucherDiscountRepository, VoucherDiscountRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
        }

        public static void AddServices(this IServiceCollection services)
        {
            // Add your service registrations here
            services.AddScoped<IBodyGrowthCalculator, BodyGrowthCalculator>();
            services.AddScoped<IValidationService, ValidationService>();
            services.AddScoped<IAppointmentService, AppointmentService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IBranchService, BranchService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IDesignRequestService, DesignRequestService>();
            services.AddScoped<IMaternityDressService, MaternityDressService>();
            services.AddScoped<IMaternityDressDetailService, MaternityDressDetailService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IStyleService, StyleService>();
            services.AddScoped<IComponentService, ComponentService>();
            services.AddScoped<IComponentOptionService, ComponentOptionService>();
            services.AddTransient<IEmailSenderSevice, EmailSenderService>();
            services.AddScoped<ICloudinaryService, CloudinaryService>();
            services.AddScoped<IMeasurementDiaryService, MeasurementDiaryService>();
            services.AddScoped<IMeasurementService, MeasurementService>();
            services.AddScoped<IChatService, ChatService>();
            services.AddScoped<IVoucherBatchService, VoucherBatchService>();
            services.AddScoped<IVoucherDiscountService, VoucherDiscountService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IOrderItemService, OrderItemService>();
        }

        public static IServiceCollection AddHttpClientServices(this IServiceCollection services)
        {
            services.AddHttpClient();
            return services;
        }

        public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection") ;
            services.AddDbContext<ApplicationDbContext>(options =>
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
                options.EnableAnnotations();
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
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!)),
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero // Tùy chỉnh: giảm thời gian lệch đồng hồ, cho chặt
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            // Read the token from query string for SignalR
                            var accessToken = context.Request.Query["access_token"];

                            // If the request is for our hub...
                            var path = context.HttpContext.Request.Path;
                            if (!string.IsNullOrEmpty(accessToken) && (path.StartsWithSegments("/chatHub")))
                            {
                                // Read the token out of the query string
                                context.Token = accessToken;
                            }
                            return Task.CompletedTask;
                        },
                        OnChallenge = async context =>
                        {
                            context.HandleResponse(); // Ngăn hệ thống trả lỗi mặc định

                            var response = context.Response;
                            response.ContentType = "application/json";

                            string message = "Token is invalid";
                            string errorCode = ApiCodes.TOKEN_INVALID;
                            int statusCode = StatusCodes.Status401Unauthorized;

                            if (context.AuthenticateFailure is SecurityTokenExpiredException)
                            {
                                message = "Token is expired";
                                errorCode = ApiCodes.TOKEN_EXPIRED;
                                statusCode = StatusCodes.Status403Forbidden;
                            }
                            else if (string.IsNullOrEmpty(context.Request.Headers["Authorization"]))
                            {
                                message = "No token provided";
                                errorCode = ApiCodes.UNAUTHENTICATED;
                            }

                            response.StatusCode = statusCode;

                            var result = JsonSerializer.Serialize(new
                            {
                                errorCode,
                                message,
                            });

                            await response.WriteAsync(result);
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
            services.AddSignalR();
        }

        private static void AddAutoMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddAutoMapper(typeof(MapperEntities).Assembly);
        }
    }
}
