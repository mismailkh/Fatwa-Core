using FATWA_DOMAIN.Models.BaseModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.CaseManagment
{
    //< History Author = 'Hassan Abbas' Date = '2022-09-29' Version = "1.0" Branch = "master" >Case Party Link</History>
    [Table("CMS_CASE_PARTY_LINK")]
    public class CasePartyLink : TransactionalBaseModel
    {
        [Key]
        public Guid Id { get; set; }
        public Guid ReferenceGuid { get; set; }
        public int CategoryId { get; set; }
        public int TypeId { get; set; }
        public string? Name { get; set; }
        public string? CivilId { get; set; }
        public string? CRN { get; set; }
        public int? EntityId { get; set; }
        public Guid? RepresentativeId { get; set; }
        public string? PACINumber { get; set; }
        public string? Address { get; set; }
        public string? CompanyCivilId { get; set; }
        public string? MOCIFileNumber { get; set; }
        public string? LicenseNumber { get; set; }
        public string? MembershipNumber { get; set; }
        [NotMapped]
        public string? GovtEntity_En { get; set; }
        [NotMapped]
        public string? GovtEntity_Ar { get; set; }
        [NotMapped]
        public UploadedDocument? UploadedDocument { get; set; }
    }

    //< History Author = 'Hassan Abbas' Date = '2024-02-15' Version = "1.0" Branch = "master" >Case Party Mismatched Data For Moj Execution</History>
    [Table("CMS_EXECUTION_PARTY_LINK")]
    public class ExecutionPartyLink : TransactionalBaseModel
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
