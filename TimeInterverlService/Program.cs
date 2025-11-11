using FATWA_DOMAIN.Common.Service;
using FATWA_DOMAIN.Interfaces;
using FATWA_DOMAIN.Interfaces.Notification;
using FATWA_DOMAIN.Interfaces.WorkerService;
using FATWA_INFRASTRUCTURE.Database;
using FATWA_INFRASTRUCTURE.Repository;
using FATWA_INFRASTRUCTURE.Repository.NotificationRepo;
using FATWA_INFRASTRUCTURE.Repository.WorkerService;
using FATWATIMEINTERVALSERVICES.Helper;
using FATWATIMEINTERVALSERVICES.RabbitMQ;
using FATWATIMEINTERVALSERVICES.Services;
using Microsoft.EntityFrameworkCore;
using Quartz;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        IConfiguration configuration = hostContext.Configuration;

        services.AddDbContext<DatabaseContext>(options => options.UseSqlServer(configuration.GetConnectionString("LocalDBConnection")));
        services.Configure<WorkerServiceCRONExpression>(configuration.GetSection("WorkerServiceCronsExpression"));
        var Crons = configuration.GetSection("WorkerServiceCronsExpression").Get<WorkerServiceCRONExpression>();

        services.AddQuartz(q =>
        {
            var jobKey = new JobKey("MOJReminderJob");
            q.AddJob<AssignToMOJReminderService>(opts => opts.WithIdentity(jobKey));

            q.AddTrigger(opts => opts
                .ForJob(jobKey)
                .WithIdentity("MOJReminderJob-trigger")
                .WithCronSchedule(Crons.MOJReminderJob));
            //.WithCronSchedule("0 4 15 * 1-12 ?"));
        });
        services.AddQuartz(q =>
        {
            var jobKey = new JobKey("DataPopulationJob");
            q.AddJob<DataPopulationFromHistoryToMainServiceFatwa>(opts => opts.WithIdentity(jobKey));

            q.AddTrigger(opts => opts
                .ForJob(jobKey)
                .WithIdentity("DataPopulation-trigger")
                .WithCronSchedule(Crons.DataPopulationJob));
        });
        services.AddQuartz(q =>
        {
            var jobKey = new JobKey("OpinionLetterReminderJob");
            q.AddJob<OpinionLetterReminderService>(opts => opts.WithIdentity(jobKey));

            q.AddTrigger(opts => opts
                .ForJob(jobKey)
                .WithIdentity("OpinionLetterReminder-trigger")
                .WithCronSchedule(Crons.OpinionLetterReminderJob));
        });
        services.AddQuartz(q =>
        {
            var jobKey = new JobKey("HOSReminderServiceRegionalAppealSupremeJob");
            q.AddJob<HOSReminderServiceRegionalAppealSupreme>(opts => opts.WithIdentity(jobKey));

            q.AddTrigger(opts => opts
                .ForJob(jobKey)
                .WithIdentity("HOSReminderServiceAppeal-trigger")
                .WithCronSchedule(Crons.HOSReminderServiceRegionalAppealSupremeJob));
        });
        services.AddQuartz(q =>
        {
            var jobKey = new JobKey("CompleteClaimStatement");
            q.AddJob<ReminderToCompleteClaimStatementDefenseLetter>(opts => opts.WithIdentity(jobKey));

            q.AddTrigger(opts => opts
                .ForJob(jobKey)
                .WithIdentity("CompleteClaimStatement-trigger")
                .WithCronSchedule(Crons.CompleteClaimStatement));
        });
        services.AddQuartz(q =>
        {
            var jobKey = new JobKey("RequestForAdditionalInfoReminderServiceJob");
            q.AddJob<RequestForAdditionalInfoReminderService>(opts => opts.WithIdentity(jobKey));

            q.AddTrigger(opts => opts
                .ForJob(jobKey)
                .WithIdentity("RequestForAdditionalInfoReminderService-trigger")
                .WithCronSchedule(Crons.RequestForAdditionalInfoReminderServiceJob));
        });
        services.AddQuartz(q =>
        {
            var jobKey = new JobKey("RequestForAdditionalInfoServiceJob");
            q.AddJob<RequestForAdditionalInfoService>(opts => opts.WithIdentity(jobKey));

            q.AddTrigger(opts => opts
                .ForJob(jobKey)
                .WithIdentity("RequestForAdditionalInfoService-trigger")
                .WithCronSchedule(Crons.RequestForAdditionalInfoServiceJob));
        });
        services.AddQuartz(q =>
        {
            var jobKey = new JobKey("ReviewDraftReminderServiceJob");
            q.AddJob<ReviewDraftReminderService>(opts => opts.WithIdentity(jobKey));

            q.AddTrigger(opts => opts
                .ForJob(jobKey)
                .WithIdentity("ReviewDraftReminderService-trigger")
                .WithCronSchedule(Crons.ReviewDraftReminderServiceJob));
        });
        services.AddQuartz(q =>
        {
            var jobKey = new JobKey("RabbitMQUnpublishedMessageJob");
            q.AddJob<RabbitMQUnpublishedMessagesService>(opts => opts.WithIdentity(jobKey));

            q.AddTrigger(opts => opts
                .ForJob(jobKey)
                .WithIdentity("RabbitMQUnpublishedMessage-trigger")
                .WithCronSchedule(Crons.RabbitMQUnpublishedMessageJob));
        });
        services.AddQuartz(q =>
        {
            var jobKey = new JobKey("TaskDecisionPendingReminderServiceJob");
            q.AddJob<ReminderForPendingTaskDecisionService>(opts => opts.WithIdentity(jobKey));

            q.AddTrigger(opts => opts
                .ForJob(jobKey)
                .WithIdentity("TaskDecisionPendingReminderService-trigger")
                .WithCronSchedule(Crons.TaskDecisionPendingReminderServiceJob));
        });
        services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
        services.AddScoped<ReminderMethods>();
        services.AddScoped<WorkerServiceRepository>();
        services.AddScoped<IAuditLog, AuditLogRepository>();
        services.AddTransient<INotification, NotificationRepository>();
        services.AddTransient<IEmailService, EmailService>();
        services.AddScoped<ITimeIntervals, TimeIntervalRepository>();
        services.AddTransient<RabbitMQClient>();
    })
    .Build();

await host.RunAsync();
