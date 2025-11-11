using System.Net;
using System.Text;
using System.Text.Json.Serialization;
using FATWA_API.Helpers;
using FATWA_GENERAL.Helpers;
using FATWA_API.SwaggerOptions;
using FATWA_DOMAIN.Common.Service;
using FATWA_DOMAIN.Interfaces;
using FATWA_DOMAIN.Interfaces.CaseManagement;
using FATWA_DOMAIN.Interfaces.Common;
using FATWA_DOMAIN.Interfaces.Communication;
using FATWA_DOMAIN.Interfaces.Consultation;
using FATWA_DOMAIN.Interfaces.ContactManagment;
using FATWA_DOMAIN.Interfaces.Meet;
using FATWA_DOMAIN.Interfaces.Notification;
using FATWA_DOMAIN.Interfaces.PatternNumber;
using FATWA_DOMAIN.Interfaces.Tasks;
using FATWA_DOMAIN.Interfaces.TimeTracking;
using FATWA_DOMAIN.Interfaces.TimeInterval;
using FATWA_DOMAIN.Interfaces.WorkerService;
using FATWA_DOMAIN.Models.BaseModels;
using FATWA_DOMAIN.Models.ViewModel;
using FATWA_GENERAL.Helper;
using FATWA_INFRASTRUCTURE.Database;
using FATWA_INFRASTRUCTURE.Repository;
using FATWA_INFRASTRUCTURE.Repository.CaseManagement;
using FATWA_INFRASTRUCTURE.Repository.CommonRepos;
using FATWA_INFRASTRUCTURE.Repository.Communications;
using FATWA_INFRASTRUCTURE.Repository.Consultation;
using FATWA_INFRASTRUCTURE.Repository.MeetRepo;
using FATWA_INFRASTRUCTURE.Repository.NotificationRepo;
using FATWA_INFRASTRUCTURE.Repository.PatternNumber;
using FATWA_INFRASTRUCTURE.Repository.RolesAndPermissions;
using FATWA_INFRASTRUCTURE.Repository.Tasks;
using FATWA_INFRASTRUCTURE.Repository.TimeTracking;
using FATWA_INFRASTRUCTURE.Repository.WorkerService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using FATWA_DOMAIN.Interfaces.LLSLegalPrinciple;
using FATWA_INFRASTRUCTURE.Repository.LLSLegalPrinciple;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using FATWA_DOMAIN.Interfaces.AutomationMonitoring;
using FATWA_INFRASTRUCTURE.Repository.AutomationMonitoring;
using FATWA_API.RabbitMQ;
using FATWA_INFRASTRUCTURE.Repository.G2G;
using FATWA_DOMAIN.Interfaces.ArchivedCases;
using FATWA_INFRASTRUCTURE.Repository.ArchivedCases;
using FATWA_INFRASTRUCTURE.Repository.BugReporting;
using FATWA_DOMAIN.Interfaces.BugReporting;
using FATWA_API.AuthenticationSchemes;
using Microsoft.AspNetCore.Authentication;

var builder = WebApplication.CreateBuilder(args);
IApiVersionDescriptionProvider provider;
// Add services to the container.
//builder.Services.AddJWTTokenServices(builder.Configuration);
// Add services to the container.
builder.Services.AddDbContext<DatabaseContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("FatwaLocalDBConnection")));
builder.Services.AddDbContext<DmsDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DmsLocalDBConnection")));
builder.Services.AddDbContext<G2GDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("G2GLocalDBConnection")));
builder.Services.AddDbContext<AutoMonInterfaceDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("AutomonILocalDBConnection")));
builder.Services.AddDbContext<ArchivedCasesDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("FatwaArchivedCasesLocalDbConnection")));
//builder.Services.AddTransient<ITasks, TasksRepository>();
builder.Services.AddTransient<IAccount, AccountRepository>();
builder.Services.AddTransient<AccountRepository>();

