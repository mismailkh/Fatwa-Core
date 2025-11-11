using FATWA_DOMAIN.Models.BaseModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static FATWA_DOMAIN.Enums.WorkflowEnums;

namespace FATWA_DOMAIN.Models.ViewModel.WorkerServiceVMs
{

    //<History Author = 'Nabeel ur Rehman' Date='2023-08-24' Version="1.0" Branch="master"> Add document entity</History>
    public partial class WSCmsCaseFileForHOSIntervalVM
    {
        public int Id { get; set; }
        public int CmsComsReminderTypeId { get; set; }
        public string IntervalNameEn { get; set; }
        public string IntervalNameAr { get; set; }
        //   public bool? IsPrimary { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Createdby { get; set; }
        public bool? IsNotification { get; set; }
        public bool IsTask { get; set; }

        public bool IsFirstReminder { get; set; }
        public bool IsSecondReminder { get; set; }
        public bool IsThirdReminder { get; set; }
    }
}
