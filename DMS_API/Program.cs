using System.Net;
using System.Text;
using System.Text.Json.Serialization;
using DMS_API;
using DMS_API.AuthenticationSchemes;
using DMS_API.Helpers;
using DMS_API.SwaggerOptions;
using FATWA_DOMAIN.Interfaces;
using FATWA_DOMAIN.Interfaces.Common;
using FATWA_DOMAIN.Interfaces.PatternNumber;
using FATWA_GENERAL.Helper;
using FATWA_INFRASTRUCTURE.Database;
using FATWA_INFRASTRUCTURE.Repository;
using FATWA_INFRASTRUCTURE.Repository.CommonRepos;
using FATWA_INFRASTRUCTURE.Repository.PatternNumber;
using FATWA_INFRASTRUCTURE.Repository.RolesAndPermissions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<DmsDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DmsLocalDBConnection")));
builder.Services.AddDbContext<DatabaseContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("FatwaLocalDBConnection")));
builder.Services.AddDbContext<ArchivedCasesDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("FatwaArchivedCasesLocalDbConnection")));
builder.Services.AddTransient<ITempFileUpload, TempFileUploadRepository>();
builder.Services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();
builder.Services.AddTransient<IAuditLog, AuditLogRepository>();
builder.Services.AddTransient<ILookups, LookupsRepository>();
//builder.Services.AddScoped<ApiKeyAuthorizeAttribute>();

Log.Logger = new LoggerConfiguration()
                 .MinimumLevel.Debug()
                 .WriteTo.File("DigitalSignatureLog.json")
                 .CreateLogger();

#region CMS Number Pattern
builder.Services.AddTransient<CMSCOMSInboxOutboxPatternNumberRepository>();
builder.Services.AddTransient<ICMSCOMSInboxOutboxRequestPatternNumber, CMSCOMSInboxOutboxPatternNumberRepository>();
#endregion

#region AutoMapper
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));
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
builder.Services.AddAuthentication(x =>
{
	x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
	x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
	x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
	x.RequireHttpsMetadata = false;
	x.SaveToken = true;
	x.TokenValidationParameters = tokenValidationparameter;
}).AddScheme<AuthenticationSchemeOptions, ApiKeyHandler>("DmsApiKey", options => { });
if (builder.Environment.IsDevelopment())
{
	builder.Services.AddCors(options =>
	{
		options.AddPolicy("FileUploadPolicies",
			policy =>
			{
				policy.WithOrigins("https://localhost:7214", "https://localhost:7089", "https://localhost:7171", "https://localhost:7238", "https://localhost:5048", "https://localhost:7161")
									.AllowAnyHeader()
									.AllowAnyMethod();
			});
	});
}
else
{
	builder.Services.AddCors(options =>
	{
		options.AddPolicy("FileUploadPolicies",
			policy =>
            {
                policy.WithOrigins("http://192.168.251.11:8082", "http://192.168.251.11:8084", "http://115.186.185.190:1044",
					"http://115.186.185.190:6092", "http://115.186.185.190:6093", 
					"http://192.168.210.100:8081", "http://192.168.1.209:8082", 
					"http://192.168.1.209:8083", "http://uatapp01:8081", 
					"http://192.168.210.100:8082", "http://115.186.185.190:6072", "http://115.186.185.190:6074",
                    "http://115.186.185.190:6062", "http://192.168.210.101:8082", 
					"http://115.186.185.190:6071", "http://192.168.1.209:8085", 
					"http://192.168.1.209:8089", "http://192.168.251.12:8082",
                    "http://192.168.100.204:8083", "http://192.168.100.204:8084",
                    "http://192.168.172.31:8081", "http://192.168.172.31:8082",
                    "http://192.168.172.33:8081", "http://192.168.172.33:8082",
                    "http://192.168.171.11", "http://192.168.171.12"
                    )
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
	});
}


builder.Services.Configure<FormOptions>(x =>
{
	x.ValueLengthLimit = int.MaxValue;
	x.MultipartBodyLengthLimit = int.MaxValue;
	x.MemoryBufferThreshold = int.MaxValue;
});

builder.WebHost.ConfigureKestrel(x =>
{
	x.Limits.MaxRequestBodySize = int.MaxValue;
});

builder.Services.AddControllers().AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
builder.Services.AddApiVersioning(setup =>
{
	setup.DefaultApiVersion = new ApiVersion(1, 1);
	setup.AssumeDefaultVersionWhenUnspecified = true;
	setup.ReportApiVersions = true;
});
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
			}, new string[] {}
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
var app = builder.Build();

#region Syncfusion License
//Register Syncfusion license
Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MzA2NDM2OUAzMjM0MmUzMDJlMzBPSjZsbDkxZlIxQmg3bytlZ05WK2J3R3JtbWhEUHpkR0FMT0pjWTRRUHpNPQ==");
#endregion

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
app.UseCors();

app.UseEndpoints(endpoints =>
{
	endpoints.MapControllers();
}); 
app.Run();
