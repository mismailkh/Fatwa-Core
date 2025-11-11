using Append.Blazor.Printing;
using BlazorDownloadFile;
using Blazored.LocalStorage;
using Blazored.Toast;
using DiffPlex;
using DiffPlex.DiffBuilder;
using FATWA_DOMAIN.Models.ViewModel;
using Fatwa_WEB.Services.Communications;
using FATWA_WEB.Data;
using FATWA_WEB.Extensions;
using FATWA_WEB.Services;
using FATWA_WEB.Services.CaseManagement;
using FATWA_WEB.Services.Consultation;
using FATWA_WEB.Services.ContactManagment;
using FATWA_WEB.Services.InventoryManagement;
using FATWA_WEB.Services.Lms;
using FATWA_WEB.Services.Meet;
using FATWA_WEB.Services.Tasks;
using FATWA_WEB.Services.MojStatistics;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Localization;
using Radzen;
using Syncfusion.Blazor;
using System.Globalization;
using Telerik.Blazor.Services;
using FATWA_WEB.Services.Generic;
using Microsoft.AspNetCore.Authentication.Negotiate;
using FATWA_WEB.Services.TimeTracking;
using FATWA_WEB.Services.PACI;
using FATWA_WEB.Services.MOJRolls;
using AutoMapper;
using FATWA_WEB.Helpers;
using FATWA_WEB.Services.LLSLegalPrincipleService;
using FATWA_WEB.Services.Communications;
using FATWA_WEB.Services.ServiceRequestService;
using FATWA_WEB.Services.TimeInterval;
using FATWA_WEB.Services.OrganizingCommittee;
using FATWA_WEB.Services.ServiceRequestService.HiringManagement;
using FATWA_WEB.Services.ArchivedCases;
using FATWA_WEB.Services.BugReporting;
using FATWA_WEB.Services.General;

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

#region Http Client
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

#endregion

#region Localization

builder.Services.AddControllers();
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

#endregion

#region Services
// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddServerSideBlazor().AddCircuitOptions(option => { option.DetailedErrors = true; });
builder.Services.AddSyncfusionBlazor();
//General/Shared
builder.Services.AddScoped<ProtectedLocalStorage>();
builder.Services.AddScoped<LoginState>();
builder.Services.AddSingleton<TranslationState>();
builder.Services.AddSingleton<RadzenGridSearchExtension>();
builder.Services.AddSingleton<SystemSettingState>();
builder.Services.AddScoped<NavigationState>();
builder.Services.AddScoped<SpinnerService>();
builder.Services.AddScoped<SideMenuService>();
builder.Services.AddScoped<ProcessLogService>();
builder.Services.AddScoped<ErrorLogService>();
builder.Services.AddScoped<AdvancedSearchsService>();
builder.Services.AddScoped<SystemConfigurationService>();
builder.Services.AddScoped<SystemSettingService>();
builder.Services.AddScoped<DashboardService>();
builder.Services.AddScoped<ExcelExportService>();
builder.Services.AddScoped<ExcelImportService>();
builder.Services.AddSingleton<ApplicationState>();
builder.Services.AddBlazorDownloadFile();
builder.Services.AddScoped<FileUploadService>();
builder.Services.AddScoped<DialogService>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<CustomNotificationService>();
builder.Services.AddScoped<TooltipService>();
builder.Services.AddScoped<ContextMenuService>();
builder.Services.AddScoped<DataCommunicationState>();
//LMS
builder.Services.AddScoped<LmsLiteratureService>();
builder.Services.AddScoped<LmsLiteratureClassificationService>();
builder.Services.AddScoped<LmsLiteratureBorrowDetailService>();
builder.Services.AddScoped<LmsLiteratureTypeService>();
builder.Services.AddScoped<LmsLiteratureIndexService>();
builder.Services.AddTransient<LmsLiteratureParentIndexService>();
builder.Services.AddTransient<LmsLiteratureIndexDivisionService>();
builder.Services.AddScoped<LmsLiteratureIndexDivisionService>();
//LDS
builder.Services.AddScoped<LegalLegislationService>();
//LLSLegalPrinciple
builder.Services.AddScoped<LLSLegalPrincipleService>();

