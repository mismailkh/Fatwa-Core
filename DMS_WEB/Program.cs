using BlazorDownloadFile;
using Blazored.LocalStorage;
using DMS_WEB.Data;
using DMS_WEB.Extensions;
using DMS_WEB.Services;
using Microsoft.AspNetCore.Authentication.Negotiate;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Localization;
using Radzen;
using Syncfusion.Blazor;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

#region appsettings

var configuration = builder.Configuration;

var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

configuration
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddJsonFile($"appsettings.{env}.json", true, true);

#endregion

#region SSO
if (configuration.GetValue<string>("Environment") != "QA")
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

#region Services

//Singleton Services
builder.Services.AddSingleton<TranslationState>();
builder.Services.AddSingleton<SystemSettingState>();

//Scoped Services
builder.Services.AddSingleton<RadzenGridSearchExtension>();
builder.Services.AddScoped<DataCommunicationState>();
builder.Services.AddScoped<SpinnerService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<RoleService>();
builder.Services.AddScoped<GroupService>();
builder.Services.AddScoped<ErrorLogService>();
builder.Services.AddScoped<ProcessLogService>();
builder.Services.AddScoped<FileUploadService>();
builder.Services.AddScoped<GlobalsService>();
builder.Services.AddScoped<NotificationDetailService>();
builder.Services.AddScoped<SideMenuService>();
builder.Services.AddScoped<SystemConfigurationService>();
builder.Services.AddScoped<SystemSettingService>();
builder.Services.AddScoped<UserRoleService>();
builder.Services.AddScoped<LoginState>();
builder.Services.AddScoped<DialogService>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<NavigationState>();
builder.Services.AddScoped<TooltipService>();
builder.Services.AddScoped<ContextMenuService>();



#endregion

builder.Services.AddHttpClient();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<HttpClient>(serviceProvider =>
{

    var uriHelper = serviceProvider.GetRequiredService<NavigationManager>();

    return new HttpClient
    {
        BaseAddress = new Uri(uriHelper.BaseUri)
    };
});

// Add services to the container.
builder.Services.AddSyncfusionBlazor();
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddServerSideBlazor().AddCircuitOptions(option => { option.DetailedErrors = true; });
builder.Services.AddControllers();
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

builder.Services.AddScoped<ProtectedLocalStorage>();
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
builder.Services.AddBlazorDownloadFile();

var app = builder.Build();
//Register Syncfusion license
Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1NDaF5cWWtCf1FpRmJGdld5fUVHYVZUTXxaS00DNHVRdkdnWH1ecXRVR2NYUUB2WEs=");

#region Culture settings

// define the list of cultures your app will support
var supportedCultures = new List<CultureInfo>()
{
    new CultureInfo("ar-KW"),
    new CultureInfo("en-US")
};
app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("ar-KW"),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures,
});

#endregion

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

//app.MapBlazorHub();
//app.MapFallbackToPage("/_Host");
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapBlazorHub();
    app.MapFallbackToPage("/_Host");
});

app.Run();
