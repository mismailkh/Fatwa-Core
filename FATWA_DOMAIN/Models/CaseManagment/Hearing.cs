using FATWA_DOMAIN.Models.BaseModels;
using FATWA_DOMAIN.Models.ViewModel.DmsVMs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.CaseManagment
{
    //< History Author = 'Hassan Abbas' Date = '2022-12-05' Version = "1.0" Branch = "master">Hearing of a Case</History>
    [Table("CMS_HEARING")]
    public class Hearing: TransactionalBaseModel
    {
        [Key]
        public Guid Id { get; set; }
        public Guid CaseId { get; set; }
        public DateTime HearingDate { get; set; }
        public TimeSpan HearingTime { get; set; }
        public int StatusId { get; set; }
        public string Description { get; set; }
        public string LawyerId { get; set; }
        [NotMapped]
        public DateTime Time { get; set; }
        [NotMapped]
        public bool SendPortfolioRequestMoj { get; set; }
        [NotMapped]
        public MojRequestForDocument? RequestForDocument { get; set; }
        [NotMapped]
        public List<int>? DeletedAttachementIds { get; set; } = new List<int>();
        [NotMapped]
        public List<CopyAttachmentVM>? CopyAttachmentVMs { get; set; } = new List<CopyAttachmentVM>();
        [NotMapped]
        public int? SectorTypeId { get; set; }
        [NotMapped]
        public bool IsUpdated { get; set; }
    }
}