builder.Services.AddTransient<IUsers, UsersRepository>();
builder.Services.AddTransient<ILmsLiterature, LmsLiteraturesRepository>();
builder.Services.AddTransient<ITempFileUpload, TempFileUploadRepository>();
builder.Services.AddTransient<G2GRepository>();
//builder.Services.AddTransient<IFileRemove<T>, FileRemoveRepository<T>();
builder.Services.AddTransient<ILmsLiteratureClassification, LmsLiteratureClassificationRepository>();
builder.Services.AddTransient<ILiteratureTypes, LmsLiteratureTypesRepository>();
builder.Services.AddTransient<ILmsLiteratureBorrowDetail, LmsLiteratureBorrowDetailRepository>();
//builder.Services.AddTransient<Logging>();
builder.Services.AddTransient<ILmsLiteratureIndex, LmsLiteratureIndexRepository>();
builder.Services.AddTransient<ILmsLiteratureParentIndex, LmsLiteraturesParentIndexRepository>();
builder.Services.AddTransient<ILmsLiteratureIndexDivisionAisle, LmsLiteraturesIndexDivisionAisleRepository>();
builder.Services.AddTransient<ILegalLibrary, LegalLibraryRepository>();
builder.Services.AddTransient<ISystemSetting, SystemSettingRepository>();
builder.Services.AddScoped<IClaims, ClaimUmsRepository>();
builder.Services.AddTransient<ITempFileUpload, TempFileUploadRepository>();

//builder.Services.AddTransient<IErrorLogs, ErrorLogRepository>();
//builder.Services.AddTransient<IProcessLogs, ProcessLogRepository>();
builder.Services.AddTransient<IAuditLog, AuditLogRepository>();

builder.Services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();
builder.Services.AddTransient<LmsLiteratureService>();
builder.Services.AddTransient<BorrowDetailVM>();
builder.Services.AddTransient<INotification, NotificationRepository>();
//Demo
//builder.Services.AddTransient<IEmployee, EmployeeRepository>();


#region CASE MANAGEMENT
builder.Services.AddTransient<CMSCaseRequestRepository>();
builder.Services.AddTransient<CmsCaseFileRepository>();
builder.Services.AddTransient<CmsRegisteredCaseRepository>();
builder.Services.AddTransient<NotificationRepository>();
builder.Services.AddTransient<RoleRepository>();

builder.Services.AddTransient<CmsSharedRepository>();
builder.Services.AddTransient<ComsSharedRepository>();
builder.Services.AddTransient<ICmsShared, CmsSharedRepository>();
builder.Services.AddTransient<ICmsMojRPA, CmsMojRPARepository>();

builder.Services.AddTransient<ICMSCaseRequest, CMSCaseRequestRepository>();
builder.Services.AddScoped<ICmsCaseFile, CmsCaseFileRepository>();
builder.Services.AddScoped<ICmsCaseTemplate, CmsCaseTemplateRepository>();
builder.Services.AddScoped<ICmsRegisteredCase, CmsRegisteredCaseRepository>();
#endregion

builder.Services.AddTransient<ITranslation, TranslationRepository>();
builder.Services.AddScoped<ILegalLegislation, LegalLegislationRepository>();
builder.Services.AddScoped<ILookups, LookupsRepository>();


builder.Services.AddScoped<ICNTContact, CNTContactRepository>();
builder.Services.AddTransient<TimeTrackingRepository>();
builder.Services.AddTransient<ITimeTracking, TimeTrackingRepository>();
#region Bug Reporting
builder.Services.AddTransient<BugReportingRepository>();
builder.Services.AddTransient<IBugReporting, BugReportingRepository>();

#endregion

#region RabbitMQ
builder.Services.AddTransient<RabbitMQClient>();
#endregion

Log.Logger = new LoggerConfiguration()
                 .MinimumLevel.Debug()
                 .WriteTo.File("ActiveDirectory.json")
                 .CreateLogger();

