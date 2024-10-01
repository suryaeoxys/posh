using Posh_TRPT_Utility.Resources;
using Posh_TRPT_Infrastructure;
using Posh_TRPT_Infrastructure.Repositories;
using Posh_TRPT_Services.Employees;
using Microsoft.EntityFrameworkCore;
using Posh_TRPT_Domain.Interfaces;
using Posh_TRPT_Domain.Employees;
using Microsoft.AspNetCore.Identity;
using Posh_TRPT_Domain.Entity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Posh_TRPT_Domain.Token;
using Posh_TRPT_Services.Token;
using Microsoft.OpenApi.Models;
using Posh_TRPT_Utility.JwtUtils;
using Posh_TRPT_Domain.Register;
using Posh_TRPT_Services.Register;
using Posh_TRPT_Services.RoleMenu;
using Posh_TRPT_Domain.RoleMenu;
using Posh_TRPT_Services.MasterTable;
using Posh_TRPT_Domain.MasterTable;
using Posh_TRPT_Services.PushNotification;
using Posh_TRPT_Domain.PushNotification;
using Posh_TRPT.Helpers.CustomValidators;
using Posh_TRPT_Services.Customer;
using Posh_TRPT_Domain.Customer;
using Posh_TRPT_Domain.BookingSystem;
using Posh_TRPT_Services.BookingSystem;
using Posh_TRPT_Domain.StripePayment;
using Posh_TRPT_Services.StripePayment;
using Posh_TRPT_Domain.DashBoard;
using Posh_TRPT_Services.DashBoard;
using Posh_TRPT_Domain.Report;
using Posh_TRPT_Services.Report;
using Quartz;
using Quartz.AspNetCore;
using Posh_TRPT.Controllers;
using Posh_TRPT_Domain.InspectionData;
using Posh_TRPT_Services.InspectionExpiryEmail;

namespace Posh_TRPT.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            // Configure DbContext with Scoped lifetime

            services.AddControllers();
            services.AddControllersWithViews();
            services.Configure<StripeSettings>(configuration.GetSection("StripeSettings"));
            services.AddTransient<IUserValidator<ApplicationUser>, OptionalEmailUserValidator<ApplicationUser>>();
            services.AddIdentity<ApplicationUser, IdentityRole>(options => options.User.RequireUniqueEmail = true).AddUserValidator<OptionalEmailUserValidator<ApplicationUser>>().AddEntityFrameworkStores<PoshDbContext>().AddDefaultTokenProviders();
            services.AddDbContext<PoshDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")!,
                    sqlServerOptions => sqlServerOptions.CommandTimeout(1800));
                options.UseLazyLoadingProxies();
            }
          );
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(option =>
            {
                option.SaveToken = true;
                option.RequireHttpsMetadata = false;
                option.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = configuration["JWT:ValidIssuer"],
                    ValidAudience = configuration["JWT:ValidAudience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]!)),
                    ValidateLifetime = false,
                    ClockSkew = TimeSpan.Zero
                };
            });

            services.AddHttpClient();
            services.AddScoped<Func<PoshDbContext>>((provider) => () => provider.GetService<PoshDbContext>()!);
            services.AddScoped<DbFactory>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    policy => { policy.WithOrigins("*").AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader(); }
                    );
            });
            services.AddAuthorization(o =>
            {
                o.AddPolicy("SuperAdmin", policy => policy.RequireClaim("SuperAdmin"));
                o.AddPolicy("Driver", policy => policy.RequireClaim("Driver"));
                o.AddPolicy("DriverCustomer", policy => policy.RequireRole("Driver", "Customer"));
                o.AddPolicy("DriverCustomerSuperAdmin", policy => policy.RequireRole("Driver", "Customer", "SuperAdmin"));
                o.AddPolicy("DriverSuperAdmin", policy => policy.RequireRole("Driver", "SuperAdmin"));
            });

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddDistributedMemoryCache();
            services.AddSession(option => option.IdleTimeout = TimeSpan.FromDays(7));
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Posh-TRPT", Version = "v1" });
                c.AddSecurityDefinition(JwtAuthenticationDefaults.AuthenticationScheme,
                    new OpenApiSecurityScheme
                    {
                        Description = "Bearer",
                        Name = JwtAuthenticationDefaults.HeaderName,
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.Http,
                        Scheme = "Bearer"
                    });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                    new OpenApiSecurityScheme
                    {
                        Reference=new OpenApiReference
                        {
                            Type=ReferenceType.SecurityScheme,
                            Id=JwtAuthenticationDefaults.AuthenticationScheme
                        }
                    },
                     new List<string>()
                    }
                });
            });
            services.AddQuartz(q =>
            {
                // Configure the Quartz scheduler
                // This is where you set up your job and trigger configurations
                q.AddJob<EmailShceduler>(opts => opts.WithIdentity("EmailShceduler"));
                q.AddTrigger(opts => opts
                 .ForJob("EmailShceduler")
                 .WithIdentity("EmailShcedulerTrigger")
                 .WithCronSchedule("0 0 13 * * ?") // Runs at 04:30 PM IST every day
            );
            });

            services.AddQuartzServer(options =>
            {
                // Configure the server options
                // For example, you can set options.WaitForJobsToComplete = true to wait for jobs to complete before shutting down
                options.WaitForJobsToComplete = true;
            });
            return services;
        }


        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            return services
                .AddScoped(typeof(IRepository<>), typeof(Repository<>))
                .AddScoped<IEmployeeRepository, EmployeeRepository>()
                .AddScoped<ITokenRepository, TokenRepository>()
                .AddScoped<IRegisterRepository, RegisterRepository>()
                .AddScoped<IRoleMenuRepository, RoleMenuRepository>()
                .AddScoped<IEmployeeRepository, EmployeeRepository>()
                .AddScoped<IPushNotificationRepository, PushNotificationRepository>()
                .AddScoped<ICustomerRepository, CustomerRepository>()
                 .AddScoped<IBookingSystemRepository, BookingSystemRepository>()
                .AddScoped<IMasterTableRepository, MasterTableRepository>()
                .AddScoped<IDashBoardRepository, DashBoardRepository>()
                .AddScoped<IReportRepository, ReportRepository>()
                .AddScoped<IExpireInspectionEmails, ExpireInspectionEmailsRepository>()
                .AddScoped<IStripePaymentRepository, StripePaymentRepository>();
        }
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            return services
                .AddScoped<EmployeeService>()
                .AddScoped<TokenService>()
                .AddScoped<RegisterService>()
                .AddScoped<RoleMenuService>()
                .AddScoped<PushNotificationService>()
                .AddScoped<CustomerService>()
                .AddScoped<BookingSystemService>()
             .AddScoped<MasterTableService>()
             .AddScoped<ReportService>()
             .AddScoped<DashBoardService>()
             .AddScoped<ExpireInspectionEmailsService>()
             .AddScoped<StripePaymentService>();
        }
    }
}
