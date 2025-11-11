using FATWA_DOMAIN.Models.Consultation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel.ConsultationVMs
{
    public class ViewConsultationVM
    {
        public Guid ConsultationRequestId { get; set; }
        public string RequestNumber { get; set; }
        public string? GEOpinion { get; set; }
        public string? Remarks { get; set; }
        public int? RequestStatusId { get; set; }
        public int? GovtEntityId { get; set; }
        public string? Introduction { get; set; }
        public bool ContractAmount75000KD { get; set; }
        public string? ReferenceNo { get; set; }
        public DateTime? RequestDate { get; set; }
        public DateTime? ReferenceDate { get; set; }
        public string? RequestTitle { get; set; }
        public string Subject { get; set; }
        public bool IsConfidential { get; set; }
        public string? priorityEn { get; set; }
        public string? priorityAr { get; set; }
        public string? gvtEntityEn { get; set; }
        public string? gvtEntityAr { get; set; }
        public string? departmentEn { get; set; }
        public string? departmentAr { get; set; }
        public string? contractSubTypeEn { get; set; }
        public string? contractSubTypeAr { get; set; }
        public string? requestStatusEn { get; set; }
        public string? requestStatusAr { get; set; }
        public string? Description { get; set; }
        public int? RequestTypeId { get; set; }
        public string? RequestTypeAr { get; set; }
        public string? RequestTypeEn { get; set; }
        public int? TransferStatusId { get; set; }
        public string? SectorName_En { get; set; }
        public string? SectorName_Ar { get; set; }
        public int? SectorTypeId { get; set; }
        public string? FileTypeAr { get; set; }
        public string? FileTypeEn { get; set; }
        public string? CompetentAuthorityEn { get; set; }
        public string? CompetentAuthorityAr { get; set; }
        public string? ComplainantName { get; set; }
        public string? ComplaintAgainst { get; set; }
        public string? ComplainantDecisionNumber { get; set; }
        public string? CompetentAuthorityOpinionWithNote { get; set; } 
        public string? CreatedBy { get; set; }
        public string? GEUserNameEn { get; set; }
        public string? GEUserNameAr { get; set; }
        public string? ReviewerNameEn { get; set; }
        public string? ReviewerNameAr { get; set; }
        public string? ReceiverNameEn { get; set; }
        public string? ReceiverNameAr { get; set; }
        public string? ApproverNameEn { get; set; }
        public string? ApproverNameAr { get; set; }
        public string? InternationalArbitrationTypeNameEn { get; set; }
        public string? InternationalArbitrationTypeNameAr { get; set; }
        public string? HighPriorityReason { get; set; }
        public int? PriorityId { get; set; }
        [NotMapped]
        public IList<ConsultationParty> ConsultationPartys { get; set; } = new List<ConsultationParty>();
        [NotMapped]
        public IList<ConsultationArticle> ConsultationArticles { get; set; } = new List<ConsultationArticle>();



    }
}