#region Workflow
builder.Services.AddTransient<IWorkflow, WorkflowRepository>();
builder.Services.AddTransient<WorkflowRepository>();
#endregion

#region UserManagement
builder.Services.AddScoped<IRole, RoleRepository>();
builder.Services.AddScoped<IUmsGroup, UmsGroupsRepository>();
builder.Services.AddScoped<ITransferUser, TransferUserRepository>();
builder.Services.AddScoped<ISystemConfiguration, SystemConfigurationRepository>();
#endregion

#region LLS Legal Principle
builder.Services.AddScoped<ILLSLegalPrinciple, LLSLegalPrincipleRepository>();
#endregion

#region Dashboard

builder.Services.AddTransient<IDashboard, DashboardRepository>();

#endregion

#region Meeting

builder.Services.AddTransient<IMeeting, MeetingRepository>();

#endregion

#region EmailConfiguration
builder.Services.AddTransient<IEmailService, EmailService>();
#endregion

#region CommonRepository
builder.Services.AddTransient<CommonRepository>();

#endregion

#region Task

builder.Services.AddTransient<ITask, TaskRepository>();
builder.Services.AddTransient<TaskRepository>();

#endregion

#region Communication

builder.Services.AddTransient<CommunicationRepository>();
builder.Services.AddTransient<ICommunication, CommunicationRepository>();
builder.Services.AddTransient<TempFileUploadRepository>();

#endregion
#region Communication

builder.Services.AddTransient<CommunicationTarasolRPARepository>();
builder.Services.AddTransient<ICommunicationTarasolRPA, CommunicationTarasolRPARepository>();

#endregion

#region Consultation

builder.Services.AddTransient<COMSConsultationFileRepository>();
builder.Services.AddTransient<ICOMSConsultationFile, COMSConsultationFileRepository>();
builder.Services.AddTransient<ICOMSConsultation, COMSConsultationRepository>();
builder.Services.AddTransient<IComsShared, ComsSharedRepository>();
builder.Services.AddTransient<COMSConsultationRepository>();

#endregion

#region AutoMapper
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));
#endregion

#region ActiveDirectorySettings

builder.Services.Configure<ActiveDirectorySettings>(options =>
{
    options.ServerIPAddress = builder.Configuration["ActiveDirectory:ServerIPAddress"];
    options.Container = builder.Configuration["ActiveDirectory:Container"];
    options.UserCreationContainer = builder.Configuration["ActiveDirectory:UserCreationContainer"];
    options.DomainName = builder.Configuration["ActiveDirectory:DomainName"];
    options.OrganizationalUnit = builder.Configuration["ActiveDirectory:OrganizationalUnits"];
    options.MachineAccountName = builder.Configuration["ActiveDirectory:MachineAccountName"];
    options.MachineAccountPassword = builder.Configuration["ActiveDirectory:MachineAccountPassword"];
});

#endregion

#region LLS Legal Principle
builder.Services.AddTransient<ILLSLegalPrinciple, LLSLegalPrincipleRepository>();
#endregion

#region Time Interval
builder.Services.AddTransient<ITimeIntervals, TimeIntervalRepository>();
#endregion

#region Worker Service
builder.Services.AddScoped<IWorkerService, WorkerServiceRepository>();
#endregion

#region CMS COMS Inbox and outbox Number Pattern
builder.Services.AddTransient<CMSCOMSInboxOutboxPatternNumberRepository>();
builder.Services.AddTransient<ICMSCOMSInboxOutboxRequestPatternNumber, CMSCOMSInboxOutboxPatternNumberRepository>();
#endregion
#region FireBase Configuration
string configFilePath = $"\\FireBaseConfig\\fatwamobileapp-firebase-adminsdk-xxtbh-d709859f27.json";
FirebaseApp.Create(new AppOptions
{
    Credential = GoogleCredential.FromFile(Path.Combine(Directory.GetCurrentDirectory() + configFilePath))
});
#endregion
#region Automation Monitoring Interface

