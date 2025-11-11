using FATWA_DOMAIN.Models.AdminModels.UserManagement;
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
    //< History Author = 'Danish' Date = '2022-12-09' Version = "1.0" Branch = "master" >Request For Document</History>
    [Table("CMS_DOCUMENT_PORTFOLIO_REQUEST")]
    public class MojRequestForDocument : TransactionalBaseModel
    {
        [Key]
        public Guid Id { get; set; }
        public int AttachmentTypeId { get; set; }
        public Guid CaseId { get; set; }
        public DateTime HearingDate { get; set; }
        public string? RequiredDocuments { get; set; }
        public bool IsAddressed { get; set; }
        public int SectorTypeId { get; set; }
        [NotMapped] 
        public UserPersonalInformation? User { get; set;}
    }
}
