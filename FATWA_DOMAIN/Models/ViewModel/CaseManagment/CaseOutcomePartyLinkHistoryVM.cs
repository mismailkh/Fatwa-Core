using FATWA_DOMAIN.Models.BaseModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static FATWA_DOMAIN.Enums.CaseManagementEnums;

namespace FATWA_DOMAIN.Models.ViewModel.CaseManagment
{
   public class CaseOutcomePartyLinkHistoryVM : TransactionalBaseModel
    {
        [Key]
        public Guid Id { get; set; }
        public Guid PartyId { get; set; }
        public Guid ReferenceGuid { get; set; }
        public int CategoryId { get; set; }
        public int TypeId { get; set; }
        public string? CategoryName_En { get; set; }
        public string? CategoryName_Ar { get; set; }
        public string? TypeName_En { get; set; }
        public string? TypeName_Ar { get; set; }
        public string? GovtEntity_En { get; set; }
        public string? GovtEntity_Ar { get; set; }
        public string? Name { get; set; }
        public string? CivilId { get; set; }
        public string? CRN { get; set; }
        public int? EntityId { get; set; }
        public Guid? RepresentativeId { get; set; }
        public string? RepresentativeEn { get; set; }
        public string? RepresentativeAr { get; set; }
        public string? CompanyCivilId { get; set; }
        public int? AttachmentCount { get; set; }
        public int? ActionId { get; set; }  
         [NotMapped]
        public CasePartyTypeEnum CasePartyType { get; set; }
        [NotMapped]
        public CasePartyCategoryEnum CasePartyCategory { get; set; }
         [NotMapped]
        public CaseOutcomePartyActionEnum CasePartyAction { get; set; }
        [NotMapped]
        public string? CasePartyActionName { get; set; }


    }

}