//UMS
builder.Services.AddScoped<userService>();
builder.Services.AddScoped<RoleService>();
builder.Services.AddScoped<GroupService>();
//CMS
builder.Services.AddScoped<AdvanceSearchCmsCaseRequestVM>();
builder.Services.AddScoped<AdvanceSearchCmsCaseFileVM>();
builder.Services.AddScoped<CaseRequestService>();
builder.Services.AddScoped<CmsCaseFileService>();
builder.Services.AddScoped<CmsRegisteredCaseService>();
builder.Services.AddScoped<CmsSharedService>();
builder.Services.AddScoped<CmsCaseTemplateService>();
//COMS
builder.Services.AddScoped<COMSConsultationService>();
builder.Services.AddScoped<ComsSharedService>();
builder.Services.AddScoped<IdentifyDocumentTypesForDraft>();
builder.Services.AddScoped<COMSConsultationFileService>();
//ODRP
builder.Services.AddScoped<MojStatisticsCaseStudyService>();
builder.Services.AddScoped<MojStatisticsDashboardService>();
builder.Services.AddScoped<PACIRequestService>();
builder.Services.AddScoped<MOJRollsService>();
//WF
builder.Services.AddScoped<WorkflowService>();
//NOTIF
builder.Services.AddScoped<NotificationDetailService>();
//LOOKUP
builder.Services.AddScoped<LookupService>();
//COMM
builder.Services.AddScoped<CommunicationService>();
builder.Services.AddScoped<CommunicationTarasolService>();

//CNT
builder.Services.AddScoped<CNTContactService>();
//INV
builder.Services.AddScoped<INVInventoryService>();
//Time Tracking
builder.Services.AddScoped<TimeTrackingService>();
//Meeting
builder.Services.AddScoped<MeetingService>();
//Task
builder.Services.AddScoped<TaskService>();

//ServiceRequest
builder.Services.AddScoped<ServiceRequestSharedService>();
builder.Services.AddScoped<ComplaintRequestService>();
builder.Services.AddScoped<InventoryRequestService>();
builder.Services.AddScoped<LeaveAndAttendanceRequestService>();
builder.Services.AddScoped<HiringRequestService>();

builder.Services.AddScoped<OrganizingCommitteeService>();


builder.Services.AddScoped<TimeIntervalService>();

builder.Services.AddScoped<ArchivedCasesService>();

builder.Services.AddScoped<BugReportingService>();
builder.Services.AddScoped<InvalidRequestHandlerService>();
builder.Services.AddScoped<BugTicketEventService>();
builder.Services.AddScoped<DraftHandlerService>();

//Others
builder.Services.AddTelerikBlazor();
// register a custom localizer for the Telerik components, after registering the Telerik services
builder.Services.AddSingleton(typeof(ITelerikStringLocalizer), typeof(SampleResxLocalizer));
builder.Services.AddBlazoredToast();
builder.Services.AddScoped<ISideBySideDiffBuilder, SideBySideDiffBuilder>();
builder.Services.AddScoped<IDiffer, Differ>();
builder.Services.AddScoped<IPrintingService, PrintingService>();
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
#endregion

#region AutoMapper

var mapperConfiguration = new MapperConfiguration(configuration =>
{
    configuration.AddProfile(new AutoMapperProfile());
});


var mapper = mapperConfiguration.CreateMapper();
builder.Services.AddSingleton(mapper);
#endregion

var app = builder.Build();

#region Syncfusion License
//Register Syncfusion license
Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1NDaF5cWWtCf1FpRmJGdld5fUVHYVZUTXxaS00DNHVRdkdnWH1ecXRVR2NYUUB2WEs=");
#endregion

#region Workflow
using (var scope = app.Services.CreateScope())
{
    var _workflowService = scope.ServiceProvider.GetRequiredService<WorkflowService>();
    var _documentService = scope.ServiceProvider.GetRequiredService<LegalLegislationService>();
    var _translationState = scope.ServiceProvider.GetRequiredService<TranslationState>();
    var _fileUploadService = scope.ServiceProvider.GetRequiredService<FileUploadService>();
    var _cmsCaseTemplateService = scope.ServiceProvider.GetRequiredService<CmsCaseTemplateService>();
    var _taskService = scope.ServiceProvider.GetRequiredService<TaskService>();
    var _config = scope.ServiceProvider.GetRequiredService<IConfiguration>();
    WorkflowImplementationService.WorkflowImplementationServiceConfigure(_translationState, _workflowService, _fileUploadService, _cmsCaseTemplateService, _config, _taskService);
    //WorkflowImplementationService.WorkflowImplementationServiceConfigure(app.Services.GetService<IConfiguration>(), app.Services.GetService<LoginState>(), _workflowService, _documentService);
    //do your stuff....
}
#endregion

#region Localization/Culture
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

var supportedCultures = new List<CultureInfo>()
{
    new CultureInfo("en-US"),
    arabicKuwaitCulture
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

app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapBlazorHub();
    app.MapFallbackToPage("/_Host");
});

app.Run();