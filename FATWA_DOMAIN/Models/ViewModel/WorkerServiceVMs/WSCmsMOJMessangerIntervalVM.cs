using FATWA_DOMAIN.Models.BaseModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static FATWA_DOMAIN.Enums.WorkflowEnums;

namespace FATWA_DOMAIN.Models.ViewModel.WorkerServiceVMs
{
    //<History Author = 'Nabeel ur Rehman' Date='2023-08-24' Version="1.0" Branch="master"> Add document entity</History>
    public class WSCmsMOJMessangerIntervalVM
    {
        public Guid MOJRequestId { get; set; }    
        public DateTime RequestCreatedDate { get; set; }
        public string RequestCreatedbyId { get; set; }
        public int SectorTypeId { get; set; }
        public string FileNumber { get; set; }
        public string FileName { get; set; }
        public Guid FileId { get; set; }

    }
}
