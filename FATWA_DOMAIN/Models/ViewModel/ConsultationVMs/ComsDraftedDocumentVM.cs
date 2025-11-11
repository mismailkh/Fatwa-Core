using FATWA_DOMAIN.Models.BaseModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel.ConsultationVMs
{
    public class ComsDraftedDocumentVM : TransactionalBaseModel
    {
        [Key]
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public int? DraftNumber { get; set; }
        public double VersionNumber { get; set; }
        public Guid ReferenceId { get; set; }
        public int? TemplateId { get; set; }
        public string? ReviewerUserId { get; set; }
        public string? ReviewerRoleId { get; set; }
        public int? StatusId { get; set; } 
        public string? StatusEn { get; set; }
        public string? StatusAr { get; set; }
        public string? Description { get; set; }
        public int? AttachmentTypeId { get; set; } 
        [NotMapped]
        public string? TypeEn { get; set; }
        [NotMapped]
        public string? TypeAr { get; set; }
    }
    public class ComsDraftedDocumentReasonVM
    {
        [Key]
        public int Id { get; set; }
        public string? Reason { get; set; }
        public string? UserNameEn { get; set; }
        public string? UserNameAr { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
