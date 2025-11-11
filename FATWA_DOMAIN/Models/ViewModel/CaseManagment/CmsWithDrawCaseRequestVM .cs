using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models.ViewModel.CaseManagment
{
    public class CmsWithDrawCaseRequestVM
    {
       
        public Guid RequestId { get; set; }
        public Guid WithdrawRequestId { get; set; }
        public string RequestNumber { get; set; }
        public DateTime? RequestDate { get; set; }
        public decimal? ClaimAmount { get; set; }
        public string? ReferenceNo { get; set; }
        public DateTime? ReferenceDate { get; set; }
        public string? Remarks { get; set; }
        public string? CaseRequirements { get; set; }
        public string? Subject { get; set; }
        public bool? IsConfidential { get; set; }
        public int? GovtEntityId { get; set; }
        public int? DepartmentId { get; set; }
        public int? SectorTypeId { get; set; }
        public int? RequestTypeId { get; set; }
        public int? SubTypeId { get; set; }
        public int? PriorityId { get; set; }
        public int? StatusId { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public bool? IsDeleted { get; set; }
        public string? Department_Name_En { get; set; }
        public string? Department_Name_Ar { get; set; }
        public string? GovermentEntity_Name_En { get; set; }
        public string? GovermentEntity_Name_Ar { get; set; }
        public string? RequestType_Name_En { get; set; }
        public string? RequestType_Name_Ar { get; set; }
        public string? SectorType_Name_En { get; set; }
        public string? SectorType_Name_Ar { get; set; }
        public string? Subtype_Name_En { get; set; }
        public string? Subtype_Name_Ar { get; set; }
        public string? Priority_Name_En { get; set; }
        public string? Priority_Name_Ar { get; set; }
        public string? Status_Name_En { get; set; }
        public string? Status_Name_Ar { get; set; }
        public int? CourtTypeId { get; set; }
        public string? Court_Type_Name_En { get; set; }
        public string? Court_Type_Name_Ar { get; set; }
        public int? WithDrawStatusId { get; set; }


    }
}
