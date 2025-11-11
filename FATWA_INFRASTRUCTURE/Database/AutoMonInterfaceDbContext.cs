using FATWA_DOMAIN.Models.AdminModels.AutomationMonitoring;
using FATWA_DOMAIN.Models.DigitalSignature;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.AutomationMonitoring;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_INFRASTRUCTURE.Database
{
    public partial class AutoMonInterfaceDbContext : IdentityDbContext
    {
        public AutoMonInterfaceDbContext(DbContextOptions<AutoMonInterfaceDbContext> options) : base(options)
        {
        }

        #region Entities/Models

        public virtual DbSet<AMSProcesses> AMProcesses { get; set; } = null!;
        public virtual DbSet<AMSWorkQueue> WorkQueues { get; set; } = null!;
        public virtual DbSet<AMSWorkQueueItem> WorkQueueItems { get; set; } = null!;
        public virtual DbSet<AMSWorkQueueLog> WorkQueueLogs { get; set; } = null!;
        public virtual DbSet<AMSQueueItemStatus> QueueItemStatuses { get; set; } = null!;
        public virtual DbSet<AMSExeceptions> Execeptions { get; set; } = null!;
        public virtual DbSet<AMSResources> Resources { get; set; } = null!;
        public virtual DbSet<AMSResource_Status> Resource_Statuses { get; set; } = null!;
        public virtual DbSet<AMSSessionLog> SessionLogs { get; set; } = null!;
        public virtual DbSet<AMSSession> Sessions { get; set; } = null!;
        public virtual DbSet<AMSSessionStatus> SessionStatuses { get; set; } = null!;
        public virtual DbSet<AMSItemStatusHistory> AMSItemStatusHistories { get; set; } = null!;

        #endregion
        #region View Models
        [NotMapped]
        public virtual DbSet<AutomationMonitoringProcessVM> AutomationMonitoringProcessVMs { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<AutomationMonitoringQueueVM> AutomationMonitoringQueueVMs { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<AutomationMonitoringQueueItemVM> AutomationMonitoringQueueItemVMs { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<AMSCaseDataExtractionVM> AMSCaseDataExtractionVMs { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<AMSSessionListVM> SessionListVMs { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<AMSSessionLogsVM> SessionLogsVMs { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<AMSItemLogVM> ItemLogVMs { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<AMSExceptionsDetailsVM> ExceptionsDetailsVMs { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<AMSResourcesVM> AMSResourcesVMs { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<AMSQueueListVM> AMSQueueListVMs { get; set; } = null!;    
        #endregion
        partial void OnModelBuilding(ModelBuilder builder);

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<AutomationMonitoringProcessVM>(builder => { builder.HasNoKey(); });
            modelBuilder.Entity<AutomationMonitoringQueueVM>(builder => { builder.HasNoKey(); });
            modelBuilder.Entity<AutomationMonitoringQueueItemVM>(builder => { builder.HasNoKey(); });
            modelBuilder.Entity<AMSSessionListVM>(builder => { builder.HasNoKey(); });
            modelBuilder.Entity<AMSSessionLogsVM>(builder => { builder.HasNoKey(); });
            modelBuilder.Entity<AMSItemLogVM>(builder => { builder.HasNoKey(); });
            modelBuilder.Entity<AMSExceptionsDetailsVM>(builder => { builder.HasNoKey(); });
            modelBuilder.Entity<AMSCaseDataExtractionVM>(builder => { builder.HasNoKey(); });

            // Authentication 
            modelBuilder.Entity<DSPAuthenticationResponse>(builder => { builder.HasNoKey(); });
            modelBuilder.Entity<MIDAuthSignResponse>(builder => { builder.HasNoKey(); });
            modelBuilder.Entity<RequestDetails>(builder => { builder.HasNoKey(); });
            modelBuilder.Entity<ResultDetails>(builder => { builder.HasNoKey(); });
            modelBuilder.Entity<PersonalData>(builder => { builder.HasNoKey(); });
            modelBuilder.Entity<Address>(builder => { builder.HasNoKey(); });
        }
    }
}
