using FATWA_DOMAIN.Models.Consultation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel.ConsultationVMs
{
    public class ConsultationFileDetailVM
    {
        public Guid? FileId { get; set; }
        public Guid ConsultationRequestId { get; set; }
        public string? FileNumber { get; set; }
        public string? CreatedBy { get; set; }
        public string? FileCreatedBy { get; set; }
        public string? FileName { get; set; }
        public DateTime FileDate { get; set; }
        public string? StatusNameEn { get; set; }
        public string? StatusNameAr { get; set; }
        public int? TransferStatusId { get; set; }
        public int? StatusId { get; set; }
        public string? RequestNumber { get; set; }
        public DateTime? RequestDate { get; set; }
        public int? GovtEntityId { get; set; }
        public int? DepartmentId { get; set; }
        public int? SectorTypeId { get; set; }
        public int? PriorityId { get; set; }
        public bool? ContractAmount75000KD { get; set; }
        public string? Subject { get; set; }
        public bool IsConfidential { get; set; }
        public string? Remarks { get; set; }
        public string? ReferenceNo { get; set; }
        public string? StatusName_En { get; set; }
        public string? StatusName_Ar { get; set; }
        public string? EntityName_En { get; set; }
        public string? EntityName_Ar { get; set; }
        public string? DepartmentName_En { get; set; }
        public string? DepartmentName_Ar { get; set; }
        public string? SectorName_En { get; set; }
        public string? SectorName_Ar { get; set; }
        public int? RequestTypeId { get; set; }
        public string? RequestType_Name_En { get; set; }
        public string? RequestType_Name_Ar { get; set; }
        public string? PriorityName_En { get; set; }
        public string? PriorityName_Ar { get; set; }
        public string? SubType_En { get; set; }
        public string? SubType_Ar { get; set; }
        public string? GEUserNameEn { get; set; }
        public string? GEUserNameAr { get; set; }
        public string? ReviewerNameEn { get; set; }
        public string? ReviewerNameAr { get; set; }
        public string? ReceiverNameEn { get; set; }
        public string? ReceiverNameAr { get; set; }
        public string? ApproverNameEn { get; set; }
        public string? ApproverNameAr { get; set; }
        public DateTime? ReferenceDate { get; set; }
        public DateTime? ReviewedDate { get; set; }
        public DateTime? ReceivedDate { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public string? FatwaPriorityEn { get; set; }
        public string? FatwaPriorityAr { get; set; }
        public int? FatwaPriorityId { get; set; }
        public string? GEOpinion { get; set; }
        public string? ComplainantName { get; set; }
        [NotMapped]
        public IList<ConsultationPartyListVM>? ConsultationPartysDetailsForFile { get; set; } = new List<ConsultationPartyListVM>();
        [NotMapped]
        public IList<ConsultationArticleByConsultationIdListVM>? ConsultationArticlesDetailsForFile { get; set; } = new List<ConsultationArticleByConsultationIdListVM>();
        [NotMapped]
        public string? ConsultationIntroductionForFile { get; set; } 
        [NotMapped]
        public string? LawyerId { get; set; }
        public string? RequestTemplateContent { get; set; }
        public int? TemplateId { get; set; }
    }
}
