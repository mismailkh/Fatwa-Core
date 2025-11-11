using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using FATWA_DOMAIN.Models.BaseModels;
using System.ComponentModel.DataAnnotations.Schema;
using FATWA_DOMAIN.Models.ViewModel.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel;
using FATWA_DOMAIN.Models.Notifications.ViewModel;

namespace FATWA_DOMAIN.Models.CaseManagment
{
    //<History Author = 'Hassan Abbas' Date='2023-03-04' Version="1.0" Branch="master">Moj Execution Request</History>
    [Table("CMS_MOJ_EXECUTION_REQUEST")]
    public class MojExecutionRequest : TransactionalBaseModel
    {
        [Key]
        public Guid Id { get; set; }
        public Guid? CaseId { get; set; }
        public int? StatusId { get; set; }
        public string? Remarks { get; set; }
        [NotMapped]
        public IList<CmsRegisteredCaseVM> SelectedCases = new List<CmsRegisteredCaseVM>();
        [NotMapped]
        public IList<TempAttachementVM> SelectedDocuments = new List<TempAttachementVM>();
        [NotMapped]
        public int SectorTypeId { get; set; }

        [NotMapped]
        public string TaskUserId { get; set; }
        [NotMapped]
        public NotificationParameter NotificationParameter { get; set; } = new NotificationParameter();
    }
}
