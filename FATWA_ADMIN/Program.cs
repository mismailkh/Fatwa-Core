using BlazorDownloadFile;
using Blazored.LocalStorage;
using FATWA_ADMIN.Data;
using FATWA_ADMIN.Services.General;
using FATWA_ADMIN.Services.UserManagement;
using FATWA_INFRASTRUCTURE.Database;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Radzen;
using Syncfusion.Blazor;
using System.Globalization;
using Telerik.Blazor.Services;
using FATWA_ADMIN.Extensions;
using FATWA_ADMIN.Services.TimeInterval;
using Microsoft.AspNetCore.Authentication.Negotiate;
using FATWA_ADMIN.Services.Notifications;
using FATWA_ADMIN.Services.AutomationMonitoring;
using FATWA_ADMIN.Helpers;
using FATWA_ADMIN.Services.ServiceRequest;
using FATWA_ADMIN.Services;
using FATWA_ADMIN.Services.BugReporting;

var builder = WebApplication.CreateBuilder(args);

#region appsettings

var configuration = builder.Configuration;

var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

configuration
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddJsonFile($"appsettings.{env}.json", true, true);

#endregion

#region SSO
if (configuration.GetValue<string>("Environment") != "QA" && configuration.GetValue<string>("Environment") != "DPS")
{
    // Add services to the container.
    builder.Services.AddAuthentication(NegotiateDefaults.AuthenticationScheme)
   .AddNegotiate();

    builder.Services.AddAuthorization(options =>
    {
        // By default, all incoming requests will be authorized according to the default policy.
        options.FallbackPolicy = options.DefaultPolicy;

    });
}
#endregion

builder.Services.AddScoped<ProtectedLocalStorage>();
builder.Services.AddSingleton<SystemSettingState>();

#region Localization

var arabicKuwaitCulture = new CultureInfo("ar-KW");
CultureInfo.DefaultThreadCurrentCulture = arabicKuwaitCulture;
CultureInfo.DefaultThreadCurrentUICulture = arabicKuwaitCulture;
arabicKuwaitCulture.NumberFormat.NativeDigits = new string[10] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
arabicKuwaitCulture.NumberFormat.CurrencyDecimalSeparator = ".";
arabicKuwaitCulture.NumberFormat.NumberDecimalSeparator = ".";
arabicKuwaitCulture.NumberFormat.PercentDecimalSeparator = ".";
arabicKuwaitCulture.NumberFormat.NumberGroupSeparator = ",";
arabicKuwaitCulture.NumberFormat.CurrencyGroupSeparator = ",";
arabicKuwaitCulture.NumberFormat.PercentGroupSeparator = ",";

builder.Services.AddControllers();
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new List<CultureInfo>()
    {
        new CultureInfo("en-US"),
        arabicKuwaitCulture
    };
    options.DefaultRequestCulture = new RequestCulture("ar-KW");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
});


// the custom localizer service is registered later, after the Telerik services

#endregion
// Add services to the container.
builder.Services.AddDbContext<DatabaseContext>
    (options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("LocalDBConnection")));

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddServerSideBlazor().AddCircuitOptions(option => { option.DetailedErrors = true; });
builder.Services.AddSyncfusionBlazor();
builder.Services.AddScoped<LoginState>();
builder.Services.AddSingleton<TranslationState>();
builder.Services.AddScoped<SpinnerService>();
builder.Services.AddScoped<DialogService>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<TooltipService>();
builder.Services.AddScoped<ContextMenuService>();
builder.Services.AddScoped<SideMenuService>();
builder.Services.AddScoped<RoleService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<LookupService>();
builder.Services.AddScoped<GroupService>();
builder.Services.AddScoped<TransferUserService>();
builder.Services.AddScoped<SystemConfigurationService>();
builder.Services.AddScoped<SystemSettingService>();
builder.Services.AddBlazorDownloadFile();
builder.Services.AddScoped<FileUploadService>();
builder.Services.AddScoped<TranslationService>();
builder.Services.AddScoped<UmsClaimService>();
builder.Services.AddSingleton<RadzenGridSearchExtension>();
builder.Services.AddScoped<TimeIntervalService>();
builder.Services.AddScoped<NotificationsService>();
builder.Services.AddScoped<AutomationMonitoringService>();

builder.Services.AddScoped<ProcessLogService>();
builder.Services.AddScoped<ErrorLogService>();
//ServiceRequest
builder.Services.AddScoped<ServiceRequestService>();
builder.Services.AddScoped<BugReportingService>();


builder.Services.AddScoped<NotificationDetailService>();
builder.Services.AddScoped<InvalidRequestHandlerService>();
builder.Services.AddTelerikBlazor();

// register a custom localizer for the Telerik components, after registering the Telerik services
builder.Services.AddSingleton(typeof(ITelerikStringLocalizer), typeof(SampleResxLocalizer));

builder.Services.Configure<FormOptions>(x =>
{
    x.ValueLengthLimit = int.MaxValue;
    x.MultipartBodyLengthLimit = int.MaxValue;
    x.MemoryBufferThreshold = int.MaxValue;
});
builder.Services.AddServerSideBlazor().AddHubOptions(o =>
{
    o.MaximumReceiveMessageSize = 102400000;

});

builder.Services.AddBlazoredLocalStorage();

#region AutoMapper
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));
#endregion

var app = builder.Build();
//Register Syncfusion license
Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1NDaF5cWWtCf1FpRmJGdld5fUVHYVZUTXxaS00DNHVRdkdnWH1ecXRVR2NYUUB2WEs=");
app.UseRequestLocalization(app.Services.GetService<IOptions<RequestLocalizationOptions>>().Value);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapBlazorHub();
    app.MapFallbackToPage("/_Host");
});
app.Run();
