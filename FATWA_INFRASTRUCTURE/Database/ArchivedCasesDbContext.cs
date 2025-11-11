using FATWA_DOMAIN.Models.ArchivedCasesModels;
using FATWA_DOMAIN.Models.ViewModel.ArchivedCases;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FATWA_INFRASTRUCTURE.Database
{
    //< History Author = 'Ammaar Naveed' Date = '2024-12-10' Version = "1.0" Branch = "master" >Created DbContext for Archived Cases</History>
    public partial class ArchivedCasesDbContext : IdentityDbContext
    {
        public ArchivedCasesDbContext(DbContextOptions<ArchivedCasesDbContext> options) : base(options)
        {
        }

        #region Domain Entities
        public virtual DbSet<ArchivedCasesModel> ArchivedCases { get; set; } = null!;
        public virtual DbSet<ArchivedCaseParties> ArchivedCaseParties { get; set; } = null!;
        public virtual DbSet<ArchivedCaseAutomatedNumber> ArchivedCaseAutomatedNumber { get; set; } = null!;
        public virtual DbSet<ArchivedCaseDocuments> ArchivedCaseDocuments { get; set; } = null!;
        public virtual DbSet<ArchivedCaseDataPayload> ArchivedCaseDataPayload { get; set; } = null!;
        public virtual DbSet<ArchivedCasesPartyRoles> ArchivedCasePartyRoles { get; set; } = null!;
        public virtual DbSet<ArchivedCaseDocumentTypes> ArchivedCaseDocumentTypes { get; set; } = null!;
        #endregion

        #region View Models
        public virtual DbSet<AddArchivedCaseDataRequestPayload> ArchivedCaseDataRequestPayload { get; set; } = null!;
        public virtual DbSet<AddArchivedCaseVM> ArchivedCaseVM { get; set; } = null!;
        public virtual DbSet<AddArchivedCasePartyVM> ArchivedCasePartyVM { get; set; } = null!;
        public virtual DbSet<AddArchivedCaseDocumentsVM> AddArchivedCaseDocumentsVM { get; set; } = null!;
        public virtual DbSet<ArchivedCaseListVM> ArchivedCaseListVM { get; set; } = null!;
        public virtual DbSet<ArchivedCaseDetailVM> ArchivedCaseDetailVM { get; set; } = null!;
        public virtual DbSet<ArchivedCasePartiesVM> ArchivedCasePartiesVM { get; set; } = null!;
        public virtual DbSet<ArchivedCaseDocumentsVM> ArchivedCaseDocumentsVM { get; set; } = null!;
        public virtual DbSet<ArchivedCaseAdvanceSearchVM> ArchivedCaseAdvanceSearchVM { get; set; } = null!;
        #endregion

        #region Model Builder
        partial void OnModelBuilding(ModelBuilder builder);

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AddArchivedCaseDataRequestPayload>(builder => { builder.HasNoKey(); });
            modelBuilder.Entity<AddArchivedCaseVM>(builder => { builder.HasNoKey(); });
            modelBuilder.Entity<AddArchivedCasePartyVM>(builder => { builder.HasNoKey(); });
            modelBuilder.Entity<AddArchivedCaseDocumentsVM>(builder => { builder.HasNoKey(); });
            modelBuilder.Entity<ArchivedCasePartiesVM>(builder => { builder.HasNoKey(); });
            modelBuilder.Entity<ArchivedCaseDocumentsVM>(builder => { builder.HasNoKey(); });
            modelBuilder.Entity<ArchivedCaseAdvanceSearchVM>(builder => { builder.HasNoKey(); });
        }
        #endregion

    }
}
