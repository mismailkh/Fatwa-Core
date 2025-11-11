using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.DigitalSignature;
using FATWA_DOMAIN.Models.Dms;
using FATWA_DOMAIN.Models.ViewModel;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.Lookups;
using FATWA_DOMAIN.Models.ViewModel.DmsVMs;
using FATWA_DOMAIN.Models.ViewModel.LLSLegalPrinciple;
using FATWA_DOMAIN.Models.ViewModel.MobileAppVMs;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_INFRASTRUCTURE.Database
{
    //< History Author = 'Hassan Abbas' Date = '2023-08-20' Version = "1.0" Branch = "master">Created DbContext for DMS</History>
    public partial class DmsDbContext : IdentityDbContext
    {
        public DmsDbContext(DbContextOptions<DmsDbContext> options) : base(options)
        {
        }

        #region Entities/Models

        public virtual DbSet<UploadedDocument> UploadedDocuments { get; set; } = null!;
        public virtual DbSet<TempAttachement> TempAttachements { get; set; } = null!;
        public virtual DbSet<DmsUserFavouriteDocument> DmsUserFavouriteDocuments { get; set; } = null;
        public virtual DbSet<DmsSharedDocument> DmsSharedDocuments { get; set; } = null;
        public virtual DbSet<AttachmentType> AttachmentType { get; set; } = null!;
        public virtual DbSet<DmsDocumentClassification> DmsDocumentClassifications { get; set; } = null!;
        public virtual DbSet<DmsAddedDocument> DmsAddedDocuments { get; set; } = null!;
        public virtual DbSet<DmsAddedDocumentVersion> DmsAddedDocumentVersions { get; set; } = null!;
        public virtual DbSet<DmsAddedDocumentReason> DmsAddedDocumentReasons { get; set; } = null!;
        public virtual DbSet<DmsFileTypes> FileTypes { get; set; } = null!;
        public virtual DbSet<DmsAddedDocumentVersion> DmsAddedDocumentsVersion { get; set; } = null!;
        public virtual DbSet<KayPublication> KayPublications { get; set; } = null!;
        public virtual DbSet<MojDocument> MojDocuments { get; set; } = null!;
        public virtual DbSet<DSPRequestLog> DSPRequestLogs { get; set; } = null!;
        public virtual DbSet<DSPAuthenticationRequestLog> DSPAuthenticationRequestLogs { get; set; } = null!;
        public virtual DbSet<DsSigningMethods> DsSigningMethods { get; set; } = null!;
        public virtual DbSet<DsSigningRequestTaskLog> DsSigningRequestTaskLogs { get; set; } = null!;
        public virtual DbSet<DsAttachmentTypeDesignationMapping> DsAttachmentTypeDesignationMapping { get; set; } = null!;
        public virtual DbSet<DSAttachmentTypeSigningMethods> DSAttachmentTypeSigningMethods { get; set; } = null!;

        #endregion

        #region View Models

        [NotMapped]
        public virtual DbSet<TempAttachementVM> TempAttachementVM { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<DsSigningRequestTaskLogVM> DsSigningRequestTaskLogVM { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<LegalPrincipleTempAttachmentVM> LegalPrincipleTempAttachmentVMs { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<UploadedDocumentVM> UploadedDocumentVM { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<DMSDocumentListVM> DMSDocumentListVMs { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<DMSDocumentDetailVM> DMSDocumentDetailVMs { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<DmsAddedDocumentReasonVM> DmsAddedDocumentReasonVMs { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<DMSKayPublicationDocumentListVM> DMSKayPublicationDocumentListVMs { get; set; } = null!;
        public virtual DbSet<MojDocumentVM> MojDocumentVMs { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<LLSLegalPrincipleDocumentVM> LLSLegalPrincipleDocumentVM { get; set; } = null!;   
        [NotMapped]
        public virtual DbSet<LLSLegalPrinciplContentLinkedDocumentVM> LLSLegalPrinciplContentLinkedDocumentVM { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<LLSLegalPrinciplLegalAdviceDocumentVM> LLSLegalPrinciplLegalAdviceDocumentVM { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<LLSLegalPrincipleKuwaitAlYoumDocuments> LLSLegalPrincipleKuwaitAlYoumDocuments { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<LLSLegalPrinciplOtherDocumentVM> LLSLegalPrinciplOtherDocumentVM { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<LLSLegalPrincipleAppealSupremenContentLinkedDocVM> LLSLegalPrincipleAppealSupremenContentLinkedDocVM { get; set; } = null!;   
        [NotMapped]
        public virtual DbSet<LLSLegalPrincipleLegalAdviceContentLinkedDocVM> LLSLegalPrincipleLegalAdviceContentLinkedDocVM { get; set; } = null!;   
        [NotMapped]
        public virtual DbSet<LLSLegalPrincipleKuwaitAlYoumContentLinkedDocVM> LLSLegalPrincipleKuwaitAlYoumContentLinkedDocVM { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<LLSLegalPrincipleOthersContentLinkedDocVM> LLSLegalPrincipleOthersContentLinkedDocVM { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<MobileAppDMSDocumentDetailVM> MobileAppDMSDocumentDetailVM { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<MobileAppUploadDocumentsVM> MobileAppUploadDocumentsVM { get; set; } = null!;
        #endregion
        #region Attachment Type VM 
        public virtual DbSet<AttachmentTypeVM> AttachmentTypeVMs { get; set; } = null!;
      
        #endregion

        partial void OnModelBuilding(ModelBuilder builder);

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<DMSDocumentListVM>(builder => { builder.HasNoKey(); });
            modelBuilder.Entity<DMSDocumentDetailVM>(builder => { builder.HasNoKey(); });
            modelBuilder.Entity<LLSLegalPrinciplContentLinkedDocumentVM>(builder => { builder.HasNoKey(); });
            modelBuilder.Entity<LLSLegalPrincipleLinkedDocVM>(builder => { builder.HasNoKey(); });

            // Authentication 
            modelBuilder.Entity<DSPAuthenticationResponse>(builder => { builder.HasNoKey(); });
            modelBuilder.Entity<MIDAuthSignResponse>(builder => { builder.HasNoKey(); });
            modelBuilder.Entity<RequestDetails>(builder => { builder.HasNoKey(); });
            modelBuilder.Entity<ResultDetails>(builder => { builder.HasNoKey(); });
            modelBuilder.Entity<PersonalData>(builder => { builder.HasNoKey(); });
            modelBuilder.Entity<Address>(builder => { builder.HasNoKey(); });
            modelBuilder.Entity<MobileAppDMSDocumentDetailVM>(builder => { builder.HasNoKey(); });

        }
    }
}