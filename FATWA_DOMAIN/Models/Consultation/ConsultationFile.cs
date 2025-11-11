using FATWA_DOMAIN.Models.ViewModel.CaseManagment;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FATWA_DOMAIN.Models.BaseModels;
using FATWA_DOMAIN.Models.ViewModel.ConsultationVMs;
using FATWA_DOMAIN.Models.ViewModel;

namespace FATWA_DOMAIN.Models.Consultation
{
    [Table("COMS_CONSULTATION_FILE")]
    public class ConsultationFile : TransactionalBaseModel
    {
        [Key]
        public Guid FileId { get; set; }
        public Guid RequestId { get; set; }
        public string FileNumber { get; set; }
        public int? ShortNumber { get; set; }
        public string? FileName { get; set; }
        public int StatusId { get; set; }
        public int? TransferStatusId { get; set; }
        public int? FatwaPriorityId { get; set; }
        public bool? IsAssignedBack { get; set; }
		public string? ComsFileNumberFormat { get; set; }
        public string PatternSequenceResult { get; set; }

        [NotMapped]
        public IList<ConsultationPartyVM> ConsultationPartyLinks { get; set; } = new List<ConsultationPartyVM>();
        [NotMapped]
        public int? SectorTypeId { get; set; }  
       

        [NotMapped]
        public string? Remarks { get; set; }
        [NotMapped]
        public bool? IsAssignment { get; set; }
        [NotMapped]
        public IList<ConsultationParty> PartyLinks { get; set; } = new List<ConsultationParty>();
        [NotMapped]
        public IList<UploadedDocumentVM> UploadedDocuments { get; set; } = new List<UploadedDocumentVM>();
    }
}
