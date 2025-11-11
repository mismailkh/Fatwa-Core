using FATWA_DOMAIN.Models.BaseModels;
using FATWA_DOMAIN.Models.ViewModel.CaseManagment;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel.CaseManagment
{
    public class CaseRequestDetailVM : TransactionalBaseModel
    {
        [Key]
        public Guid RequestId { get; set; }
        public string? RequestNumber { get; set; }
        public DateTime? RequestDate { get; set; }
        public int StatusId { get; set; }
        public int? RequestTypeId { get; set; }
        public int? GovtEntityId { get; set; }
        public int? DepartmentId { get; set; }
        public int? SectorTypeId { get; set; }
        public int? PriorityId { get; set; }
        public int? SubTypeId { get; set; }
        public decimal? ClaimAmount { get; set; }
        public string? Subject { get; set; }
        public bool IsConfidential { get; set; }
        public string? Remarks { get; set; }
        public string? CaseRequirements { get; set; }
        public string? ReferenceNo { get; set; }
        public string? StatusName_En { get; set; }
        public string? StatusName_Ar { get; set; }
        public string? EntityName_En { get; set; }
        public string? EntityName_Ar { get; set; }
        public string? DepartmentName_En { get; set; }
        public string? DepartmentName_Ar { get; set; }
        public string? SectorName_En { get; set; }
        public string? SectorName_Ar { get; set; }
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
        public int? CourtTypeId { get; set; }
        public string? Court_Type_Name_En { get; set; }
        public string? Court_Type_Name_Ar { get; set; }
        public string? Pre_Court_Type_Name_En { get; set; }
        public string? Pre_Court_Type_Name_Ar { get; set; }
        public DateTime? ReferenceDate { get; set; }
        public DateTime? ReviewedDate { get; set; }
        public DateTime? ReceivedDate { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public int? TransferStatusId { get; set; }
        [NotMapped]
        public IList<CasePartyLinkVM>? CasePartyLinks { get; set; } = new List<CasePartyLinkVM>(); 
    }
}