builder.Services.AddTransient<IAutomationMonitoring, AutomationMonitoringRepository>();

#endregion

#region FATWA Archived Cases
builder.Services.AddScoped<IArchivedCase, ArchivedCasesRepository>();
#endregion

var jwtsettings = new JwtSettings();
builder.Configuration.Bind(nameof(jwtsettings), jwtsettings);
builder.Services.AddSingleton(jwtsettings);
var tokenValidationparameter = new TokenValidationParameters
{
    ValidateIssuerSigningKey = true,
    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtsettings.Secret)),
    ValidateIssuer = false,
    ValidateAudience = false,
    RequireExpirationTime = false,
    ValidateLifetime = true
};

builder.Services.AddSingleton(tokenValidationparameter);
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; // Default to JwtBearer'
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = tokenValidationparameter;
    x.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var accessToken = context.Request.Query["access_token"];
            var path = context.HttpContext.Request.Path;
            if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/notifications"))
            {
                context.Token = accessToken;
            }
            return Task.CompletedTask;
        }
    };
}).AddScheme<AuthenticationSchemeOptions, ApiKeyHandler>("FatwaApiKey", options => { });
//builder.Services.AddCors();

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
.AddErrorDescriber<LocalizedIdentityErrorDescriber>()
.AddEntityFrameworkStores<DatabaseContext>()
.AddEntityFrameworkStores<AutoMonInterfaceDbContext>()
.AddDefaultUI()
.AddDefaultTokenProviders();
// Modifies the default password policy settings for Identity.
builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.Password.RequiredLength = 8;
    options.Password.RequiredUniqueChars = 0;
});


builder.Services.Configure<FormOptions>(x =>
{
    x.ValueLengthLimit = int.MaxValue;
    x.MultipartBodyLengthLimit = int.MaxValue;
    x.MemoryBufferThreshold = int.MaxValue;
});

builder.Services.AddControllers();
builder.Services.AddControllers().AddJsonOptions(x =>
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
builder.Services.AddApiVersioning(setup =>
{
    setup.DefaultApiVersion = new ApiVersion(1, 0);
    setup.AssumeDefaultVersionWhenUnspecified = true;
    setup.ReportApiVersions = true;
});
//Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddVersionedApiExplorer(setup =>
{
    setup.GroupNameFormat = "'v'VVV";
    setup.SubstituteApiVersionInUrl = true;
});
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme."
    });
    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
{
{
new Microsoft.OpenApi.Models.OpenApiSecurityScheme
{
Reference = new Microsoft.OpenApi.Models.OpenApiReference
{
Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
Id = "Bearer"
}
},
new string[] {}
}
});
});
builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();
var logger = new LoggerConfiguration()
.ReadFrom.Configuration(builder.Configuration)
.Enrich.FromLogContext()
.CreateLogger();


var knownProxies = builder.Configuration.GetSection("KnownProxies").Value;
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    options.ForwardLimit = null;
    options.KnownProxies.Clear();
    foreach (var ip in knownProxies.Split(new char[] { ',' }))
    {
        options.KnownProxies.Add(IPAddress.Parse(ip));
    }
});

builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);
builder.Services.AddSingleton<IUserIdProvider, CustomUserIdProvider>();
builder.Services.AddSignalR();
var app = builder.Build();

var apiVersionDescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions)
    {
        options.SwaggerEndpoint(
        $"/swagger/{description.GroupName}/swagger.json",
        description.GroupName.ToUpperInvariant());
    }
});
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor |
    ForwardedHeaders.XForwardedProto
});
app.UseRouting();
app.UseHttpsRedirection();
app.UseCors();
app.UseForwardedHeaders();

app.UseAuthentication();
app.UseAuthorization();
//app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
app.UseCors();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});
app.UseStaticFiles();
app.MapHub<NotificationsHub>("notifications");
app.Run(); 