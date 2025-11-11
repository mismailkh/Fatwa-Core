using FATWA_DOMAIN.Models.BaseModels;
using FATWA_DOMAIN.Models.Notifications.ViewModel;
using FATWA_DOMAIN.Models.ViewModel.CaseManagment;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.CaseManagment
{
    //<History Author = 'Hassan Abbas' Date='2022-11-29' Version="1.0" Branch="master"> Merge Cases Request</History>
    [Table("CMS_MERGE_REQUEST")]
    public class MergeRequest : TransactionalBaseModel
    {
        [Key]
        public Guid Id { get; set; }
        public Guid PrimaryId { get; set; }
        public int StatusId { get; set; }
        public string Reason { get; set; }
        public bool IsMergeTypeCase { get; set; }
        [NotMapped]
        public IList<CmsRegisteredCaseVM> RegisteredCases { get; set; } = new List<CmsRegisteredCaseVM>();
        [NotMapped]
        public NotificationParameter NotificationParameter { get; set; } = new NotificationParameter();
    }
}
