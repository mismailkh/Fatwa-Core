using FATWA_DOMAIN.Models.BaseModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel.CaseManagment
{
    public class CmsDraftedDocumentVM 
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public int? DraftNumber { get; set; }
        public DateTime TempCreatedDate { get; set; }
        public decimal VersionNumber { get; set; }
        public Guid VersionId { get; set; }
        public Guid ReferenceId { get; set; }
        public int? TemplateId { get; set; }
        public string? ReviewerUserId { get; set; }
        public string? ReviewerEn { get; set; }
        public string? ReviewerAr { get; set; }
        public string? ReviewerRoleId { get; set; }
        public string? TypeEn { get; set; }
        public string? TypeAr { get; set; }
        public string? StatusEn { get; set; }
        public string? StatusAr { get; set; }
        public int? AttachmentTypeId { get; set; }
        public int? StatusId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string? GovtEntityNamesEn { get; set; }
        public string? GovtEntityNamesAr { get; set; }
    }

    public class CmsDraftedTemplateVM
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public int? DraftNumber { get; set; }
        public int? TemplateId { get; set; }
        public string? TypeEn { get; set; }
        public string? TypeAr { get; set; }
        public int? AttachmentTypeId { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? StatusId { get; set; }
        public string? GovtEntityNamesEn { get; set; }
        public string? GovtEntityNamesAr { get; set; }

    }

    public class CmsDraftedTemplateVersionVM
    {
        public Guid VersionId { get; set; }
        public Guid DraftedTemplateId { get; set; }
        public decimal VersionNumber { get; set; }
        public string? ReviewerUserId { get; set; }
        public string? ReviewerEn { get; set; }
        public string? ReviewerAr { get; set; }
        public string? ReviewerRoleId { get; set; }
        public string? StatusEn { get; set; }
        public string? StatusAr { get; set; }
        public int? StatusId { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class CmsDraftedDocumentReasonVM
    {
        [Key]
        public int Id { get; set; }
        public string? Reason { get; set; }
        public string? UserNameEn { get; set; }
        public string? UserNameAr { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
    public class CmsDraftedDocumentOpioninVM
    {
        [Key]
        public int Id { get; set; }
        public string? Opinion { get; set; }
        public string? UserNameEn { get; set; }
        public string? UserNameAr { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
